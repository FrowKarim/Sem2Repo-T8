using LogicLayer.Models;
using System.Collections.Generic;

namespace LogicLayer.Interfaces
{
    public interface IUserRepository
    {
        User GetUserById(int id);
        User GetUserByUsername(string username);
        User GetUserByEmail(string email);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int userId);



    }
}