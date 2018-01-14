using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProctorApi.DTO;
using ProctorApi.Models;
using ProctorApi.ViewModels;
using ProctorApi.Providers;

namespace ProctorApi.Repositories
{
    public class SessionRepository
    {
        private string _speakerFeed;
        private string _sessionFeed;
        private readonly ProctorContext _context;

        public SessionRepository()
        {
            _speakerFeed = ConfigurationManager.AppSettings["SpeakerFeed"];
            _sessionFeed = ConfigurationManager.AppSettings["SessionFeed"];
            _context = new ProctorContext();
        }

        internal IList<RoomDto> getRooms()
        {
            return _context.Rooms.Select(r => new RoomDto() { Name = r.Name }).Distinct().ToList();
        }

        public void ImportFromFeed()
        {
            ImportSpeakers();
            ImportSessions();            
        }

        internal IList<SessionDto> getSessions()
        {
            List<SessionDto> sessionsDto = new List<SessionDto>();

            var sessions = _context.Sessions
                .Include("Rooms")
                .Include("Assignees")
                .Include("ProctorCheckIns")
                .ToList();

            foreach (var session in sessions)
            {
                SessionDto sessionDto = MapToDto.MapSessionToDto(session);

                sessionsDto.Add(sessionDto);
            }

            return sessionsDto;

        }

        internal object getSessionById(int id)
        {
            var session = _context.Sessions.Include("Rooms")
                .Include("Assignees")
                .Include("Speakers")
                .Include("ProctorCheckIns")
                .FirstOrDefault(s => s.Id == id);            

            SessionDto sessionDto = MapToDto.MapSessionToDto(session);

            return sessionDto;
        }

        internal void AutoAssign()
        {
            _context.Database.ExecuteSqlCommand("AutoAssignUsersToSessions");
        }

        internal List<UserDto> GetSessionsPerUser()
        {
            List<UserDto> usersDto = new List<UserDto>();
            var users = _context.Users.ToList();

            foreach (var user in users)
            {
                usersDto.Add(MapToDto.MapUserToDto(user));
            }

            return usersDto;
        }

        

        internal UserDto GetSessionsForUser(string userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            return MapToDto.MapUserToDto(user);
        }

        private void ImportSpeakers()
        {
            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(_speakerFeed);
                // Now parse with JSON.Net
                List<SpeakerImport> speakerImport = JsonConvert.DeserializeObject<List<SpeakerImport>>(json);

                foreach (SpeakerImport speaker in speakerImport)
                {
                    AddUpdateSpeaker(speaker);
                }
            }
        }

        private void AddUpdateSpeaker(SpeakerImport speaker)
        {
            var spkr = _context.Speakers.FirstOrDefault(s => s.Id == speaker.Id);
            if (spkr == null)
            {
                var newSpeaker = new Models.Speaker();

                newSpeaker.Id = speaker.Id;
                newSpeaker.FirstName = speaker.FirstName;
                newSpeaker.LastName = speaker.LastName;
                newSpeaker.Biography = speaker.Biography;
                newSpeaker.GravatarUrl = speaker.GravatarUrl;
                newSpeaker.TwitterLink = speaker.TwitterLink;
                newSpeaker.GitHubLink = speaker.GitHubLink;
                newSpeaker.LinkedInProfile = speaker.LinkedInProfile;
                newSpeaker.BlogUrl = speaker.BlogUrl;

                _context.Speakers.Add(newSpeaker);
                _context.SaveChanges();
            }
            else
            {
                spkr.FirstName = speaker.FirstName;
                spkr.LastName = speaker.LastName;
                spkr.Biography = speaker.Biography;
                spkr.GravatarUrl = speaker.GravatarUrl;
                spkr.TwitterLink = speaker.TwitterLink;
                spkr.GitHubLink = speaker.GitHubLink;
                spkr.LinkedInProfile = speaker.LinkedInProfile;
                spkr.BlogUrl = speaker.BlogUrl;
                _context.SaveChanges();
            }
        }

