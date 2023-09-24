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

        public bool RedactToDb(int id, DateTime planStartDate, DateTime factStartDate, DateTime planFinishDate, DateTime factFinishDate, string name, string adr, string supervisor, int planWorkPrice, int factWorkPrice, int planMaterialPrice, int factMaterialPrice)
        {
            throw new NotImplementedException();
        }
    }
}
