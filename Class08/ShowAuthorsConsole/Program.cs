using BooksProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowAuthorsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            BooksLoader loader = new BooksLoader();
            var authors = loader.GetAllAuthors();

            // Console.WriteLine(authors.Count());

            // Console.WriteLine(authors.Count(a => a.Books.Count() >=50));

            //var authorsResult = authors.Where(a =>
            //{
            //    Console.WriteLine($"Checking author {a.Name}");
            //    return a.Books.Count() >= 50;
            //});

            //foreach (var author in authorsResult)
            //{
            //    Console.WriteLine($"{author.Name}  has {author.Books.Count()} books");
            //    Console.ReadLine();
            //}
            //Console.WriteLine(authorsResult.Count());

            //   var authorCount = authors.Count(a => a.Books.Count() >= 50);
            var authorCount = authors.Where(a => a.Books.Count() >= 70);
            Console.WriteLine($"Authors with more than 50 books: {authorCount}");
            Console.WriteLine("--------------------------------------------------------------------------------------");

          //  authorCount = authors.Count(a => a.Books.Count(b => string.IsNullOrEmpty(b.Series)) >= 20);
            Console.WriteLine($"Authors with more than 20 non-series books: {authorCount}");

          //  authorCount = authors.Count(a => a.Books.Count(b => b.SeriesIndex == null) >= 20);
            Console.WriteLine($"Authors with more than 20 non-series books: {authorCount}");

            //Console.WriteLine(authors.Count(a => a.Books.Count(b => string.IsNullOrEmpty(b.Series)) >= 20));
            //Console.WriteLine(authors.Count(a => a.Books.Count(b => b.SeriesIndex == null) >= 20));

            var weirdBooks = from a in authors
                             from b in a.Books
                             where b.SeriesIndex == null
                                 && !string.IsNullOrEmpty(b.Series)
                             select b;
            //foreach (var book in weirdBooks)
            //{
            //    Console.WriteLine(book);
            //}

            var weirdBooks2 = authors.SelectMany(a => a.Books.Where(b => b.SeriesIndex == null && !string.IsNullOrEmpty(b.Series)));
            Console.WriteLine(weirdBooks2.Count());

            var allBooks = authors.SelectMany(a => a.Books);
            Console.WriteLine(allBooks.Count());

            Console.WriteLine(allBooks.Where(b => b.Title.StartsWith("X")).First());

            var allBooksWithAuthor = authors.SelectMany(a => a.Books.Select(b => new { b.ID, a.Name, b.Title }));
            Console.WriteLine(allBooksWithAuthor.Count());
            // We KNOW we will have at least one element
            Console.WriteLine(allBooksWithAuthor.First(b => b.Title.StartsWith("X")));
            // We DON'T KNOW if we will have at least one element
            Console.WriteLine(allBooksWithAuthor.FirstOrDefault(b => b.Title.StartsWith("XYZ")));
            // We KNOW we will have at least one element
            Console.WriteLine(allBooksWithAuthor.Last(b => b.Title.StartsWith("X")));
            // We DON'T KNOW if we will have at least one element
            Console.WriteLine(allBooksWithAuthor.LastOrDefault(b => b.Title.StartsWith("XYZ")));
            // We KNOW there will be exactly one element
            Console.WriteLine(allBooksWithAuthor.Single(b => b.ID == 1));
            // We KNOW there will be at most one element
            Console.WriteLine(allBooksWithAuthor.SingleOrDefault(b => b.ID == 40000));

            var allAuthorsByName = authors.OrderBy(a => a.Name);
            Console.WriteLine(allAuthorsByName.First());
            Console.WriteLine(allAuthorsByName.Last());

            var allAuthorsByBookCountDesc = authors
                .OrderByDescending(a => a.Books.Count())
                .ThenBy(a => a.Name);
            Console.WriteLine(allAuthorsByBookCountDesc.First());
            Console.WriteLine(allAuthorsByBookCountDesc.Last());

            // count of authors with the specied number of books
            //Console.WriteLine(authors.Count(a => a.Books.Count() == 1));
            //Console.WriteLine(authors.Count(a => a.Books.Count() == 218));

            var nonSeries = authors.Select(a => new
            {
                a.ID,
                a.Name,
                NonSeriesCount = a.Books.Count(b => string.IsNullOrEmpty(b.Series))
            }).OrderByDescending(a => a.NonSeriesCount);

            Console.WriteLine(nonSeries.First());
            Console.WriteLine(nonSeries.Skip(1).First());
            Console.WriteLine(nonSeries.Skip(2).First());

            // positions 11 to 20
            var nonSeries11to20 = nonSeries.Skip(10).Take(10);
            PrintAuthors(nonSeries11to20);

            var nonSeriesBetween30and40 = nonSeries
                .SkipWhile(a => a.NonSeriesCount > 40)
                .TakeWhile(a => a.NonSeriesCount > 30);
            PrintAuthors(nonSeriesBetween30and40);

            var nonSeriesMax = authors.Select(a => new
            {
                a.ID,
                a.Name,
                NonSeriesCount = a.Books.Count(b => string.IsNullOrEmpty(b.Series))
            }).Max(a => a.NonSeriesCount);

            Console.WriteLine(nonSeriesMax);

            // Grouping

            var booksByLetter = authors
                .SelectMany(a => a.Books)
                .GroupBy(b => b.Title.First());

            Console.WriteLine(booksByLetter.Count());

            var firstGrouping = booksByLetter.First();
            //Console.WriteLine($"{firstGrouping.Key}: {firstGrouping.Count()}");
            //PrintAuthors(firstGrouping);

            foreach (var group in booksByLetter)
            {
                Console.WriteLine($"{group.Key}: {group.Count()}");
            }
            Console.WriteLine("---------------");

            var letterWithMostBooks = booksByLetter.OrderByDescending(g => g.Count()).First();
            Console.WriteLine($"{letterWithMostBooks.Key}: {letterWithMostBooks.Count()}");
            Console.WriteLine("---------------");

            //var lastGrouping = booksByLetter.Last();
            //PrintAuthors(lastGrouping);

            var authorsByLetter = authors
                .GroupBy(a => a.Name.First())
                .Select(g => new { g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count);

            var letterWithMostAuthors = authorsByLetter.First();
            Console.WriteLine($"{letterWithMostAuthors.Key}: {letterWithMostAuthors.Count}");
            Console.WriteLine("---------------");
            foreach (var group in authorsByLetter)
            {
                Console.WriteLine($"{group.Key}: {group.Count}");
            }
            Console.WriteLine("---------------");
            
            Console.WriteLine(authors.Single(a => a.Name.StartsWith("X")));

            Console.WriteLine(authors.Any(a => a.Books.Count() == 72));
            Console.WriteLine(authors.Any(a => a.Books.Count() == 222));

            Console.WriteLine(authors.All(a => a.Books.Count() > 0));
            Console.WriteLine(authors.All(a => a.Books.Count() > 1));

            Console.WriteLine("-----------------HOMEWORK--------------------------");

            //1 
            // What is the average number of books per autors?
            var numOfBooksPerAuthors = (authors.SelectMany(a => a.Books).Count()) / (authors.Select(a => a.Books).Count());
            Console.WriteLine(numOfBooksPerAuthors);
            
            // 2
            //Which book(s) has the longest title, and how long is it?
            var longestTitle = allBooks.OrderByDescending(t => t.Title.Length).First();
            var longestTitleNum = allBooks.Max(t => t.Title.Length);
            Console.WriteLine($"Longest title is {longestTitle} and has {longestTitleNum} letters");

            // 3
            //  Which author has the shortest average title for a book?           
            var shortestAvgTitle = authors.Select(a => new { a.Name, shortesAvg = a.Books.Average(b => b.Title.Length) }).OrderBy(a => a.shortesAvg).First();
            Console.WriteLine(shortestAvgTitle);

           //4 
           // Which author has the shortest average title for a book? (Discount authors with less than three books)
            var lessThan3 = authors.Where(a => a.Books.Count() > 3)
                                   .Select(a => new { a.Name, shortesAvg = a.Books.Average(b => b.Title.Length) })
                                   .OrderBy(a => a.shortesAvg).First();
            Console.WriteLine(lessThan3);

            // 5
            //  What series has the most books?
            var allSeries = allBooks.Where(s => !string.IsNullOrEmpty(s.Series)).GroupBy(s => s.Series).OrderByDescending(s => s.Count()).First();
            Console.WriteLine($"{allSeries.Key} is series with {allSeries.Count()} books");


            // 6
            // Which year has the most books published?
            var booksPerYer = allBooks.GroupBy(y => y.Year)
                                      .Select(b => new { b.Key, Count = b.Count() })
                                      .OrderByDescending(b => b.Count);
            Console.WriteLine($"The most books are published in: {booksPerYer.First()}");


            // 7
            // What is the average number of books published for years in the 21st centrury ? (Starting with 2001, not 2000)
            var avgCentury21 = authors.SelectMany(a => a.Books).Where(b => b.Year > 2000).GroupBy(b => b.Year).Average(c => c.Count());
            Console.WriteLine($"The average number of books published in 21st century is {avgCentury21}");


            // Which author has the most different series?
            var authorWithDiffSeries = authors.Select(a => new { a.Name, seriesNumber = a.Books.GroupBy(b => b.Series).Count()})
                                              .OrderByDescending(b => b.seriesNumber).ThenBy(n => n.Name).ToList();
            Console.WriteLine(authorWithDiffSeries.First().Name);


            // Which author has the most books written that belong to a series?
            var authorWithmostSeries = authors.Select(a => new { a.Name, NumOfSeries = a.Books.Where(b => !string.IsNullOrEmpty(b.Series)).Count()})
                                              .OrderByDescending(a => a.NumOfSeries)
                                              .First();
            Console.WriteLine($"Author with most series is: {authorWithmostSeries.Name} ({authorWithmostSeries.NumOfSeries})");


            // Which author has the longest career?
            var longestCarrier = authors.Select(a => new { a.Name, YearOfCarrer = a.Books.Select(b => b.Year).Max() - a.Books.Select(b => b.Year).Min()})    
                                        .OrderByDescending(y => y.YearOfCarrer)
                                        .First();
            Console.WriteLine($"{longestCarrier.Name} has the longest career of {longestCarrier.YearOfCarrer}");


        }

        static void PrintAuthors<T>(IEnumerable<T> authors)
        {
            foreach (var author in authors)
            {
                Console.WriteLine(author);
            }
            Console.WriteLine("---------------");
        }
    }
}
