using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Features.Guilds;
using NetGore.IO;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.Features.Guilds
{
    [TestFixture]
    public class GuildMemberNameRankTests
    {
        [Test]
        public void GuildMemberNameRankIOTest()
        {
            foreach (CreateCreatorHandler createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    var a = new GuildMemberNameRank("asdfasdf", 52);
                    var b = new GuildMemberNameRank("xva", 12);
                    var c = new GuildMemberNameRank("fwer", 5);
                    var d = new GuildMemberNameRank("asdf wer", 1);

                    using (IValueWriter w = creator.GetWriter())
                    {
                        w.Write("a", a);
                        w.Write("b", b);
                        w.Write("c", c);
                        w.Write("d", d);
                    }

                    IValueReader r = creator.GetReader();
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
    }
}
