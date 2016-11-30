using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GoLocal.Models;

namespace GoLocal.Controllers
{
    public class VotesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Votes
        public async Task<ActionResult> Index()
        {
            var voteList = db.VoteList.Include(v => v.Feed).Include(v => v.User);
            if (!User.IsInRole("Admin"))
            {
                voteList = voteList.Where(v => v.User.Email.ToLower() == User.Identity.Name.ToLower());
            }
            return View(await voteList.ToListAsync());
        }

        // GET: Votes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vote vote = await db.VoteList.FindAsync(id);
            if (vote == null)
            {
                return HttpNotFound();
            }
            return View(vote);
        }

        // GET: Votes/Create
        public async Task<ActionResult> Upvote(int? feedId)
        {
            if (feedId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.UserList.Where(u => u.Email.ToLower() == User.Identity.Name.ToLower()).ToList()[0];
            if (user.Status == "I")
            {
                ModelState.AddModelError("", "User is inactive!");
            }
            Feed feed = await db.FeedList.FindAsync(feedId);
            if (feed == null || feed.Status != "A")
            {
                ModelState.AddModelError("", "Feed is inactive!");
            }
            int count = db.VoteList.Where(v => v.Feed.ID == feedId && v.User.Email.ToLower() == User.Identity.Name.ToLower()).Count();
            if (count > 0)
            {
                ModelState.AddModelError("", "User has already voted!");
            }
            if (ModelState.IsValid)
            {
                Vote vote = new Vote();
                vote.FeedID = feed.ID;
                vote.Type = "U";
                vote.Timestamp = DateTime.Now;
                vote.UserID = user.ID;
                db.VoteList.Add(vote);
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Feeds", routeValues: new { id = feed.ID });
        }

        // POST: Votes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        public async Task<ActionResult> Downvote(int? feedId)
        {
            if(feedId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.UserList.Where(u => u.Email.ToLower() == User.Identity.Name.ToLower()).ToList()[0];
            if (user.Status == "I")
            {
                ModelState.AddModelError("", "User is inactive!");
            }
            Feed feed = await db.FeedList.FindAsync(feedId);
            if (feed == null || feed.Status != "A")
            {
                ModelState.AddModelError("", "Feed is inactive!");
            }
            int count = db.VoteList.Where(v => v.Feed.ID == feedId && v.User.Email.ToLower() == User.Identity.Name.ToLower()).Count();
            if(count > 0)
            {
                ModelState.AddModelError("", "User has already voted!");
            }
            int totalCount = db.VoteList.Where(v => v.FeedID == feedId).Count();
            int downVote = db.VoteList.Where(v => v.FeedID == feedId && v.Type == "D").Count();
            if(totalCount <= 10 && downVote >= 6)
            {
                feed.Status = "I";
            }
            if (ModelState.IsValid)
            {
                Vote vote = new Vote();
                vote.FeedID = feed.ID;
                vote.Type = "D";
                vote.Timestamp = DateTime.Now;
                vote.UserID = user.ID;
                db.VoteList.Add(vote);
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Feeds", routeValues: new { id = feed.ID });
        }

        // GET: Votes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vote vote = await db.VoteList.FindAsync(id);
            if (vote == null || vote.User.Email.ToLower() != User.Identity.Name.ToLower())
            {
                return HttpNotFound();
            }
            return View(vote);
        }

        // POST: Votes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Vote vote = await db.VoteList.FindAsync(id);
            if(vote == null || vote.User.Email.ToLower() != User.Identity.Name.ToLower())
            {
                return HttpNotFound();
            }
            db.VoteList.Remove(vote);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
