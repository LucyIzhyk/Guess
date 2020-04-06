using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using UserAttemptsToGuess.Data;
using UserAttemptsToGuess.Models;
using UserAttemptsToGuess.Helpers;

namespace UserAttemptsToGuess.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        private const string SESSION_KEY = "UserAttempts";
        private const int MIN_NUMBER = 3;
        private const int MAX_NUMBER = 250;
        private const int TOTAL_ATTEMPTS = 3;
        private const int PREDEFINED_NUMBER = 105;

        [BindProperty]
        public UserAttempt UserAttempt { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public int MinNumber { get { return MIN_NUMBER; } }

        [BindProperty]
        public int MaxNumber { get { return MAX_NUMBER; } }

        [BindProperty]
        public bool NumberMatches { get; set; } = false;

        [BindProperty]
        public int? AvailableAttempts { get; set; }

        public class InputModel
        {
            [Required]
            [Range(MIN_NUMBER, MAX_NUMBER)]
            public int Number { get; set; }
        }

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ValidateSessionKey();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var sessionKey = ValidateSessionKey();
            var attemptsCount = _context.UserAttepmts.Count(user => user.SessionKey == sessionKey);

            if (attemptsCount >= TOTAL_ATTEMPTS)
            {
                ModelState.AddModelError(string.Empty, "You have no more attempts to try");
                return Page();
            }

            UserAttempt.InputNumber = Input.Number;
            UserAttempt.SessionKey = sessionKey;
            UserAttempt.AttemptDate = DateTime.Now;
            attemptsCount++;

            AvailableAttempts = TOTAL_ATTEMPTS - attemptsCount;

            if (Input.Number == PREDEFINED_NUMBER)
            {
                NumberMatches = true;
            }
            
            _context.UserAttepmts.Update(UserAttempt);
            await _context.SaveChangesAsync();

            return Page();
        }

        private string ValidateSessionKey()
        {
            var sessionKey = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, SESSION_KEY);
            if (sessionKey == null)
            {
                sessionKey = Guid.NewGuid().ToString();
                SessionHelper.SetObjectAsJson(HttpContext.Session, SESSION_KEY, sessionKey);
            }

            return sessionKey;
        }
    }
}
