using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject.Objecten;
using System.Drawing;

namespace HotelTests
{
    [TestClass]
    public class UTGym
    {
        /// <summary>
        /// Test het aanmaken van een gym.
        /// </summary>
        [TestMethod]
        public void TestGym()
        {
            Gym gym = new Gym(1, new Point(1, 1), new Point(1, 1));

            Assert.IsNotNull(gym);
        }

        /// <summary>
        /// Test het aanmaken van een gym.
        /// </summary>
        [TestMethod]
        public void TestEnter()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Gym gym = hotel.Rooms[7, 2] as Gym;
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = gym;
            guest.EnterRoom();

            Assert.IsTrue(gym.Inhabitants.Contains(guest));
            Assert.IsTrue(guest.State == State.Fitnessing);
            Assert.IsTrue(guest.IsInRoom);
        }

        /// <summary>
        /// Test het aanmaken van een gym.
        /// </summary>
        [TestMethod]
        public void TestExit()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Gym gym = hotel.Rooms[7, 2] as Gym;
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = gym;
            guest.EnterRoom();
            guest.ExitRoom();

            Assert.IsTrue(guest.State == State.Idle);
            Assert.IsFalse(guest.IsInRoom);
            Assert.IsFalse(gym.Inhabitants.Contains(guest));
        }
    }
}
