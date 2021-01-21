using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DBWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DBWebApi.Controllers
{
    public class ActivitiesController : ApiController
    {
        private DBContx db = new DBContx();

        // GET: api/Activities
        public IQueryable<Activity> GetActivities()
        {
            return db.Activities;
        }

        // GET: api/Activities/5
        [ResponseType(typeof(Activity))]
        public IHttpActionResult GetActivity(int id)
        {
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return NotFound();
            }

            return Ok(activity);
        }
        public IHttpActionResult GetActivity(int id,int userId)
        {
            ActAttending actAttending = new ActAttending() { UserId = userId, ActivityId = id };
            if (db.ActAttending.Find(actAttending)==null)
                db.ActAttending.Add(actAttending);

            return Ok();
        }

        // PUT: api/Activities/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutActivity(int id, Activity activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != activity.Id)
            {
                return BadRequest();
            }

            db.Entry(activity).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(id))
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

        // POST: api/Activities
        [ResponseType(typeof(Activity))]
        public IHttpActionResult PostActivity(ActWithCat activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int length = activity.categories.Length;

            for (int i = 0; i < length; i++)
            {
                if (activity.categories[i].Id == -1)
                    db.Categories.Add(activity.categories[i]);
            }
            db.Activities.Add(activity);
            db.SaveChanges();

            for (int i = 0; i < activity.categories.Length; i++)
                db.ActCategories.Add(new ActCategory() { Activity = activity, Category = activity.categories[i] });
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = activity.Id }, activity);
        }

        [ResponseType(typeof(Activity))]
        public IHttpActionResult PostActivity(Activity activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Activities.Add(activity);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = activity.Id }, activity);
        }

        // DELETE: api/Activities/5
        [ResponseType(typeof(Activity))]
        public IHttpActionResult DeleteActivity(int id)
        {
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return NotFound();
            }

            db.Activities.Remove(activity);
            db.SaveChanges();

            return Ok(activity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ActivityExists(int id)
        {
            return db.Activities.Count(e => e.Id == id) > 0;
        }
    }
}