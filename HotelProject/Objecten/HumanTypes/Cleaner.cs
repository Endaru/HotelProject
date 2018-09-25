using System;
using System.Collections.Generic;
using System.Drawing;

namespace HotelProject.Objecten
{
    ///<summary>Klasse van de schoonmaker die kamers van vies naar clean omzet</summary>
    public class Cleaner : Human
    {
        ///<summary>Snelheid waarmee de schoonmaker schoonmaakt.</summary>
        public static int CleanSpeed { get; set; }

        ///<summary>Duratie van schoonmaken vanuit event.</summary>
        public int Duration { get; set; }

        ///<summary>Kamer die ze gaan schoonmaken</summary>
        public Tuple<Room, int> BackupRooms { get; set; }

        /// <summary>
        /// Constructor van cleaner
        /// </summary>
        /// <param name="hotel">Het hotel waar de schoonmaker in werkt.</param>
        public Cleaner(Hotel hotel)
        {
            Name = "Cleaner";
            Graphics = Properties.Resources.Cleaner;
            Position = new Point(1, 0);
            CleanSpeed = 1;
            Hotel = hotel;
            State = State.Idle;
        }

        /// <summary>
        /// zet de tijd naar timetofinishactivity vanuit cleanspeed.
        /// </summary>
        public void SetTime()
        {
            TimeToFinishActivity = Duration;
        }

        /// <summary>
        /// De schoonmakers maken de meegegeven kamer schoon.
        /// </summary>
        /// <param name="room">De kamer die schoongemaakt moet worden.</param>
        public void Clean(Room room, int duration)
        {
            if (room != null && room.Dirty)
            {
                Duration = duration;
                room.ToBeCleaned = true;
                GoToRoom(room);
            }
        }
    }
}
