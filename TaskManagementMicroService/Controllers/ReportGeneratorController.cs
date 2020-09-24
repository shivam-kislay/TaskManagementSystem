using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementMicroService.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManagementMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportGeneratorController : ControllerBase
    {
        TaskManagementDatabaseSystemContext dbContext;
        public ReportGeneratorController()
        {
            dbContext = new TaskManagementDatabaseSystemContext();
        }

        /// <summary>
        /// This method returns a CSV file with list of tasks with status as inProgress
        /// url: api/ReportGenerator/TaskListInCSV
        /// </summary>
        /// <returns></returns>
        [HttpGet("TaskListInCSV")]
        public IActionResult TaskListInCSV()
        {
            var taskList = dbContext.Task.ToList();
            var builder = new StringBuilder();
            builder.AppendLine("Task Id, Task Name, Task Description, Start Date, Finish Date, State");
            if (taskList.Count() != 0)
            {
                var inProgressTask = taskList.Where(x => x.State == "inProgress").ToList();
                if (inProgressTask.Count() != 0)
                {
                    foreach (var t in inProgressTask)
                    {
                        builder.AppendLine($"{t.TaskId}, {t.TaskName},{t.TaskDescription},{t.StartDate}, {t.FinishDate}, {t.State}");
                    }
                    return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "TaskList.csv");
                }
                else
                    return StatusCode(StatusCodes.Status404NotFound);
            }

            return StatusCode(StatusCodes.Status404NotFound);
        }

    }
}
