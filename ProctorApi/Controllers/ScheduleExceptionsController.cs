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
    [Authorize]
    public class ScheduleExceptionsController : ApiController
    {
        private ProctorContext db = new ProctorContext();

        // GET: api/ScheduleExceptions
        public IQueryable<ScheduleException> GetScheduleExceptions()
        {
            return db.ScheduleExceptions;
        }

        // GET: api/ScheduleExceptions/5
        [ResponseType(typeof(ScheduleException))]
        public IHttpActionResult GetScheduleException(int id)
        {
            ScheduleException scheduleException = db.ScheduleExceptions.Find(id);
            if (scheduleException == null)
            {
                return NotFound();
            }

            return Ok(scheduleException);
        }

        // PUT: api/ScheduleExceptions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutScheduleException(int id, ScheduleException scheduleException)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != scheduleException.Id)
            {
                return BadRequest();
            }

            db.Entry(scheduleException).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduleExceptionExists(id))
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

        // POST: api/ScheduleExceptions
        [ResponseType(typeof(ScheduleException))]
        public IHttpActionResult PostScheduleException(ScheduleException scheduleException)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ScheduleExceptions.Add(scheduleException);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = scheduleException.Id }, scheduleException);
        }

        // DELETE: api/ScheduleExceptions/5
        [ResponseType(typeof(ScheduleException))]
        public IHttpActionResult DeleteScheduleException(int id)
        {
            ScheduleException scheduleException = db.ScheduleExceptions.Find(id);
            if (scheduleException == null)
            {
                return NotFound();
            }

            db.ScheduleExceptions.Remove(scheduleException);
            db.SaveChanges();

            return Ok(scheduleException);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ScheduleExceptionExists(int id)
        {
            return db.ScheduleExceptions.Count(e => e.Id == id) > 0;
        }
    }
}