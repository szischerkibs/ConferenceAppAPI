using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProctorApi.Models;

namespace ProctorApi.Repositories
{
    public class RoomRepository : BaseSqlRepository
    {
        private readonly ProctorContext _context;

        public RoomRepository()
        {
            _context = new ProctorContext();            
        }

        public void AddRoomToSession(string name, int sessionId)
        {
            var session = _context.Sessions.Include("Rooms").FirstOrDefault(s => s.Id == sessionId);
            var newRoom = _context.Rooms.Create();
            newRoom.Name = name;
            newRoom.SessionId = sessionId;
            session.Rooms.Add(newRoom);
            _context.SaveChanges();
        }

        public void DeleteRoomFromSession(string name, int sessionId)
        {
            //var session = _context.Sessions.Include("Rooms").FirstOrDefault(s => s.Id == sessionId);
            var room = _context.Rooms.FirstOrDefault(r => r.Name == name && r.SessionId == sessionId);
            _context.Rooms.Remove(room);
            _context.SaveChanges();
        }
    }
}