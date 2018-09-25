using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject.Objecten;
using System.Drawing;

namespace HotelTests
{
    [TestClass]
    public class UTSwimmingPool
    {
        /// <summary>
        /// Test of er een swimmingpool aangemaakt kan worden.
        /// </summary>
        [TestMethod]
        public void TestSwimmingPool()
        {
            SwimmingPool swimmingPool = new SwimmingPool(1, new Point(1, 1), new Point(1, 1));

            Assert.IsNotNull(swimmingPool);
        }
    }
}
