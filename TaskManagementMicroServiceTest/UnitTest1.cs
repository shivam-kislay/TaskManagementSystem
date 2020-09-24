using System;
using TaskManagementMicroService.Controllers;
using Task = TaskManagementMicroService.Models.Task;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TaskManagementMicroService.Models;
using TaskManagementMicroService.PostRequestModel;

namespace TaskManagementMicroServiceTest
{
    public class UnitTest1
    {
        TaskManagementController TMSController = new TaskManagementController();

        [Fact]
        public void TestTask()
        {
            DateTime dt1 = new DateTime(2020, 09, 23);
            DateTime dt2 = new DateTime(2021, 09, 23);

            Task task = new Task()
            {
                TaskName = "Create a Portfolio",
                TaskDescription = "Create a Employee portfolio to Check for iLoy",
                StartDate = dt1,
                FinishDate = dt2
            };
            IActionResult result = TMSController.PostTask(task);
            Console.WriteLine(result);
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
            IActionResult actionResult = TMSController.PostSubTask(subTask);
            Console.WriteLine(actionResult);
        }

        [Fact]
        public void TestTaskStatusUpdate()
        {
            StatusUpdate statusParams = new StatusUpdate()
            {
                taskId = 3,
                status = "Completed"
            };

            IActionResult actionResult = TMSController.UpdateTaskStatus(statusParams);
            Console.WriteLine(actionResult);
        }

        [Fact]
        public void TestSubTaskStatusUpdate()
        {
            StatusUpdate statusParams = new StatusUpdate()
            {
                taskId = 1,
                status = "inProgress"
            };

            IActionResult actionResult = TMSController.UpdateSubTaskStatus(statusParams);
            Console.WriteLine(actionResult);
        }

        [Fact]
        public void TestTaskDelete()
        {
            IActionResult actionResult = TMSController.DeleteTask(2);
            Console.WriteLine(actionResult);
        }

        [Fact]
        public void TestSubTaskDelete()
        {
            IActionResult actionResult = TMSController.DeleteSubTask(1);
            Console.WriteLine(actionResult);
        }
    }
}
