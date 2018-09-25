using HotelProject.Objecten;
using System.Linq;

namespace HotelProject
{
    public class EvacuationObserver
    {
        public static int EvacCapacity { get; set; }
        private static int _evacCounter = 0;
        private static Hotel _hotel;

        public EvacuationObserver(Hotel hotel)
        {
            _hotel = hotel;
        }

        public static void Notify()
        {
            _evacCounter = 0;
            //Krijg een aantal van hoeveel mensen er gevacueerd moeten zijn
            EvacCapacity = _hotel.Guests.Count + _hotel.Cleaners.Length;
            foreach (var guest in _hotel.Guests)
            {
                if (guest.Evac == EvacProcess.Evacuated)
                    _evacCounter++;
            }
            foreach (var cleaner in _hotel.Cleaners)
            {
                if (cleaner.Evac == EvacProcess.Evacuated)
                    _evacCounter++;
            }
            //Als dit hetzelfde is dan mag iedereen weer terug in het hotel.
            if (EvacCapacity == _evacCounter)
            {
                _hotel.Devacuate();
            }
        }
    }
}
