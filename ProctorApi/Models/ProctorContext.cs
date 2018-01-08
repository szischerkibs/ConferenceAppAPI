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

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<UserCheckIn> UserCheckIns { get; set; }
        public DbSet<ScheduleException> ScheduleExceptions { get; set; }
        public DbSet<SessionSwitch> SessionSwitch { get; set; }


        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            var existingToken = RefreshTokens.Where(r => r.UserId == token.UserId).SingleOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            RefreshTokens.Add(token);

            return await SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshTokenAsync(string token)
        {
            return await RefreshTokens.SingleOrDefaultAsync(i => i.Token == token);
        }

        public async Task<bool> RemoveRefreshToken(string refreshToken)
        {
            var refreshTokenModel = await RefreshTokens.SingleAsync(i => i.Token == refreshToken);

            if (refreshToken != null)
            {
                RefreshTokens.Remove(refreshTokenModel);
                return await SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            RefreshTokens.Remove(refreshToken);
            return await SaveChangesAsync() > 0;
        }
    }
}