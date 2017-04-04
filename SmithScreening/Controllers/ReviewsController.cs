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
using SmithScreening.Models;

namespace SmithScreening.Controllers
{
    public class ReviewsController : ApiController
    {
        private DBContext db = new DBContext();

        // GET: api/Reviews
        public IQueryable<Review> Getreviews()
        {
            return db.reviews;
        }

        // GET: api/Reviews/5
        [ResponseType(typeof(GridViewModel))]
        [Route("api/Reviews/GetReview/{id}/{rowsperpage}/{sortBy}/{order}")]
        public  IHttpActionResult GetReview(int id,int rowsperpage,string sortBy,string order)
        {
            var reviews =  db.reviews.ToList();
            var count = reviews.Count();
            if (sortBy != "undefined")
            {
                System.Reflection.PropertyInfo prop = typeof(Review).GetProperty(sortBy);
                if(order=="true")
                reviews = reviews.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                else reviews = reviews.OrderBy(x => prop.GetValue(x, null)).ToList();
            }
          
            var gridcount = new GridViewModel()
            {
                reviews = reviews = reviews.Skip(id).Take(rowsperpage).ToList(),
                totalPages = count
            };
           
            if (reviews == null)
            {
                return NotFound();
            }

            return Ok(gridcount);
        }

        // PUT: api/Reviews/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutReview(int id, Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != review.id)
            {
                return BadRequest();
            }

            db.Entry(review).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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

        // POST: api/Reviews
        [ResponseType(typeof(Review))]
        public async Task<IHttpActionResult> PostReview(Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.reviews.Add(review);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = review.id }, review);
        }

        // DELETE: api/Reviews/5
        [ResponseType(typeof(Review))]
        public async Task<IHttpActionResult> DeleteReview(int id)
        {
            Review review = await db.reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            db.reviews.Remove(review);
            await db.SaveChangesAsync();

            return Ok(review);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReviewExists(int id)
        {
            return db.reviews.Count(e => e.id == id) > 0;
        }
    }
}