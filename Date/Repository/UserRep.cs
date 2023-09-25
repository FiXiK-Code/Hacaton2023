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
    }
}
