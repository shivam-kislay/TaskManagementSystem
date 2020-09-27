using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TaskManagementMicroService.Models
{
    public partial class SubTask
    {
        public int SubTaskId { get; set; }

        [Required(ErrorMessage = "Task Must Have a Name")]
        [StringLength(30)]
        public string TaskName { get; set; }

        [StringLength(50)]
        public string TaskDescription { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Incorrect date format")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Incorrect date format")]
        public DateTime FinishDate { get; set; }

        [Required(ErrorMessage = "Must Have a Parent Task ID")]
        public int TaskId { get; set; }

        // State can have only "Planned", "inProgress", "Completed" values
        public string[] AllowedState = new string[] { "Planned", "inProgress", "Completed" };
        // Set default value to Planned if the User does not set the State value
        public string _State = "Planned";

        [Required(ErrorMessage = "Task Must Have a State")]
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
