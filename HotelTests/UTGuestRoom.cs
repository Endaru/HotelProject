using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject.Objecten;
using System.Drawing;

namespace HotelTests
{
    [TestClass]
    public class UTGuestRoom
    {
        /// <summary>
        /// Test het aanmaken van de GuestRoom
        /// </summary>
        [TestMethod]
        public void GuestRoomTest()
        {
            GuestRoom guestroom = new GuestRoom(1, new Point(1, 1), new Point(1, 1),1);

            Assert.IsNotNull(guestroom);
        }

        /// <summary>
        /// Test als een gast de kamer binnen gaat.
        /// </summary>
        [TestMethod]
        public void TestGuestRoomEnterGuest()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            GuestRoom guestRoom = hotel.Rooms[1, 1] as GuestRoom;
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = guestRoom;
            guest.EnterRoom();

            Assert.IsTrue(guestRoom.Inhabitant == guest);
            Assert.IsTrue(guest.State == State.Sleeping);
            Assert.IsTrue(guest.IsInRoom);
        }

        /// <summary>
        /// Test als een cleaner de kamer binnen gaat
        /// </summary>
        [TestMethod]
        public void TestGuestRoomEnterCleaner()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            GuestRoom guestRoom = hotel.Rooms[1, 1] as GuestRoom;
            Cleaner cleaner = new Cleaner(hotel);
            cleaner.Room = guestRoom;
            cleaner.EnterRoom();

            Assert.IsTrue(cleaner.State == State.Idle);
            Assert.IsTrue(cleaner.IsInRoom);
        }

        /// <summary>
        /// Test als een gast de kamer uit probeert te gaan
        /// </summary>
        [TestMethod]
        public void TestGuestRoomExitGuest()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            GuestRoom guestRoom = hotel.Rooms[1, 1] as GuestRoom;
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = guestRoom;
            guest.ExitRoom();

            Assert.IsTrue(guest.State == State.Idle);
            Assert.IsFalse(guest.IsInRoom);
        }

        /// <summary>
        /// test als een cleaner de kamer uit gaat
        /// </summary>
        [TestMethod]
        public void TestGuestRoomExitCleaner()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            GuestRoom guestRoom = hotel.Rooms[1, 1] as GuestRoom;
            Cleaner cleaner = new Cleaner(hotel);
            cleaner.Room = guestRoom;
            cleaner.ExitRoom();

            Assert.IsTrue(cleaner.State == State.Idle);
            Assert.IsFalse(cleaner.IsInRoom);
        }

        /// <summary>
        /// Test als een gast de kamer correct uit gaat.
        /// </summary>
        [TestMethod]
        public void TestGuestRoomExitGuestCorrect()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            GuestRoom guestRoom = hotel.Rooms[1, 1] as GuestRoom;
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = guestRoom;
            guest.EnterRoom();
            guest.ExitRoom();

            Assert.IsTrue(guest.State == State.Idle);
            Assert.IsFalse(guestRoom.Inhabitant == guest);
            Assert.IsFalse(guest.IsInRoom);
        }
    }
}
