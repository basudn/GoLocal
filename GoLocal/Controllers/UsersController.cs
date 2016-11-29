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

namespace GoLocal.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Users
        public async Task<ActionResult> Index(int page = 0)
        {
            if (User.IsInRole("Admin"))
            {
                int pageSize = 5; // you can always do something more elegant to set this
                int count = db.UserList.Count();
                List<User> users = await db.UserList.OrderBy(u => u.ID).Skip(page * pageSize).Take(pageSize).ToListAsync();
                this.ViewBag.MaxPage = (count / pageSize) - (count % pageSize == 0 ? 1 : 0);
                this.ViewBag.Page = page;
                return View(users);
            }
            else
            {
                string user = User.Identity.Name;
                List<User> users = await db.UserList.Where(u => u.Email.ToLower() == user.ToLower()).ToListAsync();
                return RedirectToAction("Details", new { id = users[0].ID });
            }
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.UserList.FindAsync(id);
            if (user == null || (!User.IsInRole("Admin") && user.Email.ToLower() != User.Identity.Name.ToLower()))
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public async Task<ActionResult> Create()
        {
            if (User.IsInRole("Admin"))
            {
                return View();
            }
            else
            {
                string user = User.Identity.Name;
                List<User> users = await db.UserList.Where(u => u.Email.ToLower() == user.ToLower()).ToListAsync();
                return RedirectToAction("Details", new { id = users[0].ID });
            }
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "ID,Email,Name,DateOfBirth,PhoneNumber,Status")] User user)
        {
            if (ModelState.IsValid)
            {
                user.RegDate = DateTime.Now;
                user.Status = "A";
                db.UserList.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } 
            User user = await db.UserList.FindAsync(id);
            if (user == null || (!User.IsInRole("Admin") && user.Email.ToLower() != User.Identity.Name.ToLower()))
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult EditProfile()
        {
            User user = db.UserList.Where(u => u.Email.ToLower() == User.Identity.Name.ToLower()).ToList()[0];
            if (user == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Edit", routeValues: new { id = user.ID });
        }

        // GET: Users/Edit/5
        public ActionResult GetKey()
        {
            User user = db.UserList.Where(u => u.Email.ToLower() == User.Identity.Name.ToLower()).ToList()[0];
            if (user == null)
            {
                return HttpNotFound();
            }
            var prop = AppUtil.GetProps();
            AppUtil.SendMail(user.Email, prop["keySub"], string.Format(prop["keyBody"], (user.Name == null ? user.Email : user.Name), user.Email));
            return RedirectToAction("Index", "Home");
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Email,Name,DateOfBirth,PhoneNumber,Status")] User user)
        {
            User storedUser = await db.UserList.FindAsync(user.ID);
            if (storedUser == null || (!User.IsInRole("Admin") && storedUser.Email.ToLower() != User.Identity.Name.ToLower()))
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                storedUser.Name = user.Name;
                storedUser.DateOfBirth = user.DateOfBirth;
                storedUser.PhoneNumber = user.PhoneNumber;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.UserList.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin"))
            {
                return View(user);
            }
            else
            {
                List<User> users = await db.UserList.Where(u => u.Email.ToLower() == User.Identity.Name.ToLower()).ToListAsync();
                return RedirectToAction("Details", new { id = users[0].ID });
            }
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.UserList.FindAsync(id);
            user.Status = "I";
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
