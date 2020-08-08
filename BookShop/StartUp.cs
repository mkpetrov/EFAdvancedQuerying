using BookShop.Data;
using BookShop.Models;
using BookShop.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new BookShopContext();

            var input = Console.ReadLine();

            var output = GetBookTitlesContaining(context, input);

            //var output = GetAuthorNamesEndingIn(context, input);

            //var output = GetBooksReleasedBefore(context, input);

            //var year = int.Parse(Console.ReadLine());

            //var output = GetBooksNotReleasedIn(context, year);

            //var output = GetBooksByPrice(context);

            //var output = GetGoldenBooks(context);

            //var restriction = Console.ReadLine().ToUpper();

            //var output = GetBooksByAgeRestriction(context, restriction);

            Console.WriteLine(output);

            //Seed(context);
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .FromSqlInterpolated($"SELECT * FROM Books WHERE Title LIKE '%{input}%'")
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title}");
            }

            return sb.ToString().Trim();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .FromSqlInterpolated($"SELECT * FROM Authors WHERE FirstName LIKE '%{input}'")
                .ToList();

            var sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FirstName} {author.LastName}");
            }

            return sb.ToString().Trim();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var books = context.Books
                .FromSqlInterpolated($"SELECT * FROM Books WHERE ReleaseDate < CONVERT(datetime, {date})")
                .ToList();

            var sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price}");
            }

            return sb.ToString().Trim();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .FromSqlInterpolated($"SELECT * FROM Books WHERE YEAR(ReleaseDate) > {year} OR YEAR(ReleaseDate) < {year} ORDER BY BookId DESC")
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().Trim();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .FromSqlRaw("SELECT * FROM Books WHERE Price > 40 ORDER BY Price DESC")
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price}");
            }

            return sb.ToString().Trim();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .FromSqlRaw($"SELECT * FROM Books WHERE Copies < 5000 Order By BookId")
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().Trim();
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            int ageRestriction = default;
            switch (command)
            {
                case "MINOR":
                    ageRestriction = (int)AgeRestriction.Minor;
                    break;

                case "TEEN":
                    ageRestriction = (int)AgeRestriction.Teen;
                    break;

                case "ADULT":
                    ageRestriction = (int)AgeRestriction.Adult;
                    break;
            }

            var books = context.Books
                .FromSqlInterpolated($"SELECT * FROM Books WHERE AgeRestriction = {ageRestriction}")
                .OrderBy(b => b.Title)
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().Trim();
        }

        private static void Seed(BookShopContext context)
        {
            var authors = new List<Author>
            {
                new Author
                {
                    FirstName = "Martin",
                    LastName = "Petrov"
                },
                new Author
                {
                    FirstName = "Nikol",
                    LastName = "Petrova"
                },
                new Author
                {
                    FirstName = "Aneliya",
                    LastName = "Petrova"
                },
                new Author
                {
                    FirstName = "George",
                    LastName = "Powell"
                },
                new Author
                {
                    FirstName = "Jane",
                    LastName = "Ortiz"
                },
                new Author
                {
                    FirstName = "Randy",
                    LastName = "Morales"
                }
            };

            context.AddRange(authors);

            var categories = new List<Category>
            {
                new Category
                {
                    Name = "horror"
                },
                new Category
                {
                    Name = "mystery"
                },
                new Category
                {
                    Name = "drama"
                }
            };
            context.AddRange(categories);
            context.SaveChanges();

            var books = new List<Book>
            {
                new Book
                {
                    Title = "A Confederacy of Dunces",
                    AgeRestriction = AgeRestriction.Minor,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description"
                },
                new Book
                {
                    Title = "A Farewell to Arms",
                    AgeRestriction = AgeRestriction.Minor,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description"
                },
                new Book
                {
                    Title = "A Handful of Dust",
                    AgeRestriction = AgeRestriction.Minor,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    Price = 48.63M
                },
                new Book
                {
                    Title = "A Passage to India",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description"
                },
                new Book
                {
                    Title = "A Scanner Darkly",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description"
                },
                new Book
                {
                    Title = "A Swiftly Tilting Planet",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description"
                },
                new Book
                {
                    Title = "Lilies of the Field",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Gold
                },
                new Book
                {
                    Title = "Look Homeward",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Gold
                },
                new Book
                {
                    Title = "The Mirror Crack'd from Side to Side",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Gold
                },
                new Book
                {
                    Title = "O Pioneers!",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Gold,
                    Price = 49.90M
                },
                new Book
                {
                    Title = "That Hideous Strength",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Gold,
                    Price = 48.63M
                },
                new Book
                {
                    Title = "Absalom",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Gold,
                    Price = 48.63M,
                    ReleaseDate = DateTime.Now
                },
                new Book
                {
                    Title = "Nectar in a Sieve",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Gold,
                    Price = 48.63M,
                    ReleaseDate = DateTime.Now
                },
                new Book
                {
                    Title = "Nine Coaches Waiting",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Gold,
                    Price = 48.63M,
                    ReleaseDate = DateTime.Now
                },
                new Book
                {
                    Title = "The Needle's Eye",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Gold,
                    Price = 48.63M,
                    ReleaseDate = DateTime.Now
                },
                new Book
                {
                    Title = "No Country for Old Men",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Gold,
                    Price = 48.63M,
                    ReleaseDate = DateTime.Now
                },
                new Book
                {
                    Title = "No Highway",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Gold,
                    Price = 48.63M,
                    ReleaseDate = DateTime.Now
                },
                new Book
                {
                    Title = "A Fanatic Heart",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Gold,
                    Price = 48.63M,
                    ReleaseDate = new DateTime(1988, 12, 30)
                },
                new Book
                {
                    Title = "A Glass of Blessings",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Gold,
                    Price = 48.63M,
                    ReleaseDate = DateTime.Now
                },
                new Book
                {
                    Title = "If I Forget Thee Jerusalem",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Gold,
                    Price = 33.21M,
                    ReleaseDate = new DateTime(1991, 4, 12)
                },
                new Book
                {
                    Title = "Oh! To be in England",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Normal,
                    Price = 46.67M,
                    ReleaseDate = new DateTime(1991, 4, 12)
                },
                new Book
                {
                    Title = "The Monkey's Raincoat",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Normal,
                    Price = 46.67M,
                    ReleaseDate = new DateTime(1991, 4, 12)
                },
                new Book
                {
                    Title = "The Curious Incident of the Dog in the Night",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Normal,
                    Price = 23.41M,
                    ReleaseDate = new DateTime(1988, 12, 30)
                },
                new Book
                {
                    Title = "The Other Side of Silence",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Normal,
                    Price = 46.26M,
                    ReleaseDate = new DateTime(1988, 12, 30)
                },
                new Book
                {
                    Title = "A Catskill Eagle",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Normal,
                    Price = 46.26M,
                    ReleaseDate = new DateTime(1988, 12, 30)
                },
                new Book
                {
                    Title = "The Daffodil Sky",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Normal,
                    Price = 46.26M,
                    ReleaseDate = new DateTime(1988, 12, 30)
                },
                new Book
                {
                    Title = "The Skull Beneath the Skin",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Normal,
                    Price = 46.26M,
                    ReleaseDate = new DateTime(1988, 12, 30)
                },
                new Book
                {
                    Title = "Great Work of Time",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Normal,
                    Price = 46.26M,
                    ReleaseDate = new DateTime(1988, 12, 30)
                },
                new Book
                {
                    Title = "Terrible Swift Sword",
                    AgeRestriction = AgeRestriction.Teen,
                    AuthorId = authors.First().AuthorId,
                    Description = "Some description",
                    EditionType = EditionType.Normal,
                    Price = 46.26M,
                    ReleaseDate = new DateTime(1988, 12, 30)
                }
            };

            context.AddRange(books);
            context.SaveChanges();

            var fanaticBook = books.First(b => b.Title == "A Fanatic Heart");
            var farewellBook = books.First(b => b.Title == "A Farewell to Arms");
            var glassBook = books.First(b => b.Title == "A Glass of Blessings");

            var horror = categories.First(c => c.Name == "horror");
            var mystery = categories.First(c => c.Name == "mystery");
            var drama = categories.First(c => c.Name == "drama");

            var bookCategories = new List<BookCategory>
            {
                new BookCategory
                {
                    BookId = fanaticBook.BookId,
                    CategoryId = horror.CategoryId
                },
                new BookCategory
                {
                    BookId = farewellBook.BookId,
                    CategoryId = mystery.CategoryId
                },
                new BookCategory
                {
                    BookId = glassBook.BookId,
                    CategoryId = drama.CategoryId
                }
            };

            context.AddRange(bookCategories);
            context.SaveChanges();
        }
    }
}
