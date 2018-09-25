using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Linq;
using HotelEvents;

namespace HotelProject.Objecten
{
    ///<summary>Hotel waar alles in komt.</summary>
    public class Hotel
    {
        ///<summary>Alle kamers in het hotel</summary>
        public Room[,] Rooms { get; set; }

        ///<summary>Alle gasten in het hotel</summary>
        public List<Guest> Guests { get; set; }

        ///<summary>Alle cleaners in het hotel</summary>
        public Cleaner[] Cleaners { get; set; }

        ///<summary>De bitmap van het hotel</summary>
        private Bitmap _bitmap { get; set; }

        ///<summary>Maximum hoogte van het hotel</summary>
        private int _maxHeight;

        ///<summary>Maximum breedte van het hotel</summary>
        private int _maxWidth;

        ///<summary>Clickable regions op bitmap</summary>
        private List<Rectangle> _rects;

        ///<summary>Of er voor de eerste keer door hotel wordt gegaan.</summary>
        private bool _firsttime;

        ///<summary>DLL met events.</summary>
        private HotelEventHandler _eventHandler;

        ///<summary>De kamers die vies zijn op dit moment in het hotel.</summary>
        ///TODO: list probably broken.
        public Queue<Tuple<Room, int>> _dirtyRooms;

        ///<summary>De observer voor de cleaners</summary>
        private CleaningObserver _cleaningObserver;

        ///<summary>De observer voor de evacuatie</summary>
        private EvacuationObserver _evacuationObserver;

        ///<summary>Totaal aantal mensen die dood zijn gegaan.</summary>
        public int totalDead;

        ///<summary>Elevator die in het hotel is.</summary>
        public Elevator elevator;

        /// <summary>
        /// Constructor van hotel
        /// </summary>
        public Hotel()
        {
            Rooms = new Room[0, 0];
            Guests = new List<Guest>();
            Cleaners = new Cleaner[2];
            _rects = new List<Rectangle>();
            _firsttime = true;
            _dirtyRooms = new Queue<Tuple<Room, int>>();
            _cleaningObserver = new CleaningObserver(this);
            _evacuationObserver = new EvacuationObserver(this);
        }

        /// <summary>
        /// Gaat zoeken of er vieze kamers zijn
        /// </summary>
        /// <param name="sender">waar het event vandaan komt</param>
        /// <param name="e">Argumenten die zijn megegeven aan het event</param>
        public void AddDirtyRoom(Room dirtyRoom, int duration)
        {
            _dirtyRooms.Enqueue(new Tuple<Room, int>(dirtyRoom, duration));
             AlertCleaners();
        }

        /// <summary>
        /// Voeg een gast toe aan het hotel.
        /// </summary>
        /// <param name="name">Naam van de gast</param>
        /// <param name="starcount">Aantal sterren dat de gast minimaal wil voor zijn kamer</param>
        public void AddGuest(string name, int starcount)
        {
            Guest guest = new Guest(name,starcount, this);
            guest.CurrentTask.SetTask(guest.CheckIn);
        }

