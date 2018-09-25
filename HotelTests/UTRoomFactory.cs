using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject.Objecten;
using System.Drawing;

namespace HotelTests
{
    [TestClass]
    public class UTRoomFactory
    {
        /// <summary>
        /// Test of er van een data object een cinema gemaakt kan worden.
        /// </summary>
        [TestMethod]
        public void TestCreateRoom1()
        {
            Data data = new Data();
            data.AreaType = "Cinema";
            data.Position = new Point(1, 1);
            data.Dimension = new Point(2, 2);

            Room room = RoomFactory.CreateRoom(data);

            Assert.IsTrue(room.GetType() == typeof(Cinema));
        }

        /// <summary>
        /// Test of er van een data object een diningroom gemaakt kan worden.
        /// </summary>
        [TestMethod]
        public void TestCreateRoom2()
        {
            Data data = new Data();
            data.AreaType = "Restaurant";
            data.Position = new Point(1, 1);
            data.Dimension = new Point(2, 2);

            Room room = RoomFactory.CreateRoom(data);

            Assert.IsTrue(room.GetType() == typeof(DiningRoom));
        }

        /// <summary>
        /// Test of er van een data object een gym gemaakt kan worden.
        /// </summary>
        [TestMethod]
        public void TestCreateRoom3()
        {
            Data data = new Data();
            data.AreaType = "Fitness";
            data.Position = new Point(1, 1);
            data.Dimension = new Point(2, 2);

            Room room = RoomFactory.CreateRoom(data);

            Assert.IsTrue(room.GetType() == typeof(Gym));
        }

        /// <summary>
        /// Test of er van een data object een guestroom gemaakt kan worden.
        /// </summary>
        [TestMethod]
        public void TestCreateRoom4()
        {
            Data data = new Data();
            data.AreaType = "Room";
            data.Position = new Point(1, 1);
            data.Dimension = new Point(2, 2);
            data.Classification = "1 ster";

            Room room = RoomFactory.CreateRoom(data);

            Assert.IsTrue(room.GetType() == typeof(GuestRoom));
        }

        /// <summary>
        /// Test of er van een data object een swimmingpool gemaakt kan worden.
        /// </summary>
        [TestMethod]
        public void TestCreateRoom5()
        {
            Data data = new Data();
            data.AreaType = "Pool";
            data.Position = new Point(1, 1);
            data.Dimension = new Point(2, 2);

            Room room = RoomFactory.CreateRoom(data);

            Assert.IsTrue(room.GetType() == typeof(SwimmingPool));
        }
    }
}
