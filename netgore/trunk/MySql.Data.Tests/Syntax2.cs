using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    [TestFixture]
    public class Syntax2 : BaseTest
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
        public void CommentsInSQL()
        {
            string sql = "INSERT INTO Test /* my table */ VALUES (1 /* this is the id */, 'Test' );" +
                         "/* These next inserts are just for testing \r\n" + "   comments */\r\n" + "INSERT INTO \r\n" +
                         "  # This table is bogus\r\n" + "Test VALUES (2, 'Test2')";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();

            MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM Test", conn);
            DataTable table = new DataTable();
            da.Fill(table);
            Assert.AreEqual(1, table.Rows[0]["id"]);
            Assert.AreEqual("Test", table.Rows[0]["name"]);
            Assert.AreEqual(2, table.Rows.Count);
            Assert.AreEqual(2, table.Rows[1]["id"]);
            Assert.AreEqual("Test2", table.Rows[1]["name"]);
        }

        [Test]
        public void LastInsertid()
        {
            execSQL("DROP TABLE Test");
            execSQL("CREATE TABLE Test(id int auto_increment, name varchar(20), primary key(id))");
            MySqlCommand cmd = new MySqlCommand("INSERT INTO Test VALUES(NULL, 'test')", conn);
            cmd.ExecuteNonQuery();
            Assert.AreEqual(1, cmd.LastInsertedId);

            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
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
            Assert.AreEqual(2, cmd.LastInsertedId);

            cmd.CommandText = "SELECT id FROM Test";
            cmd.ExecuteScalar();
            Assert.AreEqual(-1, cmd.LastInsertedId);
        }

/*        [Category("NotWorking")]
        [Test]
        public void TestCase()
        {
            string importQuery = "SET FOREIGN_KEY_CHECKS = 1;DELETE FROM Category " +
                "WHERE id=\'0205342903\';SET FOREIGN_KEY_CHECKS = 0;INSERT INTO Category " +
                "VALUES(\'d0450f050a0dfd8e00e6da7bda3bb07e\',\'0205342903\',\'000000000000000 " +
                "00000000000000000\',\'\',\'0\');INSERT INTO Attribute " +
                "VALUES(\'d0450f050a0dfd8e00e6da7b00dfa3c5\',\'d0450f050a0dfd8e00e6da7bda3bb0 " +
                "7e\',\'eType\',\'machine\',null);SET FOREIGN_KEY_CHECKS = 1;";
            string deleteQuery = "SET FOREIGN_KEY_CHECKS=1;DELETE FROM Attribute " +
                "WHERE foreignuuid=\'d0450f050a0dfd8e00e6da7bda3bb07e\' AND " +
                "name=\'eType\'";
            string insertQuery = "SET FOREIGN_KEY_CHECKS = 0;INSERT INTO Attribute " +
                "VALUES(\'d0563ba70a0dfd8e01df43e22395b352\',\'d0450f050a0dfd8e00e6da7bda3bb0 " +
                "7e\',\'eType\',\'machine\',null);SET FOREIGN_KEY_CHECKS = 1";
            string updateQuery = "SET FOREIGN_KEY_CHECKS = 1;DELETE FROM Attribute " +
                "WHERE foreignuuid=\'d0450f050a0dfd8e00e6da7bda3bb07e\' AND " + 
                "name=\'eType\';SET FOREIGN_KEY_CHECKS = 0;INSERT INTO Attribute " + 
                "VALUES(\'d0563ba70a0dfd8e01df43e22395b352\',\'d0450f050a0dfd8e00e6da7bda3bb0 " +
                "7e\',\'eType\',\'machine\',null);SET FOREIGN_KEY_CHECKS = 1;";
            string bugQuery = "SELECT name,value FROM Attribute WHERE " +
                "foreignuuid=\'d0450f050a0dfd8e00e6da7bda3bb07e\'";

            execSQL("SET FOREIGN_KEY_CHECKS=0");
            execSQL("DROP TABLE IF EXISTS Attribute");
            execSQL("CREATE TABLE IF NOT EXISTS Attribute (uuid char(32) NOT NULL," +
                "foreignuuid char(32), name character varying(254), value character varying(254)," +
                "fid integer, PRIMARY KEY (uuid), INDEX foreignuuid (foreignuuid), " +
                "INDEX name (name(16)), INDEX value (value(8)), CONSTRAINT `attribute_fk_1` " +
                "FOREIGN KEY (`foreignuuid`) REFERENCES `Category` (`uuid`) ON DELETE CASCADE" +
                ") CHARACTER SET utf8 ENGINE=InnoDB;");

            execSQL("DROP TABLE IF EXISTS Category");
            execSQL("CREATE TABLE IF NOT EXISTS Category (uuid char(32) NOT NULL," +
                "id character varying(254), parentuuid char(32), name character varying(254)," +
                "sort integer, PRIMARY KEY (uuid), INDEX parentuuid (parentuuid), INDEX id (id)," +
                "CONSTRAINT `parent_fk_1` FOREIGN KEY (`parentuuid`) REFERENCES `Category` " +
                "(`uuid`) ON DELETE CASCADE) CHARACTER SET utf8 ENGINE=InnoDB;");
            execSQL("SET FOREIGN_KEY_CHECKS=1");

            try
            {
                conn.InfoMessage += new MySqlInfoMessageEventHandler(conn_InfoMessage);
                MySqlCommand cmd = new MySqlCommand(importQuery, conn);
                cmd.ExecuteNonQuery();

                for (int i = 0; i <= 5000; i++)
                {
                    cmd.CommandText = deleteQuery;
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = insertQuery;
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = bugQuery;
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        void conn_InfoMessage(object sender, MySqlInfoMessageEventArgs args)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        */
    }
}