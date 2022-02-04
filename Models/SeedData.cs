using EBook.Areas.Identity.Data;
using EBook.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBook.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<EBookUser>>();
            IdentityResult roleResult;
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            EBookUser user = await UserManager.FindByEmailAsync("admin@ebook.com");
            if (user == null)
            {
                var User = new EBookUser();
                User.Email = "admin@ebook.com";
                User.UserName = "admin@ebook.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }
        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new EBookContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<EBookContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();
                if (context.Book.Any() || context.Author.Any())
                {
                    return;
                }

                context.Book.AddRange(
                    new Book { /*Id=1*/ Title = "The Hobbit", Genre = "Fantasy", Year = 1937 },
                    new Book { /*Id=2*/ Title = "The Perks of Being a Wallflower", Genre = "Novel",Year = 1999 },
                    new Book { /*Id=3*/ Title = "Harry Potter and the Philosopher's Stone", Genre = "Fantasy", Year = 1997 },
                    new Book { /*Id=4*/ Title = "Harry Potter and the Chamber of Secrets", Genre = "Fantasy", Year = 1998 },
                    new Book { /*Id=5*/ Title = "Harry Potter and the Prisoner of Azkaban", Genre = "Fantasy", Year = 1999 },
                    new Book { /*Id=6*/ Title = "Harry Potter and the Goblet of Fire", Genre = "Fantasy", Year = 2000 },
                    new Book { /*Id=7*/ Title = "Harry Potter and the Order of the Phoenix", Genre = "Fantasy", Year = 2003 },
                    new Book { /*Id=8*/ Title = "Harry Potter and the Half-Blood Prince", Genre = "Fantasy", Year = 2005 },
                    new Book { /*Id=9*/ Title = "Harry Potter and the Deathly Hallows", Genre = "Fantasy", Year = 2007 }

                );

                context.SaveChanges();

                context.Author.AddRange(
                    new Author { /*Id=1*/ Name = "J. R. R. Tolkien", BirthDate = DateTime.Parse("1892-01-03") },
                    new Author { /*Id=2*/ Name = "Stephen Chbosky", BirthDate = DateTime.Parse("1970-01-25") },
                    new Author { /*Id=3*/ Name = "J. K. Rowling", BirthDate = DateTime.Parse("1965-07-31") }
                );

                context.SaveChanges();


                context.BookAuthor.AddRange(
                    new BookAuthor { BookId = 1, AuthorId = 1 },
                    new BookAuthor { BookId = 2, AuthorId = 2 },
                    new BookAuthor { BookId = 3, AuthorId = 3 },
                    new BookAuthor { BookId = 4, AuthorId = 3 },
                    new BookAuthor { BookId = 5, AuthorId = 3 },
                    new BookAuthor { BookId = 6, AuthorId = 3 },
                    new BookAuthor { BookId = 7, AuthorId = 3 },
                    new BookAuthor { BookId = 8, AuthorId = 3 },
                    new BookAuthor { BookId = 9, AuthorId = 3 }
                );

                context.SaveChanges();

            }
        }
    }
}
