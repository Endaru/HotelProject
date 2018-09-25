using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject.Objecten;

namespace HotelTests
{
    [TestClass]
    public class UTRoom
    {
        /// <summary>
        /// Test of een kamer word aangemaakt met constructor 1.
        /// </summary>
        [TestMethod]
        public void TestRoom1()
        {
            Room room = new Room();

            Assert.IsNotNull(room);
        }

        /// <summary>
        /// Test of een kamer word aangemaakt met constructor 2.
        /// </summary>
        [TestMethod]
        public void TestRoom2()
        {
            Room room = new Room(1, new System.Drawing.Point(1, 2), new System.Drawing.Point(1, 1));

            Assert.IsNotNull(room);
        }

        /// <summary>
        /// Test of een kamer vies wordt door deze methode.
        /// </summary>
        [TestMethod]
        public void TestSetDirty()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Room room = hotel.Rooms[1, 1];
            room.SetDirty(1);

            Assert.IsTrue(room.Dirty);
        }
    }
}
