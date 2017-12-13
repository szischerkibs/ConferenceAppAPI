using ProctorApi.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ProctorApi.Models
{
    

    public class ProctorContext : IdentityDbContext<User>
    {
        public ProctorContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ProctorContext Create()
        {
            return new ProctorContext();
        }
    }
}