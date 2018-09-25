using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelProject.Objecten
{
    public static class RoomFactory
    {
        /// <summary>
        /// Maak het goede soort kamer aan en return deze.
        /// </summary>
        /// <param name="d">Data waarmee de kamer wordt gemaakt.</param>
        /// <returns>Kamer van het goede soort.</returns>
        public static Room CreateRoom(Data d)
        {
            switch (d.AreaType)
            {
                case "Cinema":
                    return new Cinema(d.ID, new System.Drawing.Point(d.Position.X, d.Position.Y), d.Dimension);
                case "Restaurant":
                    return new DiningRoom(d.ID, new System.Drawing.Point(d.Position.X, d.Position.Y), d.Dimension, d.Capacity);
                case "Fitness":
                    return new Gym(d.ID, new System.Drawing.Point(d.Position.X, d.Position.Y), d.Dimension);
                case "Room":
                    return new GuestRoom(d.ID, new System.Drawing.Point(d.Position.X, d.Position.Y), d.Dimension, Convert.ToInt32(d.Classification.Substring(0, 1)));
                case "Pool":
                   return new SwimmingPool(d.ID, new System.Drawing.Point(d.Position.X, d.Position.Y), d.Dimension);
                default:
                    return new Room(d.ID, new System.Drawing.Point(d.Position.X, d.Position.Y), d.Dimension);
            }
        }
    }
}
