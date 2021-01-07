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
        private DBContx db = new DBContx();

        // GET: api/Users
        public IQueryable<User> GetUser()
        {
            return db.User;
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(string mail, string pass)
        {
            User user = db.User.Where(x => x.Mail == mail && x.Password == pass).First();
            if (user == null)
                //Хз че тут написать, но пусть пока будет так
                return BadRequest("Неверная комбинация логина и пароля");
            if (!user.MailConfirm)
                return BadRequest("Нужно подтверждение почты");
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

            db.User.Add(user);

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
            User user = db.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.User.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        [ResponseType(typeof(QuestWithTasks[]))]
        public IHttpActionResult GetQuests(int id)
        {
            //Check
            //User user = db.User.Where(u=>u.Id ==id).Include(u=>u.UserQuest).ThenInclude(uq=>uq.Quest).ThenInclude(q=>q.Quest_Task).ThenInclude(qt=>qt.Quest_Task_User).ThenInclude(qt=>qt.Quest_Task.Task).FirstOrDefault();
            User user = db.User.Where(u => u.Id == id).First();
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
            return db.User.Count(e => e.Id == id) > 0;
        }

        public IHttpActionResult SendThanks(int userId)
        {
            User user = db.User.Where(e => e.Id == userId).First();
            if (user == null)
                //Хз че тут написать, но пусть пока будет так
                return BadRequest(noSuchUserError);
            user.Thanks++;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }
    }
}