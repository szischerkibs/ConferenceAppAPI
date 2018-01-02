using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProctorApi.DTO;
using ProctorApi.Models;
using ProctorApi.Providers;

namespace ProctorApi.Repositories
{
    public class UserRepository : BaseSqlRepository
    {
        private readonly ProctorContext _context = new ProctorContext();

        public UserDto GetUserById(string Id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == Id);
            return MapToDto.MapUserToDto(user);
        }

        public List<UserDto> GetUsers()
        {
            var usersDto = new List<UserDto>();
            var users = _context.Users.ToList();

            foreach(var user in users)
            {
                usersDto.Add(MapToDto.MapUserToDto(user));
            }

            return usersDto;
        }

    }
}