using Microsoft.VisualStudio.TestTools.UnitTesting;
using Memory;
using System;
using System.IO;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media;

namespace Memory.Tests
{
    [TestClass()]
    public class MemoryGridTests
    {
        [TestMethod()]
        public void MemoryGridTest()
        {
            string delimiter = ";";
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "MemoryGame");
            path += "\\testSave.csv";
            File.WriteAllText(path, delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter);

            System.Windows.Controls.Grid grid = new System.Windows.Controls.Grid();
            Memory.MemoryGrid mg = new MemoryGrid(grid, 4, 4, path);

            Assert.IsNotNull(mg);
        }

        [TestMethod()]
        public void GetImagesListTest()
        {
            string delimiter = ";";
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "MemoryGame");
            path += "\\testSave.csv";
            File.WriteAllText(path, delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter);

            System.Windows.Controls.Grid grid = new System.Windows.Controls.Grid();
            Memory.MemoryGrid mg = new MemoryGrid(grid, 4, 4, path);

            List<ImageSource> images = mg.GetImagesList();

            Assert.AreEqual(images.Count, 16);
        }

        [TestMethod()]
        public void CheckPairTestRight()
        {
            string delimiter = ";";
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "MemoryGame");
            path += "\\testSave.csv";
            File.WriteAllText(path, delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter);

            System.Windows.Controls.Grid grid = new System.Windows.Controls.Grid();
            Memory.MemoryGrid mg = new MemoryGrid(grid, 4, 4, path);

            Image good_img_1 = new Image();    
            Image good_img_2 = new Image();

            Assert.AreEqual(mg.CheckPair(good_img_1, good_img_2), true);
        }

        [TestMethod()]
        public void CheckPairTestWrong()
        {
            string delimiter = ";";
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "MemoryGame");
            path += "\\testSave.csv";
            File.WriteAllText(path, delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter);

            System.Windows.Controls.Grid grid = new System.Windows.Controls.Grid();
            Memory.MemoryGrid mg = new MemoryGrid(grid, 4, 4, path);

            Image bad_img_1 = new Image();
            bad_img_1.Source = new System.Windows.Media.Imaging.BitmapImage();

            Image bad_img_2 = new Image();

            Assert.AreNotEqual(mg.CheckPair(bad_img_1, bad_img_2), true);
        }
    }
}