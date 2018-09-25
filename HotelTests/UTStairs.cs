using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject.Objecten;

namespace HotelTests
{
    [TestClass]
    public class UTStairs
    {
        /// <summary>
        /// Test het aanmaken van stairs
        /// </summary>
        [TestMethod]
        public void TestStairs()
        {
            Stairs stairs = new Stairs();

            Assert.IsNotNull(stairs);
        }
    }
}
