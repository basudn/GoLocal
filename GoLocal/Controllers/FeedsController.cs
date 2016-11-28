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
    public class FeedsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Feeds
        public async Task<ActionResult> Index()
        {
            var feedList = db.FeedList.Include(f => f.User);
            return View(await feedList.ToListAsync());
        }

        // GET: Feeds/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feed feed = await db.FeedList.FindAsync(id);
            if (feed == null)
            {
                return HttpNotFound();
            }
            return View(feed);
        }

        // GET: Feeds/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.UserList, "ID", "Email");
            return View();
        }

        // POST: Feeds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Title,Content,TimeStamp,UserID,LocationName,Lat,Long")] Feed feed)
        {
            if (ModelState.IsValid)
            {
                db.FeedList.Add(feed);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.UserList, "ID", "Email", feed.UserID);
            return View(feed);
        }

        // GET: Feeds/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feed feed = await db.FeedList.FindAsync(id);
            if (feed == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.UserList, "ID", "Email", feed.UserID);
            return View(feed);
        }

        // POST: Feeds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Title,Content,TimeStamp,UserID,LocationName,Lat,Long")] Feed feed)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feed).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.UserList, "ID", "Email", feed.UserID);
            return View(feed);
        }

        // GET: Feeds/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feed feed = await db.FeedList.FindAsync(id);
            if (feed == null)
            {
                return HttpNotFound();
            }
            return View(feed);
        }

        // POST: Feeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Feed feed = await db.FeedList.FindAsync(id);
            db.FeedList.Remove(feed);
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
