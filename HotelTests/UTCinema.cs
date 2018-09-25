using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject.Objecten;
using System.Drawing;

namespace HotelTests
{
    [TestClass]
    public class UTCinema
    {
        /// <summary>
        /// Test het aanmaken van een cinema.
        /// </summary>
        [TestMethod]
        public void TestCinema()
        {
            Cinema cinema = new Cinema(1, new Point(1, 1), new Point(1, 1));

            Assert.IsNotNull(cinema);
        }

        /// <summary>
        /// Test het starten van een film in de cinema.
        /// </summary>
        [TestMethod]
        public void TestStartCinema()
        {
            Cinema cinema = new Cinema(1, new Point(1, 1), new Point(1, 1));
            cinema.CinemaStart();

            Assert.IsTrue(cinema.CinemaStarted);
        }

        /// <summary>
        /// Test of een gast gaat wachten als hij een cinema probeert binnen te gaan als de film gestart is.
        /// </summary>
        [TestMethod]
        public void TestEnter1()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Cinema cinema = hotel.Rooms[3, 3] as Cinema;
            cinema.CinemaStart();
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = cinema;
            guest.EnterRoom();

            Assert.IsTrue(guest.State == State.Waiting);
        }

        /// <summary>
        /// Test of een gast de cinema binnen gaat.
        /// </summary>
        [TestMethod]
        public void TestEnter2()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Cinema cinema = hotel.Rooms[3, 3] as Cinema;
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = cinema;
            guest.EnterRoom();

            Assert.IsTrue(cinema.Inhabitants.Contains(guest));
            Assert.IsTrue(guest.State == State.Watching);
            Assert.IsTrue(guest.IsInRoom);
        }

        /// <summary>
        /// Test wat er gebeurt als een gast een cinema probeert uit te gaan terwijl hij er niet in zit.
        /// </summary>
        [TestMethod]
        public void TestExit1()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Cinema cinema = hotel.Rooms[3, 3] as Cinema;
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = cinema;
            guest.ExitRoom();

            Assert.IsTrue(guest.State == State.Idle);
            Assert.IsFalse(guest.IsInRoom);
        }

        /// <summary>
        /// Test wat er gebeurt als een gast een cinema probeert uit te gaan terwijl hij er niet in zit.
        /// </summary>
        [TestMethod]
        public void TestExit2()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Cinema cinema = hotel.Rooms[3, 3] as Cinema;
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = cinema;
            guest.EnterRoom();
            guest.ExitRoom();

            Assert.IsTrue(guest.State == State.Idle);
            Assert.IsFalse(guest.IsInRoom);
            Assert.IsFalse(cinema.Inhabitants.Contains(guest));
        }
    }
}
