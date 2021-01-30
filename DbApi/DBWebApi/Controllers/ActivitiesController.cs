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
        private const float KPIAddFotAttending = 0.1f;

        // GET: api/Activities
        public ActWithCatGet[] GetActivities()
        {
            Activity[] activities = db.Activities.Include(x=>x.ActCategories).ThenInclude(x=>x.Category).ToArray();
            ActWithCatGet[] actWithCat = new ActWithCatGet[activities.Length];
            for (int i = 0; i < activities.Length; ++i)
            {
                if (activities[i].ActCategories != null)
                    actWithCat[i] = new ActWithCatGet(activities[i], activities[i].ActCategories.Select(x => x.Category.Name).ToArray());
                else
                    actWithCat[i] = new ActWithCatGet(activities[i]);
            }
            return actWithCat;
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

            Activity activity = db.Activities.Find(id);
            User user = db.Users.Find(userId);
            ActAttending actAttending = new ActAttending() { ActivityId = id, UserId = userId };
            if (activity == null || user == null)
                return NotFound();
            if (db.ActAttending.Count(x => x.ActivityId == id && x.UserId == userId) > 0)
                return BadRequest("Activity attending already exist");
            user.ActAttendings.Add(actAttending);
            user.AddKPI(db, KPIAddFotAttending);
            db.SaveChanges();

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
        [Route("Activities/WithCat")]
        [HttpPost]
        [ResponseType(typeof(ActWithCatPost))]
        public IHttpActionResult PostActivity(ActWithCatPost activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int length = activity.categories.Length;

            for (int i = 0; i < length; i++)
            {
                if (activity.categories[i].Id == 0)
                    db.Categories.Add(activity.categories[i]);
            }
            Activity actReal = activity;
            db.Activities.Add(actReal);
            db.SaveChanges();

            for (int i = 0; i < activity.categories.Length; i++)
            {
                ActCategory actCategory = new ActCategory() { Activity = actReal, Category = activity.categories[i] };
                db.ActCategories.Add(actCategory);
                actReal.ActCategories.Add(actCategory);
                db.Entry(actReal).State = EntityState.Modified;
                activity.categories[i].ActCategories.Add(actCategory);
                db.Entry(activity.categories[i]).State = EntityState.Modified;
            }
            db.SaveChanges();

            return Ok(activity);
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