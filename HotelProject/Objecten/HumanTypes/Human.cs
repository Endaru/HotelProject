using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace HotelProject.Objecten
{
    ///<summary>De status die een gast kan heben.</summary>
    public enum State { Idle, Walking, Watching, Eating, Fitnessing, Sleeping, Cleaning, Waiting, CheckOut, Elevatoring }

    ///<summary>Mogelijheden van status van evacuatie.</summary>
    public enum EvacProcess { None, Evacuating, Evacuated }

    public abstract class Human
    {
        ///<summary>Naam van het mens.</summary>
        public string Name { get; set; }

        ///<summary>Afbeelding van het persoon dat weergegeven wordt op het scherm.</summary>
        public Image Graphics { get; set; }

        ///<summary>Wat de persoon op dit moment doet.</summary>
        public State State { get; set; }

        ///<summary>Object voor pathfinding.</summary>
        private Pathfinding _pathfinding = new Pathfinding();

        ///<summary>De kamer waarin de persoon zich op dit moment bevind.</summary>
        public Room Room { get; set; }

        ///<summary>Positie van deze persoon in het hotel.</summary>
        public Point Position { get; set; }

        ///<summary>Is de klant in de kamer</summary>
        public bool IsInRoom = false;

        ///<summary>Het hotel waar de human zich bevind</summary>
        protected Hotel Hotel { get; set; }

        ///<summary>Het path wat de human moet afleggen</summary>
        public List<Room> Path;

        ///<summary>De taak die de human nu aan het uitvoeren is.</summary>
        public TaskWithNotifier CurrentTask;

        ///<summary>Tijdsduur van een activiteit</summary>
        public int TimeToFinishActivity = 0;

        ///<summary>Enum of er moet worden geavacueerd.</summary>
        public EvacProcess Evac = EvacProcess.None;

        ///<summary>Zorgt ervoor dat het lopen op een trap langer duurt dan op de hal.</summary>
        private int timesIterated = 0;

        /// <summary>
        /// Constructor van human.
        /// </summary>
        public Human()
        {
            CurrentTask = new TaskWithNotifier(this);
            Path = new List<Room>();
        }

        /// <summary>
        /// Zoek een soort kamer in het hotel.
        /// </summary>
        /// <typeparam name="T">Soort kamer waar op gezocht moet worden</typeparam>
        /// <param name="hotel">Het hotel waar je in moet zoeken</param>
        /// <returns>Taak die uitgevoerd kan worden.</returns>
        public void FindRoom<T>(Hotel hotel) where T : Room
        {
            //lijst waar de kamers in komen die het hotel heeft
            List<Room> rooms = new List<Room>();

            //zet alle kamers in een lijst.
            foreach (Room item in hotel.Rooms)
            {
                rooms.Add(item);
            }

            //Searches through all the rooms for a specific type.
            var roomQuery =
                (from item in rooms
                 where item != null && item.GetType() == typeof(T)
                 orderby item.Position.Y ascending
                 select item).FirstOrDefault();

            //if its found then go to that room.
            if (roomQuery != null)
                GoToRoom(roomQuery);
        }

        /// <summary>
        /// Deze functie wordt aangeroepen als het hotel wordt gerefreshd. via deze functie wordt er uitgezocht
        /// wat de human nu gaat doen.
        /// </summary>
        public void Action()
        {
            timesIterated++;
            //als movmentspeed gelijk is aan iteratie dan ga je door.
            if(Room.MovementSpeed == timesIterated)
            {
                //zet weer naar null voor de volgende kamer
                timesIterated = 0;
                switch (State)
                {
                    case State.Walking:
                        //als de state walking is en path is niet null
                        if (Path != null)
                            GoToNextRoom();
                        break;
                    case State.Fitnessing:
                        //als deze is ingevuld en status is Fitnessing dan haal je het omlaag
                        if (TimeToFinishActivity > 0 && State == State.Fitnessing)
                            TimeToFinishActivity--;
                        //anders is het afgelopen en ga je terug naar je gehuurde kamer.
                        else
                        {
                            (this as Guest).GoToRoom((this as Guest).RentedRoom);
                        }
                        break;
                    case State.Cleaning:
                        //Als ze nog niet klaar zijn met schoonmaken en status is Cleaning dan gaat het omlaag.
                        if (TimeToFinishActivity > 0 && State == State.Cleaning)
                            TimeToFinishActivity--;
                        //anders is het schoonmaken klaar en gaat de schoonmaker uit de kamer.
                        else
                        {
                            Room.Dirty = false;
                            Room.ToBeCleaned = false;
                            (this as Cleaner).BackupRooms = null;
                            ExitRoom();
                            Hotel.AlertCleaners();
                        }
                        break;
                    case State.Eating:
                        //Als ze nog bezig zijn met eten dan haal je TimeToFinishActivity omlaag
                        if (TimeToFinishActivity > 0 && State == State.Eating)
                        {
                            TimeToFinishActivity--;
                        }
                        //als ze klaar zijn gaan ze terug naar hun gehuurde kamer.
                        else
                        {
                            (this as Guest).GoToRoom((this as Guest).RentedRoom);
                        }
                        break;
                    case State.Waiting:
                        //alleen een gast kan dood gaan tijdens het wachten
                        if (this.GetType() == typeof(Guest))
                        {
                            //Als de starvation hetzelfde is als zijn timeToStarve dan gaan ze dood.
                            Guest guest = (Guest)this;
                            guest.Starvation++;
                            if (guest.Starvation >= Guest.TimeToStarve)
                            {
                                guest.CurrentTask.SetTask(Die);
                                break;
                            }
                        }
                        EnterRoom();
                        break;
                    case State.CheckOut:
                        //check of path niet null is en of EvacProcess None is zodat ze niet uitchecken totdat het evacueren is afgelopen.
                        if ((Path != null) && Evac == EvacProcess.None)
                            GoToNextRoom();
                        break;
                }
            }
        }

        /// <summary>
        /// Verplaats deze persoon naar het doel.
        /// </summary>
        /// <param name="target">De kamer waar de persoon heen moet.</param>
        public void GoToRoom(Room target)
        {
            if (IsInRoom)
                ExitRoom();
            if (State != State.CheckOut)
                State = State.Walking;
            Room[] chosenpath = _pathfinding.FindPath(Room, target);
            Path = chosenpath.ToList();
        }

        /// <summary>
        /// Human loopt een kamer verder in zijn path of gaat naar binnen als zijn path nul is.
        /// </summary>
        public void GoToNextRoom() 
        {
            //als het path is afgelopen
            if (Path.Count == 0)
            {
                //als ze aan het evacuaren zijn zet ze dan op evacuated en notify de observer.
                if (Evac == EvacProcess.Evacuating)
                {
                    Evac = EvacProcess.Evacuated;
                    EvacuationObserver.Notify();
                }
                // als ze niet hoeven te evacueren ga dan door.
                else if(Evac != EvacProcess.Evacuated)
                {
                    //zet ze op idle.
                    State = State.Idle;

                    //kijk of de kamer een lobby is want dan willen ze uitchecken.
                    if (this.GetType() == typeof(Guest) && Room.GetType() == typeof(Lobby))
                        (Room as Lobby).CheckOutGuest((Guest)this, Hotel);
                    else
                    {
                        EnterRoom();
                        //als het een cleaner is komen ze alleen schoonmaken.
                        if (this.GetType() == typeof(Cleaner))
                        {
                            State = State.Cleaning;
                            (this as Cleaner).SetTime();
                        }
                    }
                }
            }
            //als het path nog niet null is ga dan door.
            else if (Path.Count > 0)
            {
                if(State != State.Waiting && this.GetType() == typeof(Guest))
                {
                    (this as Guest).Starvation = 0;
                }
                if (State != State.Elevatoring && State != State.Waiting)
                {
                    Room = Path.First();
                    Path.Remove(Room);
                    Position = Room.Position;
                }

                if (Room is ElevatorShaft && Path.Count > 0 && Path[0] is ElevatorShaft)
                    State = State.Waiting;
            }
        }

        /// <summary>
        /// Maakt de persoon onzichtbaar als die in een kamer is.
        /// </summary>
        public void EnterRoom()
        {
            if (!IsInRoom)
            {
                if (!(Room is Lobby))
                    Room.Enter(this);
            }
        }

        /// <summary>
        /// Laat de persoon weer zien als hij uit de kamer gaat.
        /// </summary>
        public void ExitRoom()
        {
            if (IsInRoom || Room.GetType() == typeof(ElevatorShaft))
                Room.Exit(this);
        }

        /// <summary>
        /// Laat de persoon dood gaan. Deze verdwijnt uit het hotel.
        /// </summary>
        public void Die()
        {
            Guest guest = (Guest)this;
            guest.RentedRoom.isHired = false;
            guest.RentedRoom.RentedGuest = null;
            Hotel.Guests.Remove(guest);

            guest.RentedRoom.SetDirty(1);

            if (guest.Evac == EvacProcess.Evacuating)
                EvacuationObserver.Notify();

            guest = null;

            Hotel.totalDead++;
        }
    }
}
