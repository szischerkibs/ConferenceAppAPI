using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProctorApi.DTO;
using ProctorApi.Models;
using ProctorApi.Repositories;

namespace ProctorApi.Controllers
{
    [Authorize]
    public class RoomController : ApiController
    {
        private ProctorContext db = new ProctorContext();
        private SessionRepository _sessionRepository;

        public RoomController()
        {
            _sessionRepository = new SessionRepository();
        }

        // GET: api/Sessions
        public IList<RoomDto> GetSessions()
        {
            return _sessionRepository.getRooms();
        }
    }
}
