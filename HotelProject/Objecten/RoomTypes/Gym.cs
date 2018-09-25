using System.Drawing;

namespace HotelProject.Objecten
{
    ///<summary>Een Gym waar klanten heen kunnen gaan.</summary>
    public class Gym : EventRoom
    {
        /// <summary>
        /// Constructor van de Gym
        /// </summary>
        /// <param name="position">De positie die de kamer heeft in het hotel.</param>
        /// <param name="dimension">De groote van de kamer.</param>
        public Gym(int id, Point position, Point dimension)
        {
            ID = id;
            Position = position;
            Dimension = dimension;
            Graphics = Properties.Resources.Gym;
            MovementSpeed = 1;
        }

        /// <summary>
        /// Functie waarmee een human in een IInteractable kan komen
        /// </summary>
        /// <param name="human">Dit is de human die de kamer betreed</param>
        public override void Enter(Human human)
        {
            human.State = State.Fitnessing;
            Inhabitants.Add(human);

            if (Inhabitants.Count > 0)
                Graphics = Properties.Resources.Gym_lit;

            human.IsInRoom = true;
        }

        /// <summary>
        /// Functie waarmee een human eenIInteractable kan uitgaan
        /// </summary>
        /// <param name="human">De persoon die uit de kamer gaat.</param>
        public override void Exit(Human human)
        {
            Inhabitants.Remove(human);
            if (Inhabitants.Count == 0)
                Graphics = Properties.Resources.Gym;

            human.IsInRoom = false;
            human.State = State.Idle;
        }
    }
}
