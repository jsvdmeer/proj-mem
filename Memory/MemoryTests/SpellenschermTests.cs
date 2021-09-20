using Microsoft.VisualStudio.TestTools.UnitTesting;
using Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Memory.Tests
{
    [TestClass()]
    public class SpellenschermTests
    {
        [TestMethod()]
        public void SpellenschermTest()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "MemoryGame");
            path += "\\testSave.csv";

            Spellenscherm spellenscherm = new Spellenscherm(path);
            Assert.IsNotNull(spellenscherm);
        }
    }
}