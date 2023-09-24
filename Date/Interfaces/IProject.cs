using MVP.Date.Api;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
namespace MVP.Date.Interfaces
{
    public interface IProject
    {
        public List<Project> GetAllProjects { get; }
        public Project GetProj(int id);
        public bool RedactToDb(int id,
            DateTime planStartDate,
            DateTime factStartDate,
            DateTime planFinishDate,
            DateTime factFinishDate,
            string name,
            string adr,
            string supervisor,
            int planWorkPrice,
            int factWorkPrice,
            int planMaterialPrice,
            int factMaterialPrice);

        public void AddToDb(Project user);
    }
}
