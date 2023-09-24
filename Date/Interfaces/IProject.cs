using MVP.Date.Models;
using System.Collections.Generic;
namespace MVP.Date.Interfaces
{
    public interface IProject
    {
        public List<Project> GetAllProjects { get; }
    }
}
