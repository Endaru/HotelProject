using System.Drawing;

namespace HotelProject.Objecten
{
    ///<summary>Kamer die een klant kan huren</summary>
    public class GuestRoom : Room
    {
        ///<summary> Het aantal sterren dat de kamer heeft.</summary>
        public int Stars { get; set; }

        ///<summary>De gast die in deze kamer overnacht.</summary>
        public Guest RentedGuest { get; set; }

        ///<summary>De Persoon die op dit moment in de kamer is.</summary>
        public Guest Inhabitant { get; set; }

        ///<summary>Is de kamer verhuurd of niet.</summary>
        public bool isHired = false;

        /// <summary>
        /// De constructor voor een gastenkamer
        /// </summary>
        /// <param name="position">De positie die de kamer heeft in het hotel.</param>
        /// <param name="dimension">De groote van de kamer.</param>
        /// <param name="stars">Het aantal sterren van de kamer</param>
        public GuestRoom(int id, Point position, Point dimension, int stars)
        {
            ID = id;
            Position = position;
            Dimension = dimension;
            Stars = stars;
            MovementSpeed = 1;

            //meerdere sprites laden aan de hand van de dimensie
                if (Dimension.X == 1 && Dimension.Y == 1)
                    Graphics = Properties.Resources.Room1_11;
                else if (Dimension.X == 2 && Dimension.Y == 1)
                    Graphics = Properties.Resources.Room1_2;
                else if (Dimension.X == 2 && Dimension.Y == 2)
                    Graphics = Properties.Resources.Room2_2;
                else
                    Graphics = Properties.Resources.GuestRoom;
        }

        /// <summary>
        /// Functie waarmee een human in een IInteractable kan komen
        /// </summary>
        /// <param name="human">Dit is de human die de kamer betreed</param>
        public override void Enter(Human human)
        {
            if(Inhabitant == null && human.GetType() == typeof(Guest))
            {
                Inhabitant = human as Guest;
                human.State = State.Sleeping;
            }
            if (Inhabitant != null || human.GetType() == typeof(Cleaner))
            {
                if (Dimension.X == 1 && Dimension.Y == 1)
                    Graphics = Properties.Resources.Room1_1_lit;
                else if (Dimension.X == 2 && Dimension.Y == 1)
                    Graphics = Properties.Resources.Room1_2_lit;
                else if (Dimension.X == 2 && Dimension.Y == 2)
                    Graphics = Properties.Resources.Room2_2_lit;
            }
            human.IsInRoom = true;
        }

        /// <summary>
        /// Functie waarmee een human eenIInteractable kan uitgaan
        /// </summary>
        /// <param name="human">De persoon die uit de kamer gaat.</param>
        public override void Exit(Human human)
        {
            if(human.GetType() == typeof(Guest) && Inhabitant == human)
            {
                Inhabitant = null;
            }

            if (Dimension.X == 1 && Dimension.Y == 1)
                Graphics = Properties.Resources.Room1_11;
            else if (Dimension.X == 2 && Dimension.Y == 1)
                Graphics = Properties.Resources.Room1_2;
            else if (Dimension.X == 2 && Dimension.Y == 2)
                Graphics = Properties.Resources.Room2_2;

            human.IsInRoom = false;
            human.State = State.Idle;
        }
    }
}
