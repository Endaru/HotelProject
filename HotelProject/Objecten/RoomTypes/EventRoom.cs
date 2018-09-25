using System.Collections.Generic;

namespace HotelProject.Objecten
{
    /// <summary>Abstracte room waar events in plaats kunnen vinden</summary>
    public abstract class EventRoom : Room
    {
        ///<summary>Lijst van de mensen die zich in deze kamer bevinden.</summary>
        public List<Human> Inhabitants { get; set; }

        /// <summary>
        /// Constructor van de eventroom.
        /// </summary>
        public EventRoom()
        {
            Neighbors = new List<Room>();
            Inhabitants = new List<Human>();
            Dirty = false;
        }
    }
}
