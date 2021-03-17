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
    public class ActChatsController : ControllerBase
    {
        private readonly DBContx db;
        private const float KPIAddFotComment = 0.1f;
        private const float KPIAddForThanks = 0.5f;
        private const string noSuchCommentError = "No such comment";

        public ActChatsController(DBContx context)
        {
            db = context;
        }

        // GET: api/ActChats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActChat>>> GetActChats()
        {
            return await db.ActChats.ToListAsync();
        }

        // GET: api/ActChats/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActChat[]>> GetActChat(int id)
        {
            ActChat[] actChat = await db.ActChats.Where(x => x.ActivityId == id).Include(x=>x.ChatPhotos).ToArrayAsync();

            if (actChat == null)
            {
                return NotFound();
            }

            return actChat;
        }

        // PUT: api/ActChats/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActChat(int id, ActChat actChat)
        {
            if (id != actChat.Id)
            {
                return BadRequest();
            }

            db.Entry(actChat).State = EntityState.Modified;

            List<ChatPhoto> chatPhotos = db.ChatPhotos.Where(x => x.ActChatId == id).ToList();
            foreach(ChatPhoto chatPhoto in actChat.ChatPhotos)
            {
                bool noSkip = true;
                for (int j = 0; j < chatPhotos.Count; ++j)
                {
                    if (chatPhoto.Id == chatPhotos[j].Id)
                    {
                        noSkip = false;
                        chatPhotos.RemoveAt(j);
                        break;
                    }
                }
                if (noSkip)
                    db.ChatPhotos.Add(chatPhoto);
            }
            for (int j = 0; j < chatPhotos.Count; ++j)
            {
                db.ChatPhotos.Remove(chatPhotos[j]);
            }

            try
            {
                await db.SaveChangesAsync();
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

            return NoContent();
        }

        // POST: api/ActChats
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ActChat>> PostActChat(ActChat actChat)
        {
            User user = db.Users.Find(actChat.UserId);
            if (user == null)
                return NotFound();
            user.AddKPI(db, KPIAddFotComment);
            foreach (var photo in actChat.ChatPhotos)
                db.ChatPhotos.Add(photo);
            db.ActChats.Add(actChat);
            await db.SaveChangesAsync();

            return CreatedAtAction("GetActChat", new { id = actChat.Id }, actChat);
        }

        // DELETE: api/ActChats/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ActChat>> DeleteActChat(int id)
        {
            var actChat = await db.ActChats.FindAsync(id);
            if (actChat == null)
            {
                return NotFound();
            }

            db.ActChats.Remove(actChat);
            await db.SaveChangesAsync();

            return actChat;
        }

        private bool ActChatExists(int id)
        {
            return db.ActChats.Any(e => e.Id == id);
        }

        public async Task<ActionResult> SendThanks(int actChatId)
        {
            ActChat actChat = await db.ActChats.FindAsync(actChatId);
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
