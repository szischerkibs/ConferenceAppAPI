using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using ProctorApi.DTO;
using ProctorApi.Models;

namespace ProctorApi.Providers
{
    public static class MapToDto
    {
        public static RoleDto MapRolesToDto(IdentityRole role)
        {
            var roleDto = new RoleDto()
            {
                Id = role.Id,
                Name = role.Name,
                Users = new List<UserDto>()
            };

            foreach (var user in role.Users)
            {
            }


            return roleDto;
        }

        public static SessionDto MapSessionToDto(Session session)
        {
            var sessionDto = new SessionDto()
            {
                Id = session.Id,
                SessionStartTime = session.SessionStartTime,
                SessionEndTime = session.SessionEndTime,
                Rooms = session.Rooms,
                Title = session.Title,
                Abstract = session.Abstract,
                SessionType = session.SessionType,
                Tags = session.Tags,
                Category = session.Category,
                Speakers = new List<SpeakerDto>(),
                VolunteersRequired = session.VolunteersRequired,
                Assignees = new List<UserDto>(),
                ActualSessionEndTime = session.ActualSessionEndTime,
                ActualSessionStartTime = session.ActualSessionStartTime,
                Attendees10 = session.Attendees10,
                Attendees50 = session.Attendees50,
                Notes = session.Notes,
                ProctorCheckIns = new List<UserCheckIn>()
                
            };

            foreach (var assignee in session.Assignees)
            {
                sessionDto.Assignees.Add(new UserDto()
                {
                    Id = assignee.Id,
                    FirstName = assignee.FirstName,
                    LastName = assignee.LastName
                });
            }

            foreach (var proctor in session.ProctorCheckIns)
            {
                sessionDto.ProctorCheckIns.Add(new UserCheckIn()
                {
                    Id = proctor.Id,
                    CheckInTime = proctor.CheckInTime,
                    SessionId = proctor.SessionId,
                    UserId = proctor.UserId
                });
            }

            foreach (var speaker in session.Speakers)
            {
                sessionDto.Speakers.Add(new SpeakerDto()
                {
                    Id = speaker.Id,
                    FirstName = speaker.FirstName,
                    LastName = speaker.LastName,
                    Biography = speaker.Biography,
                    BlogUrl = speaker.BlogUrl,
                    GitHubLink = speaker.GitHubLink,
                    GravatarUrl = speaker.GravatarUrl,
                    LinkedInProfile = speaker.LinkedInProfile,
                    TwitterLink = speaker.TwitterLink
                });
            }

            return sessionDto;
        }

        public static UserDto MapUserToDto(User user)
        {
            if (user == null) { return null; }
            var userDto = new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CellNumber = user.CellNumber,
                Gravatar = user.Gravatar,
                UserName = user.UserName,
                Email = user.Email,
                Roles = new List<RoleDto>(),
                Sessions = new List<SessionDto>()
            };

            foreach (var session in user.Sessions)
            {
                userDto.Sessions.Add(new SessionDto()
                {
                    Id = session.Id,
                    SessionStartTime = session.SessionStartTime,
                    SessionEndTime = session.SessionEndTime,
                    Rooms = session.Rooms,
                    Title = session.Title,
                    Abstract = session.Abstract,
                    SessionType = session.SessionType,
                    Tags = session.Tags,
                    Category = session.Category,                    
                    VolunteersRequired = session.VolunteersRequired
                });
            }

            return userDto;
        }
    }
}