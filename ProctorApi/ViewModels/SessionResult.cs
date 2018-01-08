using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProctorApi.ViewModels
{
    public class SessionResult
    {
        public int Id { get; set; }
        public DateTime? SessionStartTime { get; set; }
        public DateTime? SessionEndTime { get; set; }
        public string Rooms { get; set; }
        public string Title { get; set; }
        public string SessionType { get; set; }
        public string Assignees { get; set; }
        public DateTime? ProctorCheckInTime { get; set; }
        public DateTime? ActualSessionStartTime { get; set; }
        public DateTime? ActualSessionEndTime { get; set; }
        public int Attendees10 { get; set; }
        public int Attendees50 { get; set; }
        public string Notes { get; set; }
    }
}