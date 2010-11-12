using System.IO;
using System.Linq;
using NetGore.Graphics;
using NUnit.Framework;

namespace NetGore.Tests.Graphics
{
    [TestFixture]
    public class AutomaticAnimatedGrhDataTests
    {
        static readonly string[] _pathSeps;

        /// <summary>
        /// Initializes the <see cref="AutomaticAnimatedGrhDataTests"/> class.
        /// </summary>
        static AutomaticAnimatedGrhDataTests()
        {
            _pathSeps =
                new string[] { "\\", "/", Path.DirectorySeparatorChar.ToString(), Path.AltDirectorySeparatorChar.ToString() }.
                    Distinct().ToArray();
        }

        /// <summary>
        /// Makes sure the path could be parsed correctly with the given parameters and works for all path separators.
        /// </summary>
        /// <param name="prefix">The prefix string (to ensure it is properly ignored).</param>
        /// <param name="title">The sprite title.</param>
        /// <param name="speed">The animation speed.</param>
        static void RunValidPathTest(string prefix, string title, string speed)
        {
            foreach (var pathSep in _pathSeps)
            {
                var path = string.Format("{0}{4}{1}{2}{1}frames{1}{3}", prefix, AutomaticAnimatedGrhData.DirectoryNameDelimiter,
                    title, speed, pathSep);

                var v = AutomaticAnimatedGrhData.GetAutomaticAnimationInfo(path);
                Assert.IsNotNull(v);
                Assert.AreEqual(v.Speed.ToString(), speed, "Path sep: " + pathSep);
                Assert.AreEqual(v.Title, title, "Path sep: " + pathSep);
            }
        }

        #region Unit tests

        [Test]
        public void GetInfoDoppelgangerPrefixATest()
        {
            RunValidPathTest("\\_asdf_frames_999", "Test", "150");
        }

        [Test]
        public void GetInfoDoppelgangerPrefixBTest()
        {
            RunValidPathTest("_asd sdfwerf_frames_999", "Test", "150");
        }

        [Test]
        public void GetInfoDoppelgangerPrefixCTest()
        {
            RunValidPathTest("_asdf_frames_999", "Test", "150");
        }

        [Test]
        public void GetInfoDoppelgangerPrefixDTest()
        {
            RunValidPathTest("\\_asdf_frames_999\\", "Test", "150");
        }

        [Test]
        public void GetInfoDoppelgangerPrefixETest()
        {
            RunValidPathTest("/_asdf_frames_999/", "Test", "150");
        }

        [Test]
        public void GetInfoDoppelgangerPrefixFTest()
        {
            RunValidPathTest("\\_asdf_frames_999\\_asdf_frames_999\\", "Test", "150");
        }

        [Test]
        public void GetInfoDoppelgangerPrefixGTest()
        {
            RunValidPathTest("/_asdf_frames_999/_asdf_frames_999/", "Test", "150");
        }

        [Test]
        public void GetInfoNameATest()
        {
            RunValidPathTest(string.Empty, "My test", "150");
        }

        [Test]
        public void GetInfoNameBTest()
        {
            RunValidPathTest(string.Empty, "My test asdf rew", "150");
        }

        [Test]
        public void GetInfoNameCTest()
        {
            RunValidPathTest(string.Empty, " My test asdf rew ", "150");
        }

        [Test]
        public void GetInfoNameDTest()
        {
            RunValidPathTest(string.Empty, "_my_test_", "150");
        }

        [Test]
        public void GetInfoNameETest()
        {
            RunValidPathTest(string.Empty, "_my test_", "150");
        }

        [Test]
        public void GetInfoNameFTest()
        {
            RunValidPathTest(string.Empty, "f", "150");
        }

        [Test]
        public void GetInfoNoPrefixTest()
        {
            RunValidPathTest(string.Empty, "Test", "150");
        }

        [Test]
        public void GetInfoPrefixATest()
        {
            RunValidPathTest("\\", "Test", "150");
        }

        [Test]
        public void GetInfoPrefixBTest()
        {
            RunValidPathTest("//", "Test", "150");
        }

        [Test]
        public void GetInfoPrefixCTest()
        {
            RunValidPathTest(@"C:\Aasdf\ werawer asdf\fasd\", "Test", "150");
        }

        [Test]
        public void GetInfoPrefixDTest()
        {
            RunValidPathTest(@"C:/Aasdf/ werawer asdf/fasd/", "Test", "150");
        }

        [Test]
        public void GetInfoPrefixETest()
        {
            RunValidPathTest(@"C:\A_asdf\_werawer_asdf\_fasd_\", "Test", "150");
        }

        [Test]
        public void GetInfoPrefixFTest()
        {
            RunValidPathTest(@"C:/A_asdf/_werawer_asdf/_fasd_/", "Test", "150");
        }

        [Test]
        public void GetInfoPrefixGTest()
        {
            RunValidPathTest(@"\...\..\_fasd_\...\", "Test", "150");
        }

        [Test]
        public void GetInfoPrefixHTest()
        {
            RunValidPathTest(@"/.../../_fasd_/.../", "Test", "150");
        }

        #endregion
    }
}