using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DbApiCore.Models;

namespace DbApiCore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly DBContx db;
        private const float KPIAddFotAttending = 0.1f;

        public ActivitiesController(DBContx context)
        {
            db = context;
        }

        // GET: api/Activities
        [HttpGet]
        public async Task<ActWithCatGet[]> GetActivities()
        {
            Activity[] activities = await db.Activities.Include(x => x.ActCategories).ThenInclude(x => x.Category).ToArrayAsync();
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
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Activity>> GetActivity(int id)
        //{
        //    var activity = await db.Activities.FindAsync(id);

        //    if (activity == null)
        //    {
        //        return NotFound();
        //    }

        //    return activity;
        //}
        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(int id, int? userId)
        {
            Activity activity = await db.Activities.FindAsync(id);
            if (activity == null)
                return NotFound();
            if (userId.HasValue)
            {
                User user = db.Users.Find(userId.Value);
                ActAttending actAttending = new ActAttending() { ActivityId = id, UserId = userId.Value };
                if (activity == null || user == null)
                    return NotFound();
                if (db.ActAttending.Count(x => x.ActivityId == id && x.UserId == userId.Value) > 0)
                    return BadRequest("Activity attending already exist");
                user.ActAttendings.Add(actAttending);
                user.AddKPI(db, KPIAddFotAttending);
                db.SaveChanges();
                return Ok();
            }
            return activity;
        }

        [HttpGet]
        [Route("/Activities/Attending/{id}")]
        public async Task<ActAttending[]> Attending(int id)
        {
            ActAttending[] chat = await db.ActAttending.Where(x => x.ActivityId == id).Include(x => x.User).ToArrayAsync();
            return chat;
        }

        [HttpGet]
        [Route("/Activities/Categories/{id}")]
        public async Task<Category[]> Categories(int id)
        {
            Category[] cats = await db.ActCategories.Where(x => x.ActivityId == id).Include(x => x.Category).Select(x=>x.Category).ToArrayAsync();
            return cats;
        }

        [HttpGet]
        [Route("/Activities/Chat/{id}")]
        public async Task<ActChat[]> Chat(int id)
        {
            ActChat[] chat = await db.ActChats.Where(x => x.ActivityId == id).ToArrayAsync();
            return chat;
        }

        [HttpGet]
        [Route("/Activities/DelAttending/{id}")]
        public async Task<IActionResult> DelAttending(int id, int userId)
        {
            Activity activity = db.Activities.Find(id);
            User user = db.Users.Find(userId);
            ActAttending actAttending = db.ActAttending.FirstOrDefault(x => x.ActivityId == id && x.UserId == userId);
            if (activity == null || user == null || actAttending == null)
                return NotFound();
            user.ActAttendings.Remove(actAttending);
            db.ActAttending.Remove(actAttending);
            user.EngPoints -= KPIAddFotAttending;
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("/Activities/DelAtt/{id}")]
        public async Task<IActionResult> DelAtt(int id)
        {
            ActAttending actAttending = db.ActAttending.Find(id);
            if (actAttending == null)
                return NotFound();
            actAttending.User.EngPoints -= KPIAddFotAttending;
            db.Entry(actAttending.User).State = EntityState.Modified;
            db.ActAttending.Remove(actAttending);
            await db.SaveChangesAsync();
            return Ok();
        }

        // PUT: api/Activities/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivity(int id, ActWithCatPost activity)
        {
            if (id != activity.id)
            {
                return BadRequest();
            }

            int length = 0;
            if (activity.categories != null)
                length = activity.categories.Length;

            for (int i = 0; i < length; i++)
            {
                if (activity.categories[i].Id == 0)
                    db.Categories.Add(activity.categories[i]);
            }
            db.Entry((Activity)activity).State = EntityState.Modified;
            db.SaveChanges();

            List<ActCategory> actCategories = db.ActCategories.Where(x => x.ActivityId == id).ToList();
            for (int i = 0; i < length; ++i)
            {
                bool noSkip = true;
                for (int j = 0; j < actCategories.Count;++j)
                {
                    if(activity.categories[i].Id == actCategories[j].CategoryId)
                    {
                        noSkip = false;
                        actCategories.RemoveAt(j);
                        break;
                    }
                }
                if (noSkip)
                    db.ActCategories.Add(new ActCategory() { ActivityId = id, CategoryId = activity.categories[i].Id });
            }
            for (int j = 0; j < actCategories.Count; ++j)
            {
                db.ActCategories.Remove(actCategories[j]);
            }

            try
            {
                await db.SaveChangesAsync();
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

            return NoContent();
        }

        // POST: api/Activities
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Activity>> PostActivity(Activity activity)
        {
            db.Activities.Add(activity);
            await db.SaveChangesAsync();

            return CreatedAtAction("GetActivity", new { id = activity.Id }, activity);
        }
        [Route("/Activities/WithCat")]
        [HttpPost]
        public async Task<ActionResult<Activity>> PostActivity(ActWithCatPost activity)
        {
            int length = 0;
            if (activity.categories != null)
                length = activity.categories.Length;

            for (int i = 0; i < length; i++)
            {
                if (activity.categories[i].Id == 0)
                    db.Categories.Add(activity.categories[i]);
            }
            Activity actReal = activity;
            db.Activities.Add(actReal);
            await db.SaveChangesAsync();

            for (int i = 0; i < length; i++)
            {
                ActCategory actCategory = new ActCategory() { Activity = actReal, Category = activity.categories[i] };
                db.ActCategories.Add(actCategory);
                actReal.ActCategories.Add(actCategory);
                db.Entry(actReal).State = EntityState.Modified;
                activity.categories[i].ActCategories.Add(actCategory);
                db.Entry(activity.categories[i]).State = EntityState.Modified;
            }
            await db.SaveChangesAsync();

            return Ok(activity);
        }

        // DELETE: api/Activities/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Activity>> DeleteActivity(int id)
        {
            var activity = db.Activities.Find(id);
            if (activity == null)
            {
                return NotFound();
            }
            db.Activities.Remove(activity);
            await db.SaveChangesAsync();

            return activity;
        }

        private bool ActivityExists(int id)
        {
            return db.Activities.Any(e => e.Id == id);
        }
    }
}
