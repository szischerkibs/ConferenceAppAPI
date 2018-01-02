using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using ProctorApi.DTO;
using ProctorApi.Models;
using ProctorApi.Repositories;

namespace ProctorApi.Controllers
{
    public class RoleController : ApiController
    {
        private RoleRepository _roleRepository;

        public RoleController()
        {
            _roleRepository = new RoleRepository();
        }

        // GET api/<controller>
        public IEnumerable<RoleDto> Get()
        {
            return _roleRepository.GetRoles();
        }

        // GET api/<controller>/5
        public RoleDto Get(string id)
        {
            return _roleRepository.GetRoleById(id);
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
            _roleRepository.CreateRole(value);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]IdentityRole value)
        {
            _roleRepository.UpdateRole(value);
        }

        // DELETE api/<controller>/5
        public void Delete(string id)
        {
            _roleRepository.DeleteRole(id);
        }

        // GET api/<controller>/GetUsersForRole/5
        [Route("api/Role/GetUsersForRole")]
        [HttpGet]
        public IEnumerable<UserDto> GetUsersForRole(string id)
        {
            return _roleRepository.GetUsersForRole(id);
        }

        // POST api/<controller>/AddUserToRole
        [Route("api/Role/AddUserToRole")]
        [HttpPost]
        public void AddUserToRole(string userId, string roleId)
        {
            _roleRepository.AddUserToRole(userId, roleId);
        }

        // DELETE api/<controller>/RemoveUserFromRole
        [Route("api/Role/RemoveUserFromRole")]
        [HttpDelete]
        public void RemoveUserFromRole(string userId, string roleId)
        {
            _roleRepository.RemoveUserFromRole(userId, roleId);
        }
    }
}