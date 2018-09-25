using HotelProject.Objecten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelProject
{
    public interface IInteractable
    {
        /// <summary>
        /// Functie waarmee een human in een IInteractable kan komen
        /// </summary>
        /// <param name="human">Dit is de human die de kamer betreed</param>
        void Enter(Human human);

        /// <summary>
        /// Functie waarmee een human eenIInteractable kan uitgaan
        /// </summary>
        /// <param name="human">De persoon die uit de kamer gaat.</param>
        void Exit(Human human);
    }
}
