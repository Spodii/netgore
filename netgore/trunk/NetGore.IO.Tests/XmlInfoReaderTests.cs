using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NetGore.IO.Tests.Properties;
using NUnit.Framework;

namespace NetGore.IO.Tests
{
    [TestFixture]
    public class XmlInfoReaderTests
    {
        [Test]
        public void BasicReadTestFailBecauseNoSplitting()
        {
            
            var tmpFile = Path.GetTempFileName();

            File.WriteAllText(tmpFile, Resources.BasicXmlFile);

            try
            {
                XmlInfoReader.ReadFile(tmpFile);
                Assert.Fail("Expected ArgumentException");
            }
            catch (ArgumentException)
            {
            }
            finally
            {
                if (File.Exists(tmpFile))
                    File.Delete(tmpFile);
            }
        }

        [Test]
        public void BasicReadTest()
        {
            var tmpFile = Path.GetTempFileName();

            File.WriteAllText(tmpFile, Resources.BasicXmlFile);

            try
            {
                var r = XmlInfoReader.ReadFile(tmpFile, true);
                var r1 = r.First(x => x["Body.Index"] == "1");
                var r2 = r.First(x => x["Body.Index"] == "2");

                Assert.AreEqual(r1["Body.Index"], "1");
                Assert.AreEqual(r1["Body.Size.Width"], "30");
                Assert.AreEqual(r1["Body.Size.Height"], "78");
                Assert.AreEqual(r1["Body.Body.SkelBody"], "basic");
                Assert.AreEqual(r1["Body.Stand.SkelSet"], "stand");
                Assert.AreEqual(r1["Body.Walk.SkelSet"], "walk");
                Assert.AreEqual(r1["Body.Jump.SkelSet"], "jump");
                Assert.AreEqual(r1["Body.Fall.SkelSet"], "fall");
                Assert.AreEqual(r1["Body.Punch.SkelSet"], "punch");
                Assert.AreEqual(r1["Body.Punch.X"], "$width/2");
                Assert.AreEqual(r1["Body.Punch.Y"], "0");
                Assert.AreEqual(r1["Body.Punch.Width"], "$width");
                Assert.AreEqual(r1["Body.Punch.Height"], "$height/2");

                Assert.AreEqual(r2["Body.Index"], "2");
                Assert.AreEqual(r2["Body.Size.Width"], "300");
                Assert.AreEqual(r2["Body.Size.Height"], "78");
                Assert.AreEqual(r2["Body.Body.SkelBody"], "aa");
                Assert.AreEqual(r2["Body.Stand.SkelSet"], "b");
                Assert.AreEqual(r2["Body.Walk.SkelSet"], "c");
                Assert.AreEqual(r2["Body.Jump.SkelSet"], "dd");
                Assert.AreEqual(r2["Body.Fall.SkelSet"], "ee");
                Assert.AreEqual(r2["Body.Punch.SkelSet"], "ff");
                Assert.AreEqual(r2["Body.Punch.X"], "$x/2");
                Assert.AreEqual(r2["Body.Punch.Y"], "3");
                Assert.AreEqual(r2["Body.Punch.Width"], "$x");
                Assert.AreEqual(r2["Body.Punch.Height"], "$x/2");
            }
            finally
            {
                if (File.Exists(tmpFile))
                    File.Delete(tmpFile);
            }
        }

