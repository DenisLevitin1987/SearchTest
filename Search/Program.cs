using System;
using System.Collections.Generic;
using System.Diagnostics;
using SearchEngine;
using SearchEngine.Models;
using SearchEngine.Searcher;
using SearchEngine.SearchExpressionParser;
using SearchEngine.Storage;

namespace Search
{
    class Program
    {
        private static void PrintResult(IReadOnlyCollection<Laptop> laptops)
        {
            foreach (var laptop in laptops)
            {
                Console.WriteLine($"{laptop.Id} {laptop.Name}");
            }
        }

        static void Main(string[] args)
        {
            var dataStorage = new LaptopDataProvider();
            var searchParser = new SearchQueryParser();
            var searcher = new Searcher(searchParser, dataStorage);
            
            while (true)
            {
                Console.WriteLine("type your query");
                var entered = Console.ReadLine();
                if (entered == "exit")
                {
                    break;
                }
                else
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    var result = searcher.Search(entered);
                    sw.Stop();
                    if (string.IsNullOrWhiteSpace(result.Error))
                    {
                        if (result.Laptops.Count > 0)
                        {
                            Console.WriteLine("your result: ");
                            PrintResult(result.Laptops);
                            Console.WriteLine($"search took {sw.ElapsedMilliseconds} ms");
                        }
                        else
                        {
                            Console.WriteLine("nothing found");
                        }
                    }
                    else
                    {
                        Console.WriteLine(result.Error);
                    }
                }
            }
        }
    }
}
