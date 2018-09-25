using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject.Objecten;

namespace HotelTests
{
    [TestClass]
    public class UTCleaner
    {
        /// <summary>
        /// Test het aanmaken van een cleaner.
        /// </summary>
        [TestMethod]
        public void TestCleaner()
        {
            Hotel hotel = new Hotel();
            Cleaner cleaner = new Cleaner(hotel);

            Assert.IsNotNull(cleaner);
        }

        /// <summary>
        /// Test het zetten van de tijd om schoon te maken.
        /// </summary>
        [TestMethod]
        public void TestSetTime()
        {
            Hotel hotel = new Hotel();
            Cleaner cleaner = new Cleaner(hotel);
            cleaner.Duration = 3;
            cleaner.SetTime();

            Assert.IsTrue(cleaner.TimeToFinishActivity == 3);
        }

        /// <summary>
        /// Test wat er gebeurt als er een kamer wordt meegegeven die niet vies is: Er gebeurt niks.
        /// </summary>
        [TestMethod]
        public void TestClean1()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Cleaner cleaner = new Cleaner(hotel);
            cleaner.Clean(hotel.Rooms[1, 1], 5);

            Assert.IsTrue(cleaner.Duration == 0);
            Assert.IsFalse(hotel.Rooms[1, 1].ToBeCleaned);
        }

        /// <summary>
        /// Test wat er gebeurt als er een kamer wordt meegegeven die wel vies is: Duration wordt gelijk gezet aan de parameter 
        /// en de kamer gaat schoongemaakt worden.
        /// </summary>
        [TestMethod]
        public void TestClean2()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Cleaner cleaner = new Cleaner(hotel);
            cleaner.Room = hotel.Rooms[1, 0];
            hotel.Rooms[1, 1].Dirty = true;
            cleaner.Clean(hotel.Rooms[1, 1], 5);

            Assert.IsTrue(cleaner.Duration == 5);
            Assert.IsTrue(hotel.Rooms[1, 1].ToBeCleaned);
        }
    }
}
