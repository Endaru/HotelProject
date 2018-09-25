using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelProject.Objecten;
using HotelEvents;

namespace HotelProject
{
    public class HotelEventHandler : HotelEventListener
    {
        ///<summary>Het hotel waar HotelEventHandler de events naar doorstuurt.</summary>
        private Hotel _hotel;

        /// <summary>
        /// Constructor van HotelEventHandler
        /// </summary>
        /// <param name="hotel">Het hotel waar de events naartoe moeten worden gestuurd</param>
        public HotelEventHandler(Hotel hotel)
        {
            _hotel = hotel;
            HotelEventManager.Register(this);
            HotelEventManager.Start();
        }

        /// <summary>
        /// Deze functie kijkt wat we binnenkrijgen en stuurt dit door naar het correcte onderdeel.
        /// </summary>
        /// <param name="evt">Event wat mee wordt gegeven.</param>
        public void Notify(HotelEvent evt)
        {
            if (evt.Data != null || evt.EventType == HotelEventType.EVACUATE)
            {
                switch (evt.EventType)
                {
                    case HotelEventType.CHECK_IN:
                        EventCheckIn(evt);
                        break;
                    case HotelEventType.CHECK_OUT:
                        EventCheckOut(evt);
                        break;
                    case HotelEventType.CLEANING_EMERGENCY:
                        EventCleaningEmergency(evt);
                        break;
                    case HotelEventType.EVACUATE:
                        EventEvacuate(evt);
                        break;
                    case HotelEventType.GODZILLA:
                        EventGodzilla(evt);
                        break;
                    case HotelEventType.GOTO_CINEMA:
                        EventGoToCinema(evt);
                        break;
                    case HotelEventType.GOTO_FITNESS:
                        EventGoToFitness(evt);
                        break;
                    case HotelEventType.NEED_FOOD:
                        EventNeedFood(evt);
                        break;
                    case HotelEventType.START_CINEMA:
                        EventStartCinema(evt);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Gast checkt via dit event in, in het hotel.
        /// </summary>
        /// <param name="evt">Event wat mee wordt gegeven.</param>
        private void EventCheckIn(HotelEvent evt)
        {
            _hotel.AddGuest(evt.Data.ElementAt(0).Key, Int32.Parse(evt.Data.ElementAt(0).Value.SkipWhile(x => !char.IsDigit(x)).TakeWhile(char.IsDigit).ToArray()[0].ToString()));
        }

        /// <summary>
        /// Gast checkt uit het hotel.
        /// </summary>
        /// <param name="evt">Event wat mee wordt gegeven.</param>
        private void EventCheckOut(HotelEvent evt)
        {
            Guest guest = null;
            foreach (Guest g in _hotel.Guests)
            {
                if (g.Name == evt.Data.ElementAt(0).Key + evt.Data.ElementAt(0).Value)
                {
                    guest = g;
                    break;
                }
            }

            if (guest != null)
            {
                guest.CurrentTask.SetTask(guest.CheckOut);
            }
        }

        /// <summary>
        /// Een cleaning Emergency moet worden uitgevoerd
        /// </summary>
        /// <param name="evt">Event wat mee wordt gegeven.</param>
        private void EventCleaningEmergency(HotelEvent evt)
        {
            Room room = null;
            foreach (Room r in _hotel.Rooms)
            {
                if (r != null && r.ID == Convert.ToInt32(evt.Data.ElementAt(0).Value))
                {
                    room = r;
                    break;
                }
            }

            if (room != null)
                room.SetDirty(Convert.ToInt32(evt.Data.ElementAt(1).Value));
        }

        /// <summary>
        /// Hotel moet worden geavacueerd.
        /// </summary>
        /// <param name="evt">Event wat mee wordt gegeven.</param>
        private void EventEvacuate(HotelEvent evt)
        {
            _hotel.Evacuate();
        }

        /// <summary>
        /// Godjira!!!!!! "screams in japanese"
        /// </summary>
        /// <param name="evt">Event wat mee wordt gegeven.</param>
        private void EventGodzilla(HotelEvent evt)
        {

        }

        /// <summary>
        /// Een gast wil naar de Cinema
        /// </summary>
        /// <param name="evt">Event wat mee wordt gegeven.</param>
        private void EventGoToCinema(HotelEvent evt)
        {
            Guest guest = null;
            foreach (Guest g in _hotel.Guests)
            {
                if (g != null && g.Name == evt.Data.ElementAt(0).Key + evt.Data.ElementAt(0).Value)
                {
                    guest = g;
                    break;
                }
            }

            if (guest != null)
            {
                guest.CurrentTask.SetTask(guest.FindRoom<Cinema>, _hotel);
            }
        }

        /// <summary>
        /// Gast wil naar de fitness
        /// </summary>
        /// <param name="evt">Event wat mee wordt gegeven.</param>
        private void EventGoToFitness(HotelEvent evt)
        {
            Guest guest = null;
            foreach (Guest g in _hotel.Guests)
            {
                if (g.Name == evt.Data.ElementAt(0).Key + evt.Data.ElementAt(0).Value)
                {
                    guest = g;
                    break;
                }
            }

            if (guest != null)
            {
                guest.TimeToFinishActivity = Convert.ToInt32(evt.Data.ElementAt(1).Value);
                guest.CurrentTask.SetTask(guest.FindRoom<Gym>, _hotel);
            }
        }

        /// <summary>
        /// Gast heeft voedsel nodig en wil naar een eetzaal.
        /// </summary>
        /// <param name="evt">Event wat mee wordt gegeven.</param>
        private void EventNeedFood(HotelEvent evt)
        {
            Guest guest = null;
            foreach (Guest g in _hotel.Guests)
            {
                if (g.Name == evt.Data.ElementAt(0).Key + evt.Data.ElementAt(0).Value)
                {
                    guest = g;
                    break;
                }
            }

            if (guest != null)
            {
                guest.CurrentTask.SetTask(guest.FindRoom<DiningRoom>, _hotel);
            }
        }

        /// <summary>
        /// De cinema wordt via dit event gestart.
        /// </summary>
        /// <param name="evt">Event wat mee wordt gegeven.</param>
        private void EventStartCinema(HotelEvent evt)
        {
            foreach (var item in _hotel.Rooms)
            {
                if (item != null && item.ID == Convert.ToInt32(evt.Data.ElementAt(0).Value))
                    (item as Cinema).CinemaStart();
            }
        }
    }
}
