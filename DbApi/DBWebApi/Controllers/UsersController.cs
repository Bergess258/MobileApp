using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using DBWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DBWebApi.Controllers
{
    public class UsersController : ApiController
    {
        private const string noSuchUserError = "No such user";
        private const string noSuchCommentError = "No such comment";
        private const float KPIAddForThanks = 0.5f;
        private const float KPIAddForEntry = 0.1f;
        private DBContx db = new DBContx();

        // GET: api/Users
        public IQueryable<User> GetUser()
        {
            return db.Users;
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(string mail, string pass)
        {
            User user = db.Users.Where(x => x.Mail == mail && x.Password == pass).First();
            if (user == null)
                //Хз че тут написать, но пусть пока будет так
                return BadRequest("Неверная комбинация логина и пароля");
            if (!user.MailConfirm)
                return BadRequest("Нужно подтверждение почты");
            int daysBetweenEntry = DateTime.Today.Subtract(user.LastEntry).Days;
            if (daysBetweenEntry > 1)
                user.Bonus = 0;
            if(daysBetweenEntry==1)
                user.AddKPI(db, KPIAddForEntry * ++user.Bonus);
            db.SaveChanges();
            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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
        
        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return GetUser(user.Id);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        [ResponseType(typeof(QuestWithTasks[]))]
        public IHttpActionResult GetQuests(int id)
        {
            //Check
            //User user = db.User.Where(u=>u.Id ==id).Include(u=>u.UserQuest).ThenInclude(uq=>uq.Quest).ThenInclude(q=>q.Quest_Task).ThenInclude(qt=>qt.Quest_Task_User).ThenInclude(qt=>qt.Quest_Task.Task).FirstOrDefault();
            User user = db.Users.Where(u => u.Id == id).First();
            if (user == null)
                //Хз че тут написать, но пусть пока будет так
                return BadRequest(noSuchUserError);
            var userQuestsNoTask = user.UserQuests.ToArray();
            QuestWithTasks[] userQuests = new QuestWithTasks[user.UserQuests.Count];
            for (int i = 0; i < userQuestsNoTask.Length; ++i)
            {
                var questTasks = userQuestsNoTask[i].Quest.QuestTasks.ToArray();
                userQuests[i] = new QuestWithTasks(userQuestsNoTask[i].Quest, questTasks.Length, userQuestsNoTask[i].Status);
                if(userQuestsNoTask[i].Status)
                    for (int j = 0; j < questTasks.Length; ++j)
                        userQuests[i].tasks[j] = new TaskWithCounter(questTasks[i].Task, questTasks[i].CountToComplete, questTasks[i].CountToComplete);
                else
                    for (int j = 0; j < questTasks.Length; ++j)
                        userQuests[i].tasks[j] = new TaskWithCounter(questTasks[i].Task, questTasks[i].QuestTaskUser.FirstOrDefault().Counter, questTasks[i].CountToComplete);
            }
            return Ok(userQuests);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }

        public IHttpActionResult SendThanks(int actChatId)
        {
            ActChat actChat = db.ActChats.Find(actChatId);
            if (actChat == null)
                //Хз че тут написать, но пусть пока будет так
                return BadRequest(noSuchCommentError);
            actChat.Thanks++;
            actChat.User.Thanks++;
            actChat.User.AddKPI(db, KPIAddForThanks);
            db.Entry(actChat).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }
    }
}