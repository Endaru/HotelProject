using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelProject.Objecten;

namespace HotelTests
{
    [TestClass]
    public class UTElevatorShaft
    {
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ElevatorShaftTest()
        {
            Elevator elevator = new Elevator();
            ElevatorShaft elevatorShaft = new ElevatorShaft(ref elevator);

            Assert.IsNotNull(elevatorShaft);
        }

        /// <summary>
        /// kan niet in de elevatorshaft omdat de elevator niet op die vloer is.
        /// </summary>
        [TestMethod]
        public void ElevatorShaftEnterWait()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Elevator elevator = new Elevator();
            ElevatorShaft elevatorShaft = hotel.Rooms[0, 1] as ElevatorShaft;
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = elevatorShaft;
            guest.Path.Add(elevatorShaft);
            guest.EnterRoom();

            Assert.IsTrue(guest.State == State.Waiting);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ElevatorShaftEnter()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Elevator elevator = new Elevator();
            ElevatorShaft elevatorShaft = hotel.Rooms[0, 0] as ElevatorShaft;
            ElevatorShaft elevatorShaft1 = hotel.Rooms[0, 1] as ElevatorShaft;
            Guest guest = new Guest("a", 1, hotel);
            elevator.MyFloor = elevatorShaft;
            elevatorShaft.Elevator = elevator;
            guest.Room = elevatorShaft;
            guest.EnterRoom();

            Assert.IsTrue(guest.State == State.Elevatoring);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ElevatorShaftExitNotInRoom()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Elevator elevator = new Elevator();
            ElevatorShaft elevatorShaft = hotel.Rooms[0, 0] as ElevatorShaft;
            Guest guest = new Guest("a", 1, hotel);
            guest.Room = elevatorShaft;
            guest.ExitRoom();

            Assert.IsTrue(guest.State == State.Walking);
            Assert.IsFalse(guest.IsInRoom);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ElevatorShaftExit()
        {
            Hotel hotel = new Hotel();
            hotel.AddRooms();
            Elevator elevator = new Elevator();
            ElevatorShaft elevatorShaft = hotel.Rooms[0, 0] as ElevatorShaft;
            Guest guest = new Guest("a", 1, hotel);
            elevatorShaft.Elevator = elevator;
            guest.Room = elevatorShaft;
            guest.EnterRoom();
            guest.ExitRoom();

            Assert.IsTrue(guest.State == State.Walking);
        }
    }
}
