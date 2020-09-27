using System;
using TaskManagementMicroService.Controllers;
using Task = TaskManagementMicroService.Models.Task;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TaskManagementMicroService.Models;
using TaskManagementMicroService.PostRequestModel;
using TaskManagementMicroService.Repository;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace TaskManagementMicroServiceTest
{
    public class TaskManagementControllerTests
    {
        private readonly Mock<ITaskRepository> _taskRepository = new Mock<ITaskRepository>();
        private readonly Mock<ISubTaskRepository> _subTaskRepository = new Mock<ISubTaskRepository>();
        private readonly Mock<ILogger<TaskManagementController>> _logger = new Mock<ILogger<TaskManagementController>>();
        private readonly TaskManagementController _taskManagementController;
        private readonly ITestOutputHelper _output;
        public TaskManagementControllerTests(ITestOutputHelper output) 
        {
            _output = output;
            _taskManagementController = new TaskManagementController(_taskRepository.Object, _subTaskRepository.Object, _logger.Object);
        }

        [Fact]
        public void TestTask()
        {
            DateTime dt1 = new DateTime(2020, 09, 23);
            DateTime dt2 = new DateTime(2021, 09, 23);
            

            Task task = new Task()
            {
                TaskName = "Create a Car",
                TaskDescription = "Create a car for iLoy",
                StartDate = dt1,
                FinishDate = dt2
            };
            _taskRepository.Setup(x => x.Add(task));
            IActionResult result = _taskManagementController.PostTask(task);
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void TestSubTask()
        {
            DateTime dt1 = new DateTime(2020, 09, 23);
            DateTime dt2 = new DateTime(2020, 12, 23);
            SubTask subTask = new SubTask()
            {
                TaskName = "Salary Table",
                TaskDescription = "Create the first Salary table",
                StartDate = dt1,
                FinishDate = dt2,
                State = "inProgress",
                TaskId = 2
            };
            _subTaskRepository.Setup(x => x.Add(subTask));
            IActionResult actionResult = _taskManagementController.PostSubTask(subTask);
            _output.WriteLine(actionResult.ToString());
        }

        [Fact]
        public void TestTaskStatusUpdate()
        {
            DateTime dt1 = new DateTime(2020, 09, 23);
            DateTime dt2 = new DateTime(2020, 12, 23);
            StatusUpdate statusParams = new StatusUpdate()
            {
                taskId = 5,
                status = "Completed"
            };
            Task task = new Task()
            {
                TaskId = 5,
                TaskDescription = "Test",
                State = "inProgress",
                StartDate = dt1,
                FinishDate = dt2
            };
            SubTask subTask = new SubTask()
            {
                SubTaskId = 8,
                TaskName = "New car",
                TaskDescription = "Test",
                StartDate = dt1,
                FinishDate = dt2,
                State = "inProgress",
                TaskId = 3
            };
            List<SubTask> subTasks = new List<SubTask>();
            subTasks.Add(subTask);
            _taskRepository.Setup(x => x.Get(statusParams.taskId)).Returns(task);
            _subTaskRepository.Setup(x => x.GetAll(statusParams.taskId)).Returns(subTasks);

            IActionResult actionResult = _taskManagementController.UpdateTaskStatus(statusParams);
            _output.WriteLine(actionResult.ToString());
        }

        [Fact]
        public void TestSubTaskStatusUpdate()
        {
            DateTime dt1 = new DateTime(2020, 09, 23);
            DateTime dt2 = new DateTime(2020, 12, 23);
            List<SubTask> subTasks = new List<SubTask>();
            StatusUpdate statusParams = new StatusUpdate()
            {
                taskId = 1,
                status = "inProgress"
            };
            Task task = new Task()
            {
                TaskId = 5,
                TaskDescription = "Test",
                State = "inProgress",
                StartDate = dt1,
                FinishDate = dt2
            };
            SubTask subTask = new SubTask()
            {
                SubTaskId = 8,
                TaskName = "New car",
                TaskDescription = "Test",
                StartDate = dt1,
                FinishDate = dt2,
                State = "inProgress",
                TaskId = 3
            };
            subTasks.Add(subTask);
            _subTaskRepository.Setup(x => x.Get(statusParams.taskId)).Returns(subTask);
            _taskRepository.Setup(x => x.Get(statusParams.taskId)).Returns(task);
            _subTaskRepository.Setup(x => x.GetAll(statusParams.taskId)).Returns(subTasks);
            IActionResult actionResult = _taskManagementController.UpdateSubTaskStatus(statusParams);
            _output.WriteLine(actionResult.ToString());
        }

        [Fact]
        public void TestTaskDelete()
        {
            DateTime dt1 = new DateTime(2020, 09, 23);
            DateTime dt2 = new DateTime(2020, 12, 23);
            List<SubTask> subTasks = new List<SubTask>();
            StatusUpdate statusParams = new StatusUpdate()
            {
                taskId = 1,
                status = "inProgress"
            };
            Task task = new Task()
            {
                TaskId = 5,
                TaskDescription = "Test",
                State = "inProgress",
                StartDate = dt1,
                FinishDate = dt2
            };
            SubTask subTask = new SubTask()
            {
                SubTaskId = 8,
                TaskName = "New car",
                TaskDescription = "Test",
                StartDate = dt1,
                FinishDate = dt2,
                State = "inProgress",
                TaskId = 3
            };
            subTasks.Add(subTask);
            _taskRepository.Setup(x => x.Get(statusParams.taskId)).Returns(task);
            _subTaskRepository.Setup(x => x.GetAll(statusParams.taskId)).Returns(subTasks);
            IActionResult actionResult = _taskManagementController.DeleteTask(2);
            _output.WriteLine(actionResult.ToString());
        }

        [Fact]
        public void TestSubTaskDelete()
        {
            DateTime dt1 = new DateTime(2020, 09, 23);
            DateTime dt2 = new DateTime(2020, 12, 23);
            List<SubTask> subTasks = new List<SubTask>();
            StatusUpdate statusParams = new StatusUpdate()
            {
                taskId = 1,
                status = "inProgress"
            };
            Task task = new Task()
            {
                TaskId = 5,
                TaskDescription = "Test",
                State = "inProgress",
                StartDate = dt1,
                FinishDate = dt2
            };
            SubTask subTask = new SubTask()
            {
                SubTaskId = 8,
                TaskName = "New car",
                TaskDescription = "Test",
                StartDate = dt1,
                FinishDate = dt2,
                State = "inProgress",
                TaskId = 3
            };
            subTasks.Add(subTask);
            _subTaskRepository.Setup(x => x.Get(statusParams.taskId)).Returns(subTask);
            _taskRepository.Setup(x => x.Get(statusParams.taskId)).Returns(task);
            _subTaskRepository.Setup(x => x.GetAll(statusParams.taskId)).Returns(subTasks);
            IActionResult actionResult = _taskManagementController.DeleteSubTask(1);
            _output.WriteLine(actionResult.ToString());
        }
    }
}
