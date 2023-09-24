using MVP.Date.Api;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
namespace MVP.Date.Interfaces
{
    public interface ITask
    {
        public List<Task> GetAllTasks { get; }
        public Task GetTask(int id);
        public bool RedactToDb(int id,
            string name,
            DateTime planStartDate,
            DateTime factStartDate,
            DateTime planFinishDate,
            DateTime factFinishDate,
            DateTime planPayDate,
            DateTime factPayDate,
            int planedPrice,
            int factPrice,
            string parentTaskName,
            string materials,
            string necesseMaterials,
            string supervisor,
            int planedMaterialPrice,
            int factMaterialPrice);
        public void AddToDb(Task task);
    }
}
