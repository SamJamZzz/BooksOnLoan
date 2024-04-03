using BooksOnLoan.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BooksOnLoan.Data
{
    public class BookContext : IdentityDbContext<CustomUser, CustomRole, string>
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Seed();

            builder.Entity<Book>().ToTable("Book");
            builder.Entity<Hold>().ToTable("Hold");

            builder.Entity<Book>().HasData(SampleData.GetBooks());
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Hold> Holds { get; set; }
    }
}