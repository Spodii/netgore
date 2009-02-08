using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    [TestFixture]
    public class StoredProcedureAccess : StoredProcedure
    {
        #region Setup/Teardown

        public override void Setup()
        {
            base.Setup();
            SetAccountPerms(true);
        }

        #endregion

        protected override string GetConnectionInfo()
        {
            string info = base.GetConnectionInfo();
            info = info.Replace("use procedure bodies=false", "");
            return info;
        }
    }

    /// <summary>
    /// Summary description for StoredProcedure.
    /// </summary>
    [TestFixture]
    public class StoredProcedure : BaseTest
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (id INT, name VARCHAR(100))");
        }

        #endregion

        static string fillError = null;

        [TestFixtureSetUp]
        public override void FixtureSetup()
        {
            pooling = false;
            csAdditions = ";procedure cache size=0;";
            base.FixtureSetup();
        }

        void MultipleResultsetsImpl(bool prepare)
        {
            if (version < new Version(5, 0))
                return;

            // create our procedure
            suExecSQL("CREATE PROCEDURE spTest() " + "BEGIN  SELECT 1; SELECT 2; END");

            MySqlCommand cmd = new MySqlCommand("spTest", conn);
            if (prepare)
                cmd.Prepare();
            cmd.CommandType = CommandType.StoredProcedure;
            MySqlDataReader reader = cmd.ExecuteReader();
            Assert.AreEqual(true, reader.Read());
            Assert.AreEqual(true, reader.NextResult());
            Assert.AreEqual(true, reader.Read());
            Assert.AreEqual(false, reader.NextResult());
            Assert.AreEqual(false, reader.Read());
            reader.Close();

            DataSet ds = new DataSet();
            MySqlCommand cmd2 = new MySqlCommand("spTest", conn);
            cmd2.CommandType = CommandType.StoredProcedure;
            MySqlDataAdapter da = new MySqlDataAdapter(cmd2);
            da.FillError += da_FillError;
            fillError = null;
            da.Fill(ds);
            Assert.AreEqual(2, ds.Tables.Count);
            Assert.AreEqual(1, ds.Tables[0].Rows.Count);
            Assert.AreEqual(1, ds.Tables[1].Rows.Count);
            Assert.AreEqual(1, ds.Tables[0].Rows[0][0]);
            Assert.AreEqual(2, ds.Tables[1].Rows[0][0]);
            Assert.IsNull(fillError);
        }

        static void da_FillError(object sender, FillErrorEventArgs e)
        {
            fillError = e.Errors.Message;
            e.Continue = true;
        }

        [Test]
        public void AmbiguousColumns()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("CREATE TABLE t1 (id INT)");
            execSQL("CREATE TABLE t2 (id1 INT, id INT)");
            execSQL(
                @"CREATE PROCEDURE spTest() BEGIN SELECT * FROM t1; 
                        SELECT id FROM t1 JOIN t2 on t1.id=t2.id; 
                        SELECT * FROM t2; END");

            MySqlCommand cmd = new MySqlCommand("spTest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
                Assert.Fail("The above should have thrown an exception");
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void BinaryAndVarBinaryParameters()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("DROP PROCEDURE IF EXISTS spTest");
            execSQL("CREATE PROCEDURE spTest(OUT out1 BINARY(20), OUT out2 VARBINARY(20)) " +
                    "BEGIN SET out1 = 'out1'; SET out2='out2'; END");

            MySqlCommand cmd = new MySqlCommand("spTest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("out1", MySqlDbType.Binary);
            cmd.Parameters[0].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("out2", MySqlDbType.VarBinary);
            cmd.Parameters[1].Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();

            var out1 = (byte[])cmd.Parameters[0].Value;
            Assert.AreEqual('o', out1[0]);
            Assert.AreEqual('u', out1[1]);
            Assert.AreEqual('t', out1[2]);
            Assert.AreEqual('1', out1[3]);

            out1 = (byte[])cmd.Parameters[1].Value;
            Assert.AreEqual('o', out1[0]);
            Assert.AreEqual('u', out1[1]);
            Assert.AreEqual('t', out1[2]);
            Assert.AreEqual('2', out1[3]);
        }

        /// <summary>
        /// Bug #16788 Only byte arrays and strings can be serialized by MySqlBinary 
        /// </summary>
        [Test]
        public void Bug16788()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (id integer(9), state varchar(2))");
            execSQL("CREATE PROCEDURE spTest(IN p1 integer(9), IN p2 varchar(2)) " + "BEGIN " +
                    "INSERT INTO Test (id, state) VALUES (p1, p2); " + "END");

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spTest";
            cmd.Parameters.Add("?p1", MySqlDbType.UInt16, 9);
            cmd.Parameters["?p1"].Value = 44;
            cmd.Parameters.Add("?p2", MySqlDbType.VarChar, 2);
            cmd.Parameters["?p2"].Value = "ss";
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Bug #20581 Null Reference Exception when closing reader after stored procedure. 
        /// </summary>
        [Test]
        public void Bug20581()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("CREATE PROCEDURE spTest(p int) BEGIN SELECT p; END");
            MySqlParameter param1;
            MySqlCommand command = new MySqlCommand("spTest", conn);
            command.CommandType = CommandType.StoredProcedure;

            param1 = command.Parameters.Add("?p", MySqlDbType.Int32);
            param1.Value = 3;

            MySqlDataReader reader = null;
            try
            {
                command.Prepare();
                reader = command.ExecuteReader();
                reader.Read();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        /// Bug #31930 Stored procedures with "ambiguous column name" error cause lock-ups 
        /// </summary>
        [Test]
        public void CallingFunction()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("DROP FUNCTION IF EXISTS spFunc");
            execSQL(
                @"CREATE FUNCTION `GetSupplierBalance`(SupplierID_ INTEGER(11))
                RETURNS double NOT DETERMINISTIC CONTAINS SQL SQL SECURITY DEFINER
                COMMENT '' 
                BEGIN
                    RETURN 1.0;
                END");

            MySqlCommand command = new MySqlCommand("GetSupplierBalance", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("?SupplierID_", MySqlDbType.Int32).Value = 1;
            command.Parameters.Add("?Balance", MySqlDbType.Double).Direction = ParameterDirection.ReturnValue;
            command.ExecuteNonQuery();
            double balance = Convert.ToDouble(command.Parameters["?Balance"].Value);
            Assert.AreEqual(1.0, balance);
        }

        [Test]
        public void CallingFunctionWithoutReturnParameter()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("CREATE FUNCTION fnTest (p_kiosk bigint(20), " +
                    "p_user bigint(20)) returns double begin declare v_return double; " +
                    "set v_return = 3.6; return v_return; end");

            MySqlCommand cmd = new MySqlCommand("fnTest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("?p_kiosk", 2);
            cmd.Parameters.AddWithValue("?p_user", 4);
            try
            {
                cmd.ExecuteNonQuery();
                Assert.AreEqual(2, cmd.Parameters.Count);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void CallingSPWithPrepare()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("DROP PROCEDURE IF EXISTS spTest");
            execSQL("CREATE PROCEDURE spTest(P int) BEGIN SELECT P; END");

            MySqlCommand cmd = new MySqlCommand("spTest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("?P", 33);
            cmd.Prepare();

            int p = (int)cmd.ExecuteScalar();
            Assert.AreEqual(33, p);
        }

        /// <summary>
        /// Bug #10644 Cannot call a stored function directly from Connector/Net 
        /// Bug #25013 Return Value parameter not found 
        /// </summary>
        [Test]
        public void CallingStoredFunctionasProcedure()
        {
            if (version < new Version(5, 0))
                return;

            suExecSQL("CREATE FUNCTION fnTest(valin int) RETURNS INT " + " LANGUAGE SQL DETERMINISTIC BEGIN return valin * 2; END");
            MySqlCommand cmd = new MySqlCommand("fnTest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("?valin", 22);
            MySqlParameter retVal = cmd.CreateParameter();
            retVal.ParameterName = "?retval";
            retVal.MySqlDbType = MySqlDbType.Int32;
            retVal.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retVal);
            cmd.ExecuteNonQuery();
            Assert.AreEqual(44, cmd.Parameters[1].Value);
        }

        /// <summary>
        /// Bug #29526  	syntax error: "show create procedure" with catalog names containing hyphens
        /// </summary>
        [Test]
        public void CatalogWithHyphens()
        {
            if (version < new Version(5, 0))
                return;

            // make sure this test is valid
            Assert.IsTrue(database0.IndexOf('-') != -1);

            MySqlCommand cmd = new MySqlCommand("CREATE PROCEDURE spTest() BEGIN SELECT 1; END", conn);
            cmd.ExecuteNonQuery();

            cmd.CommandText = "spTest";
            cmd.CommandType = CommandType.StoredProcedure;
            Assert.AreEqual(1, cmd.ExecuteScalar());
        }

        [Test]
        public void CheckNameOfReturnParameter()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("CREATE FUNCTION fnTest() RETURNS CHAR(50)" + " LANGUAGE SQL DETERMINISTIC BEGIN  RETURN \"Test\"; END");

            MySqlCommand cmd = new MySqlCommand("fnTest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            MySqlCommandBuilder.DeriveParameters(cmd);
            Assert.AreEqual(1, cmd.Parameters.Count);
            Assert.AreEqual("@RETURN_VALUE", cmd.Parameters[0].ParameterName);
        }

        [Test]
        public void ComplexDefinition()
        {
            if (version < new Version(5, 0))
                return;

            execSQL(
                @"CREATE PROCEDURE `spTest`() NOT DETERMINISTIC
					CONTAINS SQL SQL SECURITY DEFINER COMMENT '' 
					BEGIN
						SELECT 1,2,3;
					END");
            MySqlCommand command = new MySqlCommand("spTest", conn);
            command.CommandType = CommandType.StoredProcedure;
            using (MySqlDataReader reader = command.ExecuteReader())
            {
            }
        }

        /// <summary>
        /// Bug #11386  	Numeric parameters with Precision and Scale not taken into account by Connector
        /// </summary>
        [Test]
        public void DecimalAsParameter()
        {
            if (version < new Version(5, 0))
                return;

            suExecSQL("CREATE PROCEDURE spTest(IN d DECIMAL(19,4)) BEGIN SELECT d; END");

            MySqlCommand cmd = new MySqlCommand("spTest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("?d", 21);
            decimal d = (decimal)cmd.ExecuteScalar();
            Assert.AreEqual(21, d);
        }

        /// <summary>
        /// Bug #13590  	ExecuteScalar returns only Int64 regardless of actual SQL type
        /// </summary>
        /*		[Test]
                public void TestSelectingInts()
                {
                    execSQL("CREATE PROCEDURE spTest() BEGIN DECLARE myVar INT; " +
                        "SET MyVar := 1; SELECT CAST(myVar as SIGNED); END");

                    MySqlCommand cmd = new MySqlCommand("spTest", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    object val = cmd.ExecuteScalar();
                    Assert.AreEqual(1, val, "Checking value");
                    Assert.IsTrue(val is Int32, "Checking type");
                }
        */
        /// <summary>
        /// Bug #13632  	the MySQLCommandBuilder.deriveparameters has not been updated for MySQL 5
        /// Bug #15077  	Error MySqlCommandBuilder.DeriveParameters for sp without parameters.
        /// Bug #19515  	DiscoverParameters fails on numeric datatype
        /// </summary>
        [Test]
        public void DeriveParameters()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("DROP TABLE IF EXISTS test2");
            execSQL("CREATE TABLE test2 (c CHAR(20))");
            execSQL("INSERT INTO test2 values ( 'xxxx')");
            MySqlCommand cmd2 = new MySqlCommand("SELECT * FROM test2", conn);
            MySqlDataReader reader = cmd2.ExecuteReader();
            reader.Close();

            execSQL("CREATE PROCEDURE spTest(IN \r\nvalin DECIMAL(10,2), " +
                    "\nIN val2 INT, INOUT val3 FLOAT, OUT val4 DOUBLE, INOUT val5 BIT, " +
                    "val6 VARCHAR(155), val7 SET('a','b'), val8 CHAR, val9 NUMERIC(10,2)) " + "BEGIN SELECT 1; END");

            MySqlCommand cmd = new MySqlCommand("spTest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            MySqlCommandBuilder.DeriveParameters(cmd);

            Assert.AreEqual(9, cmd.Parameters.Count);
            Assert.AreEqual("@valin", cmd.Parameters[0].ParameterName);
            Assert.AreEqual(ParameterDirection.Input, cmd.Parameters[0].Direction);
            Assert.AreEqual(MySqlDbType.NewDecimal, cmd.Parameters[0].MySqlDbType);

            Assert.AreEqual("@val2", cmd.Parameters[1].ParameterName);
            Assert.AreEqual(ParameterDirection.Input, cmd.Parameters[1].Direction);
            Assert.AreEqual(MySqlDbType.Int32, cmd.Parameters[1].MySqlDbType);

            Assert.AreEqual("@val3", cmd.Parameters[2].ParameterName);
            Assert.AreEqual(ParameterDirection.InputOutput, cmd.Parameters[2].Direction);
            Assert.AreEqual(MySqlDbType.Float, cmd.Parameters[2].MySqlDbType);

            Assert.AreEqual("@val4", cmd.Parameters[3].ParameterName);
            Assert.AreEqual(ParameterDirection.Output, cmd.Parameters[3].Direction);
            Assert.AreEqual(MySqlDbType.Double, cmd.Parameters[3].MySqlDbType);

            Assert.AreEqual("@val5", cmd.Parameters[4].ParameterName);
            Assert.AreEqual(ParameterDirection.InputOutput, cmd.Parameters[4].Direction);
            Assert.AreEqual(MySqlDbType.Bit, cmd.Parameters[4].MySqlDbType);

            Assert.AreEqual("@val6", cmd.Parameters[5].ParameterName);
            Assert.AreEqual(ParameterDirection.Input, cmd.Parameters[5].Direction);
            Assert.AreEqual(MySqlDbType.VarChar, cmd.Parameters[5].MySqlDbType);

            Assert.AreEqual("@val7", cmd.Parameters[6].ParameterName);
            Assert.AreEqual(ParameterDirection.Input, cmd.Parameters[6].Direction);
            Assert.AreEqual(MySqlDbType.Set, cmd.Parameters[6].MySqlDbType);

            Assert.AreEqual("@val8", cmd.Parameters[7].ParameterName);
            Assert.AreEqual(ParameterDirection.Input, cmd.Parameters[7].Direction);
            Assert.AreEqual(MySqlDbType.String, cmd.Parameters[7].MySqlDbType);

            Assert.AreEqual("@val9", cmd.Parameters[8].ParameterName);
            Assert.AreEqual(ParameterDirection.Input, cmd.Parameters[8].Direction);
            Assert.AreEqual(MySqlDbType.NewDecimal, cmd.Parameters[8].MySqlDbType);

            execSQL("DROP PROCEDURE spTest");
            execSQL("CREATE PROCEDURE spTest() BEGIN END");
            cmd.CommandText = "spTest";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            MySqlCommandBuilder.DeriveParameters(cmd);
            Assert.AreEqual(0, cmd.Parameters.Count);
        }

        /// <summary>
        /// Bug #13632  	the MySQLCommandBuilder.deriveparameters has not been updated for MySQL 5
        /// </summary>
        [Test]
        public void DeriveParametersForFunction()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("CREATE FUNCTION fnTest(v1 DATETIME) RETURNS INT " + "  LANGUAGE SQL DETERMINISTIC BEGIN RETURN 1; END");

            MySqlCommand cmd = new MySqlCommand("fnTest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            MySqlCommandBuilder.DeriveParameters(cmd);

            Assert.AreEqual(2, cmd.Parameters.Count);
            Assert.AreEqual("@v1", cmd.Parameters[1].ParameterName);
            Assert.AreEqual(ParameterDirection.Input, cmd.Parameters[1].Direction);
            Assert.AreEqual(MySqlDbType.DateTime, cmd.Parameters[1].MySqlDbType);

            Assert.AreEqual(ParameterDirection.ReturnValue, cmd.Parameters[0].Direction);
            Assert.AreEqual(MySqlDbType.Int32, cmd.Parameters[0].MySqlDbType);
        }

        [Test]
        public void ExecuteReader()
        {
            if (version < new Version(5, 0))
                return;

            // create our procedure
            suExecSQL("CREATE PROCEDURE spTest(OUT p INT) " + "BEGIN SELECT 1; SET p=2; END");

            MySqlCommand cmd = new MySqlCommand("spTest", conn);
            cmd.Parameters.Add("?p", MySqlDbType.Int32);
            cmd.Parameters[0].Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                Assert.AreEqual(true, reader.Read());
                Assert.AreEqual(false, reader.NextResult());
                Assert.AreEqual(false, reader.Read());
            }
            Assert.AreEqual(2, cmd.Parameters[0].Value);
        }

        [Test]
        public void ExecuteScalar()
        {
            if (version < new Version(5, 0))
                return;

            // create our procedure
            suExecSQL("CREATE PROCEDURE spTest( IN valin VARCHAR(50), OUT valout VARCHAR(50) ) " +
                      "BEGIN  SET valout=valin;  SELECT 'Test'; END");

            MySqlCommand cmd = new MySqlCommand("spTest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("?valin", "valuein");
            cmd.Parameters.Add(new MySqlParameter("?valout", MySqlDbType.VarChar));
            cmd.Parameters[1].Direction = ParameterDirection.Output;
            object result = cmd.ExecuteScalar();
            Assert.AreEqual("Test", result);
            Assert.AreEqual("valuein", cmd.Parameters[1].Value);
        }

        /// <summary>
        /// Bug #13590  	ExecuteScalar returns only Int64 regardless of actual SQL type
        /// </summary>
        [Test]
        public void ExecuteScalar2()
        {
            if (version < new Version(5, 0))
                return;

            // create our procedure
            suExecSQL("CREATE PROCEDURE spTest() " + "BEGIN  DECLARE myVar1 INT; SET myVar1 := 1; SELECT myVar1; END");

            MySqlCommand cmd = new MySqlCommand("spTest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            object result = cmd.ExecuteScalar();
            Assert.AreEqual(1, result);
            Assert.IsTrue(result is Int32);
        }

        [Test]
        public void ExecuteWithCreate()
        {
            if (version < new Version(5, 0))
                return;

            // create our procedure
            string sql = "CREATE PROCEDURE spTest(IN var INT) BEGIN  SELECT var; END; call spTest(?v)";

            MySqlCommand cmd = new MySqlCommand(sql, rootConn);
            cmd.Parameters.Add(new MySqlParameter("?v", 33));
            object val = cmd.ExecuteScalar();
            Assert.AreEqual(33, val);
        }

        [Test]
        public void FunctionNoParams()
        {
            if (version < new Version(5, 0))
                return;

            suExecSQL("CREATE FUNCTION fnTest() RETURNS CHAR(50)" + " LANGUAGE SQL DETERMINISTIC BEGIN  RETURN \"Test\"; END");

            MySqlCommand cmd = new MySqlCommand("SELECT fnTest()", conn);
            cmd.CommandType = CommandType.Text;
            object result = cmd.ExecuteScalar();
            Assert.AreEqual("Test", result);
        }

        [Test]
        public void FunctionParams()
        {
            if (version < new Version(5, 0))
                return;

            suExecSQL("CREATE FUNCTION fnTest( val1 INT, val2 CHAR(40) ) RETURNS INT " +
                      " LANGUAGE SQL DETERMINISTIC BEGIN  RETURN val1 + LENGTH(val2);  END");

            MySqlCommand cmd = new MySqlCommand("SELECT fnTest(22, 'Test')", conn);
            cmd.CommandType = CommandType.Text;
            object result = cmd.ExecuteScalar();
            Assert.AreEqual(26, result);
        }

        /// <summary>
        /// </summary>
        [Test]
        public void FunctionWithNewParamHandling()
        {
            if (version < new Version(5, 0))
                return;

            // create our procedure
            execSQL("CREATE FUNCTION spTest(`value` INT) RETURNS INT " + "BEGIN RETURN value; END");

            string connStr = GetConnectionString(true);
            connStr = connStr.Replace("allow user variables=true", "allow user variables=false");
            using (MySqlConnection c = new MySqlConnection(connStr))
            {
                c.Open();

                MySqlCommand cmd = new MySqlCommand("spTest", c);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@value", MySqlDbType.Int32)).Value = 22;
                cmd.Parameters.Add(new MySqlParameter("@returnvalue", MySqlDbType.Int32)).Direction =
                    ParameterDirection.ReturnValue;
                int rowsAffected = cmd.ExecuteNonQuery();

                Assert.AreEqual(0, rowsAffected);
                Assert.AreEqual(22, cmd.Parameters[1].Value);
            }
        }

        /// <summary>
        /// Bug #25609 MySqlDataAdapter.FillSchema 
        /// Bug #27668  	FillSchema and Stored Proc with an out parameter
        /// </summary>
        [Test]
        public void GetSchema()
        {
            if (version < new Version(5, 0))
                return;

            try
            {
                execSQL("CREATE PROCEDURE spTest() BEGIN SELECT * FROM Test; END");

                MySqlCommand cmd = new MySqlCommand("spTest", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
                reader.Read();
                reader.Close();

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable schema = new DataTable();
                da.FillSchema(schema, SchemaType.Source);
                Assert.AreEqual(2, schema.Columns.Count);

                //Bug #27668  	FillSchema and Stored Proc with an out parameter
                execSQL("DROP TABLE IF EXISTS Test");
                execSQL(@"CREATE TABLE Test(id INT AUTO_INCREMENT, PRIMARY KEY (id)) ");
                execSQL("DROP PROCEDURE IF EXISTS spTest");
                execSQL(
                    @"CREATE PROCEDURE spTest (OUT id INT)
                    BEGIN INSERT INTO Test VALUES (NULL); SET id=520; END");

                cmd = new MySqlCommand("spTest", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("?id", MySqlDbType.Int32);
                cmd.Parameters[0].Direction = ParameterDirection.Output;
                da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                cmd.ExecuteNonQuery();
                da.Fill(dt);
                da.FillSchema(dt, SchemaType.Mapped);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void InputOutputParameters()
        {
            if (version < new Version(5, 0))
                return;

            // create our procedure
            suExecSQL("CREATE PROCEDURE spTest( INOUT strVal VARCHAR(50), INOUT numVal INT, OUT outVal INT UNSIGNED ) " +
                      "BEGIN  SET strVal = CONCAT(strVal,'ending'); SET numVal=numVal * 2;  SET outVal=99; END");

            MySqlCommand cmd = new MySqlCommand("spTest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("?strVal", "beginning");
            cmd.Parameters.AddWithValue("?numVal", 33);
            cmd.Parameters.AddWithValue("?outVal", MySqlDbType.Int32);
            cmd.Parameters[0].Direction = ParameterDirection.InputOutput;
            cmd.Parameters[1].Direction = ParameterDirection.InputOutput;
            cmd.Parameters[2].Direction = ParameterDirection.Output;
            int rowsAffected = cmd.ExecuteNonQuery();
            Assert.AreEqual(0, rowsAffected);
            Assert.AreEqual("beginningending", cmd.Parameters[0].Value);
            Assert.AreEqual(66, cmd.Parameters[1].Value);
            Assert.AreEqual(99, cmd.Parameters[2].Value);
        }

        /// <summary>
        /// Bug #13927  	Multiple Records to same Table in Transaction Problem
        /// </summary>
        [Test]
        public void MultipleRecords()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("DROP PROCEDURE IF EXISTS spTest");
            execSQL("CREATE PROCEDURE spTest(id int, str VARCHAR(45)) " + "BEGIN INSERT INTO Test VALUES(id, str); END");

            MySqlCommand cmd = new MySqlCommand("spTest", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("?id", 1);
            cmd.Parameters.AddWithValue("?str", "First record");
            cmd.ExecuteNonQuery();

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("?id", 2);
            cmd.Parameters.AddWithValue("?str", "Second record");
            cmd.ExecuteNonQuery();

            MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM Test", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            Assert.AreEqual(1, dt.Rows[0]["id"]);
            Assert.AreEqual(2, dt.Rows[1]["id"]);
            Assert.AreEqual("First record", dt.Rows[0]["name"]);
            Assert.AreEqual("Second record", dt.Rows[1]["name"]);
        }

        [Test]
        public void MultipleResultsets()
        {
            if (version < new Version(5, 0))
                return;

            MultipleResultsetsImpl(false);
        }

        [Test]
        public void MultipleResultsetsPrepared()
        {
            if (version < new Version(5, 0))
                return;

            MultipleResultsetsImpl(true);
        }

        /// <summary>
        /// Bug #26139 MySqlCommand.LastInsertedId doesn't work for stored procedures 
        /// Currently this is borked on the server so we are marking this as notworking
        /// until the server has this fixed.
        /// </summary>
        /*        [Test]
                public void LastInsertId()
                {
                    execSQL("DROP TABLE IF EXISTS Test");
                    execSQL("CREATE TABLE Test (id INT AUTO_INCREMENT PRIMARY KEY, name VARCHAR(200))");
                    execSQL("INSERT INTO Test VALUES (NULL, 'Test1')");
                    execSQL("CREATE PROCEDURE spTest() BEGIN " +
                        "INSERT INTO Test VALUES (NULL, 'test'); END");

                    MySqlCommand cmd = new MySqlCommand("spTest", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    Assert.AreEqual(2, cmd.LastInsertedId);
                }
                */
        [Test]
        public void NoAccessToProcedureBodies()
        {
            if (version < new Version(5, 0))
                return;

            suExecSQL("DROP PROCEDURE IF EXISTS spTest");
            suExecSQL("CREATE PROCEDURE spTest(in1 INT, INOUT inout1 INT, OUT out1 INT ) " +
                      "BEGIN SET inout1 = inout1+2; SET out1=inout1-3; SELECT in1; END");

            string connStr = GetConnectionString(true) + "; use procedure bodies=false";
            using (MySqlConnection c = new MySqlConnection(connStr))
            {
                try
                {
                    c.Open();

                    MySqlCommand cmd = new MySqlCommand("spTest", c);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("?in1", 2);
                    cmd.Parameters.AddWithValue("?inout1", 4);
                    cmd.Parameters.Add("?out1", MySqlDbType.Int32);
                    cmd.Parameters[1].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();

                    Assert.AreEqual(6, cmd.Parameters[1].Value);
                    Assert.AreEqual(3, cmd.Parameters[2].Value);
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }

        [Test]
        public void NoBatch()
        {
            if (version < new Version(5, 0))
                return;

            try
            {
                MySqlCommand cmd = new MySqlCommand("spTest;select * from Test", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                Assert.Fail("Should have thrown an exception");
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Bug #11450  	Connector/Net, current database and stored procedures
        /// </summary>
        [Test]
        public void NoDefaultDatabase()
        {
            if (version < new Version(5, 0))
                return;

            // create our procedure
            suExecSQL("CREATE PROCEDURE spTest() BEGIN  SELECT 4; END");

            string newConnStr = GetConnectionString(false);
            using (MySqlConnection c = new MySqlConnection(newConnStr))
            {
                c.Open();
                MySqlCommand cmd2 = new MySqlCommand(String.Format("use `{0}`", database0), c);
                cmd2.ExecuteNonQuery();

                MySqlCommand cmd = new MySqlCommand("spTest", c);
                cmd.CommandType = CommandType.StoredProcedure;
                object val = cmd.ExecuteScalar();
                Assert.AreEqual(4, val);

                cmd2.CommandText = String.Format("use `{0}`", database1);
                cmd2.ExecuteNonQuery();

                cmd.CommandText = String.Format("`{0}`.spTest", database0);
                val = cmd.ExecuteScalar();
                Assert.AreEqual(4, val);
            }
        }

        [Test]
        public void NoInOutMarker()
        {
            if (version < new Version(5, 0))
                return;

            // create our procedure
            suExecSQL("CREATE PROCEDURE spTest( valin varchar(50) ) BEGIN  SELECT valin;  END");

            MySqlCommand cmd = new MySqlCommand("spTest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("?valin", "myvalue");
            object val = cmd.ExecuteScalar();
            Assert.AreEqual("myvalue", val);
        }

        [Test]
        public void NonQuery()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("CREATE PROCEDURE spTest(IN value INT) BEGIN INSERT INTO Test VALUES(value, 'Test'); END");

            //setup testing data
            MySqlCommand cmd = new MySqlCommand("spTest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("?value", 2);
            int rowsAffected = cmd.ExecuteNonQuery();
            Assert.AreEqual(1, rowsAffected);

            cmd.CommandText = "SELECT * FROM Test";
            cmd.CommandType = CommandType.Text;
            MySqlDataReader reader = null;

            try
            {
                reader = cmd.ExecuteReader();
                Assert.IsTrue(reader.Read());
                Assert.AreEqual(2, reader.GetInt32(0));
                Assert.AreEqual("Test", reader.GetString(1));
                Assert.IsFalse(reader.Read());
                Assert.IsFalse(reader.NextResult());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        [Test]
        public void NoSPOnPre50()
        {
            if (version < new Version(5, 0))
                return;

            try
            {
                MySqlCommand cmd = new MySqlCommand("spTest", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                Assert.Fail("This should not have worked");
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Bug #9722 Connector does not recognize parameters separated by a linefeed 
        /// </summary>
        [Test]
        public void OtherProcSigs()
        {
            if (version < new Version(5, 0))
                return;

            // create our procedure
            suExecSQL("CREATE PROCEDURE spTest(IN \r\nvalin DECIMAL(10,2),\nIN val2 INT) " +
                      "SQL SECURITY INVOKER BEGIN  SELECT valin; END");

            MySqlCommand cmd = new MySqlCommand("spTest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("?valin", 20.4);
            cmd.Parameters.AddWithValue("?val2", 4);
            decimal val = (decimal)cmd.ExecuteScalar();
            Decimal d = new Decimal(20.4);
            Assert.AreEqual(d, val);

            // create our second procedure
            suExecSQL("DROP PROCEDURE IF EXISTS spTest");
            suExecSQL("CREATE PROCEDURE spTest( \r\n) BEGIN  SELECT 4; END");
            cmd.Parameters.Clear();
            object val1 = cmd.ExecuteScalar();
            Assert.AreEqual(4, val1);
        }

        /// <summary>
        /// Bug #17814 Stored procedure fails unless DbType set explicitly
        /// Bug #23749 VarChar field size over 255 causes a System.OverflowException 
        /// </summary>
        [Test]
        public void OutputParameters()
        {
            if (version < new Version(5, 0))
                return;

            // we don't want to run this test under no access
            string connInfo = GetConnectionInfo();
            if (connInfo.IndexOf("use procedure bodies=false") != -1)
                return;

            // create our procedure
            execSQL("CREATE PROCEDURE spTest(out value VARCHAR(350), OUT intVal INT, " +
                    "OUT dateVal TIMESTAMP, OUT floatVal FLOAT, OUT noTypeVarChar VARCHAR(20), " + "OUT noTypeInt INT) " +
                    "BEGIN  SET value='42';  SET intVal=33; SET dateVal='2004-06-05 07:58:09'; " +
                    "SET floatVal = 1.2; SET noTypeVarChar='test'; SET noTypeInt=66; END");

            // we use rootConn here since we are using parameters
            MySqlCommand cmd = new MySqlCommand("spTest", rootConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter("?value", MySqlDbType.VarChar));
            cmd.Parameters.Add(new MySqlParameter("?intVal", MySqlDbType.Int32));
            cmd.Parameters.Add(new MySqlParameter("?dateVal", MySqlDbType.DateTime));
            cmd.Parameters.Add(new MySqlParameter("?floatVal", MySqlDbType.Float));
            MySqlParameter vcP = new MySqlParameter();
            vcP.ParameterName = "?noTypeVarChar";
            vcP.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(vcP);
            MySqlParameter vcI = new MySqlParameter();
            vcI.ParameterName = "?noTypeInt";
            vcI.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(vcI);
            cmd.Parameters[0].Direction = ParameterDirection.Output;
            cmd.Parameters[1].Direction = ParameterDirection.Output;
            cmd.Parameters[2].Direction = ParameterDirection.Output;
            cmd.Parameters[3].Direction = ParameterDirection.Output;
            int rowsAffected = cmd.ExecuteNonQuery();

            Assert.AreEqual(0, rowsAffected);
            Assert.AreEqual("42", cmd.Parameters[0].Value);
            Assert.AreEqual(33, cmd.Parameters[1].Value);
            Assert.AreEqual(new DateTime(2004, 6, 5, 7, 58, 9), Convert.ToDateTime(cmd.Parameters[2].Value));
            Assert.AreEqual(1.2, (decimal)(float)cmd.Parameters[3].Value);
            Assert.AreEqual("test", cmd.Parameters[4].Value);
            Assert.AreEqual(66, cmd.Parameters[5].Value);
        }

        /// <summary>
        /// </summary>
        [Test]
        public void OutputParametersWithNewParamHandling()
        {
            if (version < new Version(5, 0))
                return;

            // create our procedure
            execSQL("CREATE PROCEDURE spTest(out val1 VARCHAR(350)) " + "BEGIN  SET val1 = '42';  END");

            string connStr = GetConnectionString(true);
            connStr = connStr.Replace("allow user variables=true", "allow user variables=false");
            using (MySqlConnection c = new MySqlConnection(connStr))
            {
                c.Open();

                MySqlCommand cmd = new MySqlCommand("spTest", c);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@val1", MySqlDbType.VarChar)).Direction = ParameterDirection.Output;
                int rowsAffected = cmd.ExecuteNonQuery();

                Assert.AreEqual(0, rowsAffected);
                Assert.AreEqual("42", cmd.Parameters[0].Value);
            }
        }

        /// <summary>
        /// Bug #6902  	Errors in parsing stored procedure parameters
        /// </summary>
        [Test]
        public void ParmWithCharacterSet()
        {
            if (version < new Version(5, 0))
                return;

            suExecSQL("CREATE PROCEDURE spTest(P longtext character set utf8) " + "BEGIN SELECT P; END");

            MySqlCommand cmd = new MySqlCommand("spTest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("?P", "This is my value");
            string p = (string)cmd.ExecuteScalar();
            Assert.AreEqual("This is my value", p);
        }

        /// <summary>
        /// Bug #17046 Null pointer access when stored procedure is used 
        /// </summary>
        [Test]
        public void PreparedReader()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE  Test (id int(10) unsigned NOT NULL default '0', " +
                    "val int(10) unsigned default NULL, PRIMARY KEY (id)) " + "ENGINE=InnoDB DEFAULT CHARSET=utf8");
            execSQL("CREATE PROCEDURE spTest (IN pp INTEGER) " + "select * from Test where id > pp ");

            MySqlCommand c = new MySqlCommand("spTest", conn);
            c.CommandType = CommandType.StoredProcedure;
            IDataParameter p = c.CreateParameter();
            p.ParameterName = "?pp";
            p.Value = 10;
            c.Parameters.Add(p);
            c.Prepare();
            MySqlDataReader reader = null;
            try
            {
                reader = c.ExecuteReader();
                while (reader.Read())
                {
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        [Explicit]
        [Test]
        public void ProcedureCache()
        {
            if (version < new Version(5, 0))
                return;

            // open a new connection using a procedure cache
            string connStr = GetConnectionString(true);
            connStr += ";procedure cache size=25;logging=true";
            MySqlConnection c = new MySqlConnection(connStr);
            try
            {
                c.Open();

                // install our custom trace listener
                GenericListener myListener = new GenericListener();
                Trace.Listeners.Add(myListener);

                for (int x = 0; x < 10; x++)
                {
                    execSQL("DROP PROCEDURE IF EXISTS spTest" + x);
                    execSQL("CREATE PROCEDURE spTest" + x + "() BEGIN SELECT 1; END");
                    MySqlCommand cmd = new MySqlCommand("spTest" + x, c);
                    cmd.CommandType = CommandType.StoredProcedure;
                    for (int y = 0; y < 20; y++)
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                // remove our custom trace listener
                Trace.Listeners.Remove(myListener);

                // now see how many times our listener recorded a cache hit
                Assert.AreEqual(190, myListener.Find("from procedure cache"));
                Assert.AreEqual(10, myListener.Find("from server"));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                if (c != null)
                    c.Close();
                for (int x = 0; x < 10; x++)
                {
                    execSQL("DROP PROCEDURE IF EXISTS spTest" + x);
                }
            }
        }

        /// <summary>
        /// Bug #23268 System.FormatException when invoking procedure with ENUM input parameter 
        /// </summary>
        [Test]
        public void ProcEnumParamTest()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test(str VARCHAR(50), e ENUM ('P','R','F','E'), i INT(6))");
            execSQL("CREATE PROCEDURE spTest(IN p_enum ENUM('P','R','F','E')) BEGIN " +
                    "INSERT INTO Test (str, e, i) VALUES (null, p_enum, 55);  END");

            try
            {
                MySqlCommand cmd = new MySqlCommand("spTest", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("?p_enum", "P");
                cmd.Parameters["?p_enum"].Direction = ParameterDirection.Input;
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                }
                cmd.CommandText = "SELECT e FROM Test";
                cmd.CommandType = CommandType.Text;
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    Assert.AreEqual("P", reader.GetString(0));
                }
            }
            catch (MySqlException ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void ReturningEmptyResultSet()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("DROP PROCEDURE IF EXISTS spTest");
            execSQL("DROP TABLE IF EXISTS test1");
            execSQL("DROP TABLE IF EXISTS test2");
            execSQL("CREATE TABLE test1 (id int AUTO_INCREMENT NOT NULL, " + "Name VARCHAR(100) NOT NULL, PRIMARY KEY(id))");
            execSQL("CREATE TABLE test2 (id int AUTO_INCREMENT NOT NULL, " +
                    "id1 INT NOT NULL, id2 INT NOT NULL, PRIMARY KEY(id))");

            execSQL("INSERT INTO test1 (Id, Name) VALUES (1, 'Item1')");
            execSQL("INSERT INTO test1 (Id, Name) VALUES (2, 'Item2')");
            execSQL("INSERT INTO test2 (Id, Id1, Id2) VALUES (1, 1, 1)");
            execSQL("INSERT INTO test2 (Id, Id1, Id2) VALUES (2, 2, 1)");

            execSQL("CREATE PROCEDURE spTest(Name VARCHAR(100), OUT Table1Id INT) " +
                    "BEGIN SELECT t1.Id INTO Table1Id FROM test1 t1 WHERE t1.Name LIKE Name; " +
                    "SELECT t3.Id2 FROM test2 t3 WHERE t3.Id1 = Table1Id; END");

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spTest";
            cmd.Parameters.AddWithValue("?Name", "Item3");
            cmd.Parameters.Add("?Table1Id", MySqlDbType.Int32);
            cmd.Parameters["?Table1Id"].Direction = ParameterDirection.Output;

            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            try
            {
                da.Fill(ds);
            }
            catch (MySqlException)
            {
                // on 5.1 this throws an exception that no rows were returned.
            }
        }

        /// <summary>
        /// Bug #7623  	Adding MySqlParameter causes error if MySqlDbType is Decimal
        /// </summary>
        [Test]
        public void ReturningResultset()
        {
            if (version < new Version(5, 0))
                return;

            // create our procedure
            execSQL("CREATE PROCEDURE spTest(val decimal(10,3)) begin select val; end");

            using (MySqlCommand cmd = new MySqlCommand("spTest", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                MySqlParameter p = cmd.Parameters.Add("?val", MySqlDbType.Decimal);
                p.Precision = 10;
                p.Scale = 3;
                p.Value = 21;

                decimal id = (decimal)cmd.ExecuteScalar();
                Assert.AreEqual(21, id);
            }
        }

        /// <summary>
        /// Bug #25625 Crashes when calling with CommandType set to StoredProcedure 
        /// </summary>
        [Test]
        public void RunWithoutSelectPrivsThrowException()
        {
            if (version < new Version(5, 0))
                return;

            // we don't want this test to run in our all access fixture
            string connInfo = GetConnectionInfo();
            if (connInfo.IndexOf("use procedure bodies=false") == -1)
                return;

            suExecSQL(String.Format("GRANT ALL ON `{0}`.* to 'testuser'@'%' identified by 'testuser'", database0));
            suExecSQL(String.Format("GRANT ALL ON `{0}`.* to 'testuser'@'localhost' identified by 'testuser'", database0));

            execSQL("DROP PROCEDURE IF EXISTS spTest");
            execSQL("CREATE PROCEDURE spTest(id int, OUT outid int, INOUT inoutid int) " +
                    "BEGIN SET outid=id+inoutid; SET inoutid=inoutid+id; END");

            string s = GetConnectionStringEx("testuser", "testuser", true);
            MySqlConnection c = new MySqlConnection(s);
            c.Open();

            try
            {
                MySqlCommand cmd = new MySqlCommand("spTest", c);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("?id", 2);
                cmd.Parameters.AddWithValue("?outid", MySqlDbType.Int32);
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("?inoutid", 4);
                cmd.Parameters[2].Direction = ParameterDirection.InputOutput;
                cmd.ExecuteNonQuery();

                Assert.AreEqual(6, cmd.Parameters[1].Value);
                Assert.AreEqual(6, cmd.Parameters[2].Value);
            }
            catch (InvalidOperationException iex)
            {
                Assert.IsTrue(iex.Message.StartsWith("Unable to retrieve"));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                if (c != null)
                    c.Close();
                suExecSQL("DELETE FROM mysql.user WHERE user = 'testuser'");
            }
        }

        /// <summary>
        /// Bug #13753  	Exception calling stored procedure with special characters in parameters
        /// </summary>
        [Test]
        public void SpecialCharacters()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("SET sql_mode=ANSI_QUOTES");
            try
            {
                execSQL("CREATE PROCEDURE spTest(\"@Param1\" text) BEGIN SELECT \"@Param1\"; END");

                MySqlCommand cmd = new MySqlCommand("spTest", conn);
                cmd.Parameters.AddWithValue("?@Param1", "This is my value");
                cmd.CommandType = CommandType.StoredProcedure;

                string val = (string)cmd.ExecuteScalar();
                Assert.AreEqual("This is my value", val);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                execSQL("SET sql_mode=\"\"");
            }
        }

        /// <summary>
        /// Bug #22452 MySql.Data.MySqlClient.MySqlException: 
        /// </summary>
        [Test]
        public void TurkishStoredProcs()
        {
            if (version < new Version(5, 0))
                return;

            suExecSQL("CREATE PROCEDURE spTest(IN p_paramname INT) BEGIN SELECT p_paramname; END");
            CultureInfo uiCulture = Thread.CurrentThread.CurrentUICulture;
            CultureInfo culture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("tr-TR");

            try
            {
                MySqlCommand cmd = new MySqlCommand("spTest", conn);
                cmd.Parameters.AddWithValue("?p_paramname", 2);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = uiCulture;
            }
        }

        [Test]
        public void UnsignedOutputParameters()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE  Test (id INT(10) UNSIGNED AUTO_INCREMENT, PRIMARY KEY (id)) ");
            execSQL("CREATE PROCEDURE spTest (OUT id BIGINT UNSIGNED) " +
                    "BEGIN INSERT INTO Test VALUES (NULL); SET id=LAST_INSERT_ID(); END");

            MySqlCommand cmd = new MySqlCommand("spTest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("?id", MySqlDbType.UInt64);
            cmd.Parameters[0].Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();

            object o = cmd.Parameters[0].Value;
            Assert.IsTrue(o is ulong);
            Assert.AreEqual(1, o);
        }

        /// <summary>
        /// Bug #27093 Exception when using large values in IN UInt64 parameters 
        /// </summary>
        [Test]
        public void UsingUInt64AsParam()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("DROP TABLE IF EXISTS Test");
            execSQL(
                @"CREATE TABLE Test(f1 bigint(20) unsigned NOT NULL,
                      PRIMARY KEY(f1)) ENGINE=InnoDB DEFAULT CHARSET=utf8");

            execSQL(
                @"CREATE PROCEDURE spTest(in _val bigint unsigned)
                      BEGIN insert into  Test set f1=_val; END");

            DbCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spTest";
            DbParameter param = cmd.CreateParameter();
            param.DbType = DbType.UInt64;
            param.Direction = ParameterDirection.Input;
            param.ParameterName = "?_val";
            ulong bigval = long.MaxValue;
            bigval += 1000;
            param.Value = bigval;
            cmd.Parameters.Add(param);
            cmd.ExecuteNonQuery();
        }

        [Test]
        public void WrongParameters()
        {
            if (version < new Version(5, 0))
                return;

            suExecSQL("CREATE PROCEDURE spTest(p1 INT) BEGIN SELECT 1; END");
            try
            {
                MySqlCommand cmd = new MySqlCommand("spTest", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("?p2", 1);
                cmd.ExecuteNonQuery();
                Assert.Fail("Should have thrown an exception");
            }
            catch (Exception)
            {
            }
        }
    }
}