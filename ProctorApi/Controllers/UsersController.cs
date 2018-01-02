﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using ProctorApi.Models;
using ProctorApi.Utils;
using ProctorApi.Repositories;
using ProctorApi.DTO;

namespace ProctorApi.Controllers
{
    public class UsersController : ApiController
    {
        private ProctorContext db = new ProctorContext();
        private UserRepository _userRepository;
        private RoleRepository _roleRepository;

        public UsersController()
        {
            _userRepository = new UserRepository();
            _roleRepository = new RoleRepository();
        }

        // GET: api/Users
        public IList<UserDto> GetUsers()
        {
            var users = _userRepository.GetUsers();
            return users;
        }

        // GET: api/Users/5
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult GetUser(string id)
        {
            UserDto user = _userRepository.GetUserById(id);
            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(string id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            var userManager = new ApplicationUserManager(new UserStore<User>(db));
            
            var newUser = userManager.Create(user, "password");
           
            if (newUser.Succeeded)
            {
                userManager.AddToRole(user.Id, "Everyone");
                userManager.AddToRole(user.Id, "Volunteers");
            }

            return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(string id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(string id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}