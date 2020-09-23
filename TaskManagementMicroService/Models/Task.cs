using System;
using System.Collections.Generic;

namespace TaskManagementMicroService.Models
{
    public partial class Task
    {
        public Task()
        {
            SubTask = new HashSet<SubTask>();
        }

        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string State { get; set; }

        public virtual ICollection<SubTask> SubTask { get; set; }
    }
}
