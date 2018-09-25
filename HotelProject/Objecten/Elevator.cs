using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace HotelProject.Objecten
{
    ///<summary>Status waarin de elevator zich kan verkeren</summary>
    public enum ElevatorStatus { Waiting, Stopped, Closing, Opening, Moveing }

    /// <summary>Kamer om naar een volgend niveau te gaan</summary>
    public class Elevator : IInteractable
    {
        ///<summary>De locatie waar de lift zich op dit moment bevind</summary>
        public ElevatorShaft MyFloor { get; set; }

        ///<summary>Snelheid waarmee de lift van verdieping naar de volgende gaat.</summary>
        public static int Speed { get; set; }

        ///<summary>Graphics van de lift</summary>
        public Image Graphics { get; set; }

        ///<summary>Status van de elevator</summary>
        public ElevatorStatus Status { get; set; } = ElevatorStatus.Stopped;

        ///<summary>De lijst van floors die de lift moet afgaan.</summary>
        private List<ElevatorShaft> _elevatorQueue { get; set; }

        ///<summary>Dictionary met humans die naar een verdieping willen</summary>
        private Dictionary<Human, ElevatorShaft> _costumerList { get; set; }

        ///<summary>Path wat de lift aan het nemen is op dit moment</summary>
        private List<ElevatorShaft> _path { get; set; }

        /// <summary>
        /// Constructor van de elevator.
        /// </summary>
        public Elevator()
        {
            Speed = 1;
            Graphics = Properties.Resources.Elevator;
            Status = ElevatorStatus.Stopped;
            _path = new List<ElevatorShaft>();
            _costumerList = new Dictionary<Human, ElevatorShaft>();
            _elevatorQueue = new List<ElevatorShaft>();
        }

        /// <summary>
        /// Een human vraagt de elevator om naar zijn vloer te komen.
        /// </summary>
        /// <param name="human">De human die de lift aanroept.</param>
        /// <param name="floor">De vloer waar de human naartoe wil.</param>
        public void RequestElevator(Human human, ElevatorShaft floor)
        {
            if (_costumerList.ContainsKey(human))
                _costumerList[human] = floor;
            else
            {
                _costumerList.Add(human, floor);
                _elevatorQueue.Add(floor);
            }
        }

        /// <summary>
        /// Unload zorgt ervoor dat de human uit de lift komt.
        /// </summary>
        /// <param name="human">De persoon die uit de lift komt.</param>
        private void Unload(Human human)
        {
            _costumerList.Remove(human);

            if (human != null)
            {
                human.ExitRoom();
            }
        }

        /// <summary>
        /// Lift gaat naar de volgende vloer en voert verschillende acties uit
        /// </summary>
        /// <param name="sender">Waarvandaan het komt.</param>
        /// <param name="e">Leeg object, maar moet ik hebben anders doet die het niet.</param>
        /// TODO: Check elke vloer of er iemand in wil.
        private void GoToNextFloor()
        {
            //als het path meer is dan nul
            if (_path.Count > 0)
            {
                //en de lift gaat naar boven dan zet je de lift op moveing.
                Status = ElevatorStatus.Moveing;

                MyFloor.Elevator = null;
                MyFloor = _path.First();
                MyFloor.Elevator = this;
                _path.Remove(MyFloor);

                for (int i = 0; i < _costumerList.Count; i++)
                {
                    if (!_costumerList.ElementAt(i).Equals(default(KeyValuePair<Human, ElevatorShaft>)) && _costumerList.ElementAt(i).Key.State == State.Elevatoring)
                    {
                        _costumerList.ElementAt(i).Key.Room = MyFloor;
                        _costumerList.ElementAt(i).Key.Position = MyFloor.Position;
                    }
                    if (!_costumerList.ElementAt(i).Equals(default(KeyValuePair<Human, ElevatorShaft>)) && _costumerList.ElementAt(i).Key.State == State.Waiting && _costumerList.ElementAt(i).Key.Room == MyFloor)
                    {
                        Status = ElevatorStatus.Waiting;
                    }
                }

                if (_path.Count == 0)
                {
                    Status = ElevatorStatus.Opening;
                }
            }
        }

        /// <summary>
        /// De lift neemt het meest effiente pad om al zijn inhabitants naar de correcte vloer te brengen.
        /// </summary>
        /// <param name="target">De kamer waar de klant naar toe moet.</param>
        private ElevatorShaft[] FindPath(ElevatorShaft start, Room target)
        {
            ElevatorShaft[] path = new ElevatorShaft[0];
            bool pathSucces = false;
            ElevatorShaft targetShaft = (ElevatorShaft)target;

            //als de targetshaft bestaat ga dan door.
            if (targetShaft != null)
            {
                Queue<ElevatorShaft> openSet = new Queue<ElevatorShaft>();
                HashSet<ElevatorShaft> closedSet = new HashSet<ElevatorShaft>();

                openSet.Enqueue(start);
                //zolang openSet grote is dan 0 ga dan door.
                while (openSet.Count > 0)
                {
                    ElevatorShaft current = openSet.Dequeue();
                    closedSet.Add(current);

                    //als de current hezelfde is als de target dan ben je klaar
                    if (current == targetShaft)
                    {
                        pathSucces = true;
                        break;
                    }

                    //Ga door elke buur een om te kijken of deze al bestaat in de open en closed set.
                    foreach (Room neighbor in current.Neighbors)
                    {
                        if (neighbor.GetType() == typeof(ElevatorShaft))
                        {
                            if (closedSet.Contains(neighbor))
                                continue;

                            if (!openSet.Contains(neighbor))
                            {
                                neighbor.Parent = current;
                                openSet.Enqueue(neighbor as ElevatorShaft);
                            }
                        }
                    }
                }
            }
            //als het successvol is dan heb je een path.
            if (pathSucces)
                path = RetracePath(start, targetShaft);

            return path;
        }

        /// <summary>
        /// Elevator kijkt wat zijn volgende actie wordt.
        /// </summary>
        /// TODO: Beste pad berekenen.
        public void ElevatorAction()
        {
            if(Status == ElevatorStatus.Opening)
            {
                if (_costumerList.Count > 0)
                {
                    for (int i = 0; i < _costumerList.Count; i++)
                    {
                        if (!_costumerList.ElementAt(i).Equals(default(KeyValuePair<Human, ElevatorShaft>)) && _costumerList.ElementAt(i).Value == MyFloor)
                        {
                            Unload(_costumerList.ElementAt(i).Key);
                            i--;
                        }
                    }
                }
                Status = ElevatorStatus.Stopped;
            }
            else if(Status == ElevatorStatus.Stopped)
            {
                if (_costumerList.Count > 0)
                {
                    //Volgende Costumer is aan de beurt in de lift.
                    ElevatorShaft[] finalPath = FindPath(MyFloor, _costumerList.First().Value);
                    _path = finalPath.ToList();
                }
                else if (_costumerList.Count == 0 && _elevatorQueue.Count != 0)
                {
                    //maak dan een nieuw pad aan.
                    ElevatorShaft[] finalPath = FindPath(MyFloor, _elevatorQueue.First());
                    _elevatorQueue.Remove(_elevatorQueue.First());
                    _path = finalPath.ToList();
                }
                if (_path.Count > 0)
                    Status = ElevatorStatus.Waiting;
            }
            else if(_path.Count > 0)
            {
                GoToNextFloor();
            }
        }

        /// <summary>
        /// Lift volgt pad en zet het om naar een array.
        /// </summary>
        /// <param name="start">Shaft waar je start</param>
        /// <param name="target">Shaft waar je naartoe wil.</param>
        /// <returns></returns>
        private ElevatorShaft[] RetracePath(ElevatorShaft start, ElevatorShaft target)
        {
            List<ElevatorShaft> path = new List<ElevatorShaft>();
            ElevatorShaft current = target;

            while (current != start)
            {
                path.Add(current);
                current = (ElevatorShaft)current.Parent;
            }

            ElevatorShaft[] finalPath = path.ToArray();
            Array.Reverse(finalPath);
            return finalPath;
        }

        /// <summary>
        /// Functie waarmee een human in een IInteractable kan komen
        /// </summary>
        /// <param name="human">Dit is de human die de kamer betreed</param>
        public void Enter(Human human)
        {
            human.State = State.Elevatoring;
            if (human.Path.Count > 0)
            {
                ElevatorShaft target = null;
                for (int i = 0; i < human.Path.Count; i++)
                {
                    if (human.Path[i].GetType() == typeof(ElevatorShaft) && human.Path[i + 1].GetType() != typeof(ElevatorShaft)) 
                        target = human.Path[i] as ElevatorShaft;
                }
                RequestElevator(human, target);
            }
        }

        /// <summary>
        /// Functie waarmee een human eenIInteractable kan uitgaan
        /// </summary>
        /// <param name="human">De persoon die uit de kamer gaat.</param>
        public void Exit(Human human)
        {
            _costumerList.Remove(human);
        }
    }
}