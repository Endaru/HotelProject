using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject.Objecten;
using HotelProject;
using System.Collections.Generic;
using System.IO;

namespace HotelTests
{
    [TestClass]
    public class UTLayoutReader
    {
        /// <summary>
        /// Test het uitlezen van de standaard .layout file.
        /// </summary>
        [TestMethod]
        public void TestReadLayoutFile1()
        {
            int b4MaxHeigth = LayoutReader.MaxHeight;
            int b4MaxWidth = LayoutReader.MaxWidth;
            List<Room> rooms = LayoutReader.ReadLayoutFile();

            Assert.IsNotNull(rooms);
            Assert.IsTrue(b4MaxHeigth != LayoutReader.MaxHeight);
            Assert.IsTrue(b4MaxWidth != LayoutReader.MaxWidth);
        }

        /// <summary>
        /// Test het uitlezen van een andere .layout file.
        /// </summary>
        [TestMethod]
        public void TestReadLayoutFile2()
        {
            int b4MaxHeigth = LayoutReader.MaxHeight;
            int b4MaxWidth = LayoutReader.MaxWidth;
            LayoutReader.FilePath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))), "HotelProject\\Resources\\Hotel3.layout");//Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"..\\..\\Resources"),"Hotel5.layout")
            List<Room> rooms = LayoutReader.ReadLayoutFile();

            Assert.IsNotNull(rooms);
            Assert.IsTrue(b4MaxHeigth != LayoutReader.MaxHeight);
            Assert.IsTrue(b4MaxWidth != LayoutReader.MaxWidth);
        }
    }
}
