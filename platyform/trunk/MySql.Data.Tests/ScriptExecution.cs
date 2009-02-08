using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    [TestFixture]
    public class ScriptExecution : BaseTest
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

        int statementCount;

        string statementTemplate1 =
            @"CREATE PROCEDURE `spTest{0}`() NOT DETERMINISTIC
					CONTAINS SQL SQL SECURITY DEFINER COMMENT '' 
					BEGIN
						SELECT 1,2,3;
					END{1}";

        void ExecuteScriptWithProcedures_QueryExecuted(object sender, MySqlScriptEventArgs e)
        {
            string stmt = String.Format(statementTemplate1, statementCount++, null);
            Assert.AreEqual(stmt, e.StatementText);
        }

        string statementTemplate2 = @"INSERT INTO Test (id, name) VALUES ({0}, 'a "" na;me'){1}";

        void ExecuteScriptWithInserts_StatementExecuted(object sender, MySqlScriptEventArgs e)
        {
            string stmt = String.Format(statementTemplate2, statementCount++, null);
            Assert.AreEqual(stmt, e.StatementText);
        }

        void ExecuteScript_ContinueOnError(object sender, MySqlScriptErrorEventArgs args)
        {
            args.Ignore = true;
            statementCount++;
        }

        void ExecuteScript_NotContinueOnError(object sender, MySqlScriptErrorEventArgs args)
        {
            args.Ignore = false;
            statementCount++;
        }

        [Test]
        public void ExecuteScriptContinueOnError()
        {
            statementCount = 0;
            string scriptText = String.Empty;
            for (int i = 0; i < 5; i++)
            {
                scriptText += String.Format(statementTemplate2, i, ";");
            }
            scriptText += "bogus statement;";
            for (int i = 5; i < 10; i++)
            {
                scriptText += String.Format(statementTemplate2, i, ";");
            }
            MySqlScript script = new MySqlScript(scriptText);
            script.Connection = conn;
            script.Error += ExecuteScript_ContinueOnError;
            int count = script.Execute();
            Assert.AreEqual(10, count);
            Assert.AreEqual(1, statementCount);

            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM Test", conn);
            Assert.AreEqual(10, cmd.ExecuteScalar());
        }

        [Test]
        public void ExecuteScriptNotContinueOnError()
        {
            statementCount = 0;
            string scriptText = String.Empty;
            for (int i = 0; i < 5; i++)
            {
                scriptText += String.Format(statementTemplate2, i, ";");
            }
            scriptText += "bogus statement;";
            for (int i = 5; i < 10; i++)
            {
                scriptText += String.Format(statementTemplate2, i, ";");
            }
            MySqlScript script = new MySqlScript(scriptText);
            script.Connection = conn;
            script.Error += ExecuteScript_NotContinueOnError;
            int count = script.Execute();
            Assert.AreEqual(5, count);
            Assert.AreEqual(1, statementCount);

            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM Test", conn);
            Assert.AreEqual(5, cmd.ExecuteScalar());
        }

        [Test]
        public void ExecuteScriptWithInserts()
        {
            statementCount = 0;
            string scriptText = String.Empty;
            for (int i = 0; i < 10; i++)
            {
                scriptText += String.Format(statementTemplate2, i, ";");
            }
            MySqlScript script = new MySqlScript(scriptText);
            script.Connection = conn;
            script.StatementExecuted += ExecuteScriptWithInserts_StatementExecuted;
            int count = script.Execute();
            Assert.AreEqual(10, count);

            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM Test", conn);
            Assert.AreEqual(10, cmd.ExecuteScalar());
        }

        [Test]
        public void ExecuteScriptWithProcedures()
        {
            if (version < new Version(5, 0))
                return;

            statementCount = 0;
            string scriptText = String.Empty;
            for (int i = 0; i < 10; i++)
            {
                scriptText += String.Format(statementTemplate1, i, "$$");
            }
            MySqlScript script = new MySqlScript(scriptText);
            script.StatementExecuted += ExecuteScriptWithProcedures_QueryExecuted;
            script.Connection = conn;
            script.Delimiter = "$$";
            int count = script.Execute();
            Assert.AreEqual(10, count);

            MySqlCommand cmd =
                new MySqlCommand(
                    String.Format(
                        @"SELECT COUNT(*) FROM information_schema.routines WHERE
                routine_schema = '{0}' AND routine_name LIKE 'spTest%'",
                        database0), conn);
            Assert.AreEqual(10, cmd.ExecuteScalar());
        }

        [Test]
        public void ExecuteScriptWithUserVariables()
        {
            string connStr = conn.ConnectionString.ToLowerInvariant();
            connStr = connStr.Replace("allow user variables=true", "allow user variables=false");
            using (MySqlConnection c = new MySqlConnection(connStr))
            {
                c.Open();
                string scriptText = "SET @myvar = 1";
                MySqlScript script = new MySqlScript(scriptText);
                script.Connection = c;
                int count = script.Execute();
                Assert.AreEqual(1, count);
            }
        }
    }
}