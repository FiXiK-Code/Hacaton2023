using MVP.Date.Interfaces;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVP.Date.Repository
{
    public class ProjectRep : IProject
    {
        private readonly AppDB _appDB;

        public ProjectRep(AppDB appDB)
        {
            _appDB = appDB;
        }

        public List<Project> GetAllProjects => _appDB.DBProject.ToList();

        public void AddToDb(Project project)
        {
            _appDB.DBProject.Add(project);
            _appDB.SaveChanges();
        }

        public Project GetProj(int id)
        {
            try
            {
                return _appDB.DBProject.FirstOrDefault(p => p.id == id);
            }
            catch (Exception) { return null; }
        }

        public bool RedactToDb(int id,
            DateTime planStartDate,
            DateTime factStartDate,
            DateTime planFinishDate,
            DateTime factFinishDate,
            string name,
            string status,
            string adr,
            string supervisor,
            int planWorkPrice,
            int factWorkPrice,
            int planMaterialPrice,
            int factMaterialPrice,
            string photoPath)
        {
            Project proj = null;
            try
            {
                proj = _appDB.DBProject.FirstOrDefault(p => p.id == id);
            }
            catch (Exception) { return false; }

            proj.planStartDate = proj.planStartDate != planStartDate && planStartDate != new DateTime(1, 1, 1) ? planStartDate : proj.planStartDate;
            proj.factStartDate = proj.factStartDate != factStartDate && factStartDate != new DateTime(1, 1, 1) ? factStartDate : proj.factStartDate;
            proj.planFinishDate = proj.planFinishDate != planFinishDate && planFinishDate != new DateTime(1, 1, 1) ? planFinishDate : proj.planFinishDate;
            proj.factFinishDate = proj.factFinishDate != factFinishDate && factFinishDate != new DateTime(1, 1, 1) ? factFinishDate : proj.factFinishDate;
            proj.name = proj.name != name && name != null ? name : proj.name;
            proj.status = proj.status != status && status != null ? status : proj.status;
            proj.adr = proj.adr != adr && adr != null ? adr : proj.adr;
            proj.supervisor = proj.supervisor != supervisor && supervisor != null ? supervisor : proj.supervisor;
            proj.planWorkPrice = proj.planWorkPrice != planWorkPrice && planWorkPrice != 0 ? planWorkPrice : proj.planWorkPrice;
            proj.factWorkPrice = proj.factWorkPrice != factWorkPrice && factWorkPrice != 0 ? factWorkPrice : proj.factWorkPrice;
            proj.planMaterialPrice = proj.planMaterialPrice != planMaterialPrice && planMaterialPrice != 0 ? planMaterialPrice : proj.planMaterialPrice;
            proj.factMaterialPrice = proj.factMaterialPrice != factMaterialPrice && factMaterialPrice != 0 ? factMaterialPrice : proj.factMaterialPrice;
            proj.photoPath = proj.photoPath != photoPath && photoPath != null ? photoPath : proj.photoPath;


            _appDB.SaveChanges();
            return true;
        }
    }
}
