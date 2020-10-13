using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagementDomain.Models;

namespace TaskManagementDomain.Repository
{
    public class TaskRepository : ITaskRepository
    {
        TaskManagementDatabaseSystemContext dbContext;
        public TaskRepository()
        {
            dbContext = new TaskManagementDatabaseSystemContext();
        }
        public void Add(Models.Task task)
        {
            dbContext.Task.Add(task);
            dbContext.SaveChanges();
        }

        public Models.Task Get(int id)
        {
            return dbContext.Task.Where(x => x.TaskId == id).FirstOrDefault();
        }

        public IEnumerable<Models.Task> GetAll()
        {
            return dbContext.Task.ToList();
        }

        public void Remove(Models.Task task)
        {
            dbContext.Task.Remove(task);
            dbContext.SaveChanges();
        }
        public void Save()
        {
            dbContext.SaveChanges();
        }
    }
}
