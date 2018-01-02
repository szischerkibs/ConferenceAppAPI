using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProctorApi.Models
{
    public class UserCheckIn
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SessionId { get; set; }
        public string UserId { get; set; }
        public DateTime CheckInTime { get; set; }
    }
}