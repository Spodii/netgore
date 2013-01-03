using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using NetGore.IO;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.IO
{
    [TestFixture]
    public class MessageCollectionBaseTests
    {
        #region Unit tests

        [Test]
        public void CreateTest1()
        {
            const string fileContents = "A: Spodi says: \"Hi\"";

            using (var f = new TempFile())
            {
                File.WriteAllText(f.FilePath, fileContents);
                new MsgColl(f.FilePath);
            }
        }

        [Test]
        public void CreateTest2()
        {
            const string fileContents =
                "A: Spodi says: \"Hi\"\n" +
                "B: Spodi hates you. >:|";

            using (var f = new TempFile())
            {
                File.WriteAllText(f.FilePath, fileContents);
                new MsgColl(f.FilePath);
            }
        }

        [Test]
        public void CreateTest3()
        {
            const string fileContents =
                "A: Spodi says: \"Hi\"\n" +
                "B: {0} hates you. >:|";

            using (var f = new TempFile())
            {
                File.WriteAllText(f.FilePath, fileContents);
                new MsgColl(f.FilePath);
            }
        }

        [Test]
        public void CreateTest4()
        {
            const string fileContents =
                "A: Spodi says: \"Hi\"\n" +
                "B: {0} says: \"$1\"";

            using (var f = new TempFile())
            {
                File.WriteAllText(f.FilePath, fileContents);
                new MsgColl(f.FilePath);
            }
        }

        [Test]
        public void GetMessageTest1()
        {
            const string fileContents = "A: Spodi says: \"Hi\"";

            MsgColl c;
            using (var f = new TempFile())
            {
                File.WriteAllText(f.FilePath, fileContents);
                c = new MsgColl(f.FilePath);
            }

            var msg = c.GetMessage(MsgType.A);

            Assert.AreEqual("Spodi says: \"Hi\"", msg);
        }

        [Test]
        public void GetMessageTest2()
        {
            const string fileContents =
                "A: Spodi says: \"Hi\"\n" +
                "B: Spodi hates you. >:|";

            MsgColl c;
            using (var f = new TempFile())
            {
                File.WriteAllText(f.FilePath, fileContents);
                c = new MsgColl(f.FilePath);
            }

            var msg = c.GetMessage(MsgType.A);
            Assert.AreEqual("Spodi says: \"Hi\"", msg);

            msg = c.GetMessage(MsgType.B);
            Assert.AreEqual("Spodi hates you. >:|", msg);
        }

        [Test]
        public void GetMessageTest3()
        {
            const string fileContents =
                "A: Spodi says: \"Hi\"\n" +
                "B: {0} hates you. >:|";

            MsgColl c;
            using (var f = new TempFile())
            {
                File.WriteAllText(f.FilePath, fileContents);
                c = new MsgColl(f.FilePath);
            }

            var msg = c.GetMessage(MsgType.A);
            Assert.AreEqual("Spodi says: \"Hi\"", msg);

            msg = c.GetMessage(MsgType.B, "Spodi");
            Assert.AreEqual("Spodi hates you. >:|", msg, "Spodi");
        }

        [Test]
        public void GetMessageTest4()
        {
            const string fileContents =
                "A: {0} says: \"{1}\"\n" +
                "B: {0} eats {1} for {2}. Yummy.";

            MsgColl c;
            using (var f = new TempFile())
            {
                File.WriteAllText(f.FilePath, fileContents);
                c = new MsgColl(f.FilePath);
            }

            var msg = c.GetMessage(MsgType.A, "Chocolate", "Gimmie caramel!");
            Assert.AreEqual("Chocolate says: \"Gimmie caramel!\"", msg);

            msg = c.GetMessage(MsgType.B, "NetGore", "vbGORE", "breakfast");
            Assert.AreEqual("NetGore eats vbGORE for breakfast. Yummy.", msg);
        }

        [Test]
        public void MissingMessageGrabFromSecondaryTest()
        {
            const string fileContentsA =
                "A: Spodi says: \"Hi\"\n" +
                "B: Spodi hates you. >:|";
            const string fileContentsB = "A: boob says: \"Hi\"";

            MsgColl cA;
            MsgColl cB;
            using (var fA = new TempFile())
            {
                File.WriteAllText(fA.FilePath, fileContentsA);
                cA = new MsgColl(fA.FilePath);
                using (var fB = new TempFile())
                {
                    File.WriteAllText(fB.FilePath, fileContentsB);
                    cB = new MsgColl(fB.FilePath, cA);
                }
            }

            Assert.AreEqual("Spodi says: \"Hi\"", cA.GetMessage(MsgType.A));
            Assert.AreEqual("Spodi hates you. >:|", cA.GetMessage(MsgType.B));

            Assert.AreEqual("boob says: \"Hi\"", cB.GetMessage(MsgType.A));
            Assert.AreEqual("Spodi hates you. >:|", cB.GetMessage(MsgType.B));
        }

        [Test]
        public void MissingMessageTest()
        {
            const string fileContents = "A: \"Hi\"";

            MsgColl c;
            using (var f = new TempFile())
            {
                File.WriteAllText(f.FilePath, fileContents);
                c = new MsgColl(f.FilePath);
            }

            Assert.IsNull(c.GetMessage(MsgType.C));
            Assert.IsNull(c.GetMessage(MsgType.C, "asdf", "fda"));
        }

        #endregion

        class MsgColl : MessageCollectionBase<MsgType>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MessageCollectionBase{T}"/> class.
            /// </summary>
            /// <param name="file">Path to the file to load the messages from.</param>
            public MsgColl(string file) : base(file)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="MessageCollectionBase{T}"/> class.
            /// </summary>
            /// <param name="file">Path to the file to load the messages from.</param>
            /// <param name="secondary">Collection of messages to add missing messages from. If null, the
            /// collection will only contain messages specified in the file. Otherwise, any message that exists
            /// in this secondary collection but does not exist in the <paramref name="file"/> will be loaded
            /// to this collection from this secondary collection.</param>
            public MsgColl(string file, IEnumerable<KeyValuePair<MsgType, string>> secondary) : base(file, secondary)
            {
            }

            /// <summary>
            /// When overridden in the derived class, tries to parse a string to get the ID.
            /// </summary>
            /// <param name="str">String to parse.</param>
            /// <param name="id">Parsed ID.</param>
            /// <returns>True if the ID was parsed successfully, else false.</returns>
            protected override bool TryParseID(string str, out MsgType id)
            {
                return ParseEnumHelper(str, out id);
            }
        }

        enum MsgType
        {
            A,
            B,
            C
        }
    }
}