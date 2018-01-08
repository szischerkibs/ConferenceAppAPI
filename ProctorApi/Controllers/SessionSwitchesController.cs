using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ProctorApi.Models;

namespace ProctorApi.Controllers
{
    public class SessionSwitchesController : ApiController
    {
        private ProctorContext db = new ProctorContext();

        // GET: api/SessionSwitches
        public IQueryable<SessionSwitch> GetSessionSwitch()
        {
            return db.SessionSwitch;
        }

        // GET: api/SessionSwitches/5
        [ResponseType(typeof(SessionSwitch))]
        public IHttpActionResult GetSessionSwitch(int id)
        {
            SessionSwitch sessionSwitch = db.SessionSwitch.Find(id);
            if (sessionSwitch == null)
            {
                return NotFound();
            }

            return Ok(sessionSwitch);
        }

        // PUT: api/SessionSwitches/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSessionSwitch(int id, SessionSwitch sessionSwitch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sessionSwitch.Id)
            {
                return BadRequest();
            }

            db.Entry(sessionSwitch).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionSwitchExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/SessionSwitches
        [ResponseType(typeof(SessionSwitch))]
        public IHttpActionResult PostSessionSwitch(SessionSwitch sessionSwitch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SessionSwitch.Add(sessionSwitch);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sessionSwitch.Id }, sessionSwitch);
        }

        // DELETE: api/SessionSwitches/5
        [ResponseType(typeof(SessionSwitch))]
        public IHttpActionResult DeleteSessionSwitch(int id)
        {
            SessionSwitch sessionSwitch = db.SessionSwitch.Find(id);
            if (sessionSwitch == null)
            {
                return NotFound();
            }

            db.SessionSwitch.Remove(sessionSwitch);
            db.SaveChanges();

            return Ok(sessionSwitch);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SessionSwitchExists(int id)
        {
            return db.SessionSwitch.Count(e => e.Id == id) > 0;
        }
    }
}