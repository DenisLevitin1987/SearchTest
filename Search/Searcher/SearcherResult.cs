using System;
using System.Collections.Generic;
using System.Text;
using Search.Models;

namespace Search
{
    public class SearcherResult
    {
        public IReadOnlyCollection<Laptop> Laptops { get; set; }

        public string Error { get; set; }
    }
}
