using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementMicroService.Models
{
    public partial class Task
    {
        public Task()
        {
            SubTask = new HashSet<SubTask>();
        }

        public int TaskId { get; set; }

        [Required(ErrorMessage ="Task Name is Required")]
        [StringLength(30)]
        public string TaskName { get; set; }

        [StringLength(50)]
        public string TaskDescription { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Incorrect date format")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Incorrect date format")]
        public DateTime FinishDate { get; set; }

        // State can have only "Planned", "inProgress", "Completed" values
        public string[] AllowedState = new string[] { "Planned", "inProgress", "Completed" };
        // Set default value to Planned if the User does not set the State value
        public string _State = "Planned";

        [StringLength(10)]
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
       
        public virtual ICollection<SubTask> SubTask { get; set; }
    }
}
