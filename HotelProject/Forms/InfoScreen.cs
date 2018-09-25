using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Linq;
using HotelProject.Objecten;

namespace HotelProject
{
    public partial class InfoScreen : Form
    {
        /// <summary>Het form dat deze heeft aangemaakt.</summary>
        private MainForm _owner { get; set; }

        /// <summary>Het hotel van de _owner</summary>
        private Hotel _hotel { get; set; } 

        /// <summary>
        /// Constructor van Infoscreen.
        /// </summary>
        /// <param name="form">_owner van het infoscreen</param>
        /// <param name="hotel">Het hotel die de _owner</param>
        public InfoScreen(MainForm form, Hotel hotel)
        {
            InitializeComponent();
            _owner = form;
            _hotel = hotel;
            InitializeDataObjects();        
        }

        /// <summary>
        /// De tabellen van Infoscreen worden in deze functie gevuld.
        /// </summary>
        private void InitializeDataObjects()
        {
            DataTable tableGuestRooms = new DataTable();
            tableGuestRooms.Columns.Add("Room nr", typeof(int));
            tableGuestRooms.Columns.Add("Position", typeof(Point));
            tableGuestRooms.Columns.Add("Dirty", typeof(bool));
            tableGuestRooms.Columns.Add("Stars", typeof(int));
            tableGuestRooms.Columns.Add("Is Hired", typeof(bool));
            tableGuestRooms.Columns.Add("Inhabitant", typeof(string));

            foreach (var item in _hotel.Rooms)
            {
                if (item != null && item.GetType() == typeof(GuestRoom))
                {
                    GuestRoom room = (GuestRoom)item;
                    tableGuestRooms.Rows.Add(room.ID, room.Position, room.Dirty, room.Stars, room.isHired, room.RentedGuest != null ? room.RentedGuest.Name : "");
                }
            }
            dataGridView1.DataSource = tableGuestRooms;

            DataTable tableSpecialRooms = new DataTable();
            tableSpecialRooms.Columns.Add("Type", typeof(string));
            tableSpecialRooms.Columns.Add("Position", typeof(Point));
            tableSpecialRooms.Columns.Add("Dirty", typeof(bool));
            tableSpecialRooms.Columns.Add("People inside", typeof(int));
            tableSpecialRooms.Columns.Add("Capacity", typeof(int));

            foreach (var item in _hotel.Rooms)
            {
                if (item != null)
                {
                    if (item.GetType() == typeof(Cinema) || item.GetType() == typeof(Gym) || item.GetType() == typeof(SwimmingPool))
                    {
                        Room room = item;
                        tableSpecialRooms.Rows.Add(room.GetType().Name, room.Position, room.Dirty, (room as EventRoom).Inhabitants.Count, null);
                    }
                    else if (item.GetType() == typeof(DiningRoom))
                    {
                        DiningRoom room = (DiningRoom)item;
                        tableSpecialRooms.Rows.Add(room.GetType().Name, room.Position, room.Dirty, (room as EventRoom).Inhabitants.Count, room.Capacity);
                    }
                }
            }

            dataGridView2.DataSource = tableSpecialRooms;

            DataTable tablePeople = new DataTable();
            tablePeople.Columns.Add("Name", typeof(string));
            tablePeople.Columns.Add("Room nr.", typeof(int));
            tablePeople.Columns.Add("Position", typeof(Point));
            tablePeople.Columns.Add("State", typeof(State));

            foreach (var item in _hotel.Guests)
                tablePeople.Rows.Add(item.Name, item.RentedRoom.ID, item.Position, item.State);

            foreach (var item in _hotel.Cleaners)
                tablePeople.Rows.Add(item.Name, null, item.Position, item.State);

            dataGridView3.DataSource = tablePeople;

            textBox1.Text = HotelEvents.HotelEventManager.HTE_Factor.ToString();
            textBox2.Text = _hotel.Rooms.OfType<Stairs>().First().MovementSpeed.ToString();
            textBox3.Text = DiningRoom.DiningTime.ToString();
            textBox4.Text = Cleaner.CleanSpeed.ToString();
            textBox5.Text = Cinema.MovieTime.ToString();
            textBox7.Text = Guest.TimeToStarve.ToString();
        }

