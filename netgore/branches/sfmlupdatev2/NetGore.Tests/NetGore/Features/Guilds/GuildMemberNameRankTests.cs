using System.Linq;
using NetGore.Features.Guilds;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.Features.Guilds
{
    [TestFixture]
    public class GuildMemberNameRankTests
    {
        #region Unit tests

        [Test]
        public void GuildMemberNameRankIOTest()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    var a = new GuildMemberNameRank("asdfasdf", 52);
                    var b = new GuildMemberNameRank("xva", 12);
                    var c = new GuildMemberNameRank("fwer", 5);
                    var d = new GuildMemberNameRank("asdf wer", 1);

                    using (var w = creator.GetWriter())
                    {
                        w.Write("a", a);
                        w.Write("b", b);
                        w.Write("c", c);
                        w.Write("d", d);
                    }

                    var r = creator.GetReader();
                    {
                        var ra = r.ReadGuildMemberNameRank("a");
                        var rb = r.ReadGuildMemberNameRank("b");
                        var rc = r.ReadGuildMemberNameRank("c");
                        var rd = r.ReadGuildMemberNameRank("d");

                        Assert.AreEqual(a, ra);
                        Assert.AreEqual(b, rb);
                        Assert.AreEqual(c, rc);
                        Assert.AreEqual(d, rd);
                    }
                }
            }
        }

        #endregion
    }
}