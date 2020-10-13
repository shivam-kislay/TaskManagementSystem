using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementDomain.Models;

namespace TaskManagementDomain.Repository
{
    public interface ISubTaskRepository
    {
        IEnumerable<SubTask> GetAll(int taskID);
        SubTask Get(int id);
        void Add(SubTask task);
        void Remove(SubTask task);
        void Save();
    }
}
