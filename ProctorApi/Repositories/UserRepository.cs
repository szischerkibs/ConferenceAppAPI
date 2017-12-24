using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProctorApi.Models;

namespace ProctorApi.Repositories
{
    public class UserRepository : BaseSqlRepository
    {
        private readonly ProctorContext _context = new ProctorContext();

        public User GetUserById(string Id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == Id);
            return user;
        }

        public List<User> GetUsers()
        {
            var users = _context.Users.ToList();
            return users;
        }

    }
}