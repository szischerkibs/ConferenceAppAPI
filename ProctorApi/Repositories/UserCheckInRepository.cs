using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProctorApi.Models;

namespace ProctorApi.Repositories
{
    public class UserCheckInRepository : BaseSqlRepository
    {
        private readonly ProctorContext _context;

        public UserCheckInRepository()
        {
            _context = new ProctorContext();
        }

        public void AddUserCheckInToSession(string userId, DateTime checkInTime , int sessionId)
        {
            var session = _context.Sessions.Include("ProctorCheckIns").FirstOrDefault(s => s.Id == sessionId);
            var newUserCheckIn = _context.UserCheckIns.Create();
            newUserCheckIn.CheckInTime = checkInTime;
            newUserCheckIn.SessionId = sessionId;
            newUserCheckIn.UserId = userId;
            session.ProctorCheckIns.Add(newUserCheckIn);
            _context.SaveChanges();
        }

        public void DeleteUserCheckInFromSession(string userId, int sessionId)
        {
            //var session = _context.Sessions.Include("Rooms").FirstOrDefault(s => s.Id == sessionId);
            var userCheckIn = _context.UserCheckIns.FirstOrDefault(r => r.UserId == userId && r.SessionId == sessionId);
            _context.UserCheckIns.Remove(userCheckIn);
            _context.SaveChanges();
        }
    }
}