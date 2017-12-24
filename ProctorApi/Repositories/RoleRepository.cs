using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProctorApi.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using ProctorApi.Utils;

namespace ProctorApi.Repositories
{
    public class RoleRepository : BaseSqlRepository
    {
        private readonly ProctorContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationUserManager _userManager;

        public RoleRepository()
        {
            _context = new ProctorContext();
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
            _userManager = new ApplicationUserManager(new UserStore<User>(_context));
        }

        public List<IdentityRole> GetRoles()
        {            
            var roles = _roleManager.Roles.ToList();
            return roles;
        }

        public IdentityRole GetRoleById(string id)
        {
            return _roleManager.Roles.FirstOrDefault(r => r.Id == id);            
        }

        public void UpdateRole(IdentityRole role)
        {
            _roleManager.Update(role);
        }

        public void CreateRole(string roleName)
        {
            _roleManager.Create(new IdentityRole() { Name = roleName });
        }

        public void DeleteRole(string id)
        {            
            var role = _context.Roles.FirstOrDefault(r => r.Id == id);
            _roleManager.Delete(role);
        }

        public IEnumerable<User> GetUsersForRole(string id)
        {

            UserRepository _userRepository = new UserRepository();
            var users = _context.Users.Where(user => user.Roles.Any(role => role.RoleId == id)).ToList();
            return users;
        }

        public void AddUserToRole(string userId, string roleId)
        {            
            var role = GetRoleById(roleId);
            _userManager.AddToRole(userId, role.Name);
        }

        public void RemoveUserFromRole(string userId, string roleId)
        {
            var role = GetRoleById(roleId);
            _userManager.RemoveFromRole(userId, role.Name);
        }

    }
}