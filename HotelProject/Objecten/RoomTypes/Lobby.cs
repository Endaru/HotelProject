using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace HotelProject.Objecten
{
    ///<summary>De lobby in het hotel.</summary>
    public class Lobby : Room
    {
        /// <summary>
        /// Constructor van lobby.
        /// </summary>
        public Lobby()
        {
            Graphics = Properties.Resources.Lobby;
            Dimension = new Point(1, 1);
            MovementSpeed = 1;
        }

        /// <summary>
        /// Gast wordt met deze functie ingecheckt in het hotel.
        /// </summary>
        /// <param name="guest">De gast die gaat inchecken</param>
        /// <param name="StarAmmount">Het aantal sterren wat de gast zoekt voor een kamer</param>
        /// <param name="hotel">Het hotel waar de klant incheckt</param>
        public void CheckInGuest(Guest guest, int StarAmmount, Hotel hotel)
        {
            hotel.Guests.Add(guest);
            guest.Room = this;
            guest.Position = Position;
            List<Tuple<GuestRoom, int>> rooms = new List<Tuple<GuestRoom, int>>();
            Pathfinding pathfinding = new Pathfinding();

            //Voeg alle kamers toe aan de lijst met de afstand van de lobby naar de kamer.
            foreach (Room room in hotel.Rooms)
            {
                if (room != null && room.GetType() == typeof(GuestRoom))
                    rooms.Add(new Tuple<GuestRoom, int>((GuestRoom)room, pathfinding.FindPath(this, room).Length));
            }

            //Zoek naar kamers die voldoen aan de requirements.
            IEnumerable<Tuple<GuestRoom, int>> possibleRooms =
                (from item in rooms
                 where item.Item1.Stars >= StarAmmount
                 && item.Item1.isHired == false
                 orderby item.Item1.Stars ascending
                 select item);

            //Neem de dichtstbijzijnde kamer
            GuestRoom roomQuery = null;
            if (possibleRooms.Count() > 0)
                roomQuery = possibleRooms.Aggregate((x, y) => x.Item2 < y.Item2 ? x : y).Item1;
            //Als de kamer niet is gevonden dan gaat de gast weg.
            else
            {
                hotel.Guests.Remove(guest);
                guest = null;
                return;
            }

            //Als de kamer is gevonden check de gast dan in.
            if (roomQuery != null)
            {
                roomQuery.RentedGuest = guest;
                guest.RentedRoom = roomQuery;
                roomQuery.isHired = true;
                guest.CurrentTask.SetTask(guest.GoToRoom, roomQuery);
            }
        }

        /// <summary>
        /// Gast kan uitchecken bij het hotel.
        /// </summary>
        /// <param name="guest">De gast die uitcheckt</param>
        /// <param name="hotel">Het hotel waar hij uitcheckt</param>
        /// <returns></returns>
        public void CheckOutGuest(Guest guest, Hotel hotel)
        {
            hotel.Guests.Remove(guest);

            if (guest.RentedRoom != null)
            {
                guest.RentedRoom.isHired = false;
                guest.RentedRoom.RentedGuest = null;
                guest.RentedRoom.SetDirty(1);
            }
            guest = null;
        }
    }
}
