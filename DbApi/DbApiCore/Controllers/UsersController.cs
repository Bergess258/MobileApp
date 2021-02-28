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
    public class UsersController : ControllerBase
    {
        private readonly DBContx db;
        private const float KPIAddForEntry = 0.1f;
        private const string noSuchUserError = "No such user";

        public UsersController(DBContx context)
        {
            db = context;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await db.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        public async Task<ActionResult<User>> GetUser(string mail, string pass)
        {
            User user = await db.Users.Where(x => x.Mail == mail && x.Password == pass).Include(x => x.ActAttendings).FirstAsync();
            if (user == null)
                //Хз че тут написать, но пусть пока будет так
                return BadRequest("Неверная комбинация логина и пароля");
            if (!user.MailConfirm)
                return BadRequest("Нужно подтверждение почты");
            int daysBetweenEntry = DateTime.Today.Subtract(user.LastEntry).Days;
            if (daysBetweenEntry > 1)
                user.Bonus = 0;
            if (daysBetweenEntry == 1)
                user.AddKPI(db, KPIAddForEntry * ++user.Bonus);
            db.SaveChanges();
            return Ok(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
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

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return db.Users.Any(e => e.Id == id);
        }

        [Route("/Users/MonthRank")]
        public async Task<ActionResult<IQueryable<UsersRank>>> MonthRank()
        {
            return Ok(db.MonthRank());
        }
        [Route("/Users/Rank")]
        public async Task<ActionResult<IQueryable<UsersRank>>> Rank()
        {
            return Ok(db.Rank());
        }
    }
}
