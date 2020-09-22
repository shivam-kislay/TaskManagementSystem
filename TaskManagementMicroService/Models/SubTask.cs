using System;
using System.Collections.Generic;

namespace TaskManagementMicroService.Models
{
    public partial class SubTask
    {
        public int SubTaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TaskId { get; set; }

        public virtual Task Task { get; set; }
    }
}
