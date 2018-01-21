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

            Assert.AreEqual(7, func("MyFooBar.txt").MatchPriority);
            Assert.AreEqual(7, func("MyFlipBlob2.jpg").MatchPriority);
        }

        [TestMethod]
        public void CamelCaseReturnsPartialMatches()
        {
            var func = StringMatch.GetShowStringDelegates("MFB");

            Assert.AreEqual(4, func("ThisIsMyFooBar.txt").MatchPriority);
            Assert.AreEqual(7, func("MyFlipBlobIsCool.jpg").MatchPriority);
            Assert.AreEqual(5, func("MySillyFlipFrogBlobs.jpg").MatchPriority);
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
            Assert.AreEqual(6, match.Parts.Count);
            Assert.AreEqual("thisisa",  match.Parts[0].Text);
            Assert.AreEqual("test",     match.Parts[1].Text);
            Assert.AreEqual(" my",      match.Parts[2].Text);
            Assert.AreEqual("test",     match.Parts[3].Text);
            Assert.AreEqual(" x",       match.Parts[4].Text);
            Assert.AreEqual("test",     match.Parts[5].Text);
        }

        [TestMethod]
        public void RegexMultiStringMatches()
        {
            var func = StringMatch.GetShowStringDelegates("test this");

            var match = func("thisisatest mytest xtest");
            Assert.AreEqual(7, match.Parts.Count);
            Assert.AreEqual("this",     match.Parts[0].Text);
            Assert.AreEqual("isa",      match.Parts[1].Text);
            Assert.AreEqual("test",     match.Parts[2].Text);
            Assert.AreEqual(" my",      match.Parts[3].Text);
            Assert.AreEqual("test",     match.Parts[4].Text);
            Assert.AreEqual(" x",       match.Parts[5].Text);
            Assert.AreEqual("test",     match.Parts[6].Text);
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
            Assert.AreEqual(7, match.Parts.Count);
            Assert.AreEqual(filename.Length,
                match.Parts[0].Text.Length +
                match.Parts[1].Text.Length +
                match.Parts[2].Text.Length +
                match.Parts[3].Text.Length +
                match.Parts[4].Text.Length +
                match.Parts[5].Text.Length +
                match.Parts[6].Text.Length);
        }

        [TestMethod]
        public void RegexMultiMoreMatchedIsBetter()
        {
            var func = StringMatch.GetShowStringDelegates("test this");

            Assert.IsTrue(func("TestThis.txt").MatchPriority > func("Test.txt").MatchPriority);
        }
    }
}
