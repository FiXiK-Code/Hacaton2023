using MVP.Date.Models;
using System;
using System.Collections.Generic;
namespace MVP.Date.Interfaces
{
    public interface IUser
    {
        public List<User> GetAllUsers { get; }
       
        public void AddToDb(User user);
    }
}
