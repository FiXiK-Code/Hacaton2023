using MVP.Date.Models;
using System.Collections.Generic;
namespace MVP.Date.Interfaces
{
    public interface ITask
    {
        public List<Task> GetAllTasks { get; }
    }
}
