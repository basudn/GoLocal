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
        public ActionResult Create()
        {
            ViewBag.FeedID = new SelectList(db.FeedList, "ID", "Title");
            ViewBag.UserID = new SelectList(db.UserList, "ID", "Email");
            return View();
        }

        // POST: Votes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,UserID,Type,FeedID,TimeStamp")] Vote vote)
        {
            if (ModelState.IsValid)
            {
                db.VoteList.Add(vote);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.FeedID = new SelectList(db.FeedList, "ID", "Title", vote.FeedID);
            ViewBag.UserID = new SelectList(db.UserList, "ID", "Email", vote.UserID);
            return View(vote);
        }

        // GET: Votes/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.FeedID = new SelectList(db.FeedList, "ID", "Title", vote.FeedID);
            ViewBag.UserID = new SelectList(db.UserList, "ID", "Email", vote.UserID);
            return View(vote);
        }

        // POST: Votes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,UserID,Type,FeedID,TimeStamp")] Vote vote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vote).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.FeedID = new SelectList(db.FeedList, "ID", "Title", vote.FeedID);
            ViewBag.UserID = new SelectList(db.UserList, "ID", "Email", vote.UserID);
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
            if (vote == null)
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
