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
using System.Web.Security;
using GoLocal.Util;

namespace GoLocal.Controllers
{
    public class FeedsServiceController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ModelFactory _modelFactory = new ModelFactory();

        // GET: api/FeedsService
        public IEnumerable<FeedModel> GetFeedList()
        {
            IEnumerable<string> apiKey = Request.Headers.GetValues("apiKey");
            string api = apiKey.ElementAt(0);
            var feedList = db.FeedList.Include(f => f.User);
            string user = api.Split(new char[] { '-' })[1];
            if (!Roles.GetRolesForUser(user).Contains("Admin"))
            {
                feedList = feedList.Where(f => f.Status == "A");
            }
            feedList = feedList.OrderByDescending(f => f.Timestamp).Take(25);
            return feedList.ToList().Select(f => _modelFactory.Wrap(f));
        }

        // GET: api/FeedsService
        public async Task<IEnumerable<FeedModel>> GetFeedList(string latLng)
        {
            IEnumerable<string> apiKey = Request.Headers.GetValues("apiKey");
            string api = apiKey.ElementAt(0);
            var feedList = db.FeedList.Include(f => f.User);
            string user = api.Split(new char[] { '-' })[1];
            if (!Roles.GetRolesForUser(user).Contains("Admin"))
            {
                feedList = feedList.Where(f => f.Status == "A");
            }
            string location = await AppUtil.GetMapData(latLng);
            feedList = feedList.Where(f => f.LocationName == location).OrderByDescending(f => f.Timestamp);
            return feedList.ToList().Select(f => _modelFactory.Wrap(f));
        }

        // GET: api/FeedsService/5
        [ResponseType(typeof(FeedModel))]
        public async Task<IHttpActionResult> GetFeed(int id)
        {
            Feed feed = await db.FeedList.FindAsync(id);
            if (feed == null)
            {
                return NotFound();
            }

            return Ok(_modelFactory.Wrap(feed));
        }

        // PUT: api/FeedsService/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFeed(int id, FeedModel feed)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != feed.ID)
            {
                return BadRequest();
            }

            db.Entry(_modelFactory.Unwrap(feed)).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedExists(id))
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

        // POST: api/FeedsService
        [ResponseType(typeof(FeedModel))]
        public async Task<IHttpActionResult> PostFeed(FeedModel feed)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FeedList.Add(_modelFactory.Unwrap(feed));
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = feed.ID }, feed);
        }

        // DELETE: api/FeedsService/5
        [ResponseType(typeof(FeedModel))]
        public async Task<IHttpActionResult> DeleteFeed(int id)
        {
            Feed feed = await db.FeedList.FindAsync(id);
            if (feed == null)
            {
                return NotFound();
            }

            db.FeedList.Remove(feed);
            await db.SaveChangesAsync();

            return Ok(_modelFactory.Wrap(feed));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FeedExists(int id)
        {
            return db.FeedList.Count(e => e.ID == id) > 0;
        }
    }
}