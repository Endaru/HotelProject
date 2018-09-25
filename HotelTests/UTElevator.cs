using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject.Objecten;

namespace HotelTests
{
    [TestClass]
    public class UTElevator
    {
        /// <summary>
        /// Test het aanmaken van een elevator
        /// </summary>
        [TestMethod]
        public void ElevatorTest()
        {
            Elevator elevator = new Elevator();

            Assert.IsNotNull(elevator);
        }
    }
}
