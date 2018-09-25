using HotelProject.Objecten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelProject
{
    public class Pathfinding
    {
        /// <summary>
        /// Vind het snelste pad naar het eindpunt.
        /// </summary>
        /// <param name="start">Het beginpunt.</param>
        /// <param name="target">Het eindpunt.</param>
        /// <returns>Een array met alle kamer waar het pad langs gaat.</returns>
        public Room[] FindPath(Room start, Room target) 
        {
            Room[] path = new Room[0];
            bool pathSucces = false;

            Queue<Room> openSet = new Queue<Room>();
            HashSet<Room> closedSet = new HashSet<Room>();

            openSet.Enqueue(start);
            while (openSet.Count > 0)
            {
                Room current = openSet.Dequeue();
                closedSet.Add(current);

                if (current == target)
                {
                    pathSucces = true;
                    break;
                }

                foreach (Room neighbor in current.Neighbors)
                {
                    //if(neighbor.GetType() != typeof(ElevatorShaft))
                    //{
                    if (closedSet.Contains(neighbor))
                        continue;

                    int newDistance = (current.GetType() == typeof(ElevatorShaft) ? current.MovementSpeed * 2 : current.MovementSpeed) + (neighbor.GetType() == typeof(ElevatorShaft) ? neighbor.MovementSpeed * 2 : neighbor.MovementSpeed);
                    if (/*newDistance < neighbor.Distance || */!openSet.Contains(neighbor))
                    {
                        neighbor.Distance = newDistance;
                        neighbor.Parent = current;

                        //if (!openSet.Contains(neighbor))
                            openSet.Enqueue(neighbor);
                    }
                    //}
                }
            }
            if (pathSucces)
                path = RetracePath(start, target);

            return path;
        }

        /// <summary>
        /// Zet het gevonden pad in elkaar.
        /// </summary>
        /// <param name="start">Het beginpunt.</param>
        /// <param name="target">Het eindpunt.</param>
        /// <returns>Een array met alle kamer waar het pad langs gaat.</returns>
        private Room[] RetracePath(Room start, Room target)
        {
            List<Room> path = new List<Room>();
            Room current = target;

            while (current != start)
            {
                    path.Add(current);
                    current = current.Parent;
                }

            Room[] finalPath = path.ToArray();
            Array.Reverse(finalPath);

            return finalPath;
        }
    }
}
