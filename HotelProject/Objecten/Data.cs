using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelProject.Objecten
{
    ///<summary>Object die er voor zorgt dat de layout van het hotel kan worden gelezen.</summary>
    public struct Data
    {
        ///<summary>ID van Data</summary>
        public int ID { get; set; }

        ///<summary>Het type area van Data</summary>
        public string AreaType { get; set; }

        ///<summary>Position van Data waar die moet komen in het hotel</summary>
        public Point Position { get; set; }

        ///<summary>De Groote van hoe die er in het hotel moet innemen</summary>
        public Point Dimension { get; set; }

        ///<summary>Aantal sterren van het object mocht hij die hebben</summary>
        public string Classification { get; set; }

        ///<summary>Hoeveelheid mensen die in de Data kunnen op een moment.</summary>
        public int Capacity { get; set; }
    }
}
