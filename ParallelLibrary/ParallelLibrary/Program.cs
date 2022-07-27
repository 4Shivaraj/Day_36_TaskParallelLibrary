using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaskParallelLibraryDemo
{
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome To Task Parallel Library Problem");
            //retrieve from gutenberg.org
            string[] words = CreateWordArray(@"http://www.gutenberg.org/files/54700/54700-0.txt");

            #region ParallelTasks

            Parallel.Invoke(() =>
            {
                Console.WriteLine("Begin first task...");
                GetLongestWord(words);
            },
            () =>
            {
                Console.WriteLine("Begin second task...");
                GetMostCommomWords(words);
            }, //close second aaction

            () =>
            {
                Console.WriteLine("Begin third task...");
                GetCountForWord(words, "sleep");
            } //close third action
            ); //close parallel.invoke
            #endregion
        }
        static string[] CreateWordArray(string url)
        {
            Console.WriteLine($"Retrieving from {url}");

            //download web page
            string blog = new WebClient().DownloadString(url);

            //separate string into an array of words removing some punctuation
            //\u000A it is unicode character
            return blog.Split(
                new char[] { ' ','\u000A',',', '.', ';', ':', '_', '-', '/' },
                StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Gets the longest word.
        /// </summary>
        /// <param name="words">The words.</param>
        /// <returns></returns>
        private static string GetLongestWord(string[] words)
        {
            var longestWord = (from w in words
                              orderby w.Length descending
                              select w).First();

            Console.WriteLine($"Task 1 - The longest word is {longestWord}.");
            return longestWord;
        }
        /// <summary>
        /// Gets the most commom words.
        /// </summary>
        /// <param name="words">The words.</param>
        private static void GetMostCommomWords(string[] words)
        {
            var FrequencyOrder = from word in words
                                 where word.Length > 6
                                 group word by word into g
                                 orderby g.Count() descending
                                 select g.Key;

            var commonwords = FrequencyOrder.Take(10);

            StringBuilder sb = new StringBuilder();  //StringBuilder represents a mutable sequence of characters.
                                                     //Since the String Class creates an immutable sequence of characters,
                                                     //the StringBuilder class provides an alternative to String Class,
                                                     //as it creates a mutable sequence of characters
            sb.AppendLine("Task 2 - The most common words are: ");
            foreach (var v in commonwords)
            {
                sb.AppendLine(" " + v);

            }
            Console.WriteLine(sb.ToString());
        }
        /// <summary>
        /// Gets the count for word.
        /// </summary>
        /// <param name="words">The words.</param>
        /// <param name="term">The term.</param>
        private static void GetCountForWord(string[] words, string term)
        {
            var findWord = from word in words
                           where word.ToUpper().Contains(term.ToUpper())
                           select word;

            Console.WriteLine($@"Task 3 The word  ""{term}"" occurs{findWord.Count()} times.");
        }
    }
}

/*
 Welcome To Task Parallel Library Problem
Retrieving from http://www.gutenberg.org/files/54700/54700-0.txt
Begin third task...
Begin second task...
Begin first task...
Task 1 - The longest word is incomprehensible.
Task 2 - The most common words are:
 Oblomov
 himself
 Schtoltz
 Gutenberg
 Project
 another
 thought
 Oblomov's
 nothing
 replied

Task 3 The word  "sleep" occurs57 times.
 */