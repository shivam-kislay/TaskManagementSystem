using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementMicroService.Models;
using Task = TaskManagementMicroService.Models.Task;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManagementMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskManagementController : ControllerBase
    {
        TaskManagementDatabaseSystemContext dbContext;
        public TaskManagementController()
        {
            dbContext = new TaskManagementDatabaseSystemContext();
        }

        /// <summary>
        /// This is the default method that runs first once the solution is run.
        /// It returns the list of Tasks in the Task table
        /// URL : api/<TaskManagementController>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Task> Get()
        {
            return dbContext.Task.ToList();
        }

        /// <summary>
        /// This method returns the task with the input taskId
        /// url: api/TaskManagement/TaskByID/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("TaskByID/{id}")]
        public Task Get(int id)
        {
            return dbContext.Task.Where(x => x.TaskId == id).FirstOrDefault();
        }

        /// <summary>
        /// This method returns the Subtask with the input sub-taskId
        /// url: api/TaskManagement/SubTaskByTaskID/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("SubTaskByTaskID/{id}")]
        public IEnumerable<SubTask> GetSubTaskList(int id)
        {
            return dbContext.SubTask.Where(x => x.TaskId == id).ToList();
        }

        /// <summary>
        /// This is a post method that is used to add a new task to the task table
        /// URL: api/TaskManagement/Task
        /// </summary>
        /// <param name="taskModel"></param>
        [HttpPost("Task")]
        public IActionResult PostTask([FromBody] Task taskModel)
        {

            try
            {
                if (taskModel != null)
                {
                    // when a new task is created there will be no subtask thus set state as Planned
                    taskModel.State = "Planned";
                    dbContext.Task.Add(taskModel);
                    dbContext.SaveChanges();
                    return StatusCode(StatusCodes.Status201Created);
                }
                else
                    return StatusCode(StatusCodes.Status204NoContent);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// This is a post method that is used to add a new sub task to the SubTask table
        /// and based on the state of subtasks under a parent task update the state of the 
        /// parent task
        /// URL: api/TaskManagement/SubTask
        /// </summary>
        /// <param name="subTaskModel"></param>
        [HttpPost("SubTask")]
        public IActionResult PostSubTask([FromBody] SubTask subTaskModel)
        {
            try
            {
                if (subTaskModel != null)
                {
                    dbContext.SubTask.Add(subTaskModel);
                    dbContext.SaveChanges();
                    UpdateTask(subTaskModel.TaskId);
                    dbContext.SaveChanges();
                    return StatusCode(StatusCodes.Status201Created);

                }
                else
                    return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return (StatusCode(StatusCodes.Status500InternalServerError, ex));
            }
        }

        /// <summary>
        /// This a post method to update the status of the task
        /// url: api/TaskManagement/UpdateTaskStatus
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost("UpdateTaskStatus")]
        public IActionResult UpdateTaskStatus([FromBody] int taskId, string status)
        {
            var task = Get(taskId);
            var subTask = GetSubTaskList(taskId);
            if(subTask.Count() == 0)
            {
                task.State = status;
                dbContext.SaveChanges();
                return (StatusCode(StatusCodes.Status202Accepted));
            }
            else
                return (StatusCode(StatusCodes.Status304NotModified));
            
        }

        /// <summary>
        /// This a post method to update the status of the sub task
        /// url: api/TaskManagement/UpdateSubTaskStatus
        /// </summary>
        /// <param name="subTaskId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost("UpdateSubTaskStatus")]
        public IActionResult UpdateSubTaskStatus([FromBody] int subTaskId, string status)
        {
            var subTask = dbContext.SubTask.Where(x => x.SubTaskId == subTaskId).FirstOrDefault();
            if (subTask != null)
            {
                subTask.State = status;
                dbContext.SaveChanges();
                UpdateTask(subTask.TaskId);
                dbContext.SaveChanges();
                return (StatusCode(StatusCodes.Status202Accepted));
            }
            else
                return (StatusCode(StatusCodes.Status304NotModified));

        }

        /// <summary>
        /// Method to delete the task with the given id only when
        /// it has no subtask
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteTask/{id}")]
        public IActionResult DeleteTask(int id)
        {
            var task = Get(id);
            var subTaskList = GetSubTaskList(id);
            if(subTaskList.Count() == 0)
            {
                dbContext.Task.Remove(task);
                dbContext.SaveChanges();
                return (StatusCode(StatusCodes.Status202Accepted));
            }
            
            return (StatusCode(StatusCodes.Status304NotModified));
        }

        /// <summary>
        /// Delete subtask with the given id and update the parent task status
        /// based on the status of the sub tasks
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteSubTask/{id}")]
        public IActionResult DeleteSubTask(int id)
        {
            var subTask = dbContext.SubTask.Where(x => x.SubTaskId == id).FirstOrDefault();
            if(subTask != null)
            {
                dbContext.Remove(subTask);
                dbContext.SaveChanges();
                UpdateTask(subTask.TaskId);
                dbContext.SaveChanges();
                return (StatusCode(StatusCodes.Status202Accepted));
            }
            return (StatusCode(StatusCodes.Status304NotModified));
        }

        /// <summary>
        /// Private method used my the action methods to update the parent task status
        /// </summary>
        /// <param name="taskID"></param>        
        private void UpdateTask(int taskID)
        {
            var task = Get(taskID);
            var SubTaskList = dbContext.SubTask.Where(x => x.TaskId == taskID).ToList();
            bool isAllCompleted = SubTaskList.All(x => x.State == "Completed");
            bool isAnyInProgress = SubTaskList.Any(x => x.State == "inProgress");
            if (isAllCompleted)
                task.State = "Completed";
            else if (isAnyInProgress)
                task.State = "inProgress";
        }
    }
}
