using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    [TestFixture]
    public class GetSchemaTests : BaseTest
    {
        [Test]
        public void Collections()
        {
            DataTable dt = conn.GetSchema();

            Assert.AreEqual("MetaDataCollections", dt.Rows[0][0]);
            Assert.AreEqual("DataSourceInformation", dt.Rows[1][0]);
            Assert.AreEqual("DataTypes", dt.Rows[2][0]);
            Assert.AreEqual("Restrictions", dt.Rows[3][0]);
            Assert.AreEqual("ReservedWords", dt.Rows[4][0]);
            Assert.AreEqual("Databases", dt.Rows[5][0]);
            Assert.AreEqual("Tables", dt.Rows[6][0]);
            Assert.AreEqual("Columns", dt.Rows[7][0]);
            Assert.AreEqual("Users", dt.Rows[8][0]);
            Assert.AreEqual("Foreign Keys", dt.Rows[9][0]);
            Assert.AreEqual("IndexColumns", dt.Rows[10][0]);
            Assert.AreEqual("Indexes", dt.Rows[11][0]);

            if (version >= new Version(5, 0))
            {
                Assert.AreEqual("Views", dt.Rows[12][0]);
                Assert.AreEqual("ViewColumns", dt.Rows[13][0]);
                Assert.AreEqual("Procedure Parameters", dt.Rows[14][0]);
                Assert.AreEqual("Procedures", dt.Rows[15][0]);
                Assert.AreEqual("Triggers", dt.Rows[16][0]);
            }
        }

        [Test]
        public void Columns()
        {
            execSQL("DROP TABLE IF EXISTS test");
            execSQL("CREATE TABLE test (col1 int, col2 decimal(20,5), " +
                    "col3 varchar(50) character set utf8, col4 tinyint unsigned)");

            var restrictions = new string[4];
            restrictions[1] = database0;
            restrictions[2] = "test";
            DataTable dt = conn.GetSchema("Columns", restrictions);
            Assert.AreEqual(4, dt.Rows.Count);
            Assert.AreEqual("Columns", dt.TableName);

            // first column
            Assert.AreEqual(database0.ToUpper(), dt.Rows[0]["TABLE_SCHEMA"].ToString().ToUpper());
            Assert.AreEqual("COL1", dt.Rows[0]["COLUMN_NAME"].ToString().ToUpper());
            Assert.AreEqual(1, dt.Rows[0]["ORDINAL_POSITION"]);
            Assert.AreEqual("YES", dt.Rows[0]["IS_NULLABLE"]);
            Assert.AreEqual("INT", dt.Rows[0]["DATA_TYPE"].ToString().ToUpper());

            // second column
            Assert.AreEqual(database0.ToUpper(), dt.Rows[1]["TABLE_SCHEMA"].ToString().ToUpper());
            Assert.AreEqual("COL2", dt.Rows[1]["COLUMN_NAME"].ToString().ToUpper());
            Assert.AreEqual(2, dt.Rows[1]["ORDINAL_POSITION"]);
            Assert.AreEqual("YES", dt.Rows[1]["IS_NULLABLE"]);
            Assert.AreEqual("DECIMAL", dt.Rows[1]["DATA_TYPE"].ToString().ToUpper());
            Assert.AreEqual("DECIMAL(20,5)", dt.Rows[1]["COLUMN_TYPE"].ToString().ToUpper());
            Assert.AreEqual(20, dt.Rows[1]["NUMERIC_PRECISION"]);
            Assert.AreEqual(5, dt.Rows[1]["NUMERIC_SCALE"]);

            // third column
            Assert.AreEqual(database0.ToUpper(), dt.Rows[2]["TABLE_SCHEMA"].ToString().ToUpper());
            Assert.AreEqual("COL3", dt.Rows[2]["COLUMN_NAME"].ToString().ToUpper());
            Assert.AreEqual(3, dt.Rows[2]["ORDINAL_POSITION"]);
            Assert.AreEqual("YES", dt.Rows[2]["IS_NULLABLE"]);
            Assert.AreEqual("VARCHAR", dt.Rows[2]["DATA_TYPE"].ToString().ToUpper());
            Assert.AreEqual("VARCHAR(50)", dt.Rows[2]["COLUMN_TYPE"].ToString().ToUpper());

            // fourth column
            Assert.AreEqual(database0.ToUpper(), dt.Rows[3]["TABLE_SCHEMA"].ToString().ToUpper());
            Assert.AreEqual("COL4", dt.Rows[3]["COLUMN_NAME"].ToString().ToUpper());
            Assert.AreEqual(4, dt.Rows[3]["ORDINAL_POSITION"]);
            Assert.AreEqual("YES", dt.Rows[3]["IS_NULLABLE"]);
            Assert.AreEqual("TINYINT", dt.Rows[3]["DATA_TYPE"].ToString().ToUpper());
        }

        [Test]
        public void Databases()
        {
            DataTable dt = conn.GetSchema("Databases");
            Assert.AreEqual("Databases", dt.TableName);

            bool foundZero = false;
            bool foundOne = false;
            foreach (DataRow row in dt.Rows)
            {
                string dbName = row[1].ToString().ToLower();
                if (dbName == database0.ToLower())
                    foundZero = true;
                else if (dbName == database1.ToLower())
                    foundOne = true;
            }
            Assert.IsTrue(foundZero);
            Assert.IsTrue(foundOne);

            dt = conn.GetSchema("Databases", new string[] { database0 });
            Assert.AreEqual(1, dt.Rows.Count);
            Assert.AreEqual(database0.ToLower(), dt.Rows[0][1].ToString().ToLower());
        }

        /// <summary>
        /// Bug #25907 DataType Column of DataTypes collection does'nt contain the correct CLR Datatype 
        /// Bug #25947 CreateFormat/CreateParameters Column of DataTypes collection incorrect for CHAR 
        /// </summary>
        [Test]
        public void DataTypes()
        {
            try
            {
                DataTable dt = conn.GetSchema("DataTypes", new string[] { });

                foreach (DataRow row in dt.Rows)
                {
                    string type = row["TYPENAME"].ToString();
                    Type systemType = Type.GetType(row["DATATYPE"].ToString());
                    if (type == "BIT")
                        Assert.AreEqual(typeof(UInt64), systemType);
                    else if (type == "DATE" || type == "DATETIME" || type == "TIMESTAMP")
                        Assert.AreEqual(typeof(DateTime), systemType);
                    else if (type == "BLOB" || type == "TINYBLOB" || type == "MEDIUMBLOB" || type == "LONGBLOB")
                        Assert.AreEqual(typeof(Byte[]), systemType);
                    else if (type == "TIME")
                        Assert.AreEqual(typeof(TimeSpan), systemType);
                    else if (type == "CHAR" || type == "VARCHAR")
                    {
                        Assert.AreEqual(typeof(String), systemType);
                        Assert.IsFalse(Convert.ToBoolean(row["IsFixedLength"]));
                        string format = type + "({0})";
                        Assert.AreEqual(format, row["CreateFormat"].ToString());
                    }
                    else if (type == "SET" || type == "ENUM")
                        Assert.AreEqual(typeof(String), systemType);
                    else if (type == "DOUBLE")
                        Assert.AreEqual(typeof(Double), systemType);
                    else if (type == "SINGLE")
                        Assert.AreEqual(typeof(Single), systemType);
                    else if (type == "TINYINT")
                    {
                        if (row["CREATEFORMAT"].ToString().EndsWith("UNSIGNED"))
                            Assert.AreEqual(typeof(Byte), systemType);
                        else
                            Assert.AreEqual(typeof(SByte), systemType);
                    }
                    else if (type == "SMALLINT")
                    {
                        if (row["CREATEFORMAT"].ToString().EndsWith("UNSIGNED"))
                            Assert.AreEqual(typeof(UInt16), systemType);
                        else
                            Assert.AreEqual(typeof(Int16), systemType);
                    }
                    else if (type == "MEDIUMINT" || type == "INT")
                    {
                        if (row["CREATEFORMAT"].ToString().EndsWith("UNSIGNED"))
                            Assert.AreEqual(typeof(UInt32), systemType);
                        else
                            Assert.AreEqual(typeof(Int32), systemType);
                    }
                    else if (type == "BIGINT")
                    {
                        if (row["CREATEFORMAT"].ToString().EndsWith("UNSIGNED"))
                            Assert.AreEqual(typeof(UInt64), systemType);
                        else
                            Assert.AreEqual(typeof(Int64), systemType);
                    }
                    else if (type == "DECIMAL")
                    {
                        Assert.AreEqual(typeof(Decimal), systemType);
                        Assert.AreEqual("DECIMAL({0},{1})", row["CreateFormat"].ToString());
                    }
                    else if (type == "TINYINT")
                        Assert.AreEqual(typeof(Byte), systemType);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Bug #26660 MySqlConnection.GetSchema fails with NullReferenceException for Foreign Keys 
        /// </summary>
        [Test]
        public void ForeignKeys()
        {
            execSQL("DROP TABLE IF EXISTS product_order");
            execSQL("DROP TABLE IF EXISTS product");
            execSQL("DROP TABLE IF EXISTS customer");
            execSQL("CREATE TABLE product (category INT NOT NULL, id INT NOT NULL, " +
                    "price DECIMAL, PRIMARY KEY(category, id)) TYPE=INNODB");
            execSQL("CREATE TABLE customer (id INT NOT NULL, PRIMARY KEY (id)) TYPE=INNODB");
            execSQL("CREATE TABLE product_order (no INT NOT NULL AUTO_INCREMENT, " +
                    "product_category INT NOT NULL, product_id INT NOT NULL, customer_id INT NOT NULL, " +
                    "PRIMARY KEY(no), INDEX (product_category, product_id), " +
                    "FOREIGN KEY (product_category, product_id) REFERENCES product(category, id) " +
                    "ON UPDATE CASCADE ON DELETE RESTRICT, INDEX (customer_id), " +
                    "FOREIGN KEY (customer_id) REFERENCES customer(id)) TYPE=INNODB");

            try
            {
                conn.GetSchema("Foreign Keys");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void Functions()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("DROP FUNCTION IF EXISTS spFunc");
            execSQL("CREATE FUNCTION spFunc (id int) RETURNS INT BEGIN RETURN 1; END");

            var restrictions = new string[4];
            restrictions[1] = database0;
            restrictions[2] = "spFunc";
            DataTable dt = conn.GetSchema("Procedures", restrictions);
            Assert.IsTrue(dt.Rows.Count == 1);
            Assert.AreEqual("Procedures", dt.TableName);
            Assert.AreEqual("spFunc", dt.Rows[0][3]);
        }

        [Test]
        public void IndexColumns()
        {
            execSQL("DROP TABLE IF EXISTS test");
            execSQL("CREATE TABLE test (id int, PRIMARY KEY(id))");
            var restrictions = new string[5];
            restrictions[2] = "test";
            restrictions[1] = database0;
            DataTable dt = conn.GetSchema("IndexColumns", restrictions);
            Assert.AreEqual(1, dt.Rows.Count);
            Assert.AreEqual("test", dt.Rows[0]["TABLE_NAME"]);
            Assert.AreEqual("id", dt.Rows[0]["COLUMN_NAME"]);

            execSQL("DROP TABLE IF EXISTS test");
            execSQL("CREATE TABLE test (id int, id1 int, id2 int, " + "INDEX key1 (id1, id2))");
            restrictions[2] = "test";
            restrictions[1] = database0;
            restrictions[4] = "id2";
            dt = conn.GetSchema("IndexColumns", restrictions);
            Assert.AreEqual(1, dt.Rows.Count);
            Assert.AreEqual("test", dt.Rows[0]["TABLE_NAME"]);
            Assert.AreEqual("id2", dt.Rows[0]["COLUMN_NAME"]);
            Assert.AreEqual(2, dt.Rows[0]["ORDINAL_POSITION"]);

            restrictions = new string[3];
            restrictions[1] = database0;
            restrictions[2] = "test";
            dt = conn.GetSchema("IndexColumns", restrictions);
            Assert.AreEqual(2, dt.Rows.Count);
            Assert.AreEqual("test", dt.Rows[0]["TABLE_NAME"]);
            Assert.AreEqual("id1", dt.Rows[0]["COLUMN_NAME"]);
            Assert.AreEqual(1, dt.Rows[0]["ORDINAL_POSITION"]);
            Assert.AreEqual("test", dt.Rows[1]["TABLE_NAME"]);
            Assert.AreEqual("id2", dt.Rows[1]["COLUMN_NAME"]);
            Assert.AreEqual(2, dt.Rows[1]["ORDINAL_POSITION"]);
        }

        [Test]
        public void Indexes()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("DROP TABLE IF EXISTS test");
            execSQL("CREATE TABLE test (id int, PRIMARY KEY(id))");
            var restrictions = new string[4];
            restrictions[2] = "test";
            restrictions[1] = database0;
            DataTable dt = conn.GetSchema("Indexes", restrictions);
            Assert.AreEqual(1, dt.Rows.Count);
            Assert.AreEqual("test", dt.Rows[0]["TABLE_NAME"]);
            Assert.AreEqual(true, dt.Rows[0]["PRIMARY"]);
            Assert.AreEqual(true, dt.Rows[0]["UNIQUE"]);

            execSQL("DROP TABLE IF EXISTS test");
            execSQL("CREATE TABLE test (id int, name varchar(50), " + "UNIQUE KEY key2 (name))");

            dt = conn.GetSchema("Indexes", restrictions);
            Assert.AreEqual(1, dt.Rows.Count);
            Assert.AreEqual("test", dt.Rows[0]["TABLE_NAME"]);
            Assert.AreEqual("key2", dt.Rows[0]["INDEX_NAME"]);
            Assert.AreEqual(false, dt.Rows[0]["PRIMARY"]);
            Assert.AreEqual(true, dt.Rows[0]["UNIQUE"]);

            execSQL("DROP TABLE IF EXISTS test");
            execSQL("CREATE TABLE test (id int, name varchar(50), " + "KEY key2 (name))");

            dt = conn.GetSchema("Indexes", restrictions);
            Assert.AreEqual(1, dt.Rows.Count);
            Assert.AreEqual("test", dt.Rows[0]["TABLE_NAME"]);
            Assert.AreEqual("key2", dt.Rows[0]["INDEX_NAME"]);
            Assert.AreEqual(false, dt.Rows[0]["PRIMARY"]);
            Assert.AreEqual(false, dt.Rows[0]["UNIQUE"]);
        }

        [Test]
        public void MultiSingleForeignKey()
        {
            execSQL("DROP TABLE IF EXISTS product_order");
            execSQL("DROP TABLE IF EXISTS product");
            execSQL("DROP TABLE IF EXISTS customer");
            execSQL("CREATE TABLE product (category INT NOT NULL, id INT NOT NULL, " +
                    "price DECIMAL, PRIMARY KEY(category, id)) TYPE=INNODB");
            execSQL("CREATE TABLE customer (id INT NOT NULL, PRIMARY KEY (id)) TYPE=INNODB");
            execSQL("CREATE TABLE product_order (no INT NOT NULL AUTO_INCREMENT, " +
                    "product_category INT NOT NULL, product_id INT NOT NULL, customer_id INT NOT NULL, " +
                    "PRIMARY KEY(no), INDEX (product_category, product_id), " +
                    "FOREIGN KEY (product_category, product_id) REFERENCES product(category, id) " +
                    "ON UPDATE CASCADE ON DELETE RESTRICT, INDEX (customer_id), " +
                    "FOREIGN KEY (customer_id) REFERENCES customer(id)) TYPE=INNODB");

            var restrictions = new string[4];
            restrictions[0] = null;
            restrictions[1] = database0;
            restrictions[2] = "product_order";
            DataTable dt = conn.GetSchema("Foreign Keys", restrictions);
            Assert.AreEqual(2, dt.Rows.Count);
            DataRow row = dt.Rows[0];
            Assert.AreEqual(DBNull.Value, row["CONSTRAINT_CATALOG"]);
            Assert.AreEqual(database0.ToLower(), row["CONSTRAINT_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("product_order_ibfk_1", row["CONSTRAINT_NAME"]);
            Assert.AreEqual(DBNull.Value, row["TABLE_CATALOG"]);
            Assert.AreEqual(database0.ToLower(), row["TABLE_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("product_order", row["TABLE_NAME"]);
            Assert.AreEqual(database0.ToLower(), row["REFERENCED_TABLE_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("product", row["REFERENCED_TABLE_NAME"]);

            row = dt.Rows[1];
            Assert.AreEqual(DBNull.Value, row["CONSTRAINT_CATALOG"]);
            Assert.AreEqual(database0.ToLower(), row["CONSTRAINT_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("product_order_ibfk_2", row["CONSTRAINT_NAME"]);
            Assert.AreEqual(DBNull.Value, row["TABLE_CATALOG"]);
            Assert.AreEqual(database0.ToLower(), row["TABLE_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("product_order", row["TABLE_NAME"]);
            Assert.AreEqual(database0.ToLower(), row["REFERENCED_TABLE_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("customer", row["REFERENCED_TABLE_NAME"]);
        }

        [Test]
        public void ProcedureParameters()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("DROP PROCEDURE IF EXISTS spTest");
            execSQL("CREATE PROCEDURE spTest (id int, name varchar(50)) BEGIN SELECT 1; END");

            var restrictions = new string[5];
            restrictions[1] = database0;
            restrictions[2] = "spTest";
            DataTable dt = conn.GetSchema("Procedure Parameters", restrictions);
            Assert.IsTrue(dt.Rows.Count == 2);
            Assert.AreEqual("Procedure Parameters", dt.TableName);
            Assert.AreEqual(database0.ToLower(), dt.Rows[0]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("sptest", dt.Rows[0]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual("id", dt.Rows[0]["PARAMETER_NAME"].ToString().ToLower());
            Assert.AreEqual(1, dt.Rows[0]["ORDINAL_POSITION"]);
            Assert.AreEqual("IN", dt.Rows[0]["PARAMETER_MODE"]);

            restrictions[4] = "name";
            dt.Clear();
            dt = conn.GetSchema("Procedure Parameters", restrictions);
            Assert.AreEqual(1, dt.Rows.Count);
            Assert.AreEqual(database0.ToLower(), dt.Rows[0]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("sptest", dt.Rows[0]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual("name", dt.Rows[0]["PARAMETER_NAME"].ToString().ToLower());
            Assert.AreEqual(2, dt.Rows[0]["ORDINAL_POSITION"]);
            Assert.AreEqual("IN", dt.Rows[0]["PARAMETER_MODE"]);

            execSQL("DROP FUNCTION IF EXISTS spFunc");
            execSQL("CREATE FUNCTION spFunc (id int) RETURNS INT BEGIN RETURN 1; END");

            restrictions[4] = null;
            restrictions[1] = database0;
            restrictions[2] = "spFunc";
            dt = conn.GetSchema("Procedure Parameters", restrictions);
            Assert.IsTrue(dt.Rows.Count == 2);
            Assert.AreEqual("Procedure Parameters", dt.TableName);
            Assert.AreEqual(database0.ToLower(), dt.Rows[0]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("spfunc", dt.Rows[0]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual(0, dt.Rows[0]["ORDINAL_POSITION"]);

            Assert.AreEqual(database0.ToLower(), dt.Rows[1]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("spfunc", dt.Rows[1]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual("id", dt.Rows[1]["PARAMETER_NAME"].ToString().ToLower());
            Assert.AreEqual(1, dt.Rows[1]["ORDINAL_POSITION"]);
            Assert.AreEqual("IN", dt.Rows[1]["PARAMETER_MODE"]);
        }

        [Test]
        public void Procedures()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("DROP PROCEDURE IF EXISTS spTest");
            execSQL("CREATE PROCEDURE spTest (id int) BEGIN SELECT 1; END");

            var restrictions = new string[4];
            restrictions[1] = database0;
            restrictions[2] = "spTest";
            DataTable dt = conn.GetSchema("Procedures", restrictions);
            Assert.IsTrue(dt.Rows.Count == 1);
            Assert.AreEqual("Procedures", dt.TableName);
            Assert.AreEqual("spTest", dt.Rows[0][3]);
        }

        [Test]
        public void SingleForeignKey()
        {
            execSQL("DROP TABLE IF EXISTS child");
            execSQL("DROP TABLE IF EXISTS parent");
            execSQL("CREATE TABLE parent (id INT NOT NULL, PRIMARY KEY (id)) TYPE=INNODB");
            execSQL("CREATE TABLE child (id INT, parent_id INT, INDEX par_ind (parent_id), " +
                    "CONSTRAINT c1 FOREIGN KEY (parent_id) REFERENCES parent(id) ON DELETE CASCADE) TYPE=INNODB");
            var restrictions = new string[4];
            restrictions[0] = null;
            restrictions[1] = database0;
            restrictions[2] = "child";
            DataTable dt = conn.GetSchema("Foreign Keys", restrictions);
            Assert.AreEqual(1, dt.Rows.Count);
            DataRow row = dt.Rows[0];
            Assert.AreEqual(DBNull.Value, row["CONSTRAINT_CATALOG"]);
            Assert.AreEqual(database0.ToLower(), row["CONSTRAINT_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("c1", row["CONSTRAINT_NAME"]);
            Assert.AreEqual(DBNull.Value, row["TABLE_CATALOG"]);
            Assert.AreEqual(database0.ToLower(), row["TABLE_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("child", row["TABLE_NAME"]);
            Assert.AreEqual(database0.ToLower(), row["REFERENCED_TABLE_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("parent", row["REFERENCED_TABLE_NAME"]);
        }

        [Test]
        public void SingleProcedureParameters()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("DROP PROCEDURE IF EXISTS spTest");
            execSQL("CREATE PROCEDURE spTest(id int, IN id2 INT(11), " + "INOUT io1 VARCHAR(20), OUT out1 FLOAT) BEGIN END");
            var restrictions = new string[4];
            restrictions[1] = database0;
            restrictions[2] = "spTest";
            DataTable procs = conn.GetSchema("PROCEDURES", restrictions);
            Assert.AreEqual(1, procs.Rows.Count);
            Assert.AreEqual("spTest", procs.Rows[0][0]);
            Assert.AreEqual(database0.ToLower(), procs.Rows[0][2].ToString().ToLower());
            Assert.AreEqual("spTest", procs.Rows[0][3]);

            DataTable parameters = conn.GetSchema("PROCEDURE PARAMETERS", restrictions);
            Assert.AreEqual(4, parameters.Rows.Count);

            DataRow row = parameters.Rows[0];
            Assert.AreEqual(DBNull.Value, row["SPECIFIC_CATALOG"]);
            Assert.AreEqual(database0.ToLower(), row["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("spTest", row["SPECIFIC_NAME"]);
            Assert.AreEqual(1, row["ORDINAL_POSITION"]);
            Assert.AreEqual("IN", row["PARAMETER_MODE"]);
            Assert.AreEqual("id", row["PARAMETER_NAME"]);
            Assert.AreEqual("INT", row["DATA_TYPE"]);

            row = parameters.Rows[1];
            Assert.AreEqual(DBNull.Value, row["SPECIFIC_CATALOG"]);
            Assert.AreEqual(database0.ToLower(), row["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("spTest", row["SPECIFIC_NAME"]);
            Assert.AreEqual(2, row["ORDINAL_POSITION"]);
            Assert.AreEqual("IN", row["PARAMETER_MODE"]);
            Assert.AreEqual("id2", row["PARAMETER_NAME"]);
            Assert.AreEqual("INT", row["DATA_TYPE"]);

            row = parameters.Rows[2];
            Assert.AreEqual(DBNull.Value, row["SPECIFIC_CATALOG"]);
            Assert.AreEqual(database0.ToLower(), row["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("spTest", row["SPECIFIC_NAME"]);
            Assert.AreEqual(3, row["ORDINAL_POSITION"]);
            Assert.AreEqual("INOUT", row["PARAMETER_MODE"]);
            Assert.AreEqual("io1", row["PARAMETER_NAME"]);
            Assert.AreEqual("VARCHAR", row["DATA_TYPE"]);

            row = parameters.Rows[3];
            Assert.AreEqual(DBNull.Value, row["SPECIFIC_CATALOG"]);
            Assert.AreEqual(database0.ToLower(), row["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("spTest", row["SPECIFIC_NAME"]);
            Assert.AreEqual(4, row["ORDINAL_POSITION"]);
            Assert.AreEqual("OUT", row["PARAMETER_MODE"]);
            Assert.AreEqual("out1", row["PARAMETER_NAME"]);
            Assert.AreEqual("FLOAT", row["DATA_TYPE"]);
        }

        [Test]
        public void Tables()
        {
            execSQL("DROP TABLE IF EXISTS test1");
            execSQL("CREATE TABLE test1 (id int)");

            var restrictions = new string[4];
            restrictions[1] = database0;
            restrictions[2] = "test1";
            DataTable dt = conn.GetSchema("Tables", restrictions);
            Assert.IsTrue(dt.Rows.Count == 1);
            Assert.AreEqual("Tables", dt.TableName);
            Assert.AreEqual("test1", dt.Rows[0][2]);
        }

        [Test]
        public void Triggers()
        {
            if (version < new Version(5, 0))
                return;

            try
            {
                suExecSQL("DROP TRIGGER trigger1");
            }
// ReSharper disable EmptyGeneralCatchClause
            catch (Exception)
// ReSharper restore EmptyGeneralCatchClause
            {
            }

            execSQL("DROP TABLE IF EXISTS test2");
            execSQL("DROP TABLE IF EXISTS test1");
            execSQL("CREATE TABLE test1 (id int)");
            execSQL("CREATE TABLE test2 (count int)");
            execSQL("INSERT INTO test2 VALUES (0)");
            suExecSQL("CREATE TRIGGER trigger1 AFTER INSERT ON test1 FOR EACH ROW BEGIN " +
                      "UPDATE test2 SET count = count+1; END");

            var restrictions = new string[4];
            restrictions[1] = database0;
            restrictions[2] = "test1";
            DataTable dt = rootConn.GetSchema("Triggers", restrictions);
            Assert.IsTrue(dt.Rows.Count == 1);
            Assert.AreEqual("Triggers", dt.TableName);
            Assert.AreEqual("trigger1", dt.Rows[0]["TRIGGER_NAME"]);
            Assert.AreEqual("INSERT", dt.Rows[0]["EVENT_MANIPULATION"]);
            Assert.AreEqual("test1", dt.Rows[0]["EVENT_OBJECT_TABLE"]);
            Assert.AreEqual("ROW", dt.Rows[0]["ACTION_ORIENTATION"]);
            Assert.AreEqual("AFTER", dt.Rows[0]["ACTION_TIMING"]);
        }

        [Test]
        public void UsingQuotedRestrictions()
        {
            execSQL("DROP TABLE IF EXISTS test1");
            execSQL("CREATE TABLE test1 (id int)");

            var restrictions = new string[4];
            restrictions[1] = database0;
            restrictions[2] = "`test1`";
            DataTable dt = conn.GetSchema("Tables", restrictions);
            Assert.IsTrue(dt.Rows.Count == 1);
            Assert.AreEqual("Tables", dt.TableName);
            Assert.AreEqual("test1", dt.Rows[0][2]);
            Assert.AreEqual("`test1`", restrictions[2]);
        }

        [Test]
        public void ViewColumns()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("DROP VIEW IF EXISTS vw");
            execSQL("CREATE VIEW vw AS SELECT Now() as theTime");

            var restrictions = new string[4];
            restrictions[1] = database0;
            restrictions[2] = "vw";
            DataTable dt = conn.GetSchema("ViewColumns", restrictions);
            Assert.IsTrue(dt.Rows.Count == 1);
            Assert.AreEqual("ViewColumns", dt.TableName);
            Assert.AreEqual(database0.ToLower(), dt.Rows[0]["VIEW_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("vw", dt.Rows[0]["VIEW_NAME"]);
            Assert.AreEqual("theTime", dt.Rows[0]["COLUMN_NAME"]);
        }

        [Test]
        public void Views()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("DROP VIEW IF EXISTS vw");
            execSQL("CREATE VIEW vw AS SELECT Now() as theTime");

            var restrictions = new string[4];
            restrictions[1] = database0;
            restrictions[2] = "vw";
            DataTable dt = conn.GetSchema("Views", restrictions);
            Assert.IsTrue(dt.Rows.Count == 1);
            Assert.AreEqual("Views", dt.TableName);
            Assert.AreEqual("vw", dt.Rows[0]["TABLE_NAME"]);
        }
    }
}