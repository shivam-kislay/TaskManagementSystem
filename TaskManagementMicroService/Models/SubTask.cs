using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagementMicroService.Models
{
    public partial class SubTask
    {
        public int SubTaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int TaskId { get; set; }

        // State can have only "Planned", "inProgress", "Completed" values
        public string[] AllowedState = new string[] { "Planned", "inProgress", "Completed" };
        // Set default value to Planned if the User does not set the State value
        public string _State = "Planned";

        /// <summary>
        /// State of the task
        /// </summary>
        public string State
        {
            get
            {
                return _State;
            }
            set
            {
                if (!AllowedState.Any(x => x == value))
                    throw new ArgumentException("Not valid state");
                _State = value;
            }
        }

        public virtual Task Task { get; set; }
    }
}