        /// <summary>
        /// Als het infoscherm word gesloten dan voer je dit uit en ga je terug naar mainform.
        /// </summary>
        /// <param name="sender">Waar het vandaan komt</param>
        /// <param name="e">Lege parameter</param>
        private void InfoScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!SaveSettings())
            {
                e.Cancel = true;
                return;
            }
            _owner.ResumeSimulation();
        }

        /// <summary>
        /// Hier worden de settings aangepast als er onderdelen zijn veranderd
        /// </summary>
        /// <returns>of savesettings is gelukt</returns>
        private bool SaveSettings()
        {
            bool succes = true;

            float hteResult = HotelEvents.HotelEventManager.HTE_Factor;
            //als de tekst voldoet aan de Regex dan mag het worden gebruikt.
            if (Regex.IsMatch(textBox1.Text, "^[0-9]{1,3}([.,][0-9]{1,3})?$"))
            {
                float.TryParse(textBox1.Text, out hteResult);
                succes = succes ? true : false;
            }
            else
            {
                //message bij geval van fout.
                MessageBox.Show("Invoer bij 'Simulation Speed' is niet juist!");
                succes = false;
            }
            HotelEvents.HotelEventManager.HTE_Factor = hteResult;

            int stairResult = _hotel.Rooms.OfType<Stairs>().First().MovementSpeed;
            //als de tekst voldoet aan de Regex dan mag het worden gebruikt.
            if (Regex.IsMatch(textBox2.Text, "^[0-9]{1,3}([.,][0-9]{1,3})?$"))
            {
                int.TryParse(textBox2.Text, out stairResult);
                succes = succes ? true : false;
            }
            else
            {
                //message bij geval van fout.
                MessageBox.Show("Invoer bij 'Stair Length' is niet juist!");
                succes = false;
            }
            foreach (Room room in _hotel.Rooms)
            {
                if (room != null && room.GetType() == typeof(Stairs))
                    room.MovementSpeed = stairResult;
            }

            int diningResult = DiningRoom.DiningTime;
            if (int.TryParse(textBox3.Text, out diningResult))
                succes = succes ? true : false;
            else
            {
                //message bij geval van fout.
                MessageBox.Show("Invoer bij 'Dining Length' is niet juist!");
                succes = false;
            }
            DiningRoom.DiningTime = diningResult;

            int cinemaResult = Cinema.MovieTime;
            if (int.TryParse(textBox5.Text, out cinemaResult))
                succes = succes ? true : false;
            else
            {
                //message bij geval van fout.
                MessageBox.Show("Invoer bij 'Movie Length' is niet juist!");
                succes = false;
            }
            Cinema.MovieTime = cinemaResult;

            int cleanerResult = Cleaner.CleanSpeed;
            if (int.TryParse(textBox4.Text, out cleanerResult))
                succes = succes ? true : false;
            else
            {
                //message bij geval van fout.
                MessageBox.Show("Invoer bij 'Clean Speed' is niet juist!");
                succes = false;
            }
            Cleaner.CleanSpeed = cleanerResult;

            int elevatorResult = Elevator.Speed;
            if (int.TryParse(textBox4.Text, out cleanerResult))
                succes = succes ? true : false;
            else
            {
                //message bij geval van fout.
                MessageBox.Show("Invoer bij 'Elevator Speed' is niet juist!");
                succes = false;
            }
            Elevator.Speed = elevatorResult;

            int timetostarveResult = Guest.TimeToStarve;
            if (int.TryParse(textBox7.Text, out timetostarveResult))
                    
                succes = succes ? true : false;
            else
            {
                //message bij geval van fout.
                MessageBox.Show("Invoer bij 'Time To Starve' is niet juist!");
                succes = false;
            }
            Guest.TimeToStarve = timetostarveResult;

            return succes;
        }
    }
}
