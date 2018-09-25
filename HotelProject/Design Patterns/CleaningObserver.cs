using HotelProject.Objecten;
using System;

namespace HotelProject
{
    public class CleaningObserver
    {
        private static Hotel _hotel;

        public CleaningObserver(Hotel hotel)
        {
            _hotel = hotel;
        }

        public static void Notify(Room dirtyRoom, int duration)
        {
            _hotel.AddDirtyRoom(dirtyRoom, duration);
        }
    }
}
