using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManagementMicroService.Models;
using TaskManagementMicroService.PostRequestModel;
using TaskManagementMicroService.Repository;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManagementMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskManagementController : ControllerBase
    {
        private readonly string _inProgress = "inProgress";
        private readonly string _completed = "Completed";
        private readonly string _planned = "Planned";
        private readonly ITaskRepository _taskRepository;
        private readonly ISubTaskRepository _subTaskRepository;
        private readonly ILogger _logger;
        public TaskManagementController(ITaskRepository taskRepos, ISubTaskRepository subTaskRepos, ILogger<TaskManagementController> logger)
        {
            this._taskRepository = taskRepos;
            this._subTaskRepository = subTaskRepos;
            this._logger = logger;
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
            try 
            {
                return _taskRepository.GetAll();
            }
            catch(Exception ex)
            {
                throw new Exception("Error Occured While Fetching Records from Task Table, Please Refer the Stack Trace for More Details", ex);
            }
            
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
            try 
            {
                return _taskRepository.Get(id);
            }
            catch(Exception ex)
            {
                throw new Exception($"Error Occured While Fetching Record ID {id} from Task Table, Please Refer the Stack Trace for More Details", ex); ;
            }
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
                    taskModel.State = _planned;
                    _taskRepository.Add(taskModel);
                    return StatusCode(StatusCodes.Status201Created);
                }
                else 
                {
                    _logger.LogWarning("Empty Object Passed");
                    return StatusCode(StatusCodes.Status204NoContent);
                }
                    

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
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
        public IActionResult UpdateTaskStatus([FromBody] StatusUpdate requestParam)
        {
            try
            {
                var task = _taskRepository.Get(requestParam.taskId);
                var subTask = _subTaskRepository.GetAll(requestParam.taskId);
                if (subTask.Count() == 0)
                {
                    task.State = requestParam.status;
                    _taskRepository.Save();
                    return (StatusCode(StatusCodes.Status202Accepted));
                }
                else 
                {
                    _logger.LogWarning("There are Subtasks Under This Task Thus Task Status Cannot be Updated");
                    return (StatusCode(StatusCodes.Status304NotModified));
                }
            }
            catch (Exception ex)
            {
                return (StatusCode(StatusCodes.Status500InternalServerError, ex));
            }
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
            var task = _taskRepository.Get(id);
            var subTaskList = _subTaskRepository.GetAll(id);
            if (task != null && subTaskList.Count() == 0)
            {
                try
                {
                    _taskRepository.Remove(task);
                    return (StatusCode(StatusCodes.Status202Accepted));
                }
                catch (Exception ex)
                {
                    return (StatusCode(StatusCodes.Status500InternalServerError, ex));
                }

            }

            return (StatusCode(StatusCodes.Status304NotModified));
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
            try 
            {
                return _subTaskRepository.GetAll(id);
            }
            catch(Exception ex)
            {
                throw new Exception("Error Occured While Fetching Records from Sub Task Table, Please Refer the Stack Trace for More Details", ex); ;
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
                    _subTaskRepository.Add(subTaskModel);
                    UpdateTask(subTaskModel.TaskId);
                    return StatusCode(StatusCodes.Status201Created);

                }
                else 
                {
                    _logger.LogWarning("Empty sub task Object passed as a parameter");
                    return StatusCode(StatusCodes.Status204NoContent);
                }
                    
            }
            catch (Exception ex)
            {
                return (StatusCode(StatusCodes.Status500InternalServerError, ex));
            }
        }

        /// <summary>
        /// This a post method to update the status of the sub task
        /// url: api/TaskManagement/UpdateSubTaskStatus
        /// </summary>
        /// <param name="subTaskId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost("UpdateSubTaskStatus")]
        public IActionResult UpdateSubTaskStatus([FromBody] StatusUpdate requestParam)
        {
            try 
            {
                var subTask = _subTaskRepository.Get(requestParam.taskId);
                if (subTask != null)
                {
                    subTask.State = requestParam.status;
                    _subTaskRepository.Save();
                    UpdateTask(subTask.TaskId);
                    return (StatusCode(StatusCodes.Status202Accepted));
                }
                else 
                {
                    _logger.LogWarning($"There is no subtask with ID {requestParam.taskId}");
                    return (StatusCode(StatusCodes.Status304NotModified));
                }
                    
            }
            catch(Exception ex)
            {
                return (StatusCode(StatusCodes.Status500InternalServerError, ex));
            }
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
            var subTask = _subTaskRepository.Get(id);
            if(subTask != null)
            {
                try 
                {
                    _subTaskRepository.Remove(subTask);
                    UpdateTask(subTask.TaskId);
                    return (StatusCode(StatusCodes.Status202Accepted));
                }
                catch(Exception ex)
                {
                    return (StatusCode(StatusCodes.Status500InternalServerError, ex));
                }
            }
            _logger.LogWarning($"There is no sub task with ID {id}");
            return (StatusCode(StatusCodes.Status304NotModified));
        }

        /// <summary>
        /// Private method used my the action methods to update the parent task status
        /// </summary>
        /// <param name="taskID"></param>        
        private void UpdateTask(int taskID)
        {
            try 
            {
                var task = _taskRepository.Get(taskID);
                var SubTaskList = _subTaskRepository.GetAll(taskID);
                if (task != null && SubTaskList.Count() != 0)
                {
                    bool isAllCompleted = SubTaskList.All(x => x.State == _completed);
                    bool isAnyInProgress = SubTaskList.Any(x => x.State == _inProgress);
                    if (isAllCompleted)
                        task.State = _completed;
                    else if (isAnyInProgress)
                        task.State = _inProgress;
                    _taskRepository.Save();
                }
                else
                    _logger.LogWarning("The task list or the subtask under the parent task is empty");
                
            }
            catch(Exception ex)
            {
                throw new Exception("Error Occured While Updating The Task Table", ex);
            }
        }
    }
}
