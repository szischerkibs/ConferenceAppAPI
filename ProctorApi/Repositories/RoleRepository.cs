using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProctorApi.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using ProctorApi.Utils;
using ProctorApi.DTO;
using ProctorApi.Providers;

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

        public List<RoleDto> GetRoles()
        {
            List<RoleDto> rolesDto = new List<RoleDto>();

            var roles = _roleManager.Roles.ToList();
            foreach(var role in roles)
            {
                rolesDto.Add(MapToDto.MapRolesToDto(role));
            }

            return rolesDto;
        }

        

        public RoleDto GetRoleById(string id)
        {
            var role = _roleManager.Roles.FirstOrDefault(r => r.Id == id);
            return MapToDto.MapRolesToDto(role);
        }

        public RoleDto GetRoleByName(string name)
        {
            var role = _roleManager.Roles.FirstOrDefault(r => r.Name == name);
            return MapToDto.MapRolesToDto(role);
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

        public IEnumerable<UserDto> GetUsersForRole(string id)
        {

            UserRepository _userRepository = new UserRepository();
            var users = _context.Users.Where(user => user.Roles.Any(role => role.RoleId == id)).ToList();
            var usersDto = new List<UserDto>();
            foreach(var user in users)
            {
                usersDto.Add(MapToDto.MapUserToDto(user));
            }

            return usersDto;
        }

        public IEnumerable<UserDto> GetUsersForRoleName(string name)
        {
            var selectedRole = _context.Roles.FirstOrDefault(r => r.Name == name);

            UserRepository _userRepository = new UserRepository();
            var users = _context.Users.Where(user => user.Roles.Any(role => role.RoleId == selectedRole.Id)).ToList();
            var usersDto = new List<UserDto>();
            foreach (var user in users)
            {
                usersDto.Add(MapToDto.MapUserToDto(user));
            }

            return usersDto;
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