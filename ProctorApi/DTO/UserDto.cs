using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProctorApi.DTO
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactAddress { get; set; }
        public bool IsActive { get; set; }
        public string Gravatar { get; set; }
        public string CellNumber { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<RoleDto> Roles { get; set; }
        public List<SessionDto> Sessions { get; set; }
    }
}