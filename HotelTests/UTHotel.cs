using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HotelProject;
using HotelProject.Objecten;
using System.Drawing;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HotelTests
{
    [TestClass]
    public class UTHotel
    {
        /// <summary>
        /// Test de constructor van hotel.
        /// </summary>
        [TestMethod]
        public void TestHotel()
        {
            Hotel hotel = new Hotel();

            Assert.IsNotNull(hotel);
        }

        /// <summary>
        /// Test of er een guest aan het hotel wordt toegevoegd.
        /// </summary>
        [TestMethod]
        public void TestAddGuest()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            hotel.AddGuest("a", 1);

            Assert.IsTrue(hotel.Guests.Count > 0);
        }

        /// <summary>
        /// Test of er kamers en cleaners toegevoegd worden aan het hotel.
        /// </summary>
        [TestMethod]
        public void TestAddRooms()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();

            Assert.IsTrue(hotel.Rooms.GetLength(0) > 0 && hotel.Rooms.GetLength(1) > 0);
            Assert.IsNotNull(hotel.Cleaners[0]);
            Assert.IsNotNull(hotel.Cleaners[1]);
        }

        /// <summary>
        /// Test of de gemaakte rectangles gereturned kunnen worden.
        /// </summary>
        [TestMethod]
        public void TestGetRectangles()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            List<Rectangle> rect = hotel.GetRectangles();

            Assert.IsNotNull(rect);
        }

        /// <summary>
        /// Test of er een bitmap wordt aangemaakt.
        /// </summary>
        [TestMethod]
        public void TestDisplayHotel()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Bitmap bmp = hotel.DisplayHotel();

            Assert.IsNotNull(bmp);
        }

        /// <summary>
        /// Test of iedereens state op evacueren gaat.
        /// </summary>
        [TestMethod]
        public void TestEvacuate()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            hotel.AddGuest("a", 1);
            hotel.AddGuest("b", 1);
            hotel.AddGuest("c", 1);
            hotel.AddGuest("d", 1);
            hotel.AddGuest("e", 1);
            hotel.Evacuate();

            foreach(Guest g in hotel.Guests)
            {
                Assert.IsTrue(g.Evac == EvacProcess.Evacuating);
            }

            foreach (Cleaner c in hotel.Cleaners)
            {
                Assert.IsTrue(c.Evac == EvacProcess.Evacuating);
            }
        }

        /// <summary>
        /// Test of gasten stoppen met evacueren en terug naar hun kamer gaan en of cleaners ook stoppen met evacueren en niks gaan doen.
        /// </summary>
        [TestMethod]
        public void TestDevacuate()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            hotel.AddGuest("a", 1);
            hotel.AddGuest("b", 1);
            hotel.AddGuest("c", 1);
            hotel.AddGuest("d", 1);
            hotel.AddGuest("e", 1);
            hotel.Evacuate();
            hotel.Devacuate();

            foreach (Guest g in hotel.Guests)
            {
                Assert.IsTrue(g.State == State.Walking);
                Assert.IsTrue(g.Evac == EvacProcess.None);
            }

            foreach (Cleaner c in hotel.Cleaners)
            {
                Assert.IsTrue(c.State == State.Idle);
                Assert.IsTrue(c.Evac == EvacProcess.None);
            }
        }
    }
}
