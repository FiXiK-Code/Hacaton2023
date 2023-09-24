using MVP.Date.Models;
using System.Collections.Generic;
namespace MVP.Date.Interfaces
{
    public interface IUser
    {
        public List<User> GetAllUsers { get; }
    }
}
