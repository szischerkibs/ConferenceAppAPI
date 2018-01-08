using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProctorApi.Models;

namespace ProctorApi.DTO
{
    public class SessionDto
    {
        public int Id { get; set; }
        public int? FeedSessionId { get; set; }
        public DateTime? SessionTime { get; set; }
        public DateTime? SessionStartTime { get; set; }
        public DateTime? SessionEndTime { get; set; }
        public List<Room> Rooms { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string SessionType { get; set; }
        public List<Tag> Tags { get; set; }
        public string Category { get; set; }
        public List<SpeakerDto> Speakers { get; set; }
        public int VolunteersRequired { get; set; }
        
        public DateTime? ActualSessionStartTime { get; set; }
        public DateTime? ActualSessionEndTime { get; set; }
        public int Attendees10 { get; set; }
        public int Attendees50 { get; set; }
        public string Notes { get; set; }

        public List<UserCheckIn> ProctorCheckIns { get; set; }
        public List<UserDto> Assignees { get; set; }
    }
}