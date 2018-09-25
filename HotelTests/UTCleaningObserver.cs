using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject;
using HotelProject.Objecten;

namespace HotelTests
{
    [TestClass]
    public class UTCleaningObserver
    {
        /// <summary>
        /// Test of een cleaningobserver aangemaakt kan worden.
        /// </summary>
        [TestMethod]
        public void TestCleaningObserver()
        {
            Hotel hotel = new Hotel();
            CleaningObserver co = new CleaningObserver(hotel);

            Assert.IsNotNull(co);
        }
    }
}
