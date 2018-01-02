using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProctorApi.DTO
{
    public class RoleDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<UserDto> Users { get; set; }
    }
}