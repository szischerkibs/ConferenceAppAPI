namespace ProctorApi.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using ProctorApi.Models;
    using ProctorApi.Utils;

    internal sealed class Configuration : DbMigrationsConfiguration<ProctorApi.Models.ProctorContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ProctorApi.Models.ProctorContext context)
        {
            //  This method will be called after migrating to the latest version.
            SeedUsers(context);
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }

        private void SeedUsers(ProctorContext db)
        {
            var userManager = new ApplicationUserManager(new UserStore<User>(db));

            var admins = new[]
            {
                new User { UserName = "admin", Email = "testuser@unknown.com", EmailConfirmed = true, IsActive = true },
                new User { UserName = "admin2", Email = "testuser2@unknown.com", EmailConfirmed = true, IsActive = true }
            };

            foreach (var user in admins)
            {
                if (userManager.Users.Any(i => i.UserName == user.UserName))
                    continue;
                userManager.Create(user, "Admin1234!");
            }

        }
    }
}
