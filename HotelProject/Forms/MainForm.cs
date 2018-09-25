using System;
using System.Collections.Generic;
using System.Drawing;
using HotelProject.Objecten;
using System.Windows.Forms;
using System.Diagnostics;

namespace HotelProject
{
    public partial class MainForm : Form
    {
        private Hotel _hotels;
        private Timer _refreshTimer;
        private Stopwatch _HTE;
        private List<Rectangle> _rectangles;
        private InfoScreen _infoscreen;

        /// <summary>
        /// Constructor van de Mainform.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Layout File";
            theDialog.Filter = ".layout |*.layout";
            theDialog.InitialDirectory = @"C:\\Documents";

            if (theDialog.ShowDialog() == DialogResult.OK)
                LayoutReader.FilePath = theDialog.FileName.ToString();

            _rectangles = new List<Rectangle>();
            _hotels = new Hotel();
            _infoscreen = null;
            Start();
        }

        /// <summary>
        /// Maak de Timer aan en initialiseer deze.
        /// </summary>
        private void InitializeTimer()
        {
            _HTE = new Stopwatch();
            _HTE.Start();
            _refreshTimer = new Timer();
            _refreshTimer.Tick += new EventHandler(RefreshTimer_Tick);
            _refreshTimer.Interval = (int)(1000 / HotelEvents.HotelEventManager.HTE_Factor);
            _refreshTimer.Start();
        }

        /// <summary>
        /// Als de timer is verstreken Refresh dan alles en toon het opnieuw.
        /// </summary>
        /// <param name="sender">Waar de opdracht vandaan komt.</param>
        /// <param name="e">Lege parameter.</param>
        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            label2.Text = (Math.Round(_HTE.Elapsed.TotalSeconds * HotelEvents.HotelEventManager.HTE_Factor)).ToString();
            label4.Text = _hotels.totalDead.ToString();

            _hotels.elevator.ElevatorAction();
            if (_hotels.Guests.Count > 0)
            {
                for (int i = 0; i < _hotels.Guests.Count; i++)
                {
                    _hotels.Guests[i].Action();
                }
            }

            for(int i = 0; i < _hotels.Cleaners.Length; i++)
            {
                _hotels.Cleaners[i].Action();
            }
            Bitmap bmp = _hotels.DisplayHotel();
            pbImage.Image = bmp;
        }

        /// <summary>
        /// Voeg kamers toe aan hotel en maak de bitmap aan.
        /// </summary>
        private void Start()
        {
            HotelEvents.HotelEventManager.HTE_Factor = 1;
            _hotels.AddRooms();

            Bitmap bitmap = _hotels.DisplayHotel();
            pbImage.Image = bitmap;
            panel1.AutoScrollPosition = new Point((pbImage.Width - panel1.Width) / 2, (pbImage.Height - panel1.Height) / 2);
            _rectangles = _hotels.GetRectangles();
            InitializeTimer();
        }

        /// <summary>
        /// Zorg dat de simualtie weer doorgaat.
        /// </summary>
        public void ResumeSimulation()
        {
            _refreshTimer.Start();
            //if (HotelEvents.HotelEventManager.Pauzed)
                HotelEvents.HotelEventManager.Pauze();
            _infoscreen = null;
            _HTE.Start();
        }

        /// <summary>
        /// Zorg dat de simulatie op pause kan.
        /// </summary>
        private void PauseSimulation()
        {
            _refreshTimer.Stop();
            HotelEvents.HotelEventManager.Pauze();
            _HTE.Stop();
        }

        /// <summary>
        /// Als er op de ImageContainer wordt geklikt kijk je of het op een locatie is waar de lobbie zich bevindt
        /// zo ja dan open je infoscreen.
        /// </summary>
        /// <param name="sender">Waar het vandaan komt.</param>
        /// <param name="e">Lege parameter.</param>
        private void pbImage_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (Rectangle r in _rectangles)
            {
                if (r.Contains(e.Location))
                {
                    if (_infoscreen == null)
                    {
                        PauseSimulation();
                        _infoscreen = new InfoScreen(this, _hotels);
                        _infoscreen.Show();
                    }
                }
            }
        }
    }
}