using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return GetTaskList();
        }

        // GET api/<TaskManagementController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TaskManagementController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TaskManagementController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TaskManagementController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        /// <summary>
        /// method to return the list of tasks from the task table
        /// </summary>
        private List<Task> GetTaskList()
        {
            return dbContext.Task.ToList();
        }
    }
}
