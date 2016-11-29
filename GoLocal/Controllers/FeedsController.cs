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
using GoLocal.Util;
using System.Net.Http;
using Newtonsoft.Json;

namespace GoLocal.Controllers
{
    public class FeedsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Feeds
        public async Task<ActionResult> Index(string latLng)
        {
            var feedList = db.FeedList.Include(f => f.User);
            if (!User.IsInRole("Admin"))
            {
                feedList = feedList.Where(f => f.Status == "A");
            }
            if (!string.IsNullOrEmpty(latLng))
            {
                string location = await AppUtil.GetMapData(latLng);
                ViewBag.weather = await AppUtil.GetWeatherData(latLng);
                feedList = feedList.Where(f => f.LocationName == location).OrderByDescending(f => f.Timestamp).Take(10);
            }
            else
            {
                feedList = feedList.OrderByDescending(f => f.Timestamp).Take(10);
            }
            return View(await feedList.ToListAsync());
        }

        // GET: Feeds
        public async Task<ActionResult> Search(string email, string startDate, string endDate, string term, string zipCode, int page = 0)
        {
            var feedList = db.FeedList.Include(f => f.User);
            bool searchCompleted = false;
            if (!string.IsNullOrWhiteSpace(email))
            {
                feedList = feedList.Where(f => f.User.Email.ToLower() == email.ToLower());
                searchCompleted = true;
            }
            if (!string.IsNullOrWhiteSpace(startDate))
            {
                try
                {
                    DateTime date = DateTime.Parse(startDate);
                    feedList = feedList.Where(f => f.Timestamp >= date);
                    searchCompleted = true;
                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, e.Message);
                }
            }
            if (!string.IsNullOrWhiteSpace(endDate))
            {
                try
                {
                    DateTime date = DateTime.Parse(endDate);
                    feedList = feedList.Where(f => f.Timestamp <= date);
                    searchCompleted = true;
                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, e.Message);
                }
            }
            if (!string.IsNullOrWhiteSpace(term))
            {
                feedList = feedList.Where(f => f.Title.ToLower().Contains(term.ToLower()));
                searchCompleted = true;
            }
            if (!string.IsNullOrWhiteSpace(zipCode))
            {
                feedList = feedList.Where(f => f.LocationName.ToLower().Contains(zipCode.ToLower()));
                searchCompleted = true;
            }
            if (searchCompleted)
            {
                int pageSize = 5;
                int count = feedList.Count();
                List<Feed> feeds = await feedList.OrderByDescending(f => f.Timestamp).Skip(pageSize * page).Take(pageSize).ToListAsync();
                ViewBag.SearchParams = string.Join(",",email,startDate,endDate,term,zipCode);
                this.ViewBag.MaxPage = (count / pageSize) - (count % pageSize == 0 ? 1 : 0);
                this.ViewBag.Page = page;
                return View(feeds);
            }
            else
            {
                return View(new List<Feed>());
            }
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
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Feeds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create([Bind(Include = "ID,Title,Content,TimeStamp,UserID,LocationName,Lat,Long")] Feed feed)
        {
            User user = db.UserList.Where(u => u.Email.ToLower() == User.Identity.Name.ToLower()).ToList()[0];
            if (user.Status == "I")
            {
                ModelState.AddModelError("", "User is inactive!");
            }
            if (feed.Lat == 0 && feed.Long == 0)
            {
                ModelState.AddModelError("", "Please allow location access!");
            }
            if (ModelState.IsValid)
            {
                feed.UserID = user.ID;
                feed.Timestamp = DateTime.Now;
                feed.LocationName = await AppUtil.GetMapData(feed.Lat + "," + feed.Long);
                feed.Status = "A";
                db.FeedList.Add(feed);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(feed);
        }

        // GET: Feeds/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feed feed = await db.FeedList.FindAsync(id);
            if (feed == null || feed.User.Email.ToLower() != User.Identity.Name.ToLower())
            {
                return HttpNotFound();
            }
            return View(feed);
        }

        // POST: Feeds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Title,Content,TimeStamp,UserID,LocationName,Lat,Long")] Feed feed)
        {
            Feed storedFeed = await db.FeedList.FindAsync(feed.ID);
            if (storedFeed == null || storedFeed.User.Email.ToLower() != User.Identity.Name.ToLower())
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                storedFeed.Title = feed.Title;
                storedFeed.Content = feed.Content;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(feed);
        }

        // GET: Feeds/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feed feed = await db.FeedList.FindAsync(id);
            if (feed == null || feed.User.Email.ToLower() != User.Identity.Name.ToLower())
            {
                return HttpNotFound();
            }
            return View(feed);
        }

        // POST: Feeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Feed feed = await db.FeedList.FindAsync(id);
            feed.Status = "I";
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
