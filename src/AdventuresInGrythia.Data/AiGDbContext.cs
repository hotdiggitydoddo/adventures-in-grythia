using AdventuresInGrythia.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdventuresInGrythia.Data
{
    public class AiGDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Entity> Entities { get; set; }
        public DbSet<Trait> Traits { get; set; }
        public DbSet<Account_Entity> Characters { get; set; }
        public DbSet<Entity_Command> Commands { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectsV13;Initial Catalog=AiGDb;Integrated Security=True;Persist Security Info=False");
            //optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=AiGDb;User Id=postgres;Password=xxxx;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<User>()
               .HasOne(p => p.Account)
               .WithOne(i => i.User)
               .HasForeignKey<Account>(a => a.UserId);

            builder.Entity<Account>()
                .HasMany(x => x.Characters)
                .WithOne(c => c.Account)
                .HasForeignKey(c => c.AccountId);

            builder.Entity<Account_Entity>()
                .HasKey(x => new { x.AccountId, x.EntityId });
            
            builder.Entity<Entity>()
                .HasMany(x => x.Children)
                .WithOne(x => x.Parent)
                .HasForeignKey(x => x.ParentId);
        }
    }
}