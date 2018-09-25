using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject.Objecten;
using System.Drawing;

namespace HotelTests
{
    [TestClass]
    public class UTDiningRoom
    {
        /// <summary>
        /// Test het maken van een diningroom.
        /// </summary>
        [TestMethod]
        public void TestDiningRoom()
        {
            DiningRoom diningRoom = new DiningRoom(1, new Point(1, 1), new Point(1, 1),1);

            Assert.IsNotNull(diningRoom);
        }

        /// <summary>
        /// Test of die niet de diningRoom ingaat.
        /// </summary>
        [TestMethod]
        public void TestEnterWaiting()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            DiningRoom diningRoom = hotel.Rooms[1, 2] as DiningRoom;
            for(int i = 0; i < diningRoom.Capacity; i++)
            {
                Guest guest = new Guest($"{i}", 1, hotel);
                guest.Room = diningRoom;
                guest.EnterRoom();
            }
            Guest guesta = new Guest("a", 1, hotel);
            guesta.Room = diningRoom;
            guesta.EnterRoom();

            Assert.IsTrue(guesta.State == State.Waiting);
        } 

        /// <summary>
        /// Test of de gast de DiningRoom ingaat.
        /// </summary>
        [TestMethod]
        public void TestEnter()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            DiningRoom diningRoom = hotel.Rooms[1, 2] as DiningRoom;

            Guest guesta = new Guest("a", 1, hotel);
            guesta.Room = diningRoom;
            guesta.EnterRoom();

            Assert.IsTrue(diningRoom.Inhabitants.Contains(guesta));
            Assert.IsTrue(guesta.State == State.Eating);
            Assert.IsTrue(guesta.IsInRoom);
        }

        /// <summary>
        /// Check als de gast er niet in zit dat die er niet uit kan.
        /// </summary>
        [TestMethod]
        public void TestExitDiningRoom()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            DiningRoom diningRoom = hotel.Rooms[1, 2] as DiningRoom;

            Guest guest = new Guest("a", 1, hotel);
            guest.Room = diningRoom;
            guest.ExitRoom();

            Assert.IsTrue(guest.State == State.Idle);
            Assert.IsFalse(guest.IsInRoom);
        }

        /// <summary>
        /// Gast uit de kamer laten gaan
        /// </summary>
        [TestMethod]
        public void TestExitDiningRoomTrue()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            DiningRoom diningRoom = hotel.Rooms[1, 2] as DiningRoom;

            Guest guest = new Guest("a", 1, hotel);
            guest.Room = diningRoom;
            guest.ExitRoom();

            Assert.IsTrue(guest.State == State.Idle);
            Assert.IsFalse(guest.IsInRoom);
            Assert.IsFalse(diningRoom.Inhabitants.Contains(guest));
        }
    }
}
