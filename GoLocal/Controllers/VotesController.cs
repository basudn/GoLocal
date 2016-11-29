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
        public ActionResult Create(int? feedId)
        {
            List<Feed> list;
            if (feedId == null)
            {
                list = db.FeedList.ToList();
            }
            else
            {
                list = db.FeedList.Where(f => f.ID == feedId).ToList();
            }
            ViewBag.FeedID = new SelectList(list, "ID", "Title");
            return View();
        }

        // POST: Votes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,UserID,Type,FeedID,TimeStamp")] Vote vote)
        {
            User user = db.UserList.Where(u => u.Email.ToLower() == User.Identity.Name.ToLower()).ToList()[0];
            if (user.Status == "I")
            {
                ModelState.AddModelError("", "User is inactive!");
            }
            Feed feed = await db.FeedList.FindAsync(vote.FeedID);
            if (feed == null || feed.Status != "A")
            {
                ModelState.AddModelError("", "Feed is inactive!");
            }
            int count = db.VoteList.Where(v => v.Feed.ID == vote.FeedID && v.User.Email.ToLower() == User.Identity.Name.ToLower()).Count();
            if(count > 0)
            {
                ModelState.AddModelError("", "User has already voted!");
            }
            if (ModelState.IsValid)
            {
                vote.Timestamp = DateTime.Now;
                vote.UserID = user.ID;
                db.VoteList.Add(vote);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.FeedID = new SelectList(db.FeedList, "ID", "Title", vote.FeedID);
            return View(vote);
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
