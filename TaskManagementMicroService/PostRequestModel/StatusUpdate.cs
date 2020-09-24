using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementMicroService.PostRequestModel
{
    public class StatusUpdate
    {
        public int taskId { get; set; }
        public string status { get; set; }
       
    }
}
