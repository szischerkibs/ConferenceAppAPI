using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProctorApi.DTO
{
    public class CheckInInfoDto
    {
        public int Id { get; set; }
        public DateTime ProctorCheckInTime { get; set; }
        public DateTime SessionStartTime { get; set; }
        public DateTime SessionEndTime { get; set; }
        public int Attendees10 { get; set; }
        public int Attendees50 { get; set; }
        public string Notes { get; set; }
        public virtual List<UserDto> ProctorCheckIns { get; set; }
    }
}