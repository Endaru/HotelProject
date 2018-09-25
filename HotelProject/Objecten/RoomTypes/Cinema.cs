using System.Drawing;
using System.Timers;

namespace HotelProject.Objecten
{
    ///<summary>Een Bioscoop waar klanten films kunnen kijken.</summary>
    public class Cinema : EventRoom
    {
        ///<summary>Hoelang de film duurt</summary>
        public static int MovieTime { get; set; }

        ///<summary>Of de Bioscoop al gestart is of niet</summary>
        public bool CinemaStarted { get; set; }

        ///<summary>Timer van de bioscoop</summary>
        public Timer CinemaTimer;

        /// <summary>
        /// Constructor van de cinema
        /// </summary>
        /// <param name="id">Id van de cinema</param>
        /// <param name="position">Positie in het hotel</param>
        /// <param name="dimension">De dimensie van de cinema</param>
        public Cinema(int id, Point position, Point dimension)
        {
            ID = id;
            CinemaStarted = false;
            Graphics = Properties.Resources.Cinema;
            Position = position;
            Dimension = dimension;
            MovieTime = 1000;
            MovementSpeed = 1;
        }

        /// <summary>
        /// Als de timer van de bioscoop is afgelopen dan gaan alle mensen eruit
        /// </summary>
        /// <param name="sender">Waar het vandaan komt</param>
        /// <param name="e">Lege parameter.</param>
        private void CinemaTimer_Tick(object sender, ElapsedEventArgs e)
        {
            CinemaTimer.Stop();
            while(Inhabitants.Count > 0)
            {
                Inhabitants[0].CurrentTask.SetTask(Inhabitants[0].GoToRoom, (Inhabitants[0] as Guest).RentedRoom);
            }
            Graphics = Properties.Resources.Cinema;
            CinemaStarted = false;
        }

        /// <summary>
        /// Start de cinema en start ook gelijk de timer.
        /// </summary>
        public void CinemaStart()
        {
            CinemaStarted = true;
            if (Inhabitants.Count != 0)
                Graphics = Properties.Resources.Cinema_lit_started;
            else
                Graphics = Properties.Resources.C_started;

            CinemaTimer = new Timer(MovieTime);
            CinemaTimer.Elapsed += CinemaTimer_Tick;
            CinemaTimer.AutoReset = false;
            CinemaTimer.Enabled = true;
            CinemaTimer.Start();
        }

        /// <summary>
        /// Ga de cinema binnen.
        /// </summary>
        /// <param name="human">De persoon die de cinema binnengaat</param>
        public override void Enter(Human human)
        {
            if (CinemaStarted)
            {
                human.State = State.Waiting;
                return;
            }
            else
            {
                Inhabitants.Add(human);
                human.State = State.Watching;
                human.IsInRoom = true;
            }

            if (Inhabitants.Count > 0 && !CinemaStarted)
                Graphics = Properties.Resources.Cinema_lit;
        }

        /// <summary>
        /// Ga uit de cinema.
        /// </summary>
        /// <param name="human">De persoon die naar buiten gaat.</param>
        public override void Exit(Human human)
        {
            if (human.IsInRoom)
            {
                Inhabitants.Remove(human);
                if (Inhabitants.Count == 0 && !CinemaStarted)
                    Graphics = Properties.Resources.Cinema;
                else if (Inhabitants.Count == 0 && CinemaStarted)
                    Graphics = Properties.Resources.C_started;

                human.IsInRoom = false;
                human.State = State.Idle;
            }
        }
    }
}
