using System.Drawing;

namespace HotelProject.Objecten
{
    ///<summary>Object stairs waarmee de gasten naar een andere verdieping kunnen</summary>
    public class Stairs : Room
    {
        /// <summary>
        /// Constructor voor het maken van een Staircase
        /// </summary>
        public Stairs()
        {
            Graphics = Properties.Resources.Stairs;
            Dimension = new Point(1, 1);
            MovementSpeed = 2;
        }
    }
}
