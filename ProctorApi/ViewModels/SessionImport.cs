using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProctorApi.ViewModels
{
    public class Speaker
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GravatarUrl { get; set; }
    }

    public class SessionImport
    {
        public int Id { get; set; }
        public DateTime SessionTime { get; set; }
        public DateTime SessionStartTime { get; set; }
        public DateTime SessionEndTime { get; set; }
        public object Room { get; set; }
        public List<string> Rooms { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string SessionType { get; set; }
        public List<string> Tags { get; set; }
        public string Category { get; set; }
        public List<Speaker> Speakers { get; set; }
    }
}