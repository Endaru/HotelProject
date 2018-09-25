using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject.Objecten;

namespace HotelTests
{
    [TestClass]
    public class UTGuest
    {
        /// <summary>
        /// Test het aanmaken van een guest.
        /// </summary>
        [TestMethod]
        public void TestGuest()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);

            Assert.IsNotNull(guest);
        }

        /// <summary>
        /// Test bij het inchecken van een gast of hij de verwachte kamer krijgt en die kamer de goede gast krijgt.
        /// </summary>
        [TestMethod]
        public void TestCheckIn()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.CheckIn();

            Assert.IsTrue(guest.RentedRoom == hotel.Rooms[1, 1]);
            Assert.IsTrue((hotel.Rooms[1, 1] as GuestRoom).RentedGuest == guest);
        }

        /// <summary>
        /// Test of bij het uitchecken de state van de guest op checkout gaat.
        /// </summary>
        [TestMethod]
        public void TestCheckOut()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("a", 1, hotel);
            guest.CheckOut();

            Assert.IsTrue(guest.State == State.CheckOut);
        }
    }
}
