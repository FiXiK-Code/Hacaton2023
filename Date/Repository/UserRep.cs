using MVP.Date.Interfaces;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVP.Date.Repository
{
    public class UserRep : IUser
    {
        private readonly AppDB _appDB;

        public UserRep(AppDB appDB)
        {
            _appDB = appDB;
        }

        public List<User> GetAllUsers => _appDB.DBUser.ToList();

        public void AddToDb(User user)
        {
            _appDB.DBUser.Add(user);
            _appDB.SaveChanges();
        }

        public User GetUser(int id)
        {
            try
            {
                return _appDB.DBUser.FirstOrDefault(p => p.id == id);
            }
            catch (Exception) { return null; }
        }

        public bool RedactToDb(string name, DateTime planStartDate, DateTime factStartDate, DateTime planFinishDate, DateTime factFinishDate, DateTime planPayDate, DateTime factPayDate, int planedPrice, int factPrice, string parentTaskName, string materials, string necesseMaterials, string supervisor, int planedMaterialPrice, int factMaterialPrice)
        {
            throw new NotImplementedException();
        }
    }
}
