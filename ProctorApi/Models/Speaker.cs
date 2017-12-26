using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProctorApi.Models
{
    public class Speaker
    {
        [Key]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public string GravatarUrl { get; set; }
        public string TwitterLink { get; set; }
        public string GitHubLink { get; set; }
        public string LinkedInProfile { get; set; }
        public string BlogUrl { get; set; }
        [ForeignKey("Id")]
        public List<Session> Sessions { get; set; }
    }
}