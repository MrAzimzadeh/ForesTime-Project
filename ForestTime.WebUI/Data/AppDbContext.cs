using ForestTime.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ForestTime.WebUI.Data
{
    public class AppDbContext:IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {
            // A constructor is a special method that is called when an object of a class is created.
            // In this case, the constructor is used to initialize the AppDbContext object with the provided DbContextOptions.
            // The base(options) syntax is used to invoke the constructor of the base class (IdentityDbContext<User>) and pass the options parameter to it.
        }
        //DbSet is a class provided by Entity Framework,
        //which represents a collection of entities in a database table
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");

        }
    }
}
