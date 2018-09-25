using HotelProject.Objecten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelProject
{
    /// <summary>
    /// Event voor het veranderen van de task.
    /// </summary>
    /// <param name="sender">Degene die het verstuurt</param>
    /// <param name="e">lege parameter</param>
    public delegate void TaskChangedEventHandler(Object sender, EventArgs e);

    /// <summary>
    /// Taak voor een human die deze kan uitvoeren. Speciaal gemaakt om veranderen van de task te herkennen en op basis daarvan de nieuwe task meteen uitvoeren.
    /// </summary>
    public class TaskWithNotifier
    {
        public event TaskChangedEventHandler Changed;
        public EventListener Listener;
        private Delegate _task;
        private object[] _parameters;
        private Human _human;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="human">De persoon die de listener krijgt</param>
        public TaskWithNotifier(Human human)
        {
            _task = null;
            _human = human;
            Listener = new EventListener(this);
        }

        /// <summary>
        /// Als changed niet null is roep dan het changed event aan van eventlistener.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChange(EventArgs e)
        {
            Changed?.Invoke(this, e);
        }

        /// <summary>
        /// Laat de human zijn task uitvoeren.
        /// </summary>
        public void RunTask()
        {
            if (_task != null && _parameters != null)
                _task.DynamicInvoke(_parameters);
            else if (_task != null)
                _task.DynamicInvoke();
        }

        /// <summary>
        /// Zet de taak van een persoon.
        /// </summary>
        /// <param name="newTask">De actie die moet worden uitgevoerd</param>
        public void SetTask(Action newTask)
        {
            if (_human.Evac != EvacProcess.Evacuating && _human.State != State.CheckOut)
            {
                _task = newTask;
                _parameters = null;
                OnChange(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Zet de taak van een persoon.
        /// </summary>
        /// <typeparam name="T">Het type van de paramaters</typeparam>
        /// <param name="newTask">De taak die die moet uitvoeren</param>
        /// <param name="parameters">De parameter van de task</param>
        public void SetTask<T>(Action<T> newTask, T parameter)
        {
            if (_human.Evac != EvacProcess.Evacuating && _human.State != State.CheckOut)
            {
                _task = newTask;
                _parameters = new object[] { parameter };
                OnChange(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Zet de taak van een persoon.
        /// </summary>
        /// <typeparam name="T">Het type van de 1e paramater.</typeparam>
        /// <typeparam name="U">Het type van de 2e paramater.</typeparam>
        /// <param name="newTask">De nieuwe taak die gezet wordt.</param>
        /// <param name="parameter1">Eerste parameter van de task method.</param>
        /// /// <param name="parameter2">Tweede parameter van de task method.</param>
        public void SetTask<T, U>(Action<T, U> newTask, T parameter1, U parameter2)
        {
            if (_human.Evac != EvacProcess.Evacuating && _human.State != State.CheckOut)
            {
                _task = newTask;
                _parameters = new object[] { parameter1, parameter2 };
                OnChange(EventArgs.Empty);
            }
        }
    }

    ///<summary>
    /// Eventlistener voor TaskWithNotifier
    ///</summary>
    public class EventListener
    {
        ///<summary>De taskwithnotifier bij deze listener hoort.</summary>
        private TaskWithNotifier _task;

        /// <summary>
        /// Eventlistener voor task.
        /// </summary>
        /// <param name="task">De taskwithnotifier die de events door krijgt.</param>
        public EventListener(TaskWithNotifier task)
        {
            _task = task;
            _task.Changed += new TaskChangedEventHandler(TaskChanged);
        }

        /// <summary>
        /// Event dat wordt aangeroepen als de task is veranderd.
        /// </summary>
        /// <param name="sender">Dit is waar het vandaan komt.</param>
        /// <param name="e">Lege parameter.</param>
        private void TaskChanged(object sender, EventArgs e)
        {
            _task.RunTask();
        }
    }
}
