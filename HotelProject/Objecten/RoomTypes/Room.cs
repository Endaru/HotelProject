using System;
using System.Collections.Generic;
using System.Drawing;

namespace HotelProject.Objecten
{
    public class Room : IInteractable
    {
        ///<summary>ID van der kamer.</summary>
        public int ID { get; set; }

        ///<summary>Sla alle neighbor rooms op in deze lijst.</summary>
        public List<Room> Neighbors { get; set; }

        ///<summary>Positie in worldspace.</summary>
        public Point Position { get; set; }

        ///<summary>Afbeelding die de kamer heeft en word weergegeven op het scherm.</summary>
        public Image Graphics { get; set; }

        ///<summary>Groote van de kamer.</summary>
        public Point Dimension { get; set; }

        ///<summary>Of de kamer vies is of niet</summary>
        public bool Dirty { get; set; }

        ///<summary>Of er al een schoonmaker naar de kamer gaat.</summary>
        public bool ToBeCleaned { get; set; }

        ///<summary>Lengthe die gebruikt wordt met pathfinding</summary>
        public int Distance { get; set; }

        ///<summary>Parent room van deze room, gebruikt voor pathfinding.</summary>
        public Room Parent { get; set; }

        ///<summary>Snelheid waarmee de persoon over de kamer loopt</summary>
        public int MovementSpeed { get; set; }

        /// <summary>
        /// Constructor van kamer waarmee de basisonderdelen worden aangemaakt
        /// </summary>
        public Room()
        {
            Neighbors = new List<Room>();
            Distance = int.MaxValue;
            MovementSpeed = 1;
        }

        /// <summary>
        /// Constructor om een kamer aan te maken met extra onderdelen.
        /// </summary>
        /// <param name="id"> ID die de kamer heeft vanuit de layout file</param>
        /// <param name="position"> positie in het hotel volgens de layout file</param>
        /// <param name="dimension"> De dimensie van de kamer.</param>
        public Room(int id, Point position, Point dimension)
        {
            ID = id;
            Position = position;
            Dimension = dimension;
            Neighbors = new List<Room>();
            Distance = int.MaxValue;
            MovementSpeed = 1;
        }

        /// <summary>
        /// Hiermee wordt de Room op Dirty gezet en wordt de CleaningObserver genotified.
        /// </summary>
        /// <param name="duration">Hoelang het schoonmaken moet duren.</param>
        public void SetDirty(int duration)
        {
            Dirty = true;
            CleaningObserver.Notify(this, duration);
        }

        /// <summary>
        /// Functie waarmee een human in een IInteractable kan komen
        /// </summary>
        /// <param name="human">Dit is de human die de kamer betreed</param>
        public virtual void Enter(Human human)
        {
            return;
        }

        /// <summary>
        /// Functie waarmee een human eenIInteractable kan uitgaan
        /// </summary>
        /// <param name="human">De persoon die uit de kamer gaat.</param>
        public virtual void Exit(Human human)
        {
            return;
        }
    }
}
