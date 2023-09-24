using MVP.Date.Interfaces;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVP.Date.Repository
{
    public class TaskRep : ITask
    {
        private readonly AppDB _appDB;

        public TaskRep(AppDB appDB)
        {
            _appDB = appDB;
        }

        public List<Task> GetAllTasks => _appDB.DBTask.ToList();

        public void AddToDb(Task task)
        {
            _appDB.DBTask.Add(task);
            _appDB.SaveChanges();
        }

        public Task GetTask(int id)
        {
            try
            {
                return _appDB.DBTask.FirstOrDefault(p => p.id == id);
            }
            catch (Exception) { return null; }
            
        }

        public bool RedactToDb(int id, string name, DateTime planStartDate, DateTime factStartDate, DateTime planFinishDate, DateTime factFinishDate, DateTime planPayDate, DateTime factPayDate, int planedPrice, int factPrice, string parentTaskName, string materials, string necesseMaterials, string supervisor, int planedMaterialPrice, int factMaterialPrice)
        {
            throw new NotImplementedException();
        }
    }
}
