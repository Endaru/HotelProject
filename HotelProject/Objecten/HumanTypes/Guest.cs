using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelProject.Objecten
{
    ///<summary>Gast die in het hotel is.</summary>
    public class Guest : Human
    {
        ///<summary>Aantal sterren wat de klant wil voor </summary>
        public int StarWish { get; set; }

        ///<summary>Kamer die deze persoon heeft gehuurd.</summary>
        public GuestRoom RentedRoom { get; set; }

        ///<summary>Tijd hoelang het duurt </summary>
        public static int TimeToStarve { get; set; }

        ///<summary>Op hoeveel starvation de Guest zit.</summary>
        public int Starvation = 0;

        /// <summary>
        /// Constructor van Guest
        /// </summary>
        /// <param name="name">naam van de gast</param>
        /// <param name="starWish">het aantal sterren dat de gast wil als kamer</param>
        /// <param name="hotel">Het hotel waar de gast ingaat.</param>
        public Guest(string name, int starWish, Hotel hotel)
        {
            Name = name;
            StarWish = starWish;
            Hotel = hotel;
            Room = Hotel.Rooms[1, 0];
            Graphics = Properties.Resources.Guest1;
            State = State.Idle;
            TimeToStarve = 20;
        }

        /// <summary>
        /// Gast checkt in bij het hotel.
        /// </summary>
        public void CheckIn()
        {
            FindRoom<Lobby>(Hotel);
            (Room as Lobby).CheckInGuest(this, StarWish, Hotel);
        }

        /// <summary>
        /// Gast checkt uit het hotel.
        /// </summary>
        public void CheckOut()
        {
            State = State.CheckOut;
            FindRoom<Lobby>(Hotel);
        }
    }
}