        /// <summary>
        /// Voeg kamers toe aan het hotel.
        /// </summary>
        public void AddRooms()
        {
            List<Room> loadedRooms = LayoutReader.ReadLayoutFile();
            _maxHeight = LayoutReader.MaxHeight;
            _maxWidth = LayoutReader.MaxWidth;
            //We maken de bitmap even groot als die moet zijn om het hotel weer te kunnen geven.
            _bitmap = new Bitmap(_maxWidth * 35 + 32, _maxHeight * 35 + 32);
            List<Point> ExtraRooms = new List<Point>();
            Rooms = new Room[_maxWidth, _maxHeight];
            elevator = new Elevator();

            //Daarna gaan we door de 2Darray heen
            for (int x = 0; x < Rooms.GetLength(0); x++)
            {
                for(int y = 0; y < Rooms.GetLength(1); y++)
                {
                    //als we op de laagste vloer zitten vul dan een lobby in.
                    if ((y == 0 && x != 0 && x != _maxWidth))
                    {
                        Rooms[x, y] = new Lobby();
                        Rooms[x, y].Position = new Point(x, y);
                    }
                    //als we aan het begin van de array zijn vul dan een liftschacht in.
                    else if (x == 0)
                    {
                        Rooms[x, y] = new ElevatorShaft(ref elevator);
                        Rooms[x, y].Position = new Point(x, y);
                    }
                    else
                    {
                        //Vul elke kamer in die uitgelezen is op de correcte positie.
                        foreach (var room in loadedRooms)
                        {
                            if (x == room.Position.X && y == room.Position.Y)
                            {
                                Rooms[x, y] = room;

                                //Als een kamer zijn dimensie groter is dan 1,1 vul dan de rest op met lege kamers.
                                //Hierdoor kunnen mensen er wel doorheen lopen, maar niet binnengaan.
                                for(int i = 0; i < room.Dimension.X; i++)
                                {
                                    if (i != 0)
                                    {
                                        Rooms[x + i, y] = new Room();
                                        Rooms[x + i, y].Position = new Point(x + i, y);
                                    }

                                    for (int j = 0; j < room.Dimension.Y; j++)
                                    {
                                        if(j != 0)
                                        {
                                            Rooms[x + i, y + j] = new Room();
                                            Rooms[x + i, y + j].Position = new Point(x + i, y + j);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (x == _maxWidth - 1)
                    {
                        Rooms[x, y] = new Stairs();
                        Rooms[x, y].Position = new Point(x, y);
                    }
                }
            }

            (Rooms[0, 0] as ElevatorShaft).Elevator = elevator;
            elevator.MyFloor = Rooms[0, 0] as ElevatorShaft;
            
            //Voeg de cleaners toe aan het hotel.
            Cleaner cleaner1 = new Cleaner(this);
            Cleaner cleaner2 = new Cleaner(this);
            cleaner1.Room = Rooms[1, 0];
            cleaner2.Room = Rooms[1, 0];
            cleaner1.Position = cleaner1.Room.Position;
            cleaner2.Position = cleaner2.Room.Position;
            Cleaners[0] = cleaner1;
            Cleaners[1] = cleaner2;
            

            foreach (Room room in Rooms)
            {
                GetNeighbors(room);
            }
        }

        /// <summary>
        /// Return the create rectangles to the mainform.
        /// </summary>
        public List<Rectangle> GetRectangles()
        {
            return _rects;
        }

        /// <summary>
        /// Haal de buren op 
        /// </summary>
        /// <param name="room">De kamer waar je de buren van gaat ophalen.</param>
        private void GetNeighbors(Room room)
        {
            if (room != null)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (x == 0 && y == 0 || Math.Abs(x) + Math.Abs(y) == 2)
                            continue;
                        if (room.GetType() != typeof(Stairs) && room.GetType() != typeof(ElevatorShaft))
                        {
                            if (y == 1 || y == -1)
                                continue;
                        }

                        int checkX = room.Position.X + x;
                        int checkY = room.Position.Y + y;

                        if (checkX >= 0 && checkX < _maxWidth && checkY >= 0 && checkY < _maxHeight)
                        {
                            if (Rooms[checkX, checkY] != null)
                                room.Neighbors.Add(Rooms[checkX, checkY]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Teken het hotel.
        /// </summary>
        /// <returns>Geeft een bitmap terug waar de tekening opstaat.</returns>
        public Bitmap DisplayHotel()
        {
            int cellSize = 32;
            //Create a pen for the grid with the color black.
            Pen p = new Pen(Color.FromArgb(255, 66, 105, 81));

            using (var graphics = Graphics.FromImage(_bitmap))
            {
                for (int x = 0; x < Rooms.GetLength(0); x++)
                {
                    for (int y = 0; y < Rooms.GetLength(1); y++)
                    {
                        if (Rooms[x, y] != null && Rooms[x, y].GetType() != typeof(Room))
                        {
                            int xas = (Rooms[x, y].Position.X + 1) * cellSize;
                            int yas = _bitmap.Height - (Rooms[x, y].Position.Y + 1) * cellSize - Rooms[x, y].Dimension.Y * cellSize;
                            graphics.DrawImage(Rooms[x, y].Graphics, xas, yas, cellSize * Rooms[x, y].Dimension.X, cellSize * Rooms[x, y].Dimension.Y);

                            // Draw the elevator.
                            if (Rooms[x, y].GetType() == typeof(ElevatorShaft))
                            {
                                if ((Rooms[x,y] as ElevatorShaft).Elevator != null)
                                    graphics.DrawImage((Rooms[x, y] as ElevatorShaft).Elevator.Graphics, xas, yas, cellSize * Rooms[x, y].Dimension.X, cellSize * Rooms[x, y].Dimension.Y);
                            }

                            //make a rectangle so the grid will be closed and not open at the last row
                            Rectangle rect = new Rectangle(xas, yas, cellSize * Rooms[x, y].Dimension.X, cellSize * Rooms[x, y].Dimension.Y);
                            graphics.DrawRectangle(p, rect);

                            if (Rooms[x, y].GetType() == typeof(Lobby) && _firsttime)
                                _rects.Add(new Rectangle(xas, yas,cellSize * Rooms[x, y].Dimension.X,cellSize * Rooms[x, y].Dimension.Y));
                        }
                    }
                }

                Guest guest = null;
                for (int i = 0; i < Guests.Count; i++)
                {
                    guest = Guests[i];
                    if(guest != null && guest.Evac != EvacProcess.Evacuated && guest.State != State.Elevatoring && !guest.IsInRoom)
                    {
                        int xas = (guest.Position.X + 1) * cellSize;
                        int yas = _bitmap.Height - (guest.Position.Y + 2) * cellSize;
                        graphics.DrawImage(guest.Graphics, xas + 8, yas + 12, guest.Graphics.Width, guest.Graphics.Height);
                    }
                }

                if(Cleaners[0] != null && Cleaners[1] != null)
                {
                    foreach (Cleaner cleaner in Cleaners)
                    {
                        if(cleaner.Evac != EvacProcess.Evacuated && cleaner.State != State.Elevatoring && !cleaner.IsInRoom)
                        {
                            int xas = (cleaner.Position.X + 1) * cellSize;
                            int yas = _bitmap.Height - (cleaner.Position.Y + 2) * cellSize;
                            graphics.DrawImage(cleaner.Graphics, xas + 8, yas + 12, cleaner.Graphics.Width, cleaner.Graphics.Height);
                        }
                    }
                }
            }
            if (_firsttime)
            {
                _eventHandler = new HotelEventHandler(this);
            }
            _firsttime = false;
            return _bitmap;
        }

        /// <summary>
        /// Vertel de cleaners dat een kamer vies is.
        /// </summary>
        /// <param name="room">De kamer die schoongemaakt moet worden.</param>
        /// <returns>Taak die wordt teruggegeven.</returns>
        public void AlertCleaners()
        {
            if (Cleaners.Count() != 0)
            {
                for (int i = 0; i < _dirtyRooms.Count; i++)
                {
                    Cleaner alertedCleaner = null;
                    int closestDistance = int.MaxValue;
                    foreach (Cleaner cleaner in Cleaners)
                    {
                        if (cleaner.State == State.Idle)
                        {
                            Pathfinding pathfinding = new Pathfinding();
                            Room[] cleanerPath = pathfinding.FindPath(cleaner.Room, _dirtyRooms.Peek().Item1);

                            if (closestDistance > cleanerPath.Length)
                            {
                                closestDistance = cleanerPath.Length;
                                alertedCleaner = cleaner;
                            }
                        }
                    }

                    if (alertedCleaner != null && alertedCleaner.State == State.Idle && alertedCleaner.Evac == EvacProcess.None)
                    {
                        Tuple<Room, int> current = _dirtyRooms.Dequeue();
                        alertedCleaner.BackupRooms = new Tuple<Room, int>(current.Item1, current.Item2);
                        alertedCleaner.CurrentTask.SetTask(alertedCleaner.Clean, current.Item1, current.Item2);
                    }
                }
            }
        }

        /// <summary>
        /// Functie die ervoor zorgt dat alle gasten gaan evacuaten.
        /// </summary>
        public void Evacuate()
        {
            foreach (Guest guest in Guests)
            {
                if (guest.IsInRoom)
                    guest.ExitRoom();
                guest.CurrentTask.SetTask(guest.FindRoom<Lobby>, this);
                guest.Evac = EvacProcess.Evacuating;
            }

            foreach (Cleaner cleaner in Cleaners)
            {
                if(cleaner.BackupRooms != null)
                {
                    AddDirtyRoom(cleaner.BackupRooms.Item1, cleaner.BackupRooms.Item2);
                }
                cleaner.CurrentTask.SetTask(cleaner.FindRoom<Lobby>, this);
                cleaner.Evac = EvacProcess.Evacuating;
            }
        }

        /// <summary>
        /// Functie die aangeroepen wordt als alle gasten buiten het hotel zijn.
        /// </summary>
        public void Devacuate()
        {
            foreach (Cleaner cleaner in Cleaners)
            {
                cleaner.State = State.Idle;
                cleaner.Evac = EvacProcess.None;
            }
            foreach (Guest guest in Guests)
            {
                guest.State = State.Idle;
                guest.Evac = EvacProcess.None;
                guest.CurrentTask.SetTask(guest.GoToRoom, guest.RentedRoom);
            }
            if (_dirtyRooms.Count > 0)
                AlertCleaners();
        }
    }
}