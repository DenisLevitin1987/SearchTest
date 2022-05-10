using System.Collections.Generic;
using SearchEngine.Models;

namespace SearchEngine.Searcher
{
    public interface ISearcher
    {
        SearcherResult Search(string query);
    }
}