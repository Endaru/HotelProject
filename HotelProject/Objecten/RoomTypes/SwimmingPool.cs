using System.Drawing;

namespace HotelProject.Objecten
{
    ///<summary>Object SwimmingPool waar gasten niks kunnen doen.</summary>
    public class SwimmingPool : EventRoom
    {
        /// <summary>
        /// Constructor van swimmingpool
        /// </summary>
        /// <param name="id">Id van de swimmingpool</param>
        /// <param name="position">Positie van de swimmingpool</param>
        /// <param name="dimension"></param>
        public SwimmingPool(int id, Point position, Point dimension)
        {
            ID = id;
            Position = position;
            Dimension = dimension;
            Graphics = Properties.Resources.swimmingpool;
            MovementSpeed = 1;
        }
    }
}
