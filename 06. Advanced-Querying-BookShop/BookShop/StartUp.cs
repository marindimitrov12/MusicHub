namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            // DbInitializer.ResetDatabase(db);
            //Console.WriteLine(GetBooksByAgeRestriction(db, "Teen"));
            //Console.WriteLine(GetGoldenBooks(db));
            //Console.WriteLine(GetBooksByPrice(db));
            // Console.WriteLine(GetBooksNotReleasedIn(db,2000));
            // Console.WriteLine(GetBooksByCategory(db, "horror mystery drama"));
            Console.WriteLine(GetBooksReleasedBefore(db, "12-04-1992"));
        }
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var com=(AgeRestriction)Enum.Parse(typeof(AgeRestriction), command);
            var result = context.Books
                .Where(x => x.AgeRestriction == com)
                .OrderBy(x=>x.Title);
            var str = new StringBuilder();
            foreach (var x in result)
            {
                str.AppendLine(x.Title);
            }
            return str.ToString().TrimEnd();
        }
        public static string GetGoldenBooks(BookShopContext context)
        {
            var result = context.Books
                .Where(x => x.EditionType == (EditionType)Enum.Parse(typeof(EditionType), "Gold") && x.Copies < 5000)
                .OrderBy(x => x.BookId);
            var str = new StringBuilder();
            foreach (var x in result) {
                str.AppendLine(x.Title);
            }
            return str.ToString().TrimEnd();    
        }
        public static string GetBooksByPrice(BookShopContext context)
        {
            var result = context.Books
                .Where(x => x.Price > 40)
                .Select(x => new
                {
                    Title = x.Title,
                    Price = x.Price
                }).OrderByDescending(x=>x.Price).ToArray();
            var str = new StringBuilder();
            foreach (var x in result)
            {
                str.AppendLine($"{x.Title} - {x.Price}");
            }
            return str .ToString().TrimEnd();
        }
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var result = context.Books
                .Where(x => x.ReleaseDate.Value.Year != year)
                .OrderBy(x => x.BookId)
                .Select(x => new
                {
                    Title=x.Title,
                })
                .ToArray();
            var str = new StringBuilder();
            foreach (var x in result)
            {
                str.AppendLine(x.Title);
            }
            return str.ToString();

        }
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
           
            var catecories = input.Split(" ");
            var str = new StringBuilder();
           
                var r = context.BooksCategories
                    .Where(x => catecories.Contains(x.Category.Name))
                    .Select(x => new
                {
                    Title=x.Book.Title,
                }).OrderBy(x=>x.Title).ToList();
                foreach (var i in r)
                {
                    str.AppendLine(i.Title);
                }
            return str.ToString();
            
           
            
        }
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var str = new StringBuilder();
            var result = context.Books
                .Where(x=>x.ReleaseDate<DateTime.Parse(date))
                .OrderByDescending(x=>x.ReleaseDate)
                .Select(x => new
                {
                    Title=x.Title,
                    EditionType=x.EditionType,
                    Price=x.Price
                }).ToList();
            foreach (var x in result)
            {
                str.AppendLine($"{x.Title} - {x.EditionType} -{x.Price}");
            }
            return str.ToString();
        }
    }
}
