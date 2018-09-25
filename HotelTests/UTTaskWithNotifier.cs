using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject;
using HotelProject.Objecten;
using System;

namespace HotelTests
{
    [TestClass]
    public class UTTaskWithNotifier
    {
        /// <summary>
        /// Test het aanmaken van een taskwithnotifier.
        /// </summary>
        [TestMethod]
        public void TestTaskWithNotifier()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1 , hotel);
            TaskWithNotifier task = new TaskWithNotifier(guest);

            Assert.IsNotNull(task);
        }

        [TestMethod]
        public void TestEventListener()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            TaskWithNotifier task = new TaskWithNotifier(guest);
            EventListener el = new EventListener(task);

            Assert.IsNotNull(el);
        }
    }
}
