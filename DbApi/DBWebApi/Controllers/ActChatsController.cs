using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DBWebApi.Data;
using DBWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DBWebApi.Controllers
{
    public class ActChatsController : ApiController
    {
        private DBContx db = new DBContx();

        // GET: api/ActChats
        public IQueryable<ActChat> GetActChats()
        {
            return db.ActChats;
        }

        // GET: api/ActChats/5
        [ResponseType(typeof(ActChat[]))]
        public IHttpActionResult GetActChat(int id)
        {
            ActChat[] actChat = db.ActChats.Where(x=> x.ActivityId ==id).ToArray();
            if (actChat == null)
            {
                return NotFound();
            }

            return Ok(actChat);
        }

        // PUT: api/ActChats/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutActChat(int id, ActChat actChat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != actChat.Id)
            {
                return BadRequest();
            }

            db.Entry(actChat).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActChatExists(id))
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

        // POST: api/ActChats
        [ResponseType(typeof(ActChat))]
        public IHttpActionResult PostActChat(ActChat actChat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ActChats.Add(actChat);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = actChat.Id }, actChat);
        }

        // DELETE: api/ActChats/5
        [ResponseType(typeof(ActChat))]
        public IHttpActionResult DeleteActChat(int id)
        {
            ActChat actChat = db.ActChats.Find(id);
            if (actChat == null)
            {
                return NotFound();
            }

            db.ActChats.Remove(actChat);
            db.SaveChanges();

            return Ok(actChat);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ActChatExists(int id)
        {
            return db.ActChats.Count(e => e.Id == id) > 0;
        }
    }
}