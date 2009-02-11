using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    [TestFixture]
    public class ProcedureParameterTests : BaseTest
    {
        [Test]
        public void DTD_Identifier()
        {
            if (version < new Version(5, 0))
                return;

            execSQL(
                @"CREATE  PROCEDURE spTest (id INT UNSIGNED ZEROFILL,
                    dec1 DECIMAL(10,2), 
                    name VARCHAR(20) /* this is a comment */ CHARACTER SET ascii,
                    t1 TINYTEXT BINARY, t2 ENUM('a','b','c'),
                    t3 /* comment */ SET(/* comment */'1','2','3'))
                    BEGIN SELECT name; END");

            var restrictions = new string[5];
            restrictions[1] = database0;
            restrictions[2] = "spTest";
            DataTable dt = conn.GetSchema("Procedure Parameters", restrictions);

            Assert.IsTrue(dt.Rows.Count == 6);
            Assert.AreEqual("INT(10) UNSIGNED ZEROFILL", dt.Rows[0]["DTD_IDENTIFIER"].ToString().ToUpper());
            Assert.AreEqual("DECIMAL(10,2)", dt.Rows[1]["DTD_IDENTIFIER"].ToString().ToUpper());
            Assert.AreEqual("VARCHAR(20)", dt.Rows[2]["DTD_IDENTIFIER"].ToString().ToUpper());
            Assert.AreEqual("TINYTEXT", dt.Rows[3]["DTD_IDENTIFIER"].ToString().ToUpper());
            Assert.AreEqual("ENUM('A','B','C')", dt.Rows[4]["DTD_IDENTIFIER"].ToString().ToUpper());
            Assert.AreEqual("SET('1','2','3')", dt.Rows[5]["DTD_IDENTIFIER"].ToString().ToUpper());
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

        /// <summary>
        /// Bug #6902 Errors in parsing stored procedure parameters 
        /// </summary>
        [Test]
        public void ProcedureParameters2()
        {
            if (version < new Version(5, 0))
                return;

            execSQL(
                @"CREATE PROCEDURE spTest(`/*id*/` /* before type 1 */ varchar(20), 
                     /* after type 1 */ OUT result2 DECIMAL(/*size1*/10,/*size2*/2) /* p2 */) 
                     BEGIN SELECT action, result; END");

            var restrictions = new string[5];
            restrictions[1] = database0;
            restrictions[2] = "spTest";
            DataTable dt = conn.GetSchema("Procedure Parameters", restrictions);

            Assert.IsTrue(dt.Rows.Count == 2);
            Assert.AreEqual(database0.ToLower(), dt.Rows[0]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("sptest", dt.Rows[0]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual("/*id*/", dt.Rows[0]["PARAMETER_NAME"].ToString().ToLower());
            Assert.AreEqual(1, dt.Rows[0]["ORDINAL_POSITION"]);
            Assert.AreEqual("IN", dt.Rows[0]["PARAMETER_MODE"]);
            Assert.AreEqual("VARCHAR", dt.Rows[0]["DATA_TYPE"].ToString().ToUpper());
            Assert.AreEqual(20, dt.Rows[0]["CHARACTER_MAXIMUM_LENGTH"]);
            Assert.AreEqual(20, dt.Rows[0]["CHARACTER_OCTET_LENGTH"]);

            Assert.AreEqual(database0.ToLower(), dt.Rows[1]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("sptest", dt.Rows[1]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual("result2", dt.Rows[1]["PARAMETER_NAME"].ToString().ToLower());
            Assert.AreEqual(2, dt.Rows[1]["ORDINAL_POSITION"]);
            Assert.AreEqual("OUT", dt.Rows[1]["PARAMETER_MODE"]);
            Assert.AreEqual("DECIMAL", dt.Rows[1]["DATA_TYPE"].ToString().ToUpper());
            Assert.AreEqual(10, dt.Rows[1]["NUMERIC_PRECISION"]);
            Assert.AreEqual(2, dt.Rows[1]["NUMERIC_SCALE"]);
        }

        [Test]
        public void ProcedureParameters3()
        {
            if (version < new Version(5, 0))
                return;

            execSQL(
                @"CREATE  PROCEDURE spTest (_ACTION varchar(20),
                    `/*dumb-identifier-1*/` int, `#dumb-identifier-2` int,
                    `--dumb-identifier-3` int, 
                    _CLIENT_ID int, -- ABC
                    _LOGIN_ID  int, # DEF
                    _WHERE varchar(2000), 
                    _SORT varchar(2000),
                    out _SQL varchar(/* inline right here - oh my gosh! */ 8000),
                    _SONG_ID int,
                    _NOTES varchar(2000),
                    out _RESULT varchar(10)
                    /*
                    ,    -- Generic result parameter
                    out _PERIOD_ID int,         -- Returns the period_id. Useful when using @PREDEFLINK to return which is the last period
                    _SONGS_LIST varchar(8000),
                    _COMPOSERID int,
                    _PUBLISHERID int,
                    _PREDEFLINK int        -- If the user is accessing through a predefined link: 0=none  1=last period
                    */) BEGIN SELECT 1; END");

            var restrictions = new string[5];
            restrictions[1] = database0;
            restrictions[2] = "spTest";
            DataTable dt = conn.GetSchema("Procedure Parameters", restrictions);

            Assert.IsTrue(dt.Rows.Count == 12);
            Assert.AreEqual(database0.ToLower(), dt.Rows[0]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("sptest", dt.Rows[0]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual("_action", dt.Rows[0]["PARAMETER_NAME"].ToString().ToLower());
            Assert.AreEqual(1, dt.Rows[0]["ORDINAL_POSITION"]);
            Assert.AreEqual("IN", dt.Rows[0]["PARAMETER_MODE"]);
            Assert.AreEqual("VARCHAR", dt.Rows[0]["DATA_TYPE"].ToString().ToUpper());
            Assert.AreEqual(20, dt.Rows[0]["CHARACTER_OCTET_LENGTH"]);

            Assert.AreEqual(database0.ToLower(), dt.Rows[1]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("sptest", dt.Rows[1]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual("/*dumb-identifier-1*/", dt.Rows[1]["PARAMETER_NAME"].ToString().ToLower());
            Assert.AreEqual(2, dt.Rows[1]["ORDINAL_POSITION"]);
            Assert.AreEqual("IN", dt.Rows[1]["PARAMETER_MODE"]);
            Assert.AreEqual("INT", dt.Rows[1]["DATA_TYPE"].ToString().ToUpper());

            Assert.AreEqual(database0.ToLower(), dt.Rows[2]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("sptest", dt.Rows[2]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual("#dumb-identifier-2", dt.Rows[2]["PARAMETER_NAME"].ToString().ToLower());
            Assert.AreEqual(3, dt.Rows[2]["ORDINAL_POSITION"]);
            Assert.AreEqual("IN", dt.Rows[2]["PARAMETER_MODE"]);
            Assert.AreEqual("INT", dt.Rows[2]["DATA_TYPE"].ToString().ToUpper());

            Assert.AreEqual(database0.ToLower(), dt.Rows[3]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("sptest", dt.Rows[3]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual("--dumb-identifier-3", dt.Rows[3]["PARAMETER_NAME"].ToString().ToLower());
            Assert.AreEqual(4, dt.Rows[3]["ORDINAL_POSITION"]);
            Assert.AreEqual("IN", dt.Rows[3]["PARAMETER_MODE"]);
            Assert.AreEqual("INT", dt.Rows[3]["DATA_TYPE"].ToString().ToUpper());

            Assert.AreEqual(database0.ToLower(), dt.Rows[4]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("sptest", dt.Rows[4]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual("_client_id", dt.Rows[4]["PARAMETER_NAME"].ToString().ToLower());
            Assert.AreEqual(5, dt.Rows[4]["ORDINAL_POSITION"]);
            Assert.AreEqual("IN", dt.Rows[4]["PARAMETER_MODE"]);
            Assert.AreEqual("INT", dt.Rows[4]["DATA_TYPE"].ToString().ToUpper());

            Assert.AreEqual(database0.ToLower(), dt.Rows[5]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("sptest", dt.Rows[5]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual("_login_id", dt.Rows[5]["PARAMETER_NAME"].ToString().ToLower());
            Assert.AreEqual(6, dt.Rows[5]["ORDINAL_POSITION"]);
            Assert.AreEqual("IN", dt.Rows[5]["PARAMETER_MODE"]);
            Assert.AreEqual("INT", dt.Rows[5]["DATA_TYPE"].ToString().ToUpper());

            Assert.AreEqual(database0.ToLower(), dt.Rows[6]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("sptest", dt.Rows[6]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual("_where", dt.Rows[6]["PARAMETER_NAME"].ToString().ToLower());
            Assert.AreEqual(7, dt.Rows[6]["ORDINAL_POSITION"]);
            Assert.AreEqual("IN", dt.Rows[6]["PARAMETER_MODE"]);
            Assert.AreEqual("VARCHAR", dt.Rows[6]["DATA_TYPE"].ToString().ToUpper());
            Assert.AreEqual(2000, dt.Rows[6]["CHARACTER_OCTET_LENGTH"]);

            Assert.AreEqual(database0.ToLower(), dt.Rows[7]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("sptest", dt.Rows[7]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual("_sort", dt.Rows[7]["PARAMETER_NAME"].ToString().ToLower());
            Assert.AreEqual(8, dt.Rows[7]["ORDINAL_POSITION"]);
            Assert.AreEqual("IN", dt.Rows[7]["PARAMETER_MODE"]);
            Assert.AreEqual("VARCHAR", dt.Rows[7]["DATA_TYPE"].ToString().ToUpper());
            Assert.AreEqual(2000, dt.Rows[7]["CHARACTER_OCTET_LENGTH"]);

            Assert.AreEqual(database0.ToLower(), dt.Rows[8]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("sptest", dt.Rows[8]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual("_sql", dt.Rows[8]["PARAMETER_NAME"].ToString().ToLower());
            Assert.AreEqual(9, dt.Rows[8]["ORDINAL_POSITION"]);
            Assert.AreEqual("OUT", dt.Rows[8]["PARAMETER_MODE"]);
            Assert.AreEqual("VARCHAR", dt.Rows[8]["DATA_TYPE"].ToString().ToUpper());
            Assert.AreEqual(8000, dt.Rows[8]["CHARACTER_OCTET_LENGTH"]);

            Assert.AreEqual(database0.ToLower(), dt.Rows[9]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("sptest", dt.Rows[9]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual("_song_id", dt.Rows[9]["PARAMETER_NAME"].ToString().ToLower());
            Assert.AreEqual(10, dt.Rows[9]["ORDINAL_POSITION"]);
            Assert.AreEqual("IN", dt.Rows[9]["PARAMETER_MODE"]);
            Assert.AreEqual("INT", dt.Rows[9]["DATA_TYPE"].ToString().ToUpper());

            Assert.AreEqual(database0.ToLower(), dt.Rows[10]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("sptest", dt.Rows[10]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual("_notes", dt.Rows[10]["PARAMETER_NAME"].ToString().ToLower());
            Assert.AreEqual(11, dt.Rows[10]["ORDINAL_POSITION"]);
            Assert.AreEqual("IN", dt.Rows[10]["PARAMETER_MODE"]);
            Assert.AreEqual("VARCHAR", dt.Rows[10]["DATA_TYPE"].ToString().ToUpper());
            Assert.AreEqual(2000, dt.Rows[10]["CHARACTER_OCTET_LENGTH"]);

            Assert.AreEqual(database0.ToLower(), dt.Rows[11]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("sptest", dt.Rows[11]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual("_result", dt.Rows[11]["PARAMETER_NAME"].ToString().ToLower());
            Assert.AreEqual(12, dt.Rows[11]["ORDINAL_POSITION"]);
            Assert.AreEqual("OUT", dt.Rows[11]["PARAMETER_MODE"]);
            Assert.AreEqual("VARCHAR", dt.Rows[11]["DATA_TYPE"].ToString().ToUpper());
            Assert.AreEqual(10, dt.Rows[11]["CHARACTER_OCTET_LENGTH"]);
        }

        [Test]
        public void ProcedureParameters4()
        {
            if (version < new Version(5, 0))
                return;

            execSQL(
                @"CREATE  PROCEDURE spTest (name VARCHAR(1200) 
                    CHARACTER /* hello*/ SET utf8) BEGIN SELECT name; END");

            var restrictions = new string[5];
            restrictions[1] = database0;
            restrictions[2] = "spTest";
            DataTable dt = conn.GetSchema("Procedure Parameters", restrictions);

            Assert.IsTrue(dt.Rows.Count == 1);
            Assert.AreEqual(database0.ToLower(), dt.Rows[0]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("sptest", dt.Rows[0]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual("name", dt.Rows[0]["PARAMETER_NAME"].ToString().ToLower());
            Assert.AreEqual(1, dt.Rows[0]["ORDINAL_POSITION"]);
            Assert.AreEqual("IN", dt.Rows[0]["PARAMETER_MODE"]);
            Assert.AreEqual("VARCHAR", dt.Rows[0]["DATA_TYPE"].ToString().ToUpper());
            Assert.AreEqual(1200, dt.Rows[0]["CHARACTER_MAXIMUM_LENGTH"]);
            if (Version.Major >= 6)
                Assert.AreEqual(4800, dt.Rows[0]["CHARACTER_OCTET_LENGTH"]);
            else
                Assert.AreEqual(3600, dt.Rows[0]["CHARACTER_OCTET_LENGTH"]);
            Assert.AreEqual("utf8", dt.Rows[0]["CHARACTER_SET_NAME"]);
            Assert.AreEqual("utf8_general_ci", dt.Rows[0]["COLLATION_NAME"]);
        }

        [Test]
        public void ProcedureParameters5()
        {
            if (version < new Version(5, 0))
                return;

            execSQL(
                @"CREATE  PROCEDURE spTest (name VARCHAR(1200) ASCII BINARY, 
                    name2 TEXT UNICODE) BEGIN SELECT name; END");

            var restrictions = new string[5];
            restrictions[1] = database0;
            restrictions[2] = "spTest";
            DataTable dt = conn.GetSchema("Procedure Parameters", restrictions);

            Assert.IsTrue(dt.Rows.Count == 2);
            Assert.AreEqual(database0.ToLower(), dt.Rows[0]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("sptest", dt.Rows[0]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual("name", dt.Rows[0]["PARAMETER_NAME"].ToString().ToLower());
            Assert.AreEqual(1, dt.Rows[0]["ORDINAL_POSITION"]);
            Assert.AreEqual("IN", dt.Rows[0]["PARAMETER_MODE"]);
            Assert.AreEqual("VARCHAR", dt.Rows[0]["DATA_TYPE"].ToString().ToUpper());
            Assert.AreEqual("latin1", dt.Rows[0]["CHARACTER_SET_NAME"]);
            Assert.AreEqual(1200, dt.Rows[0]["CHARACTER_OCTET_LENGTH"]);

            Assert.AreEqual(database0.ToLower(), dt.Rows[1]["SPECIFIC_SCHEMA"].ToString().ToLower());
            Assert.AreEqual("sptest", dt.Rows[1]["SPECIFIC_NAME"].ToString().ToLower());
            Assert.AreEqual("name2", dt.Rows[1]["PARAMETER_NAME"].ToString().ToLower());
            Assert.AreEqual(2, dt.Rows[1]["ORDINAL_POSITION"]);
            Assert.AreEqual("IN", dt.Rows[1]["PARAMETER_MODE"]);
            Assert.AreEqual("TEXT", dt.Rows[1]["DATA_TYPE"].ToString().ToUpper());
            Assert.AreEqual("ucs2", dt.Rows[1]["CHARACTER_SET_NAME"]);
        }
    }
}