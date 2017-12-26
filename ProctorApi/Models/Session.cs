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
        [Key]
        public int Id { get; set; }
        public DateTime SessionTime { get; set; }
        public DateTime SessionStartTime { get; set; }
        public DateTime SessionEndTime { get; set; }
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
    }
}