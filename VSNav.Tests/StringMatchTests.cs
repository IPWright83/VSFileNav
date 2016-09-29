using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VSNav.Tests
{
    [TestClass]
    public class StringMatchTests
    {
        [TestMethod]
        public void EmptyPatternReturnsAllResults()
        {
            var func = StringMatch.GetShowStringDelegates("");

            Assert.AreEqual(1, func("Test.txt").MatchFraction);
            Assert.AreEqual(1, func("AbC.cs").MatchFraction);
            Assert.AreEqual(1, func("image.JPG").MatchFraction);
        }

        [TestMethod]
        public void CamelCaseReturnsFullMatches()
        {
            var func = StringMatch.GetShowStringDelegates("MFB");

            Assert.AreEqual(4, func("MyFooBar.txt").MatchFraction);
            Assert.AreEqual(4, func("MyFlipBlob.jpg").MatchFraction);
        }

        [TestMethod]
        public void CamelCaseReturnsPartialMatches()
        {
            var func = StringMatch.GetShowStringDelegates("MFB");

            Assert.AreEqual(1, func("ThisIsMyFooBar.txt").MatchFraction);
            Assert.AreEqual(4, func("MyFlipBlobAreCool.jpg").MatchFraction);
            Assert.AreEqual(2, func("MySillyFlipFrogBlob.jpg").MatchFraction);
        }

        [TestMethod]
        public void RegexMultiPartMatches()
        {
            var func = StringMatch.GetShowStringDelegates("test");

            var match = func("thisisatest mytest xtest");
            Assert.AreEqual(4, match);
            Assert.AreEqual("x", match.Parts[0]);
            Assert.AreEqual("test", match.Parts[1]);
        }
    }
}
