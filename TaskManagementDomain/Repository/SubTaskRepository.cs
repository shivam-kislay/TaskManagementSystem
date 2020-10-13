using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementDomain.Models;

namespace TaskManagementDomain.Repository
{
    public class SubTaskRepository : ISubTaskRepository
    {
        TaskManagementDatabaseSystemContext dbContext;
        public SubTaskRepository()
        {
            dbContext = new TaskManagementDatabaseSystemContext();
        }
        public void Add(SubTask task)
        {
            dbContext.SubTask.Add(task);
            dbContext.SaveChanges();
        }

        public SubTask Get(int id)
        {
            return dbContext.SubTask.Where(x => x.SubTaskId == id).FirstOrDefault();
        }

        public IEnumerable<SubTask> GetAll(int taskID)
        {
            return dbContext.SubTask.Where(x => x.TaskId == taskID);
        }

        public void Remove(SubTask task)
        {
            dbContext.SubTask.Remove(task);
            dbContext.SaveChanges();
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }
    }
}
