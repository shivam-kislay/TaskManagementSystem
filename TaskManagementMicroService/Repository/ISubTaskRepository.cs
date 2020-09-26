using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementMicroService.Models;

namespace TaskManagementMicroService.Repository
{
    public interface ISubTaskRepository
    {
        IEnumerable<SubTask> GetAll(int taskID);
        SubTask Get(int id);
        void Add(SubTask task);
        void Remove(SubTask task);
    }
}
