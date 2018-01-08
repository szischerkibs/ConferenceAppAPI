using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProctorApi.Models
{
    public class Session
    {
        public Session()
        {
            Assignees = new List<User>();
            Speakers = new List<Speaker>();
            ProctorCheckIns = new List<UserCheckIn>();
        }

        [Key]
        public int Id { get; set; }
        public int? FeedSessionId { get; set; }
        public DateTime? SessionTime { get; set; }
        public DateTime? SessionStartTime { get; set; }
        public DateTime? SessionEndTime { get; set; }
        [ForeignKey("SessionId")]
        public List<Room> Rooms { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string SessionType { get; set; }
        [ForeignKey("SessionId")]
        public List<Tag> Tags { get; set; }
        public string Category { get; set; }
        [ForeignKey("Id")]
        public List<Speaker> Speakers { get; set; }
        public int VolunteersRequired { get; set; }

        public DateTime? ActualSessionStartTime { get; set; }
        public DateTime? ActualSessionEndTime { get; set; }
        public int Attendees10 { get; set; }
        public int Attendees50 { get; set; }
        public string Notes { get; set; }

        [ForeignKey("SessionId")]
        public List<UserCheckIn> ProctorCheckIns { get; set; }
        
        [ForeignKey("Id")]
        public virtual List<User> Assignees { get; set; }
                        
    }
}