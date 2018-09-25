using System;
using System.Collections.Generic;
using System.Text;
using HotelProject.Objecten;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace HotelProject
{
    public static class LayoutReader
    {
        ///<summary>Maximale hoogte van het hotel</summary>
        public static int MaxHeight { get; set; }

        ///<summary>Maximale breedte van het hotel</summary>
        public static int MaxWidth { get; set; }

        ///<summary>pad van de layout file.</summary>
        public static string FilePath { get; set; } = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))),"HotelProject\\Resources\\Hotel3.layout");

        /// <summary>
        /// Lees de layout van de file
        /// </summary>
        /// <returns>Geeft een lijst van kamers terug.</returns>
        public static List<Room> ReadLayoutFile()
        {
            Data[] readData = null;

            try
            {
                string file = File.ReadAllText(FilePath);
                readData = JsonConvert.DeserializeObject<Data[]>(file);
            }
            catch (Exception)
            {

                MessageBox.Show("Unsupported character found in file, standard file will be loaded.");
                FilePath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))), "HotelProject\\Resources\\Hotel3.layout");
                string file = File.ReadAllText(FilePath);
                readData = JsonConvert.DeserializeObject<Data[]>(file);
            }

            foreach(Data d in readData)
            {
                if (d.Position.X == 0)
                    for (int i = 0; i < readData.Length; i++)
                        readData[i].Position = new System.Drawing.Point(readData[i].Position.X + 1, readData[i].Position.Y);
            }

            List<Room> loadedRooms = new List<Room>();
            foreach (Data d in readData)
            {
                loadedRooms.Add(RoomFactory.CreateRoom(d));   
                MaxHeight = MaxHeight < d.Position.Y + (d.Dimension.Y - 1) ? d.Position.Y + (d.Dimension.Y - 1) : MaxHeight;
                MaxWidth = MaxWidth < d.Position.X + (d.Dimension.X - 1) ? d.Position.X + (d.Dimension.X - 1) : MaxWidth;
            }
            // Correcties voor de lobby bij de hoogte en de lift en trap bij de wijdte
            MaxHeight += 1;
            MaxWidth += 2;

            return loadedRooms;
        }
    }
}
