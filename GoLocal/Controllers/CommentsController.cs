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
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public async Task<ActionResult> Index()
        {
            var commentList = db.CommentList.Include(c => c.Feed).Include(c => c.User);
            if (!User.IsInRole("Admin"))
            {
                commentList = commentList.Where(c => c.Feed.Status == "A");
            }
            return View(await commentList.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.CommentList.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.FeedID = new SelectList(db.FeedList, "ID", "Title");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Content,TimeStamp,UserID,FeedID")] Comment comment)
        {
            User user = db.UserList.Where(u => u.Email.ToLower() == User.Identity.Name.ToLower()).ToList()[0];
            if (user.Status == "I")
            {
                ModelState.AddModelError("", "User is inactive!");
            }
            Feed feed = await db.FeedList.FindAsync(comment.FeedID);
            if(feed == null || feed.Status == "A")
            {
                ModelState.AddModelError("", "Feed is inactive!");
            }
            if (ModelState.IsValid)
            {
                comment.UserID = user.ID;
                comment.Timestamp = DateTime.Now;
                db.CommentList.Add(comment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.FeedID = new SelectList(db.FeedList, "ID", "Title", comment.FeedID);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.CommentList.FindAsync(id);
            if (comment == null || comment.User.Email.ToLower() != User.Identity.Name.ToLower())
            {
                return HttpNotFound();
            }
            ViewBag.FeedID = new SelectList(db.FeedList, "ID", "Title", comment.FeedID);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Content,TimeStamp,UserID,FeedID")] Comment comment)
        {
            Comment storedComment = await db.CommentList.FindAsync(comment.ID);
            if(storedComment == null || storedComment.User.Email.ToLower() != User.Identity.Name.ToLower())
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                storedComment.Content = comment.Content;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.FeedID = new SelectList(db.FeedList, "ID", "Title", comment.FeedID);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.CommentList.FindAsync(id);
            if (comment == null || comment.User.Email.ToLower() != User.Identity.Name.ToLower())
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Comment comment = await db.CommentList.FindAsync(id);
            db.CommentList.Remove(comment);
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
