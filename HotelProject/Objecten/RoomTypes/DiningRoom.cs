using System.Drawing;

namespace HotelProject.Objecten
{
    ///<summary>Kamer waar klanten naartoe gaan om te eten</summary>
    public class DiningRoom : EventRoom
    {
        ///<summary>Maximaal aantal zitplaatsen in de DiningRoom</summary>
        public int Capacity { get; set; }

        ///<summary>Hoelang het duurt voordat mensen klaar zijn met eten in het hotel.</summary>
        public static int DiningTime { get; set; }

        /// <summary>
        /// Constructor van de Diningroom
        /// </summary>
        /// <param name="id">Het id van het restaurant</param>
        /// <param name="position">De Positie van het restaurant in het hotel</param>
        /// <param name="dimension">De dimensie van het restaurant</param>
        /// <param name="capacity">Hoeveel mensen in het restaurant kunnen zijn op een moment</param>
        public DiningRoom(int id, Point position, Point dimension, int capacity)
        {
            ID = id;
            Position = position;
            Dimension = dimension;
            Capacity = capacity;
            Graphics = Properties.Resources.DiningRoom;
            DiningTime = 10;
            MovementSpeed = 1;
        }

        /// <summary>
        /// Een human gaat naar binnen.
        /// </summary>
        /// <param name="human">De persoon die naar binnen wil in de kamer</param>
        public override void Enter(Human human)
        {
            if (Inhabitants.Count < Capacity)
            {
                human.State = State.Eating;
                human.TimeToFinishActivity = DiningTime;
                Inhabitants.Add(human);
            }
            else
                human.State = State.Waiting;

            if (Inhabitants.Count > 0)
                Graphics = Properties.Resources.DiningRoom_lit;

            human.IsInRoom = true;
        }

        /// <summary>
        /// Een human gaat naar buiten
        /// </summary>
        /// <param name="human">De persoon die naar buiten gaat.</param>
        public override void Exit(Human human)
        {
            Inhabitants.Remove(human);
            if (Inhabitants.Count == 0)
                Graphics = Properties.Resources.DiningRoom;

            human.IsInRoom = false;
        }
    }
}
