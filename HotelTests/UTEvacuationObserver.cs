using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject;
using HotelProject.Objecten;

namespace HotelTests
{
    [TestClass]
    public class UTEvacuationObserver
    {
        /// <summary>
        /// Test het aanmaken van een EvacuationObserver.
        /// </summary>
        [TestMethod]
        public void TestEvacuationObserver()
        {
            Hotel hotel = new Hotel();
            EvacuationObserver eo = new EvacuationObserver(hotel);

            Assert.IsNotNull(eo);
        }

        [TestMethod]
        public void TestEvacCapacitySet()
        {
            EvacuationObserver.EvacCapacity = 3;

            Assert.IsTrue(EvacuationObserver.EvacCapacity == 3);
        }
    }
}
