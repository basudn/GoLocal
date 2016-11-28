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
    public class VotesServiceController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ModelFactory _modelFactory = new ModelFactory();

        // GET: api/VotesService
        public IEnumerable<VoteModel> GetVoteList()
        {
            return db.VoteList.ToList().Select(v => _modelFactory.Wrap(v));
        }

        // GET: api/VotesService/5
        [ResponseType(typeof(VoteModel))]
        public async Task<IHttpActionResult> GetVote(int id)
        {
            Vote vote = await db.VoteList.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }

            return Ok(_modelFactory.Wrap(vote));
        }

        // PUT: api/VotesService/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutVote(int id, VoteModel vote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vote.ID)
            {
                return BadRequest();
            }

            db.Entry(_modelFactory.Unwrap(vote)).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoteExists(id))
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

        // POST: api/VotesService
        [ResponseType(typeof(VoteModel))]
        public async Task<IHttpActionResult> PostVote(VoteModel vote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.VoteList.Add(_modelFactory.Unwrap(vote));
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = vote.ID }, vote);
        }

        // DELETE: api/VotesService/5
        [ResponseType(typeof(VoteModel))]
        public async Task<IHttpActionResult> DeleteVote(int id)
        {
            Vote vote = await db.VoteList.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }

            db.VoteList.Remove(vote);
            await db.SaveChangesAsync();

            return Ok(_modelFactory.Wrap(vote));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VoteExists(int id)
        {
            return db.VoteList.Count(e => e.ID == id) > 0;
        }
    }
}