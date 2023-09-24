using MVP.Date.Interfaces;
using MVP.Date.Models;
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
    }
}
