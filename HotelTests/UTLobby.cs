using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject.Objecten;

namespace HotelTests
{
    [TestClass]
    public class UTLobby
    {
        /// <summary>
        /// Test of er een lobby aangemaakt kan worden.
        /// </summary>
        [TestMethod]
        public void TestLobby()
        {
            Lobby lobby = new Lobby();

            Assert.IsNotNull(lobby);
        }

        /// <summary>
        /// Test of een gast zich kan inchecken bij de lobby en een kamer krijgt.
        /// </summary>
        [TestMethod]
        public void TestCheckInGuests()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("Test", 1, hotel);
            Lobby lobby = (Lobby)hotel.Rooms[1, 0];

            lobby.CheckInGuest(guest, 3, hotel);

            Assert.IsNotNull(guest.RentedRoom);
        }

        /// <summary>
        /// Check of de klant niet meer bestaat en niet meer in de klanten lijst staat.
        /// </summary>
        [TestMethod]
        public void TestCheckOutGuests()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Guest guest = new Guest("Test", 1, hotel);
            Lobby lobby = (Lobby)hotel.Rooms[1, 0];

            lobby.CheckInGuest(guest, 3, hotel);
            lobby.CheckOutGuest(guest, hotel);

            Assert.IsFalse(hotel.Guests.Contains(guest));
        }
    }
}
