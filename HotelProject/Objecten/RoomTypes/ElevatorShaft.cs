using System;
using System.Collections.Generic;
using System.Drawing;

namespace HotelProject.Objecten
{
    ///<summary>De schacht waar een lift in kan zitten</summary>
    public class ElevatorShaft : Room
    {
        ///<summary>Lift die in deze schacht zit.</summary>
        public Elevator Elevator { get; set; }

        public Elevator ShaftElevator { get; set; }

        /// <summary>
        /// Constructor van de ElevatorShaft.
        /// </summary>
        /// <param name="elevator">De elevator die in de liftschacht kan zitten.</param>
        public ElevatorShaft(ref Elevator elevator)
        {
            ShaftElevator = elevator;
            Graphics = Properties.Resources.ElevatorShaft;
            Dimension = new Point(1, 1);
            MovementSpeed = 1;
        }

        /// <summary>
        /// Functie waarmee een human in een IInteractable kan komen.
        /// </summary>
        /// <param name="human">Dit is de human die de kamer betreed</param>
        public override void Enter(Human human)
        {
            if (Elevator != null && Elevator.MyFloor == this && Elevator.Status != ElevatorStatus.Moveing)
                Elevator.Enter(human);
            else
            {
                if (human.Path.Count > 0 && human.Path[0].GetType() == typeof(ElevatorShaft))
                {
                    ShaftElevator.RequestElevator(human, human.Room as ElevatorShaft);
                    if(human.GetType() == typeof(Guest))
                        human.State = State.Waiting;
                }
            }
        }

        /// <summary>
        /// Functie waarmee een human een IInteractable kan uitgaan.
        /// </summary>
        /// <param name="human">De persoon die uit de kamer gaat.</param>
        public override void Exit(Human human)
        {
            if (Elevator != null)
                Elevator.Exit(human);

            for (int i = 0; i < human.Path.Count; i++)
            {
                if (human.Path[i].GetType() == typeof(ElevatorShaft) && human.Path[i + 1].GetType() == typeof(ElevatorShaft))
                {
                    human.Path.Remove(human.Path[i]);
                    i--;
                }
            }

            human.State = State.Walking;
        }
    }
}
