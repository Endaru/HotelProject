using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject.Objecten;

namespace HotelTests
{
    [TestClass]
    public class UTHuman
    {
        /// <summary>
        /// Test of het juiste soort kamer wordt gevonden.
        /// </summary>
        [TestMethod]
        public void TestFindRoom1()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.FindRoom<DiningRoom>(hotel);

            Assert.IsInstanceOfType(guest.Path[guest.Path.Count - 1], typeof(DiningRoom));
        }

        /// <summary>
        /// Test of het juiste soort kamer niet wordt gevonden.
        /// </summary>
        [TestMethod]
        public void TestFindRoom2()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.FindRoom<SwimmingPool>(hotel);

            Assert.IsTrue(guest.Path.Count == 0);
        }

        /// <summary>
        /// Test of een human een path creeërt naar de kamer en of het gaat lopen.
        /// </summary>
        [TestMethod]
        public void TestGoToRoom()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.GoToRoom(hotel.Rooms[5, 5]);

            Assert.IsNotNull(guest.Path);
            Assert.IsTrue(guest.State == State.Walking);
        }

        /// <summary>
        /// Test als het path van de human is afgelopen tijdens evacueren of het dan op geëvacueerd gaat.
        /// </summary>
        [TestMethod]
        public void TestGoToNextRoom1()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.Evac = EvacProcess.Evacuating;
            guest.GoToNextRoom();

            Assert.IsTrue(guest.Evac == EvacProcess.Evacuated);
        }

        /// <summary>
        /// Test of de state van een guest op sleeping wordt gezet als het klaar is met lopen, omdat het een guestroom binnen gaat.
        /// </summary>
        [TestMethod]
        public void TestGoToNextRoom2()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = hotel.Rooms[1, 1];
            guest.GoToNextRoom();

            Assert.IsTrue(guest.State == State.Sleeping);
        }

        /// <summary>
        /// Test of de state van een cleaner op cleaning wordt gezet als het klaar is met lopen, omdat het een guestroom binnen gaat.
        /// </summary>
        [TestMethod]
        public void TestGoToNextRoom3()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Cleaner cleaner = new Cleaner(hotel);
            cleaner.Room = hotel.Rooms[1, 1];
            cleaner.GoToNextRoom();

            Assert.IsTrue(cleaner.State == State.Cleaning);
        }

        /// <summary>
        /// Test of een human gaat wachten als het in een elevatorshaft staat.
        /// </summary>
        [TestMethod]
        public void TestGoToNextRoom4()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.Path.Add(hotel.Rooms[0, 2]);
            guest.Path.Add(hotel.Rooms[0, 3]);
            guest.Path.Add(hotel.Rooms[1, 3]);
            guest.Path.Add(hotel.Rooms[2, 3]);
            guest.Path.Add(hotel.Rooms[3, 3]);
            guest.GoToNextRoom();

            Assert.IsTrue(guest.State == State.Waiting);
        }

        /// <summary>
        /// Test of een human zijn path goed update en zich verplaatst naar de goede room.
        /// </summary>
        [TestMethod]
        public void TestGoToNextRoom5()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = hotel.Rooms[1, 1];
            guest.GoToRoom(hotel.Rooms[5, 1]);
            guest.GoToNextRoom();

            Assert.IsTrue(guest.Path[0] == hotel.Rooms[3, 1]);
            Assert.IsTrue(guest.Position == hotel.Rooms[2, 1].Position);
        }

        /// <summary>
        /// Test of een human een guestroom binnen gaat.
        /// </summary>
        [TestMethod]
        public void TestEnter1()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = hotel.Rooms[1, 1];
            guest.EnterRoom();

            Assert.IsTrue(guest.IsInRoom);
            Assert.IsTrue(guest.State == State.Sleeping);
            Assert.IsTrue((hotel.Rooms[1, 1] as GuestRoom).Inhabitant == guest);
        }

        /// <summary>
        /// Test of een human een cinema binnen gaat.
        /// </summary>
        [TestMethod]
        public void TestEnter2()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = hotel.Rooms[3, 3];
            guest.EnterRoom();

            Assert.IsTrue(guest.IsInRoom);
            Assert.IsTrue(guest.State == State.Watching);
            Assert.IsTrue((hotel.Rooms[3, 3] as Cinema).Inhabitants.Contains(guest));
        }

        /// <summary>
        /// Test of een human een diningroom binnen gaat.
        /// </summary>
        [TestMethod]
        public void TestEnter3()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = hotel.Rooms[1, 2];
            guest.EnterRoom();

            Assert.IsTrue(guest.IsInRoom);
            Assert.IsTrue(guest.State == State.Eating);
            Assert.IsTrue((hotel.Rooms[1, 2] as DiningRoom).Inhabitants.Contains(guest));
        }

        /// <summary>
        /// Test of een human een ElevatorShaft binnen gaat.
        /// </summary>
        [TestMethod]
        public void TestEnter4()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = hotel.Rooms[0, 0];
            guest.EnterRoom();

            Assert.IsTrue(guest.State == State.Elevatoring);
        }

        /// <summary>
        /// Test of een human een Gym binnen gaat.
        /// </summary>
        [TestMethod]
        public void TestEnter5()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = hotel.Rooms[7, 2];
            guest.EnterRoom();

            Assert.IsTrue(guest.IsInRoom);
            Assert.IsTrue(guest.State == State.Fitnessing);
            Assert.IsTrue((hotel.Rooms[7, 2] as Gym).Inhabitants.Contains(guest));
        }

        /// <summary>
        /// Test of een human een Lobby niet binnen gaat.
        /// </summary>
        [TestMethod]
        public void TestEnter6()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = hotel.Rooms[1, 0];
            guest.EnterRoom();

            Assert.IsFalse(guest.IsInRoom);
            Assert.IsTrue(guest.State == State.Idle);
        }

        /// <summary>
        /// Test wat er gebeurt als een human een kamer zonder gespecificeerde implementatie van enter probeert binnen te gaan.
        /// </summary>
        [TestMethod]
        public void TestEnter7()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = hotel.Rooms[1, 6];
            guest.EnterRoom();

            Assert.IsFalse(guest.IsInRoom);
            Assert.IsTrue(guest.State == State.Idle);
        }

        /// <summary>
        /// Test of een human een guestroom uit gaat.
        /// </summary>
        [TestMethod]
        public void TestExit1()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = hotel.Rooms[1, 1];
            guest.EnterRoom();
            guest.ExitRoom();

            Assert.IsFalse(guest.IsInRoom);
            Assert.IsTrue(guest.State == State.Idle);
        }

        /// <summary>
        /// Test of een human een cinema uit gaat.
        /// </summary>
        [TestMethod]
        public void TestExit2()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = hotel.Rooms[3, 3];
            guest.EnterRoom();
            guest.ExitRoom();

            Assert.IsFalse(guest.IsInRoom);
            Assert.IsTrue(guest.State == State.Idle);
            Assert.IsFalse((hotel.Rooms[3, 3] as Cinema).Inhabitants.Contains(guest));
        }

        /// <summary>
        /// Test of een human een diningroom uit gaat.
        /// </summary>
        [TestMethod]
        public void TestExit3()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = hotel.Rooms[1, 2];
            guest.EnterRoom();
            guest.ExitRoom();

            Assert.IsFalse(guest.IsInRoom);
            Assert.IsTrue(guest.State == State.Eating);
            Assert.IsFalse((hotel.Rooms[1, 2] as DiningRoom).Inhabitants.Contains(guest));
        }

        /// <summary>
        /// Test of een human een ElevatorShaft uit gaat.
        /// </summary>
        [TestMethod]
        public void TestExit4()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = hotel.Rooms[0, 0];
            guest.EnterRoom();
            guest.ExitRoom();

            Assert.IsTrue(guest.State == State.Walking);
        }

        /// <summary>
        /// Test of een human een Gym uit gaat.
        /// </summary>
        [TestMethod]
        public void TestExit5()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = hotel.Rooms[7, 2];
            guest.EnterRoom();
            guest.ExitRoom();

            Assert.IsFalse(guest.IsInRoom);
            Assert.IsTrue(guest.State == State.Idle);
            Assert.IsFalse((hotel.Rooms[7, 2] as Gym).Inhabitants.Contains(guest));
        }

        /// <summary>
        /// Test wat er gebeurt als een human de lobby probeert uit te gaan.
        /// </summary>
        [TestMethod]
        public void TestExit6()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = hotel.Rooms[1, 0];
            guest.EnterRoom();
            guest.ExitRoom();

            Assert.IsFalse(guest.IsInRoom);
            Assert.IsTrue(guest.State == State.Idle);
        }

        /// <summary>
        /// Test wat er gebeurt als een human een kamer zonder gespecificeerde implementatie van exit probeert uit te gaan.
        /// </summary>
        [TestMethod]
        public void TestExit7()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = hotel.Rooms[1, 6];
            guest.EnterRoom();
            guest.ExitRoom();

            Assert.IsFalse(guest.IsInRoom);
            Assert.IsTrue(guest.State == State.Idle);
        }

        /// <summary>
        /// Test of een guest dood gaat.
        /// </summary>
        [TestMethod]
        public void TestDie()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.CheckIn();
            GuestRoom guestRoom = guest.RentedRoom;
            guest.Die();

            Assert.IsTrue(guestRoom.Dirty);
            Assert.IsNull(guestRoom.RentedGuest);
            Assert.IsFalse(hotel.Guests.Contains(guest));
        }
    }
}
