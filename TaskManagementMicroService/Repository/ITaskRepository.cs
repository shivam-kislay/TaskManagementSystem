using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementMicroService.Models;
using Task = TaskManagementMicroService.Models.Task;

namespace TaskManagementMicroService.Repository
{
    public interface ITaskRepository
    {
        IEnumerable<Task> GetAll();
        Task Get(int id);
        void Add(Task task);
        void Remove(Task task);
    }
}
