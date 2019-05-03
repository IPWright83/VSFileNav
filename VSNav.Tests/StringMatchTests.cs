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

            Assert.AreEqual(1, func("Test.txt").MatchPriority);
            Assert.AreEqual(1, func("AbC.cs").MatchPriority);
            Assert.AreEqual(1, func("image.JPG").MatchPriority);
        }

        [TestMethod]
        public void CamelCaseReturnsFullMatches()
        {
            var func = StringMatch.GetShowStringDelegates("MFB");

            Assert.AreEqual(7.25, func("MyFooBar.txt").MatchPriority);
            Assert.AreEqual(7.2, func("MyFlipBlob2.jpg").MatchPriority);
        }

        [TestMethod]
        public void CamelCaseReturnsPartialMatches()
        {
            var func = StringMatch.GetShowStringDelegates("MFB");

            Assert.AreEqual(4.15, func("ThisIsMyFooBar99.txt").MatchPriority);
            Assert.AreEqual(7.15, func("MyFlipBlobIsCool.jpg").MatchPriority);
            Assert.AreEqual(5.125, func("MySillyFlipFrogBlobs.jpg").MatchPriority);
        }

        [TestMethod]
        public void CamelCaseReturnsPositivePriority()
        {
            var func = StringMatch.GetShowStringDelegates("FB");

            Assert.IsTrue(func("ThisIsMyFooBar.txt").MatchPriority > 0);
        }

        [TestMethod]
        public void RegexMultiPartMatches()
        {
            var func = StringMatch.GetShowStringDelegates("test");

            var match = func("thisisatest mytest xtest");
            Assert.AreEqual(3, match.Parts.Count);
            Assert.AreEqual("thisisa",          match.Parts[0].Text);
            Assert.AreEqual("test",             match.Parts[1].Text);
            Assert.AreEqual(" mytest xtest",    match.Parts[2].Text);
        }

        [TestMethod]
        public void RegexMultiStringMatches()
        {
            var func = StringMatch.GetShowStringDelegates("test this");

            var match = func("thisisatest mytest xtest");
            Assert.AreEqual(4, match.Parts.Count);
            Assert.AreEqual("this",             match.Parts[0].Text);
            Assert.AreEqual("isa",              match.Parts[1].Text);
            Assert.AreEqual("test",             match.Parts[2].Text);
            Assert.AreEqual(" mytest xtest",    match.Parts[3].Text);
        }

        [TestMethod]
        public void RegexPartsAddUp()
        {
            var func = StringMatch.GetShowStringDelegates("test");

            var filename = "TestThis";
            var match = func(filename);
            Assert.AreEqual(2, match.Parts.Count);
            Assert.AreEqual(filename.Length,
                match.Parts[0].Text.Length +
                match.Parts[1].Text.Length);
        }

        [TestMethod]
        public void RegexMultiStringPartsAddUp()
        {
            var func = StringMatch.GetShowStringDelegates("test this");

            var filename = "thisisatest mytest xtest";
            var match = func(filename);
            Assert.AreEqual(4, match.Parts.Count);
            Assert.AreEqual(filename.Length,
                match.Parts[0].Text.Length +
                match.Parts[1].Text.Length +
                match.Parts[2].Text.Length +
                match.Parts[3].Text.Length);
        }

        [TestMethod]
        public void RegexMultiMoreMatchedIsBetter()
        {
            var func = StringMatch.GetShowStringDelegates("test this");

            Assert.IsTrue(func("TestThis.txt").MatchPriority > func("Test.txt").MatchPriority);
            Assert.IsTrue(func("TestThis.txt").MatchPriority > func("TestThisToo.txt").MatchPriority);
        }

        [TestMethod]
        public void RegexCountMatchOnce()
        {
            var func = StringMatch.GetShowStringDelegates("foo");

            Assert.IsTrue(func("FooBar.txt").MatchPriority == func("FooFoo.txt").MatchPriority);
        }
    }
}
