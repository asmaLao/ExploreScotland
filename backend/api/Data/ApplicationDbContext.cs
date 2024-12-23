using api.Models;
using Microsoft.EntityFrameworkCore;


namespace api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<City> City { get; set; } = null!;
        public DbSet<Landmark> Landmark { get; set; } = null!;
        public DbSet<Question> Question { get; set; } = null!;
        public DbSet<QuestionChoices> QuestionChoices { get; set; } = null!;
        public DbSet<Quiz> Quiz { get; set; } = null!;
        public DbSet<User> User { get; set; } = null!;
    }
}
