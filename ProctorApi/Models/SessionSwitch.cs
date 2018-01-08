using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProctorApi.Models
{
    public class SessionSwitch
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int? ForSessionId { get; set; }
        public string OfferedBy { get; set; }
        public string OfferedTo { get; set; }
        public int Status { get; set; }
        public DateTime? OfferedOn { get; set; }
        public DateTime? StatusChangedOn { get; set; }
    }
}