using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagementDomain.Models;

namespace TaskManagementDomain.Repository
{
    public interface ITaskRepository
    {
        IEnumerable<Task> GetAll();
        Task Get(int id);
        void Add(Task task);
        void Remove(Task task);
        void Save();
    }
}
