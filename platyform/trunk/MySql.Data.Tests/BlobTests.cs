using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    /// <summary>
    /// Summary description for BlobTests.
    /// </summary>
    [TestFixture]
    public class BlobTests : BaseTest
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (id INT NOT NULL, blob1 LONGBLOB, text1 LONGTEXT, PRIMARY KEY(id))");
        }

        #endregion

        void InternalGetChars(bool prepare)
        {
            execSQL("TRUNCATE TABLE Test");

            var data = new char[20000];
            for (int x = 0; x < data.Length; x++)
            {
                data[x] = (char)(65 + (x % 20));
            }

            MySqlCommand cmd = new MySqlCommand("INSERT INTO Test VALUES (1, NULL, ?text1)", conn);
            cmd.Parameters.AddWithValue("?text1", data);
            if (prepare)
                cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT * FROM Test";
            cmd.Parameters.Clear();
            if (prepare)
                cmd.Prepare();
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                reader.Read();

                // now we test chunking
                var dataOut = new char[data.Length];
                int pos = 0;
                int lenToRead = data.Length;
                while (lenToRead > 0)
                {
                    int size = Math.Min(lenToRead, 1024);
                    int read = (int)reader.GetChars(2, pos, dataOut, pos, size);
                    lenToRead -= read;
                    pos += read;
                }
                // now see if the buffer is intact
                for (int x = 0; x < data.Length; x++)
                {
                    Assert.AreEqual(data[x], dataOut[x], "Checking first text array at " + x);
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

        void InternalInsertText(bool prepare)
        {
            var data = new byte[1024];
            for (int x = 0; x < 1024; x++)
            {
                data[x] = (byte)(65 + (x % 20));
            }

            // Create sample table
            execSQL("TRUNCATE TABLE Test");
            MySqlCommand cmd = new MySqlCommand("INSERT INTO Test VALUES (1, ?b1, ?t1)", conn);
            cmd.Parameters.Add(new MySqlParameter("?t1", data));
            cmd.Parameters.Add(new MySqlParameter("?b1", "This is my blob data"));
            if (prepare)
                cmd.Prepare();
            int rows = cmd.ExecuteNonQuery();
            Assert.AreEqual(1, rows, "Checking insert rowcount");

            cmd.CommandText = "INSERT INTO Test VALUES(2, ?b1, ?t1)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("?t1", DBNull.Value);
            const string str = "This is my text value";
            cmd.Parameters.Add(new MySqlParameter("?b1", MySqlDbType.LongBlob, str.Length, ParameterDirection.Input, true, 0, 0,
                                                  "b1", DataRowVersion.Current, str));
            rows = cmd.ExecuteNonQuery();
            Assert.AreEqual(1, rows, "Checking insert rowcount");

            MySqlDataReader reader = null;
            try
            {
                cmd.CommandText = "SELECT * FROM Test";
                if (prepare)
                    cmd.Prepare();
                reader = cmd.ExecuteReader();
                Assert.AreEqual(true, reader.HasRows, "Checking HasRows");

                Assert.IsTrue(reader.Read());

                Assert.AreEqual("This is my blob data", reader.GetString(1));
                string s = reader.GetString(2);
                Assert.AreEqual(1024, s.Length, "Checking length returned ");
                Assert.AreEqual("ABCDEFGHI", s.Substring(0, 9), "Checking first few chars of string");

                Assert.IsTrue(reader.Read());
                Assert.AreEqual(DBNull.Value, reader.GetValue(2));
                Assert.AreEqual("This is my text value", reader.GetString(1));
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
        public void BlobBiggerThanMaxPacket()
        {
            execSQL("set max_allowed_packet=500000");

            execSQL("DROP TABLE IF EXISTS test");
            execSQL("CREATE TABLE test (id INT(10), image BLOB)");

            MySqlConnection c = new MySqlConnection(GetConnectionString(true));
            try
            {
                c.Open();
                MySqlCommand cmd = new MySqlCommand("SET max_allowed_packet=500000", c);
                cmd.ExecuteNonQuery();

                var image = Utils.CreateBlob(1000000);
                cmd.CommandText = "INSERT INTO test VALUES(NULL, ?image)";
                cmd.Parameters.AddWithValue("?image", image);
                cmd.ExecuteNonQuery();
                Assert.Fail("This should have thrown an exception");
            }
            catch (Exception)
            {
                Assert.AreEqual(ConnectionState.Open, c.State);
            }
            finally
            {
                c.Close();
            }
        }

        [Test]
        public void GetChars()
        {
            InternalGetChars(false);
        }

        [Test]
        public void GetCharsOnLongTextColumn()
        {
            execSQL("INSERT INTO Test (id, text1) VALUES(1, 'Test')");

            MySqlCommand cmd = new MySqlCommand("SELECT id, text1 FROM Test", conn);
            MySqlDataReader reader = null;
            try
            {
                var buf = new char[2];

                reader = cmd.ExecuteReader();
                reader.Read();
                reader.GetChars(1, 0, buf, 0, 2);
                Assert.AreEqual('T', buf[0]);
                Assert.AreEqual('e', buf[1]);
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
        public void GetCharsPrepared()
        {
            if (version < new Version(4, 1))
                return;

            InternalGetChars(true);
        }

        [Test]
        public void InsertBinary()
        {
            const int lenIn = 400000;
            var dataIn = Utils.CreateBlob(lenIn);

            MySqlCommand cmd2 = new MySqlCommand("SELECT * FROM Test", conn);
            MySqlDataReader reader2 = cmd2.ExecuteReader();
            reader2.Read();
            reader2.Close();

            MySqlCommand cmd = new MySqlCommand("TRUNCATE TABLE Test", conn);
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO Test VALUES (?id, ?b1, NULL)";
            cmd.Parameters.Add(new MySqlParameter("?id", 1));
            cmd.Parameters.Add(new MySqlParameter("?b1", dataIn));
            int rows = cmd.ExecuteNonQuery();

            var dataIn2 = Utils.CreateBlob(lenIn);
            cmd.Parameters[0].Value = 2;
            cmd.Parameters[1].Value = dataIn2;
            rows += cmd.ExecuteNonQuery();

            Assert.AreEqual(2, rows, "Checking insert rowcount");

            MySqlDataReader reader = null;
            try
            {
                cmd.CommandText = "SELECT * FROM Test";
                reader = cmd.ExecuteReader();
                Assert.AreEqual(true, reader.HasRows, "Checking HasRows");

                reader.Read();

                var dataOut = new byte[lenIn];
                long lenOut = reader.GetBytes(1, 0, dataOut, 0, lenIn);

                Assert.AreEqual(lenIn, lenOut, "Checking length of binary data (row 1)");

                // now see if the buffer is intact
                for (int x = 0; x < dataIn.Length; x++)
                {
                    Assert.AreEqual(dataIn[x], dataOut[x], "Checking first binary array at " + x);
                }

                // now we test chunking
                int pos = 0;
                int lenToRead = dataIn.Length;
                while (lenToRead > 0)
                {
                    int size = Math.Min(lenToRead, 1024);
                    int read = (int)reader.GetBytes(1, pos, dataOut, pos, size);
                    lenToRead -= read;
                    pos += read;
                }
                // now see if the buffer is intact
                for (int x = 0; x < dataIn.Length; x++)
                {
                    Assert.AreEqual(dataIn[x], dataOut[x], "Checking first binary array at " + x);
                }

                reader.Read();
                lenOut = reader.GetBytes(1, 0, dataOut, 0, lenIn);
                Assert.AreEqual(lenIn, lenOut, "Checking length of binary data (row 2)");

                // now see if the buffer is intact
                for (int x = 0; x < dataIn2.Length; x++)
                {
                    Assert.AreEqual(dataIn2[x], dataOut[x], "Checking second binary array at " + x);
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
        public void InsertText()
        {
            InternalInsertText(false);
        }

        [Test]
        public void InsertTextPrepared()
        {
            if (version < new Version(4, 1))
                return;

            InternalInsertText(true);
        }

        [Test]
        public void MediumIntBlobSize()
        {
            execSQL("DROP TABLE IF EXISTS test");
            execSQL("CREATE TABLE test (id INT(10) UNSIGNED NOT NULL AUTO_INCREMENT, " +
                    "image MEDIUMBLOB NOT NULL, imageSize MEDIUMINT(8) UNSIGNED NOT NULL DEFAULT 0, " + "PRIMARY KEY (id))");

            var image = new byte[2048];
            for (int x = 0; x < image.Length; x++)
            {
                image[x] = (byte)(x % 47);
            }
            MySqlCommand cmd = new MySqlCommand("INSERT INTO test VALUES(NULL, ?image, ?size)", conn);
            cmd.Parameters.AddWithValue("?image", image);
            cmd.Parameters.AddWithValue("?size", image.Length);
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT imageSize, length(image), image FROM test WHERE id=?id";
            cmd.Parameters.AddWithValue("?id", 1);
            cmd.Prepare();

            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                reader.Read();
                uint actualsize = reader.GetUInt32(1);
                Assert.AreEqual(image.Length, actualsize);
                uint size = reader.GetUInt32(0);
                var outImage = new byte[size];
                long len = reader.GetBytes(reader.GetOrdinal("image"), 0, outImage, 0, (int)size);
                Assert.AreEqual(image.Length, size);
                Assert.AreEqual(image.Length, len);
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
        public void UpdateDataSet()
        {
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (id INT NOT NULL, blob1 LONGBLOB, text1 LONGTEXT, PRIMARY KEY(id))");
            execSQL("INSERT INTO Test VALUES( 1, NULL, 'Text field' )");

            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM Test", conn);
                MySqlCommandBuilder cb = new MySqlCommandBuilder(da);
                DataTable dt = new DataTable();
                da.Fill(dt);

                string s = (string)dt.Rows[0][2];
                Assert.AreEqual("Text field", s);

                var inBuf = Utils.CreateBlob(512);
                dt.Rows[0].BeginEdit();
                dt.Rows[0]["blob1"] = inBuf;
                dt.Rows[0].EndEdit();
                DataTable changes = dt.GetChanges();
                da.Update(changes);
                dt.AcceptChanges();

                dt.Clear();
                da.Fill(dt);
                cb.Dispose();

                var outBuf = (byte[])dt.Rows[0]["blob1"];
                Assert.AreEqual(inBuf.Length, outBuf.Length, "checking length of updated buffer");
                for (int y = 0; y < inBuf.Length; y++)
                {
                    Assert.AreEqual(inBuf[y], outBuf[y], "checking array data");
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }

    [Category("Compressed")]
    public class BlobTestsSocketCompressed : BlobTests
    {
        protected override string GetConnectionInfo()
        {
            return String.Format("port={0};compress=true", port);
        }
    }

    [Category("Pipe")]
    public class BlobTestsPipe : BlobTests
    {
        protected override string GetConnectionInfo()
        {
            return String.Format("protocol=pipe;pipe name={0}", pipeName);
        }
    }

    [Category("Compressed")]
    [Category("Pipe")]
    public class BlobTestsPipeCompressed : BlobTests
    {
        protected override string GetConnectionInfo()
        {
            return String.Format("protocol=pipe;pipe name={0};compress=true", pipeName);
        }
    }

    [Category("SharedMemory")]
    public class BlobTestsSharedMemory : BlobTests
    {
        protected override string GetConnectionInfo()
        {
            return String.Format("protocol=memory; shared memory name={0}", memoryName);
        }
    }

    [Category("Compressed")]
    [Category("SharedMemory")]
    public class BlobTestsSharedMemoryCompressed : BlobTests
    {
        protected override string GetConnectionInfo()
        {
            return String.Format("protocol=memory; shared memory name={0};compress=true", memoryName);
        }
    }
}