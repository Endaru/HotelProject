using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject.Objecten;
using HotelProject;
using HotelEvents;

namespace HotelTests
{
    /// <summary>
    /// Summary description for HotelEventHandler
    /// </summary>
    [TestClass]
    public class UTHotelEventHandler
    {
        /// <summary>
        /// Test of de handler gemaakt kan worden.
        /// </summary>
        [TestMethod]
        public void TestHandler()
        {
            Hotel hotel = new Hotel();
            HotelEventHandler handler = new HotelEventHandler(hotel);

            Assert.IsNotNull(handler);
        }

        /// <summary>
        /// Test of de handler gemaakt kan worden.
        /// </summary>
        [TestMethod]
        public void TestHandlerNotifyCHECK_IN()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            HotelEventHandler handler = new HotelEventHandler(hotel);
            HotelEvent evt = new HotelEvent();
            evt.EventType = HotelEventType.CHECK_IN;
            evt.Data = new Dictionary<string, string>();
            evt.Data.Add("a", "1");

            handler.Notify(evt);

            Assert.IsTrue(hotel.Guests.Exists(a => a.Name == "a"));
        }
    }
}
