using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    [TestFixture]
    public class BulkLoading : BaseTest
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
        public void BulkLoadColumnOrder()
        {
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL(
                @"CREATE TABLE Test (id INT NOT NULL, n1 VARCHAR(250), n2 VARCHAR(250), 
                        n3 VARCHAR(250), PRIMARY KEY(id))");

            // first create the external file
            string path = Path.GetTempFileName();
            StreamWriter sw = new StreamWriter(path);
            for (int i = 0; i < 20; i++)
            {
                sw.WriteLine(i + ",col3,col2,col1");
            }
            sw.Flush();
            sw.Close();

            MySqlBulkLoader loader = new MySqlBulkLoader(conn)
                                     {
                                         TableName = "Test", FileName = path, Timeout = 0, FieldTerminator = ",",
                                         LineTerminator = Environment.NewLine
                                     };
            loader.Columns.Add("id");
            loader.Columns.Add("n3");
            loader.Columns.Add("n2");
            loader.Columns.Add("n1");
            int count = loader.Load();
            Assert.AreEqual(20, count);

            MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM Test", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Assert.AreEqual(20, dt.Rows.Count);
            Assert.AreEqual("col1", dt.Rows[0][1]);
            Assert.AreEqual("col2", dt.Rows[0][2]);
            Assert.AreEqual("col3", dt.Rows[0][3].ToString().Trim());
        }

        [Test]
        public void BulkLoadConflictOptionIgnore()
        {
            // first create the external file
            string path = Path.GetTempFileName();
            StreamWriter sw = new StreamWriter(path);
            for (int i = 0; i < 20; i++)
            {
                sw.WriteLine(i + ",col1");
            }
            sw.Flush();
            sw.Close();

            MySqlBulkLoader loader = new MySqlBulkLoader(conn)
                                     { TableName = "Test", FileName = path, Timeout = 0, FieldTerminator = "," };
            int count = loader.Load();
            Assert.AreEqual(20, count);

            path = Path.GetTempFileName();
            sw = new StreamWriter(path);
            for (int i = 0; i < 20; i++)
            {
                sw.WriteLine(i + ",col2");
            }
            sw.Flush();
            sw.Close();

            loader = new MySqlBulkLoader(conn)
                     {
                         TableName = "Test", FileName = path, Timeout = 0, FieldTerminator = ",",
                         ConflictOption = MySqlBulkLoaderConflictOption.Ignore
                     };
            loader.Load();

            MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM Test", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Assert.AreEqual(20, dt.Rows.Count);
            Assert.AreEqual("col1", dt.Rows[0][1].ToString().Trim());
        }

        [Test]
        public void BulkLoadConflictOptionReplace()
        {
            // first create the external file
            string path = Path.GetTempFileName();
            StreamWriter sw = new StreamWriter(path);
            for (int i = 0; i < 20; i++)
            {
                sw.WriteLine(i + ",col1");
            }
            sw.Flush();
            sw.Close();

            MySqlBulkLoader loader = new MySqlBulkLoader(conn)
                                     { TableName = "Test", FileName = path, Timeout = 0, FieldTerminator = "," };
            int count = loader.Load();
            Assert.AreEqual(20, count);

            path = Path.GetTempFileName();
            sw = new StreamWriter(path);
            for (int i = 0; i < 20; i++)
            {
                sw.WriteLine(i + ",col2");
            }
            sw.Flush();
            sw.Close();

            loader = new MySqlBulkLoader(conn)
                     {
                         TableName = "Test", FileName = path, Timeout = 0, FieldTerminator = ",",
                         ConflictOption = MySqlBulkLoaderConflictOption.Replace
                     };
            loader.Load();

            MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM Test", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Assert.AreEqual(20, dt.Rows.Count);
            Assert.AreEqual("col2", dt.Rows[0][1].ToString().Trim());
        }

        [Test]
        public void BulkLoadEscaping()
        {
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (id INT NOT NULL, name VARCHAR(250), name2 VARCHAR(250), PRIMARY KEY(id))");

            // first create the external file
            string path = Path.GetTempFileName();
            StreamWriter sw = new StreamWriter(path);
            for (int i = 0; i < 200; i++)
            {
                sw.WriteLine(i + ",col1\tstill col1,col2");
            }
            sw.Flush();
            sw.Close();

            MySqlBulkLoader loader = new MySqlBulkLoader(conn)
                                     {
                                         TableName = "Test", FileName = path, Timeout = 0, EscapeCharacter = '\t',
                                         FieldTerminator = ","
                                     };
            int count = loader.Load();
            Assert.AreEqual(200, count);

            MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM Test", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Assert.AreEqual(200, dt.Rows.Count);
            Assert.AreEqual("col1still col1", dt.Rows[0][1]);
            Assert.AreEqual("col2", dt.Rows[0][2].ToString().Trim());
        }

        [Test]
        public void BulkLoadFieldQuoting()
        {
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (id INT NOT NULL, name VARCHAR(250), name2 VARCHAR(250), PRIMARY KEY(id))");

            // first create the external file
            string path = Path.GetTempFileName();
            StreamWriter sw = new StreamWriter(path);
            for (int i = 0; i < 200; i++)
            {
                sw.WriteLine(i + "\t`col1`\tcol2");
            }
            sw.Flush();
            sw.Close();

            MySqlBulkLoader loader = new MySqlBulkLoader(conn)
                                     {
                                         TableName = "Test", FileName = path, Timeout = 0, FieldQuotationCharacter = '`',
                                         FieldQuotationOptional = true
                                     };
            int count = loader.Load();
            Assert.AreEqual(200, count);

            MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM Test", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Assert.AreEqual(200, dt.Rows.Count);
            Assert.AreEqual("col1", dt.Rows[0][1]);
            Assert.AreEqual("col2", dt.Rows[0][2].ToString().Trim());
        }

        [Test]
        public void BulkLoadSimple()
        {
            // first create the external file
            string path = Path.GetTempFileName();
            StreamWriter sw = new StreamWriter(path);
            for (int i = 0; i < 200; i++)
            {
                sw.WriteLine(i + "\t'Test'");
            }
            sw.Flush();
            sw.Close();

            MySqlBulkLoader loader = new MySqlBulkLoader(conn) { TableName = "Test", FileName = path, Timeout = 0 };
            int count = loader.Load();
            Assert.AreEqual(200, count);

            MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM Test", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Assert.AreEqual(200, dt.Rows.Count);
            Assert.AreEqual("'Test'", dt.Rows[0][1].ToString().Trim());
        }

        [Test]
        public void BulkLoadSimple2()
        {
            // first create the external file
            string path = Path.GetTempFileName();
            StreamWriter sw = new StreamWriter(path);
            for (int i = 0; i < 200; i++)
            {
                sw.Write(i + ",'Test' xxx");
            }
            sw.Flush();
            sw.Close();

            MySqlBulkLoader loader = new MySqlBulkLoader(conn)
                                     {
                                         TableName = "Test", FileName = path, Timeout = 0, FieldTerminator = ",",
                                         LineTerminator = "xxx"
                                     };
            int count = loader.Load();
            Assert.AreEqual(200, count);

            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM Test", conn);
            Assert.AreEqual(200, cmd.ExecuteScalar());
        }

        [Test]
        public void BulkLoadSimple3()
        {
            // first create the external file
            string path = Path.GetTempFileName();
            StreamWriter sw = new StreamWriter(path);
            for (int i = 0; i < 200; i++)
            {
                sw.Write(i + ",'Test' xxx");
            }
            sw.Flush();
            sw.Close();

            MySqlBulkLoader loader = new MySqlBulkLoader(conn)
                                     {
                                         TableName = "Test", FileName = path, Timeout = 0, FieldTerminator = ",",
                                         LineTerminator = "xxx", NumberOfLinesToSkip = 50
                                     };
            int count = loader.Load();
            Assert.AreEqual(150, count);

            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM Test", conn);
            Assert.AreEqual(150, cmd.ExecuteScalar());
        }

        [Test]
        public void BulkLoadSimple4()
        {
            // first create the external file
            string path = Path.GetTempFileName();
            StreamWriter sw = new StreamWriter(path);
            for (int i = 0; i < 100; i++)
            {
                sw.Write("aaa" + i + ",'Test' xxx");
            }
            for (int i = 100; i < 200; i++)
            {
                sw.Write("bbb" + i + ",'Test' xxx");
            }
            for (int i = 200; i < 300; i++)
            {
                sw.Write("aaa" + i + ",'Test' xxx");
            }
            for (int i = 300; i < 400; i++)
            {
                sw.Write("bbb" + i + ",'Test' xxx");
            }
            sw.Flush();
            sw.Close();

            MySqlBulkLoader loader = new MySqlBulkLoader(conn)
                                     {
                                         TableName = "Test", FileName = path, Timeout = 0, FieldTerminator = ",",
                                         LineTerminator = "xxx", LinePrefix = "bbb"
                                     };
            int count = loader.Load();
            Assert.AreEqual(200, count);

            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM Test", conn);
            Assert.AreEqual(200, cmd.ExecuteScalar());
        }
    }
}