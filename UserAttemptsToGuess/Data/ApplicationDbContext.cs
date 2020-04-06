using Microsoft.EntityFrameworkCore;
using UserAttemptsToGuess.Models;

namespace UserAttemptsToGuess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<UserAttempt> UserAttepmts { get; set; }
    }
}
