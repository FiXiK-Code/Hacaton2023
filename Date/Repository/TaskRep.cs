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
            try
            {
                var proj = _appDB.DBProject.FirstOrDefault(p => p.id == task.prijId);
                proj.status = proj.status != "В работе" ? "На паузе" : proj.status;
                _appDB.SaveChanges();
            }
            catch (Exception) { }
           
        }

        public Task GetTask(int id)
        {
            try
            {
                return _appDB.DBTask.FirstOrDefault(p => p.id == id);
            }
            catch (Exception) { return null; }
            
        }

        public void RedactStatus(int id, string stat)
        {
            Task task = null;
            try
            {
                task = _appDB.DBTask.FirstOrDefault(p => p.id == id);
            }
            catch (Exception) { return; }

            Project proj = null;
            try
            {
                proj = _appDB.DBProject.FirstOrDefault(p => p.id == task.prijId);
            }
            catch (Exception) { }

            try
            {
                var parentTask = _appDB.DBTask.FirstOrDefault(p => p.id == task.parentTaskId);
                if (parentTask.status != "Выполнена")
                {
                    return;
                }
            }
            catch (Exception) { }

            if (task.status == "Создана" && stat == "В работе")
            {
                task.factStartDate = DateTime.Now;
            }
            if (task.status == "В работе" && stat == "Выполнена")
            {
                task.factFinishDate = DateTime.Now;
            }

            try
            {
                if (task.status == "На паузе")
                {
                    proj.status = "На паузе";
                    _appDB.SaveChanges();
                }
            }
            catch (Exception) { }

            try
            {
                if (task.status == "Выполнена")
                {
                    var allTasks = _appDB.DBTask.Where(p => p.prijId == task.prijId).ToList();
                    bool redact = true;
                    foreach(var item in allTasks)
                    {
                        if(item.status != "Выполнена")
                        {
                            redact = false;
                            break;
                        }
                    }
                    if (redact)
                    {
                        proj.status = "Завершен";
                        _appDB.SaveChanges();
                    }
                   
                }
            }
            catch (Exception) { }

            try
            {
                if (task.status == "В работе")
                {
                    proj.status = "В работе";
                    _appDB.SaveChanges();
                }
            }
            catch (Exception) { }

            _appDB.SaveChanges();
        }

        public bool RedactToDb(int id,
            string name,
            string status,
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
            int factMaterialPrice)
        {
            Task task = null;
            try
            {
                task = _appDB.DBTask.FirstOrDefault(p => p.id == id);
            }
            catch (Exception) { return false; }

            
            try
            {
                var parentTask = _appDB.DBTask.FirstOrDefault(p => p.id == task.parentTaskId);
                if (parentTask.status != "Выполнена")
                {
                    return false;
                }
            }
            catch (Exception) { }

            try
            {
                bool exit = false;
                var taskMaterials = _appDB.DBMaterial.Where(p => p.taskId == task.id);
                foreach(var mat in taskMaterials)
                {
                    if (mat.status == "Нет в наличии" || mat.status == "В ожидании поставки")
                    {
                        task.status = "На паузе";
                        _appDB.SaveChanges();
                        exit = true;
                        break;
                    }
                }
                if (exit) return false;
              
            }
            catch (Exception) { }


            task.name = task.name != name && name != null ? name : task.name;
            if (task.status == "Создана" && status == "В работе")
            {
                task.factStartDate = DateTime.Now;
            }
            else
            {
                task.factStartDate = task.factStartDate != factStartDate && factStartDate != new DateTime(1, 1, 1) ? factStartDate : task.factStartDate;
            }
            if (task.status == "В работе" && status == "Выполнена")
            {
                task.factFinishDate = DateTime.Now;
            }
            else
            {
                task.factFinishDate = task.factFinishDate != factFinishDate && factFinishDate != new DateTime(1, 1, 1) ? factFinishDate : task.factFinishDate;
            }
            task.status = task.status != status && status != null ? status : task.status;
            task.planStartDate = task.planStartDate != planStartDate && planStartDate != new DateTime(1,1,1) ? planStartDate : task.planStartDate;
            task.planFinishDate = task.planFinishDate != planFinishDate && planFinishDate != new DateTime(1,1,1) ? planFinishDate : task.planFinishDate;
            task.planPayDate = task.planPayDate != planPayDate && planPayDate != new DateTime(1,1,1) ? planPayDate : task.planPayDate;
            task.factPayDate = task.factPayDate != factPayDate && factPayDate != new DateTime(1,1,1) ? factPayDate : task.factPayDate;
            task.planedPrice = task.planedPrice != planedPrice && planedPrice != 0 ? planedPrice : task.planedPrice;
            task.factPrice = task.factPrice != factPrice && factPrice != 0 ? factPrice : task.factPrice;
            task.parentTaskName = task.parentTaskName != parentTaskName && parentTaskName != null ? parentTaskName : task.parentTaskName;
            task.materials = task.materials != materials && materials != null ? materials : task.materials;
            task.necesseMaterials = task.necesseMaterials != necesseMaterials && necesseMaterials != null ? necesseMaterials : task.necesseMaterials;
            task.supervisor = task.supervisor != supervisor && supervisor != null ? supervisor : task.supervisor;
            task.planedMaterialPrice = task.planedMaterialPrice != planedMaterialPrice && planedMaterialPrice != 0 ? planedMaterialPrice : task.planedMaterialPrice;
            task.factMaterialPrice = task.factMaterialPrice != factMaterialPrice && factMaterialPrice != 0 ? factMaterialPrice : task.factMaterialPrice;

            _appDB.SaveChanges();

            Project proj = null;
            try
            {
                proj = _appDB.DBProject.FirstOrDefault(p => p.id == task.prijId);
            }
            catch (Exception) { }

            try
            {
                if (task.status == "На паузе")
                {
                    
                    proj.status = "На паузе";
                    _appDB.SaveChanges();
                }
            }
            catch (Exception) { }

            try
            {
                if (task.status == "Выполнена")
                {
                    var allTasks = _appDB.DBTask.Where(p => p.prijId == task.prijId).ToList();
                    bool redact = true;
                    foreach (var item in allTasks)
                    {
                        if (item.status != "Выполнена")
                        {
                            redact = false;
                            break;
                        }
                    }
                    if (redact)
                    {
                        proj.status = "Завершен";
                        _appDB.SaveChanges();
                    }
                    else
                    {
                        proj.status = "На паузе";
                        _appDB.SaveChanges();
                    }

                }
            }
            catch (Exception) { }

            try
            {
                if (task.status == "В работе")
                {
                    proj.status = "В работе";
                    _appDB.SaveChanges();
                }
            }
            catch (Exception) { }
           

            return true;
        }
    }
}
