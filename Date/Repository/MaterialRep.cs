using MVP.Date.Interfaces;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVP.Date.Repository
{
    public class MaterialRep : IMaterial
    {
        private readonly AppDB _appDB;
        private readonly ITask _task;

        public MaterialRep(AppDB appDB, ITask task)
        {
            _appDB = appDB;
            _task = task;
        }

        public List<Material> GetAllMaterials => _appDB.DBMaterial.ToList();

        public void AddToDb(Material material)
        {
            _appDB.DBMaterial.Add(material);

            if (material.status == "Нет в наличии" || material.status == "В ожидании поставки")
            {
                try
                {
                    _task.RedactStatus(material.taskId, "На паузе");

                }
                catch (Exception) { }
            }

            _appDB.SaveChanges();
        }

        public Material GetMaterial(int id)
        {
            try
            {
                return _appDB.DBMaterial.FirstOrDefault(p => p.id == id);
            }
            catch (Exception) { return null; }
        }

        public bool RedactToDb(int id,
            string name,
            string category,
            string countName,
            int planCount,
            int factCount,
            int planPrice,
            int factPrice,
            string status,
            string taskName,
            string provider,
            DateTime planPayDate,
            DateTime factPayDate)
        {
            

            Material material = null;
            try
            {
                material = _appDB.DBMaterial.FirstOrDefault(p => p.id == id);
            }
            catch (Exception) { return false; }

            if(material.status != status &&( status == "Нет в наличии" || status == "В ожидании поставки"))
            {
                try
                {
                    _task.RedactStatus(material.taskId, "На паузе");

                }
                catch (Exception) { }
            }
            if (material.status != status && status == "В наличии")
            {
                try
                {
                    _task.RedactStatus(material.taskId, "На паузе");

                }
                catch (Exception) { }
            }
            material.status = material.status != status && status != null ? status : material.status;

            material.name = material.name != name && name != null ? name : material.name;
            material.category = material.category != category && category != null ? category : material.category;
            material.countName = material.countName != countName && countName != null ? countName : material.countName;
            material.planCount = material.planCount != planCount && planCount != 0 ? planCount : material.planCount;
            material.factCount = material.factCount != factCount && factCount != 0 ? factCount : material.factCount;
            material.planPrice = material.planPrice != planPrice && planPrice != 0 ? planPrice : material.planPrice;
            material.factPrice = material.factPrice != factPrice && factPrice != 0 ? factPrice : material.factPrice;
            material.taskName = material.taskName != taskName && taskName != null ? taskName : material.taskName;
            material.provider = material.provider != provider && provider != null ? provider : material.provider;
            material.planPayDate = material.planPayDate != planPayDate && planPayDate != new DateTime(1,1,1) ? planPayDate : material.planPayDate;
            material.factPayDate = material.factPayDate != factPayDate && factPayDate != new DateTime(1,1,1) ? factPayDate : material.factPayDate;

            _appDB.SaveChanges();
            return true;
        }
    }
}