        private void ImportSessions()
        {
            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(_sessionFeed);
                // Now parse with JSON.Net
                List<SessionImport> sessionImport = JsonConvert.DeserializeObject<List<SessionImport>>(json);



                foreach (SessionImport session in sessionImport)
                {
                    AddUpdateSession(session);
                }

                var dbSessions = _context.Sessions.ToList();

                var sessionsToBeDeleted =
                dbSessions.Where(x => !sessionImport.Any(s => s.Id == x.FeedSessionId)).ToList();

                foreach(var session in sessionsToBeDeleted)
                {
                    _context.Sessions.Remove(session);
                    _context.SaveChanges();
                }

            }
        }


        public int AddUpdateSession(SessionImport session)
        {
            var newSession = new Session();
            var sn = _context.Sessions
                            .Include("Speakers")
                            .Include("Rooms")
                            .Include("Tags")
                            .FirstOrDefault(s => s.FeedSessionId == session.Id);
            if (sn == null)
            {

                newSession.FeedSessionId = session.Id;
                newSession.SessionTime = null;
                newSession.SessionStartTime = session.SessionStartTime;
                newSession.SessionEndTime = session.SessionEndTime;
                newSession.Rooms = AddRooms(session.Rooms, session.Id);
                newSession.Title = session.Title;
                newSession.Abstract = session.Abstract;
                newSession.Tags = AddTags(session.Tags, session.Id);
                newSession.Category = session.Category;
                newSession.SessionType = session.SessionType;
                newSession.Speakers = AddSpeakers(session.Speakers);
                newSession.VolunteersRequired = 1;
                _context.Sessions.Add(newSession);
            }
            else
            {
                sn.SessionStartTime = session.SessionStartTime;
                sn.SessionEndTime = session.SessionEndTime;
                //sn.Rooms = AddRooms(session.Rooms, sn.Id);
                sn.Title = session.Title;
                sn.Abstract = session.Abstract;
                //sn.Tags = AddTags(session.Tags, sn.Id);
                sn.Category = session.Category;
                sn.SessionType = session.SessionType;
                UpdateSpeakers(session, sn);
                UpdateRooms(session, sn);
                UpdateTags(session, sn);

            }


            _context.SaveChanges();
            if (sn == null)
            {
                return newSession.Id;
            }
            else
            {
                return sn.Id;
            }
        }

        private static void UpdateSpeakers(SessionImport session, Session sn)
        {
            foreach (var speaker in session.Speakers)
            {
                if (sn.Speakers.FirstOrDefault(s => s.Id == speaker.Id) == null)
                {
                    sn.Speakers.Add(new Models.Speaker()
                    {
                        Id = speaker.Id,
                        FirstName = speaker.FirstName,
                        LastName = speaker.LastName,
                        GravatarUrl = speaker.GravatarUrl
                    });
                }
            }

            foreach (var speaker in sn.Speakers)
            {
                if (session.Speakers.FirstOrDefault(s => s.Id == speaker.Id) == null)
                {
                    sn.Speakers.Remove(speaker);
                }
            }
        }

        public List<SessionResult> GetSessionResults()
        {
            List<SessionResult> sessionResults = new List<SessionResult>();
            var sessions = _context.Sessions
                .Include("Rooms")
                .Include("Assignees")
                .Include("ProctorCheckIns")
                .Where(s => s.SessionType != "Static Session")
                .ToList();

            foreach (var session in sessions)
            {
                var sessionResult = new SessionResult();

                sessionResult.Id = session.FeedSessionId ?? 0;
                sessionResult.SessionStartTime = session.SessionStartTime;
                sessionResult.SessionEndTime = session.SessionEndTime;
                sessionResult.Title = session.Title;
                sessionResult.SessionType = session.SessionType;
                sessionResult.ActualSessionStartTime = session.ActualSessionStartTime;
                sessionResult.ActualSessionEndTime = session.ActualSessionEndTime;
                sessionResult.Attendees10 = session.Attendees10;
                sessionResult.Attendees50 = session.Attendees50;
                sessionResult.Notes = session.Notes;

                if (session.ProctorCheckIns.Count > 0)
                {
                    sessionResult.ProctorCheckInTime = session.ProctorCheckIns.OrderBy(c => c.CheckInTime).FirstOrDefault().CheckInTime;
                }

                var rooms = "";
                session.Rooms.OrderBy(r => r.Name).ToList().ForEach(r => rooms += r.Name + ",");
                sessionResult.Rooms = rooms.Substring(0,rooms.Length-1);

                if (session.Assignees.Count > 0) {
                    var assignees = "";
                    session.Assignees.ForEach(a => assignees += a.FirstName + " " + a.LastName + ";");
                    sessionResult.Assignees = assignees.Substring(0, assignees.Length - 1);
                }

                sessionResults.Add(sessionResult);
            }

            return sessionResults;
        }

        private static void UpdateRooms(SessionImport session, Session sn)
        {
            foreach (var room in session.Rooms)
            {
                if (sn.Rooms.FirstOrDefault(r => r.Name == room) == null)
                {
                    sn.Rooms.Add(new Models.Room()
                    {
                        Name = room,
                        SessionId = sn.Id
                    });
                }
            }

            foreach (var room in sn.Rooms)
            {
                if (session.Rooms.FirstOrDefault(r => r == room.Name) == null)
                {
                    sn.Rooms.Remove(room);
                }
            }
        }

        private static void UpdateTags(SessionImport session, Session sn)
        {
            foreach (var tag in session.Tags)
            {
                if (sn.Tags.FirstOrDefault(t => t.Name == tag) == null)
                {
                    sn.Tags.Add(new Models.Tag()
                    {
                        Name = tag,
                        SessionId = sn.Id
                    });
                }
            }

            foreach (var tag in sn.Tags)
            {
                if (session.Tags.FirstOrDefault(r => r == tag.Name) == null)
                {
                    sn.Tags.Remove(tag);
                }
            }
        }

        private List<Models.Speaker> AddSpeakers(List<ViewModels.Speaker> speakers)
        {
            List<Models.Speaker> speakerList = new List<Models.Speaker>();

            if (speakers == null) { return speakerList; }

            foreach (ViewModels.Speaker speaker in speakers)
            {
                var spkr = _context.Speakers.FirstOrDefault(s => s.Id == speaker.Id);
                if (spkr == null)
                {
                    speakerList.Add(new Models.Speaker()
                    {
                        Id = speaker.Id,
                        FirstName = speaker.FirstName,
                        LastName = speaker.LastName,
                        GravatarUrl = speaker.GravatarUrl
                    });
                }
                else
                {
                    speakerList.Add(spkr);
                }
            }

            return speakerList;
        }

        private List<Tag> AddTags(List<string> tags, int sessionId)
        {
            List<Tag> tagsList = new List<Tag>();

            if (tags == null) { return tagsList; }

            foreach (string tag in tags)
            {
                tagsList.Add(new Tag()
                {
                    Name = tag,
                    SessionId = sessionId
                });
            }

            return tagsList;
        }

        private List<Room> AddRooms(List<string> rooms, int sessionId)
        {
            List<Room> roomsList = new List<Room>();

            if (rooms == null) { return roomsList; }

            foreach(string room in rooms)
            {
                roomsList.Add(new Room() {
                    Name = room,
                    SessionId = sessionId
                });
            }

            return roomsList;

        }
    }
}