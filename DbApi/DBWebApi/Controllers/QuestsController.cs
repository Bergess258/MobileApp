using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DBWebApi;
using DBWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DBWebApi.Controllers
{
    public class QuestsController : ApiController
    {
        private DBContx db = new DBContx();

        // GET: api/Quests
        public IQueryable<Quest> GetQuests()
        {
            return db.Quest;
        }

        // GET: api/Quests/5
        [ResponseType(typeof(Quest))]
        public IHttpActionResult GetQuest(int id)
        {
            Quest quest = db.Quest.Find(id);
            if (quest == null)
            {
                return NotFound();
            }

            return Ok(quest);
        }

        // PUT: api/Quests/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutQuest(int id, Quest quest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != quest.Id)
            {
                return BadRequest();
            }

            db.Entry(quest).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestExists(id))
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
        //На вход должны подаваться новые таски с 
        public IHttpActionResult QuestWithTasks(QuestWithTasks quest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            QuestWithTasksDbAdd(quest);
            //Изменить
            return Ok();
        }

        private void QuestWithTasksDbAdd(QuestWithTasks quest)
        {
            db.Quest.Add(quest.GetQuest());
            TaskWithCounter[] tasks = quest.tasks;
            for (int i = 0; i < tasks.Length; ++i)
            {
                if (tasks[i].Id == -1)
                {
                    db.Task.Add(tasks[i]);
                }
            }
            db.SaveChanges();
            for (int i = 0; i < tasks.Length; ++i)
            {
                db.QuestTask.Add(new QuestTask() { Quest = quest, Task = tasks[i], CountToComplete = tasks[i].countToComplete });
            }
            db.SaveChanges();
        }

        public IHttpActionResult UsersQuestWithTasks(UserQuestWithTasks usersWithQuest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            QuestWithTasksDbAdd(usersWithQuest);
            User[] users = usersWithQuest.users;
            for (int i = 0; i < users.Length; i++)
            {
                db.UserQuest.Add(new UserQuest() {User = users[i] ,Quest = usersWithQuest, Status = false});
            }

            db.SaveChanges();
            //Изменить
            return Ok();
        }
        [ResponseType(typeof(Quest))]
        public IHttpActionResult PostQuest(Quest quest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Quest.Add(quest);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = quest.Id }, quest);
        }

        // DELETE: api/Quests/5
        [ResponseType(typeof(Quest))]
        public IHttpActionResult DeleteQuest(int id)
        {
            Quest quest = db.Quest.Find(id);
            if (quest == null)
            {
                return NotFound();
            }
            QuestTask[] quest_Tasks = db.QuestTask.Where(x => x.QuestId == id).ToArray();
            for (int i = 0; i < quest_Tasks.Length; ++i)
            {
                db.QuestTaskUser.RemoveRange(db.QuestTaskUser.Where(x=>x.QuestTaskId==quest_Tasks[i].Id));
            }
            db.QuestTask.RemoveRange(quest_Tasks);
            db.Quest.Remove(quest);
            db.SaveChanges();

            return Ok(quest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestExists(int id)
        {
            return db.Quest.Count(e => e.Id == id) > 0;
        }
    }
}