using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using GoLocal.Models;

namespace GoLocal.Controllers
{
    public class UsersServiceController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ModelFactory _modelFactory = new ModelFactory();

        // GET: api/UsersService
        public IEnumerable<UserModel> GetUserList()
        {
            return db.UserList.ToList().Select(u => _modelFactory.Wrap(u));
        }

        // GET: api/UsersService/5
        [ResponseType(typeof(UserModel))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            User user = await db.UserList.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(_modelFactory.Wrap(user));
        }

        // PUT: api/UsersService/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(int id, UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.ID)
            {
                return BadRequest();
            }

            db.Entry(_modelFactory.Unwrap(user)).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
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

        // POST: api/UsersService
        [ResponseType(typeof(UserModel))]
        public async Task<IHttpActionResult> PostUser(UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserList.Add(_modelFactory.Unwrap(user));
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = user.ID }, user);
        }

        // DELETE: api/UsersService/5
        [ResponseType(typeof(UserModel))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = await db.UserList.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.UserList.Remove(user);
            await db.SaveChangesAsync();

            return Ok(_modelFactory.Wrap(user));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.UserList.Count(e => e.ID == id) > 0;
        }
    }
}