using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyQQ.Server.Test
{
    [TestClass]
    public class WordCountTest
    {
        [TestMethod]
        public void DemoTest()
        {
            WordCount wordCount = new WordCount();
            wordCount.Demo();
        }
    }
}
