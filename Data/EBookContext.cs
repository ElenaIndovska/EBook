using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EBook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EBook.Areas.Identity.Data;

namespace EBook.Data
{
    public class EBookContext : IdentityDbContext<EBookUser>
    {
        public EBookContext (DbContextOptions<EBookContext> options)
            : base(options)
        {
        }

        public DbSet<EBook.Models.Author> Author { get; set; }

        public DbSet<EBook.Models.Book> Book { get; set; }

        public DbSet<EBook.Models.BookAuthor> BookAuthor { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BookAuthor>()
                .HasOne<Book>(p => p.Book)
                .WithMany(p => p.Authors)
                .HasForeignKey(p => p.BookId);

            builder.Entity<BookAuthor>()
                .HasOne<Author>(p => p.Author)
                .WithMany(p => p.Books)
                .HasForeignKey(p => p.AuthorId);
         
        }
    }
}
