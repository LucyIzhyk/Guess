using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAttemptsToGuess.Data;
using UserAttemptsToGuess.Models;

namespace UserAttemptsToGuess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAttemptsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserAttemptsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserAttempts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAttempt>>> GetUserAttepmts()
        {
            return await _context.UserAttepmts.ToListAsync();
        }

        // GET: api/UserAttempts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserAttempt>> GetUserAttempt(int id)
        {
            var userAttempt = await _context.UserAttepmts.FindAsync(id);

            if (userAttempt == null)
            {
                return NotFound();
            }

            return userAttempt;
        }

        // PUT: api/UserAttempts/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserAttempt(int id, UserAttempt userAttempt)
        {
            if (id != userAttempt.Id)
            {
                return BadRequest();
            }

            _context.Entry(userAttempt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAttemptExists(id))
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

        // POST: api/UserAttempts
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<UserAttempt>> PostUserAttempt(UserAttempt userAttempt)
        {
            _context.UserAttepmts.Add(userAttempt);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserAttempt", new { id = userAttempt.Id }, userAttempt);
        }

        // DELETE: api/UserAttempts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserAttempt>> DeleteUserAttempt(int id)
        {
            var userAttempt = await _context.UserAttepmts.FindAsync(id);
            if (userAttempt == null)
            {
                return NotFound();
            }

            _context.UserAttepmts.Remove(userAttempt);
            await _context.SaveChangesAsync();

            return userAttempt;
        }

        private bool UserAttemptExists(int id)
        {
            return _context.UserAttepmts.Any(e => e.Id == id);
        }
    }
}
