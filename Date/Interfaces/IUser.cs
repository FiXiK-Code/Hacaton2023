using MVP.Date.Models;
using System;
using System.Collections.Generic;
namespace MVP.Date.Interfaces
{
    public interface IUser
    {
        public List<User> GetAllUsers { get; }
        public User GetUser(int id);
        public bool RedactToDb(string name,
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
        public void AddToDb(User user);
    }
}
