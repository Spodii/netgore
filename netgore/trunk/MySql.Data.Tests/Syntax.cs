using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    [TestFixture]
    public class Syntax : BaseTest
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (id INT NOT NULL, name VARCHAR(250), PRIMARY KEY(id))");
        }

        #endregion

        [Test]
        public void AutoIncrement()
        {
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (testID int(11) NOT NULL auto_increment, testName varchar(100) default '', " +
                    "PRIMARY KEY  (testID)) ENGINE=InnoDB DEFAULT CHARSET=latin1");
            MySqlCommand cmd = new MySqlCommand("INSERT INTO Test VALUES (NULL, 'test')", conn);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT @@IDENTITY as 'Identity'";
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                reader.Read();
                int ident = Int32.Parse(reader.GetValue(0).ToString());
                Assert.AreEqual(1, ident);
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
        public void Bug6135()
        {
            execSQL("DROP TABLE IF EXISTS KLANT");
            string sql = "CREATE TABLE `KLANT` (`KlantNummer` int(11) NOT NULL auto_increment, " +
                         "`Username` varchar(50) NOT NULL default '', `Password` varchar(100) NOT NULL default '', " +
                         "`Naam` varchar(100) NOT NULL default '', `Voornaam` varchar(100) NOT NULL default '', " +
                         "`Straat` varchar(100) NOT NULL default '', `StraatNr` varchar(10) NOT NULL default '', " +
                         "`Gemeente` varchar(100) NOT NULL default '', `Postcode` varchar(10) NOT NULL default '', " +
                         "`DefaultMail` varchar(255) default '', 	`BtwNr` varchar(50) default '', " +
                         "`ReceiveMail` tinyint(1) NOT NULL default '0',	`Online` tinyint(1) NOT NULL default '0', " +
                         "`LastVisit` timestamp(14) NOT NULL, `Categorie` int(11) NOT NULL default '0', " +
                         "PRIMARY KEY  (`KlantNummer`),	UNIQUE KEY `UniqueUsername` (`Username`), " +
                         "UNIQUE KEY `UniqueDefaultMail` (`DefaultMail`)	)";
            createTable(sql, "MyISAM");

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM KLANT", conn);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
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

        [Test]
        public void CharFunction()
        {
            /*
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (id tinyint,val1	tinyint,val2 tinyint)");
            execSQL("INSERT INTO Test VALUES (65,1,1),(65,1,1)");

            MySqlDataAdapter da = new MySqlDataAdapter("SELECT CHAR(id) FROM Test GROUP BY val1,val2", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Assert.IsTrue(dt.Rows[0][0].GetType() == typeof(string));
            Assert.AreEqual("A", dt.Rows[0][0]);
            */
        }

        /// <summary>
        /// Bug #21521 # Symbols not allowed in column/table names. 
        /// </summary>
        [Test]
        public void CommentSymbolInTableName()
        {
            try
            {
                execSQL("DROP TABLE IF EXISTS Test");
                execSQL("CREATE TABLE Test (`PO#` int(11) NOT NULL auto_increment, " +
                        "`PODate` date default NULL, PRIMARY KEY  (`PO#`))");
                execSQL("INSERT INTO Test ( `PO#`, `PODate` ) " + "VALUES ( NULL, '2006-01-01' )");

                string sql = "SELECT `PO#` AS PurchaseOrderNumber, " + "`PODate` AS OrderDate FROM  Test";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                Assert.AreEqual(1, dt.Rows.Count);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Bug #27221 describe SQL command returns all byte array on MySQL versions older than 4.1.15 
        /// </summary>
        [Test]
        public void Describe()
        {
            MySqlDataAdapter da = new MySqlDataAdapter("DESCRIBE Test", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            Assert.IsTrue(dt.Columns[0].DataType == typeof(string));
            Assert.IsTrue(dt.Columns[1].DataType == typeof(string));
            Assert.IsTrue(dt.Columns[2].DataType == typeof(string));
            Assert.IsTrue(dt.Columns[3].DataType == typeof(string));
            Assert.IsTrue(dt.Columns[4].DataType == typeof(string));
            Assert.IsTrue(dt.Columns[5].DataType == typeof(string));
        }

        /// <summary>
        /// Bug #25178 Addition message in error 
        /// </summary>
        [Test]
        public void ErrorMessage()
        {
            MySqlCommand cmd = new MySqlCommand("SELEKT NOW() as theTime", conn);
            try
            {
                cmd.ExecuteScalar();
            }
            catch (MySqlException ex)
            {
                string s = ex.Message;
                Assert.IsFalse(s.StartsWith("#"));
            }
        }

        [Test]
        public void ForceWarnings()
        {
            if (version < new Version(4, 1))
                return;

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Test; DROP TABLE IF EXISTS test2; SELECT * FROM Test", conn);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                while (reader.NextResult())
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

        /// <summary>
        /// Bug #16645 FOUND_ROWS() Bug 
        /// </summary>
        [Test]
        public void FoundRows()
        {
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (testID int(11) NOT NULL auto_increment, testName varchar(100) default '', " +
                    "PRIMARY KEY  (testID)) ENGINE=InnoDB DEFAULT CHARSET=latin1");
            MySqlCommand cmd = new MySqlCommand("INSERT INTO Test VALUES (NULL, 'test')", conn);
            for (int i = 0; i < 1000; i++)
            {
                cmd.ExecuteNonQuery();
            }
            cmd.CommandText = "SELECT SQL_CALC_FOUND_ROWS * FROM Test LIMIT 0, 10";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT FOUND_ROWS()";
            object cnt = cmd.ExecuteScalar();
            Assert.AreEqual(1000, cnt);
        }

        [Test]
        public void LoadDataLocalInfile()
        {
            string connString = conn.ConnectionString + ";pooling=false";
            MySqlConnection c = new MySqlConnection(connString);
            c.Open();

            MySqlCommand cmd = new MySqlCommand("SET max_allowed_packet=250000000", c);
            cmd.ExecuteNonQuery();

            string path = Path.GetTempFileName();
            StreamWriter sw = new StreamWriter(path);
            for (int i = 0; i < 2000000; i++)
            {
                sw.WriteLine(i + ",'Test'");
            }
            sw.Flush();
            sw.Close();

            path = path.Replace(@"\", @"\\");
            cmd.CommandText = "LOAD DATA LOCAL INFILE '" + path + "' INTO TABLE Test FIELDS TERMINATED BY ','";
            cmd.CommandTimeout = 0;

            object cnt = 0;
            try
            {
                cnt = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.AreEqual(2000000, cnt);

            cmd.CommandText = "SELECT COUNT(*) FROM Test";
            cnt = cmd.ExecuteScalar();
            Assert.AreEqual(2000000, cnt);

            c.Close();
        }

        /// <summary>
        /// Bug #26960 Connector .NET 5.0.5 / Visual Studio Plugin 1.1.2 
        /// </summary>
        [Test]
        public void NullAsAType()
        {
            MySqlDataAdapter da =
                new MySqlDataAdapter(
                    @"SELECT 'localhost' as SERVER_NAME, 
                null as CATALOG_NAME, database() as SCHEMA_NAME", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Assert.IsTrue(dt.Rows[0][0].GetType() == typeof(string));
            Assert.AreEqual(DBNull.Value, dt.Rows[0][1]);
            Assert.IsTrue(dt.Rows[0][2].GetType() == typeof(string));
        }

        [Test]
        public void ProblemCharsInSQL()
        {
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (id INT NOT NULL, name VARCHAR(250), mt MEDIUMTEXT, " + "PRIMARY KEY(id))");

            MySqlCommand cmd = new MySqlCommand("INSERT INTO Test VALUES (?id, ?text, ?mt)", conn);
            cmd.Parameters.AddWithValue("?id", 1);
            cmd.Parameters.AddWithValue("?text", "This is my;test ? string-'''\"\".");
            cmd.Parameters.AddWithValue("?mt", "My MT string: ?");
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT * FROM Test";
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                Assert.IsTrue(reader.Read());
                Assert.AreEqual(1, reader.GetInt32(0));
                Assert.AreEqual("This is my;test ? string-'''\"\".", reader.GetString(1));
                Assert.AreEqual("My MT string: ?", reader.GetString(2));
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
        public void ProblemCharsInSQLUTF8()
        {
            if (version < new Version(4, 1))
                return;

            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (id INT NOT NULL, name VARCHAR(250), mt MEDIUMTEXT, " + "PRIMARY KEY(id)) CHAR SET utf8");

            MySqlConnection c = new MySqlConnection(GetConnectionString(true) + ";charset=utf8");
            c.Open();

            MySqlCommand cmd = new MySqlCommand("INSERT INTO Test VALUES (?id, ?text, ?mt)", c);
            cmd.Parameters.AddWithValue("?id", 1);
            cmd.Parameters.AddWithValue("?text", "This is my;test ? string–’‘’“”…");
            cmd.Parameters.AddWithValue("?mt", "My MT string: ?");
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT * FROM Test";
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                Assert.IsTrue(reader.Read());
                Assert.AreEqual(1, reader.GetInt32(0));
                Assert.AreEqual("This is my;test ? string–’‘’“”…", reader.GetString(1));
                Assert.AreEqual("My MT string: ?", reader.GetString(2));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (c != null)
                    c.Close();
            }
        }

        [Test]
        public void SettingAutoIncrementColumns()
        {
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (id int auto_increment, name varchar(100), primary key(id))");
            execSQL("INSERT INTO Test VALUES (1, 'One')");
            execSQL("INSERT INTO Test VALUES (3, 'Two')");

            MySqlCommand cmd = new MySqlCommand("SELECT name FROM Test WHERE id=1", conn);
            object name = cmd.ExecuteScalar();
            Assert.AreEqual("One", name);

            cmd.CommandText = "SELECT name FROM Test WHERE id=3";
            name = cmd.ExecuteScalar();
            Assert.AreEqual("Two", name);

            try
            {
                execSQL("INSERT INTO Test (id, name2) values (5, 'Three')");
                Assert.Fail("This should have failed");
            }
            catch (MySqlException)
            {
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void ShowCreateTable()
        {
            MySqlDataAdapter da = new MySqlDataAdapter("SHOW CREATE TABLE Test", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            Assert.AreEqual(1, dt.Rows.Count);
            Assert.AreEqual(2, dt.Columns.Count);
        }

        /// <summary>
        /// Bug #28448  	show processlist; returns byte arrays in the resulting data table
        /// </summary>
        [Test]
        public void ShowProcessList()
        {
            string connStr = GetConnectionString(true) + ";respect binary flags=false;";
            MySqlConnection c = new MySqlConnection(connStr);
            using (c)
            {
                c.Open();

                MySqlCommand cmd = new MySqlCommand("show processlist", c);
                DataTable dt = new DataTable();

                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    dt.Load(rdr);
                }
                DataRow row = dt.Rows[0];

                Assert.IsTrue(row["User"].GetType().Name == "String");
                Assert.IsTrue(row["Host"].GetType().Name == "String");
                Assert.IsTrue(row["Command"].GetType().Name == "String");
            }
        }

        [Test]
        public void ShowTablesInNonExistentDb()
        {
            MySqlCommand cmd = new MySqlCommand("SHOW TABLES FROM dummy", conn);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                Assert.Fail("ExecuteReader should not succeed");
            }
            catch (MySqlException)
            {
                Assert.AreEqual(ConnectionState.Open, conn.State);
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
        public void ShowTableStatus()
        {
            MySqlDataAdapter da = new MySqlDataAdapter(String.Format("SHOW TABLE STATUS FROM `{0}` LIKE 'Test'", database0), conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            Assert.IsTrue(dt.Rows[0][0].GetType() == typeof(string));
        }

        [Test]
        public void SpaceInDatabaseName()
        {
            string dbName = Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + " x";
            try
            {
                suExecSQL(String.Format("CREATE DATABASE `{0}`", dbName));
                suExecSQL(String.Format("GRANT ALL ON `{0}`.* to 'test'@'localhost' identified by 'test'", dbName));
                suExecSQL(String.Format("GRANT ALL ON `{0}`.* to 'test'@'%' identified by 'test'", dbName));
                suExecSQL("FLUSH PRIVILEGES");

                string connStr = GetConnectionString(false) + ";database=" + dbName;
                MySqlConnection c = new MySqlConnection(connStr);
                c.Open();
                c.Close();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                suExecSQL(String.Format("DROP DATABASE `{0}`", dbName));
            }
        }

        [Test]
        public void Sum()
        {
            execSQL("DROP TABLE IF EXISTS Test");

            execSQL("CREATE TABLE Test (field1 mediumint(9) default '0', field2 float(9,3) " +
                    "default '0.000', field3 double(15,3) default '0.000') engine=innodb ");
            execSQL("INSERT INTO Test values (1,1,1)");

            MySqlDataReader reader = null;

            MySqlCommand cmd2 = new MySqlCommand("SELECT sum(field2) FROM Test", conn);
            try
            {
                reader = cmd2.ExecuteReader();
                reader.Read();
                object o = reader[0];
                Assert.AreEqual(1, o);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                reader = null;
            }

            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (id int, count int)");
            execSQL("INSERT INTO Test VALUES (1, 21)");
            execSQL("INSERT INTO Test VALUES (1, 33)");
            execSQL("INSERT INTO Test VALUES (1, 16)");
            execSQL("INSERT INTO Test VALUES (1, 40)");

            MySqlCommand cmd = new MySqlCommand("SELECT id, SUM(count) FROM Test GROUP BY id", conn);
            try
            {
                reader = cmd.ExecuteReader();
                reader.Read();
                Assert.AreEqual(1, reader.GetInt32(0));
                Assert.AreEqual(110, reader.GetDouble(1));
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
    }
}