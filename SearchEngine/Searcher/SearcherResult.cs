using System;
using System.Collections.Generic;
using System.Text;
using SearchEngine.Models;

namespace SearchEngine.Searcher
{
    public class SearcherResult
    {
        public IReadOnlyCollection<Laptop> Laptops { get; set; }

        public string Error { get; set; }
    }
}
