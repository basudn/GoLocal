using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GoLocal.Models;

namespace GoLocal.Controllers
{
    public class CommentsServiceController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ModelFactory _modelFactory = new ModelFactory();

        // GET: api/CommentsService
        public IEnumerable<CommentModel> GetCommentList()
        {
            return db.CommentList.ToList().Select(c => _modelFactory.Wrap(c));
        }

        // GET: api/CommentsService/5
        [ResponseType(typeof(CommentModel))]
        public IHttpActionResult GetComment(int id)
        {
            Comment comment = db.CommentList.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(_modelFactory.Wrap(comment));
        }

        // PUT: api/CommentsService/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutComment(int id, CommentModel comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != comment.ID)
            {
                return BadRequest();
            }

            db.Entry(_modelFactory.Unwrap(comment)).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        // POST: api/CommentsService
        [ResponseType(typeof(CommentModel))]
        public IHttpActionResult PostComment(CommentModel comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CommentList.Add(_modelFactory.Unwrap(comment));
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = comment.ID }, comment);
        }

        // DELETE: api/CommentsService/5
        [ResponseType(typeof(CommentModel))]
        public IHttpActionResult DeleteComment(int id)
        {
            Comment comment = db.CommentList.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            db.CommentList.Remove(comment);
            db.SaveChanges();

            return Ok(_modelFactory.Wrap(comment));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentExists(int id)
        {
            return db.CommentList.Count(e => e.ID == id) > 0;
        }
    }
}