        [Test]
        public void BasicReadCaseInsensitivityUpperTest()
        {
            var tmpFile = Path.GetTempFileName();

            File.WriteAllText(tmpFile, Resources.BasicXmlFile);

            try
            {
                var r = XmlInfoReader.ReadFile(tmpFile, true);
                var r1 = r.First(x => x["BODY.INDEX"] == "1");
                var r2 = r.First(x => x["BODY.INDEX"] == "2");

                Assert.AreEqual(r1["BODY.INDEX"], "1");
                Assert.AreEqual(r1["BODY.SIZE.Width"], "30");
                Assert.AreEqual(r1["Body.SIZE.Height"], "78");
                Assert.AreEqual(r1["BODY.Body.SkelBody"], "basic");
                Assert.AreEqual(r1["BODY.Stand.SkelSet"], "stand");
                Assert.AreEqual(r1["Body.Walk.SKELSET"], "walk");
                Assert.AreEqual(r1["Body.Jump.SKELSET"], "jump");
                Assert.AreEqual(r1["BODY.Fall.SKELSET"], "fall");
                Assert.AreEqual(r1["Body.Punch.SKELSET"], "punch");
                Assert.AreEqual(r1["Body.Punch.X"], "$width/2");
                Assert.AreEqual(r1["BODY.Punch.Y"], "0");
                Assert.AreEqual(r1["BODY.Punch.Width"], "$width");
                Assert.AreEqual(r1["BODY.Punch.Height"], "$height/2");

                Assert.AreEqual(r2["Body.INDEX"], "2");
                Assert.AreEqual(r2["BODY.SIZE.Width"], "300");
                Assert.AreEqual(r2["Body.SIZE.Height"], "78");
                Assert.AreEqual(r2["BODY.Body.SkelBody"], "aa");
                Assert.AreEqual(r2["Body.Stand.SKELSET"], "b");
                Assert.AreEqual(r2["BODY.Walk.SKELSET"], "c");
                Assert.AreEqual(r2["BODY.Jump.SKELSET"], "dd");
                Assert.AreEqual(r2["Body.Fall.SKELSET"], "ee");
                Assert.AreEqual(r2["Body.Punch.SKELSET"], "ff");
                Assert.AreEqual(r2["Body.Punch.X"], "$x/2");
                Assert.AreEqual(r2["Body.Punch.Y"], "3");
                Assert.AreEqual(r2["Body.Punch.Width"], "$x");
                Assert.AreEqual(r2["Body.Punch.Height"], "$x/2");
            }
            finally
            {
                if (File.Exists(tmpFile))
                    File.Delete(tmpFile);
            }
        }

        [Test]
        public void BasicReadCaseInsensitivityLowerTest()
        {
            var tmpFile = Path.GetTempFileName();

            File.WriteAllText(tmpFile, Resources.BasicXmlFile);

            try
            {
                var r = XmlInfoReader.ReadFile(tmpFile, true);
                var r1 = r.First(x => x["body.index"] == "1");
                var r2 = r.First(x => x["body.index"] == "2");

                Assert.AreEqual(r1["body.index"], "1");
                Assert.AreEqual(r1["body.size.Width"], "30");
                Assert.AreEqual(r1["body.size.Height"], "78");
                Assert.AreEqual(r1["body.Body.SkelBody"], "basic");
                Assert.AreEqual(r1["body.Stand.SkelSet"], "stand");
                Assert.AreEqual(r1["Body.Walk.skelset"], "walk");
                Assert.AreEqual(r1["body.Jump.skelset"], "jump");
                Assert.AreEqual(r1["body.Fall.skelset"], "fall");
                Assert.AreEqual(r1["Body.punch.skelset"], "punch");
                Assert.AreEqual(r1["body.punch.X"], "$width/2");
                Assert.AreEqual(r1["body.Punch.Y"], "0");
                Assert.AreEqual(r1["body.punch.Width"], "$width");
                Assert.AreEqual(r1["body.punch.Height"], "$height/2");

                Assert.AreEqual(r2["body.index"], "2");
                Assert.AreEqual(r2["body.size.Width"], "300");
                Assert.AreEqual(r2["body.size.Height"], "78");
                Assert.AreEqual(r2["body.Body.SkelBody"], "aa");
                Assert.AreEqual(r2["body.Stand.skelset"], "b");
                Assert.AreEqual(r2["body.Walk.skelset"], "c");
                Assert.AreEqual(r2["body.Jump.skelset"], "dd");
                Assert.AreEqual(r2["Body.Fall.skelset"], "ee");
                Assert.AreEqual(r2["Body.punch.skelset"], "ff");
                Assert.AreEqual(r2["Body.punch.X"], "$x/2");
                Assert.AreEqual(r2["Body.punch.Y"], "3");
                Assert.AreEqual(r2["Body.Punch.Width"], "$x");
                Assert.AreEqual(r2["Body.punch.Height"], "$x/2");
            }
            finally
            {
                if (File.Exists(tmpFile))
                    File.Delete(tmpFile);
            }
        }
    }
}
