using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManagementMicroService.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManagementMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportGeneratorController : ControllerBase
    {
        private readonly string _inProgress = "inProgress";
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger _logger;
        public ReportGeneratorController(ITaskRepository taskRepos, ISubTaskRepository subTaskRepos, ILogger<ReportGeneratorController> logger)
        {
            _taskRepository = taskRepos;
            _logger = logger;
        }

        /// <summary>
        /// This method returns a CSV file with list of tasks with status as inProgress
        /// url: api/ReportGenerator/TaskListInCSV
        /// </summary>
        /// <returns></returns>
        [HttpGet("TaskListInCSV")]
        public IActionResult TaskListInCSV()
        {
            var builder = new StringBuilder();
            try 
            {
                var taskList = _taskRepository.GetAll();

                builder.AppendLine("Task Id, Task Name, Task Description, Start Date, Finish Date, State");
                if (taskList.Count() != 0)
                {
                    var inProgressTask = taskList.Where(x => x.State == _inProgress).ToList();
                    if (inProgressTask.Count() != 0)
                    {
                        foreach (var t in inProgressTask)
                        {
                            builder.AppendLine($"{t.TaskId}, {t.TaskName},{t.TaskDescription},{t.StartDate}, {t.FinishDate}, {t.State}");
                        }
                        return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "TaskList.csv");
                    }
                    else 
                    {
                        _logger.LogInformation("No task with Status in Progress");
                        return StatusCode(StatusCodes.Status404NotFound);
                    }
                        
                }
                _logger.LogWarning("Task Table has no rows");
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }   
        }
    }
}
