namespace BookShop
{
    using System;
    using System.Linq;
    using System.Text;
    using BookShop.Data;
    using BookShop.Initializer;
    using BookShop.Models;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                int numberOfDeletedBooks = RemoveBooks(db);

                Console.WriteLine(numberOfDeletedBooks);
            }
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            string[] bookTitles = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles);
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            string[] bookTitles = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            string[] books = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => $"{b.Title} - ${b.Price:F2}")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksNotRealeasedIn(BookShopContext context, int year)
        {
            string[] bookTitles = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string[] bookTitles = context.Books
                .Where(b => b.BookCategories
                    .Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var inputDate = DateTime.ParseExact(date, "dd-MM-yyyy", null);

            string[] books = context.Books
                .Where(b => b.ReleaseDate.Value < inputDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:F2}")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            string[] authors = context.Authors
                .Where(a => EF.Functions.Like(a.FirstName, $"%{input}"))
                .Select(a => $"{a.FirstName} {a.LastName}")
                .OrderBy(f => f)
                .ToArray();

            return string.Join(Environment.NewLine, authors);
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            string[] bookTitles = context.Books
                .Where(b => EF.Functions.Like(b.Title, $"%{input}%"))
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            string[] booksByAuthor = context.Books
                .Where(b => EF.Functions.Like(b.Author.LastName, $"{input}%"))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
                .ToArray();

            return string.Join(Environment.NewLine, booksByAuthor);
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            int booksCount = context.Books
                .Count(b => b.Title.Length > lengthCheck);

            return booksCount;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var booksByAuthor = context.Authors
                .Select(a => new
                {
                    AuthorFullName = $"{a.FirstName} {a.LastName}",
                    TotalBookCopies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.TotalBookCopies)
                .ToArray();

            var output = new StringBuilder();

            foreach (var author in booksByAuthor)
            {
                output.AppendLine($"{author.AuthorFullName} - {author.TotalBookCopies}");
            }

            return output.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var profitByCategory = context.Categories
                .Select(c => new
                {
                    c.Name,
                    TotalProfit = c.CategoryBooks.Sum(bc => bc.Book.Copies * bc.Book.Price)
                })
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.Name)
                .ToArray();

            var output = new StringBuilder();

            foreach (var category in profitByCategory)
            {
                output.AppendLine($"{category.Name} ${category.TotalProfit:F2}");
            }

            return output.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    c.Name,
                    CategoryBooks = c.CategoryBooks.Select(bc => new
                    {
                        bc.Book.Title,
                        bc.Book.ReleaseDate
                    })
                    .OrderByDescending(b => b.ReleaseDate)
                    .Take(3)
                    .ToArray()
                })
                .ToArray();

            var output = new StringBuilder();

            foreach (var category in categories)
            {
                output.AppendLine($"--{category.Name}");

                foreach (var bookCategory in category.CategoryBooks)
                {
                    output.AppendLine($"{bookCategory.Title} ({bookCategory.ReleaseDate.Value.Year})");
                }
            }

            return output.ToString().TrimEnd();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            Book[] booksForDeletion = context.Books
                .Where(b => b.Copies < 4200)
                .ToArray();

            int numberOfDeletedBooks = booksForDeletion.Length;

            context.Books.RemoveRange(booksForDeletion);

            context.SaveChanges();

            return numberOfDeletedBooks;
        }
    }
}
