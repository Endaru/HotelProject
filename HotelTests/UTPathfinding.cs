using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject;
using HotelProject.Objecten;

namespace HotelTests
{
    [TestClass]
    public class UTPathfinding
    {
        /// <summary>
        /// Test of de pathfinding een pad kan vinden en terug geeft.
        /// </summary>
        [TestMethod]
        public void TestFindPath()
        {
            Hotel hotel = new Hotel();
            Pathfinding pathfinding = new Pathfinding();
            hotel.AddRooms();
            Room start = hotel.Rooms[0, 0];
            Room target = hotel.Rooms[5, 5];
            Room[] path = pathfinding.FindPath(start, target);

            Assert.IsNotNull(path);
        }

        /// <summary>
        /// Test of het gemaakt pad klopt met het verwachte pad.
        /// </summary>
        [TestMethod]
        public void TestPath()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Pathfinding pathfinding = new Pathfinding();
            Room start = hotel.Rooms[1, 0];
            Room target = hotel.Rooms[5, 5];
            Room[] path = pathfinding.FindPath(start, target);

            Assert.IsTrue(path[0] == hotel.Rooms[0, 0]);
            Assert.IsTrue(path[1] == hotel.Rooms[0, 1]);
            Assert.IsTrue(path[2] == hotel.Rooms[0, 2]);
            Assert.IsTrue(path[3] == hotel.Rooms[0, 3]);
            Assert.IsTrue(path[4] == hotel.Rooms[0, 4]);
            Assert.IsTrue(path[5] == hotel.Rooms[0, 5]);
            Assert.IsTrue(path[6] == hotel.Rooms[1, 5]);
            Assert.IsTrue(path[7] == hotel.Rooms[2, 5]);
            Assert.IsTrue(path[8] == hotel.Rooms[3, 5]);
            Assert.IsTrue(path[9] == hotel.Rooms[4, 5]);
            Assert.IsTrue(path[10] == hotel.Rooms[5, 5]);
        }
    }
}
