using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    /// <summary>
    /// Summary description for ConnectionTests.
    /// </summary>
    [TestFixture]
    public class DataReaderTests : BaseTest
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (id INT NOT NULL, name VARCHAR(100), d DATE, dt DATETIME, b1 LONGBLOB, PRIMARY KEY(id))");
        }

        #endregion

        void RapeZillaThreadSpawn()
        {
            for (int i = 0; i < 500; i++)
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM `Test`";
                    using (MySqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            object o = r[0];
                        }
                        Thread.Sleep(2);
                    }
                }
            }
        }

        /// <summary>
        /// Bug #9237  	MySqlDataReader.AffectedRecords not set to -1
        /// </summary>
        [Test]
        public void AffectedRows()
        {
            MySqlCommand cmd = new MySqlCommand("SHOW TABLES", conn);
            try
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    reader.Close();
                    Assert.AreEqual(-1, reader.RecordsAffected);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void CloseConnectionBehavior()
        {
            execSQL("INSERT INTO Test(id,name) VALUES(1,'test')");

            MySqlConnection c2 = new MySqlConnection(conn.ConnectionString);
            c2.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Test", c2);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                Assert.IsTrue(reader.Read());
                reader.Close();
                Assert.IsTrue(c2.State == ConnectionState.Closed);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (c2.State == ConnectionState.Closed)
                    c2.Open();
            }
        }

        [Test]
        public void CloseConnectionBehavior2()
        {
            execSQL("INSERT INTO Test(id,name) VALUES(1,'test')");

            MySqlConnection c2 = new MySqlConnection(conn.ConnectionString);
            c2.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Test", c2);
            MySqlDataReader reader;
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                Assert.IsTrue(reader.Read());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                c2.Close();
                c2.Dispose();
            }
        }

        /// <summary>
        /// Bug #37239 MySqlReader GetOrdinal performance changes break existing functionality
        /// </summary>
        [Test]
        public void ColumnsWithSameName()
        {
            execSQL("INSERT INTO Test (id, name) VALUES (1, 'test')");

            MySqlCommand cmd = new MySqlCommand("SELECT a.name, a.name FROM Test a", conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                reader.Read();
                string name1 = reader.GetString(0);
                string name2 = reader.GetString(1);
                Assert.AreEqual(name1, name2);
                Assert.AreEqual(name1, "test");
            }

            cmd.CommandText = "SELECT 'a' AS XYZ, 'b' as Xyz";
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                reader.Read();
                string name1 = reader.GetString(0);
                string name2 = reader.GetString(1);
            }
        }

        [Test]
        public void ConsecutiveNulls()
        {
            execSQL("INSERT INTO Test (id, name, dt) VALUES (1, 'Test', NULL)");
            execSQL("INSERT INTO Test (id, name, dt) VALUES (2, NULL, now())");
            execSQL("INSERT INTO Test (id, name, dt) VALUES (3, 'Test2', NULL)");

            MySqlCommand cmd = new MySqlCommand("SELECT id, name, dt FROM Test", conn);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                reader.Read();
                Assert.AreEqual(1, reader.GetValue(0));
                Assert.AreEqual("Test", reader.GetValue(1));
                Assert.AreEqual("Test", reader.GetString(1));
                Assert.AreEqual(DBNull.Value, reader.GetValue(2));
                reader.Read();
                Assert.AreEqual(2, reader.GetValue(0));
                Assert.AreEqual(DBNull.Value, reader.GetValue(1));
                try
                {
                    reader.GetString(1);
                    Assert.Fail("Should not get here");
                }
                catch (Exception)
                {
                }
                Assert.IsFalse(reader.IsDBNull(2));
                reader.Read();
                Assert.AreEqual(3, reader.GetValue(0));
                Assert.AreEqual("Test2", reader.GetValue(1));
                Assert.AreEqual("Test2", reader.GetString(1));
                Assert.AreEqual(DBNull.Value, reader.GetValue(2));
                try
                {
                    reader.GetMySqlDateTime(2);
                    Assert.Fail("Should not get here");
                }
                catch (Exception)
                {
                }
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

        /// <summary>
        /// Bug #30204  	Incorrect ConstraintException
        /// </summary>
        [Test]
        public void ConstraintWithLoadingReader()
        {
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL(
                @"CREATE TABLE Test (ID_A int(11) NOT NULL,
				ID_B int(11) NOT NULL, PRIMARY KEY (ID_A,ID_B)
				) ENGINE=MyISAM DEFAULT CHARSET=latin1;");

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Test", conn);
            DataTable dt = new DataTable();

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dt.Load(reader);
            }

            DataRow row = dt.NewRow();
            row["ID_A"] = 2;
            row["ID_B"] = 3;
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["ID_A"] = 2;
            row["ID_B"] = 4;
            dt.Rows.Add(row);
        }

        [Test]
        public void GetBytes()
        {
            int len = 50000;
            var bytes = Utils.CreateBlob(len);
            MySqlCommand cmd = new MySqlCommand("INSERT INTO Test (id, name, b1) VALUES(1, 'Test', ?b1)", conn);
            cmd.Parameters.AddWithValue("?b1", bytes);
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT * FROM Test";
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                reader.Read();

                long sizeBytes = reader.GetBytes(4, 0, null, 0, 0);
                Assert.AreEqual(len, sizeBytes);

                var buff1 = new byte[len / 2];
                var buff2 = new byte[len - (len / 2)];
                long buff1cnt = reader.GetBytes(4, 0, buff1, 0, len / 2);
                long buff2cnt = reader.GetBytes(4, buff1cnt, buff2, 0, buff2.Length);
                Assert.AreEqual(buff1.Length, buff1cnt);
                Assert.AreEqual(buff2.Length, buff2cnt);

                for (int i = 0; i < buff1.Length; i++)
                {
                    Assert.AreEqual(bytes[i], buff1[i]);
                }

                for (int i = 0; i < buff2.Length; i++)
                {
                    Assert.AreEqual(bytes[buff1.Length + i], buff2[i]);
                }

                reader.Close();

                //  now check with sequential access
                reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                Assert.IsTrue(reader.Read());
                int mylen = len;
                var buff = new byte[8192];
                int startIndex = 0;
                while (mylen > 0)
                {
                    int readLen = Math.Min(mylen, buff.Length);
                    int retVal = (int)reader.GetBytes(4, startIndex, buff, 0, readLen);
                    Assert.AreEqual(readLen, retVal);
                    for (int i = 0; i < readLen; i++)
                    {
                        Assert.AreEqual(bytes[startIndex + i], buff[i]);
                    }
                    startIndex += readLen;
                    mylen -= readLen;
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
        public void GetChar()
        {
            execSQL("INSERT INTO Test (id, name) VALUES (1, 'a')");
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Test", conn);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                reader.Read();
                char achar = reader.GetChar(1);
                Assert.AreEqual('a', achar);
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
        public void GetSchema()
        {
            string sql = "CREATE TABLE test2(id INT UNSIGNED AUTO_INCREMENT " +
                         "NOT NULL, name VARCHAR(255) NOT NULL, name2 VARCHAR(40), fl FLOAT, " + "dt DATETIME, PRIMARY KEY(id))";

            execSQL("DROP TABLE IF EXISTS test2");
            execSQL(sql);
            execSQL("INSERT INTO test2 VALUES(1,'Test', 'Test', 1.0, now())");

            MySqlDataReader reader = null;

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM test2", conn);
                reader = cmd.ExecuteReader();
                DataTable dt = reader.GetSchemaTable();
                Assert.AreEqual(true, dt.Rows[0]["IsAutoIncrement"], "Checking auto increment");
                Assert.IsFalse((bool)dt.Rows[0]["IsUnique"], "Checking IsUnique");
                Assert.IsTrue((bool)dt.Rows[0]["IsKey"]);
                Assert.AreEqual(false, dt.Rows[0]["AllowDBNull"], "Checking AllowDBNull");
                Assert.AreEqual(false, dt.Rows[1]["AllowDBNull"], "Checking AllowDBNull");
                Assert.AreEqual(255, dt.Rows[1]["ColumnSize"]);
                Assert.AreEqual(40, dt.Rows[2]["ColumnSize"]);
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

            execSQL("DROP TABLE IF EXISTS test2");
        }

        /// <summary>
        /// Bug #23538 Exception thrown when GetSchemaTable is called and "fields" is null. 
        /// </summary>
        [Test]
        public void GetSchemaTableOnEmptyResultset()
        {
            if (Version < new Version(5, 0))
                return;

            execSQL("CREATE PROCEDURE spTest() BEGIN END");

            MySqlCommand cmd = new MySqlCommand("spTest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                DataTable dt = reader.GetSchemaTable();
                Assert.IsNull(dt);
            }
        }

        /// <summary>
        /// Bug #19294 IDataRecord.GetString method should return null for null values
        /// </summary>
        [Test]
        public void GetStringOnNull()
        {
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (id int, PRIMARY KEY(id))");
            MySqlCommand cmd = new MySqlCommand(String.Format("SHOW INDEX FROM Test FROM `{0}`", database0), conn);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                reader.Read();
                reader.GetString(reader.GetOrdinal("Sub_part"));
                Assert.Fail("We should not get here");
            }
            catch (SqlNullValueException)
            {
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
        public void HungDataReader()
        {
            MySqlCommand cmd = new MySqlCommand("USE `" + database0 + "`; SHOW TABLES", conn);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    reader.GetString(0);
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
        /// Bug #11873  	Invalid timestamp in query produces incorrect reader exception
        /// </summary>
        [Test]
        public void InvalidTimestamp()
        {
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (tm TIMESTAMP)");
            execSQL("INSERT INTO Test VALUES (NULL)");

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Test WHERE tm = '7/1/2005 12:00:00 AM'", conn);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
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
        /// Bug #24765  	Retrieving empty fields results in check for isDBNull
        /// </summary>
        [Test]
        public void IsDbNullOnNonNullFields()
        {
            execSQL("INSERT INTO Test (id, name) VALUES (1, '')");

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Test", conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                Assert.IsTrue(reader.Read());
                Assert.IsFalse(reader.IsDBNull(1));
            }
        }

        [Test]
        public void ReaderOnNonQuery()
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO Test (id,name) VALUES (1,'Test')", conn);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                Assert.IsFalse(reader.Read());
                reader.Close();

                cmd.CommandText = "SELECT name FROM Test";
                object v = cmd.ExecuteScalar();
                Assert.AreEqual("Test", v);
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
        public void ReadingFieldsBeforeRead()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Test", conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            try
            {
                reader.GetInt32(0);
            }
            catch (MySqlException)
            {
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        [Test]
        public void ReadingTextFields()
        {
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (id int, t1 TEXT)");
            execSQL("INSERT INTO Test VALUES (1, 'Text value')");

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Test", conn);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                reader.Read();
                string s = reader["t1"].ToString();
                Assert.AreEqual("Text value", s);
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
        /// Bug #8630  	Executing a query with the SchemaOnly option reads the entire resultset
        /// </summary>
        [Test]
        public void SchemaOnly()
        {
            execSQL("INSERT INTO Test (id,name) VALUES(1,'test1')");
            execSQL("INSERT INTO Test (id,name) VALUES(2,'test2')");
            execSQL("INSERT INTO Test (id,name) VALUES(3,'test3')");

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Test", conn);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
                DataTable ltable = reader.GetSchemaTable();
                Assert.AreEqual(5, ltable.Rows.Count);
                Assert.AreEqual(22, ltable.Columns.Count);
                Assert.IsFalse(reader.Read());
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
        /// Added test for IsDBNull from bug# 7399
        /// </summary>
        [Test]
        public void SequentialAccessBehavior()
        {
            execSQL("INSERT INTO Test(id,name) VALUES(1,'test1')");

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Test", conn);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                Assert.IsTrue(reader.Read());
                Assert.IsFalse(reader.IsDBNull(0));
                int i = reader.GetInt32(0);
                string s = reader.GetString(1);
                Assert.AreEqual(1, i);
                Assert.AreEqual("test1", s);

                // this next line should throw an exception
                i = reader.GetInt32(0);
                Assert.Fail("This line should not execute");
            }
            catch (MySqlException)
            {
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
        public void SimpleSingleRow()
        {
            execSQL("INSERT INTO Test(id,name) VALUES(1,'test1')");

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Test", conn);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                Assert.IsTrue(reader.Read(), "First read");
                Assert.AreEqual(1, reader.GetInt32(0));
                Assert.AreEqual("test1", reader.GetString(1));
                Assert.IsFalse(reader.Read(), "Second read");
                Assert.IsFalse(reader.NextResult(), "Trying NextResult");
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
        public void SingleRowBehavior()
        {
            execSQL("INSERT INTO Test(id,name) VALUES(1,'test1')");
            execSQL("INSERT INTO Test(id,name) VALUES(2,'test2')");
            execSQL("INSERT INTO Test(id,name) VALUES(3,'test3')");

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Test", conn);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                Assert.IsTrue(reader.Read(), "First read");
                Assert.IsFalse(reader.Read(), "Second read");
                Assert.IsFalse(reader.NextResult(), "Trying NextResult");
                reader.Close();

                cmd.CommandText = "SELECT * FROM Test where id=1";
                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                Assert.IsTrue(reader.Read());
                Assert.AreEqual("test1", reader.GetString(1));
                Assert.IsFalse(reader.Read());
                Assert.IsFalse(reader.NextResult());
                reader.Close();

                reader = null;
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
        public void SingleRowBehaviorWithLimit()
        {
            execSQL("INSERT INTO Test(id,name) VALUES(1,'test1')");
            execSQL("INSERT INTO Test(id,name) VALUES(2,'test2')");
            execSQL("INSERT INTO Test(id,name) VALUES(3,'test3')");

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Test LIMIT 2", conn);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                Assert.IsTrue(reader.Read(), "First read");
                Assert.IsFalse(reader.Read(), "Second read");
                Assert.IsFalse(reader.NextResult(), "Trying NextResult");
                reader.Close();

                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                Assert.IsTrue(reader.Read(), "First read");
                Assert.IsFalse(reader.Read(), "Second read");
                Assert.IsFalse(reader.NextResult(), "Trying NextResult");
                reader.Close();

                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                Assert.IsTrue(reader.Read(), "First read");
                Assert.IsFalse(reader.Read(), "Second read");
                Assert.IsFalse(reader.NextResult(), "Trying NextResult");
                reader.Close();
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
        public void TestManyDifferentResultsets()
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("", conn);
                // insert 100 records
                cmd.CommandText = "INSERT INTO Test (id,name,dt,b1) VALUES (?id, 'test','2004-12-05 12:57:00','long blob data')";
                cmd.Parameters.Add(new MySqlParameter("?id", 1));
                for (int i = 1; i <= 100; i++)
                {
                    cmd.Parameters[0].Value = i;
                    cmd.ExecuteNonQuery();
                }

                cmd =
                    new MySqlCommand(
                        "SELECT id FROM Test WHERE id<?param1; " + "SELECT id,name FROM Test WHERE id = -50; " +
                        "SELECT * FROM Test WHERE id >= ?param1; " + "SELECT id, dt, b1 FROM Test WHERE id = -50; " +
                        "SELECT b1 FROM Test WHERE id = -50; " + "SELECT id, dt, b1 FROM Test WHERE id < ?param1; " +
                        "SELECT b1 FROM Test WHERE id >= ?param1;", conn);

                cmd.Parameters.AddWithValue("?param1", 50);

                reader = cmd.ExecuteReader();

                Assert.IsNotNull(reader);

                //First ResultSet, should have 49 rows.
                //SELECT id FROM Test WHERE id<?param1;
                Assert.AreEqual(true, reader.HasRows);
                Assert.AreEqual(1, reader.FieldCount);
                for (int i = 0; i < 49; i++)
                {
                    Assert.IsTrue(reader.Read());
                }
                Assert.AreEqual(false, reader.Read());

                //Second ResultSet, should have no rows.
                //SELECT id,name FROM Test WHERE id = -50;
                Assert.IsTrue(reader.NextResult());
                Assert.AreEqual(false, reader.HasRows);
                Assert.AreEqual(2, reader.FieldCount);
                Assert.AreEqual(false, reader.Read());

                //Third ResultSet, should have 51 rows.
                //SELECT * FROM Test WHERE id >= ?param1;
                Assert.IsTrue(reader.NextResult());
                Assert.AreEqual(true, reader.HasRows);
                Assert.AreEqual(5, reader.FieldCount);
                for (int i = 0; i < 51; i++)
                {
                    Assert.IsTrue(reader.Read());
                }
                Assert.AreEqual(false, reader.Read());

                //Fourth ResultSet, should have no rows.
                //SELECT id, dt, b1 FROM Test WHERE id = -50;
                Assert.IsTrue(reader.NextResult());
                Assert.AreEqual(false, reader.HasRows);
                Assert.AreEqual(3, reader.FieldCount); //Will Fail if uncommented expected 3 returned 5
                Assert.AreEqual(false, reader.Read());

                //Fifth ResultSet, should have no rows.
                //SELECT b1 FROM Test WHERE id = -50;
                Assert.IsTrue(reader.NextResult());
                Assert.AreEqual(false, reader.HasRows);
                Assert.AreEqual(1, reader.FieldCount); //Will Fail if uncommented expected 1 returned 5
                Assert.AreEqual(false, reader.Read());

                //Sixth ResultSet, should have 49 rows.
                //SELECT id, dt, b1 FROM Test WHERE id < ?param1;
                Assert.IsTrue(reader.NextResult());
                Assert.AreEqual(true, reader.HasRows);
                Assert.AreEqual(3, reader.FieldCount); //Will Fail if uncommented expected 3 returned 5
                for (int i = 0; i < 49; i++)
                {
                    Assert.IsTrue(reader.Read());
                }
                Assert.AreEqual(false, reader.Read());

                //Seventh ResultSet, should have 51 rows.
                //SELECT b1 FROM Test WHERE id >= ?param1;
                Assert.IsTrue(reader.NextResult());
                Assert.AreEqual(true, reader.HasRows);
                Assert.AreEqual(1, reader.FieldCount); //Will Fail if uncommented expected 1 returned 5
                for (int i = 0; i < 51; i++)
                {
                    Assert.IsTrue(reader.Read());
                }
                Assert.AreEqual(false, reader.Read());
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
        public void TestMultipleResultsets()
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("", conn);
                // insert 100 records
                cmd.CommandText = "INSERT INTO Test (id,name) VALUES (?id, 'test')";
                cmd.Parameters.Add(new MySqlParameter("?id", 1));
                for (int i = 1; i <= 100; i++)
                {
                    cmd.Parameters[0].Value = i;
                    cmd.ExecuteNonQuery();
                }

                // execute it one time
                cmd = new MySqlCommand("SELECT id FROM Test WHERE id<50; SELECT * FROM Test WHERE id >= 50;", conn);
                reader = cmd.ExecuteReader();
                Assert.IsNotNull(reader);
                Assert.AreEqual(true, reader.HasRows);
                Assert.IsTrue(reader.Read());
                Assert.AreEqual(1, reader.FieldCount);
                Assert.IsTrue(reader.NextResult());
                Assert.AreEqual(true, reader.HasRows);
                Assert.AreEqual(5, reader.FieldCount);
                reader.Close();

                // now do it again
                reader = cmd.ExecuteReader();
                Assert.IsNotNull(reader);
                Assert.AreEqual(true, reader.HasRows);
                Assert.IsTrue(reader.Read());
                Assert.AreEqual(1, reader.FieldCount);
                Assert.IsTrue(reader.NextResult());
                Assert.AreEqual(true, reader.HasRows);
                Assert.AreEqual(5, reader.FieldCount);
                reader.Close();
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
        public void TestMultipleResultsWithQueryCacheOn()
        {
            execSQL("SET SESSION query_cache_type = ON");
            execSQL("INSERT INTO Test (id,name) VALUES (1, 'Test')");
            execSQL("INSERT INTO Test (id,name) VALUES (51, 'Test')");

            MySqlDataReader reader = null;

            try
            {
                // execute it one time
                MySqlCommand cmd = new MySqlCommand("SELECT id FROM Test WHERE id<50; SELECT * FROM Test	WHERE id >= 50;", conn);

                reader = cmd.ExecuteReader();

                Assert.IsNotNull(reader);
                Assert.AreEqual(true, reader.HasRows);
                Assert.IsTrue(reader.Read());
                Assert.AreEqual(1, reader.FieldCount);
                Assert.IsTrue(reader.NextResult());
                Assert.AreEqual(true, reader.HasRows);
                Assert.AreEqual(5, reader.FieldCount);

                reader.Close();

                // now do it again
                reader = cmd.ExecuteReader();
                Assert.IsNotNull(reader);
                Assert.AreEqual(true, reader.HasRows);
                Assert.IsTrue(reader.Read());
                Assert.AreEqual(1, reader.FieldCount);
                Assert.IsTrue(reader.NextResult());
                Assert.AreEqual(true, reader.HasRows);
                Assert.AreEqual(5, reader.FieldCount);

                reader.Close();
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
        public void TestMultithreaded()
        {
            Thread t1 = new Thread(RapeZillaThreadSpawn);
            Thread t2 = new Thread(RapeZillaThreadSpawn);
            Thread t3 = new Thread(RapeZillaThreadSpawn);

            t1.Start();
            t2.Start();
            t3.Start();

            t1.Join();
            t2.Join();
            t3.Join();
        }

        [Test]
        public void TestSingleResultSetBehavior()
        {
            execSQL("INSERT INTO Test (id, name, b1) VALUES (1, 'Test1', NULL)");
            execSQL("INSERT INTO Test (id, name, b1) VALUES (2, 'Test1', NULL)");

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Test WHERE id=1; SELECT * FROM Test WHERE id=2", conn);
            MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
            bool result = reader.Read();
            Assert.AreEqual(true, result);

            result = reader.NextResult();
            Assert.AreEqual(false, result);

            reader.Close();
        }
    }
}