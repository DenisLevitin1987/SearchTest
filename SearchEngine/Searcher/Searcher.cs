using System;
using System.Collections.Generic;
using System.Linq;
using SearchEngine.Models;
using SearchEngine.SearchExpressionParser;
using SearchEngine.Storage;

namespace SearchEngine.Searcher
{
    public class Searcher : ISearcher
    {
        private readonly ISearchQueryParser _searchParser;
        private readonly ILaptopDataProvider _dataStorage;

        public Searcher(ISearchQueryParser searchParser, ILaptopDataProvider dataStorage)
        {
            _searchParser = searchParser;
            _dataStorage = dataStorage;
        }

        private IReadOnlyDictionary<int, Laptop> FilterSimple(IReadOnlyDictionary<int, Laptop> laptops, Filter filter)
        {
            if (!filter.IsSimple)
            {
                throw new Exception("filter is not simple, but trying to use as simple");
            }

            return filter.Operator == Operator.None ? _dataStorage.GetEquals(filter.Equals) : _dataStorage.GetNotEquals(filter.Equals);
        }

        private IReadOnlyDictionary<int, Laptop> Search(IReadOnlyDictionary<int, Laptop> laptops, Filter filter)
        {
            if (filter.IsSimple)
            {
                return FilterSimple(laptops, filter);
            }
            else
            {
                if (filter.SubFilters.Length == 0)
                {
                    throw new Exception("incorrect filter");
                }

                switch (filter.Operator)
                {
                    case Operator.And:

                        foreach (var subFilter in filter.SubFilters)
                        {
                            laptops = Search(laptops, subFilter);
                        }

                        return laptops;
                    case Operator.Or:
                        var filteredSets = filter.SubFilters.Select(x => Search(laptops, x)).ToList();
                        return ProcessOrOperator(filteredSets);
                    default:
                        throw new Exception("Incorrect filter operator");
                }
            }
        }

        private IReadOnlyDictionary<int, Laptop> ProcessOrOperator(IReadOnlyCollection<IReadOnlyDictionary<int, Laptop>> filteredSets)
        {
            return filteredSets.SelectMany(x => x).Distinct().ToDictionary(x => x.Key, x => x.Value);
        }

        public SearcherResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new SearcherResult()
                {
                    Error = "the search query is empty"
                };
            }

            var filterParseResult = _searchParser.ParseSearchQuery(query);
            if (!string.IsNullOrWhiteSpace(filterParseResult.Error))
            {
                return new SearcherResult
                {
                    Laptops = null,
                    Error = filterParseResult.Error
                };
            }

            var laptops = _dataStorage.GetLaptops();

            return new SearcherResult
            {
                Laptops = Search(laptops, filterParseResult.Filter).Values.ToList()
            };
        }
    }
}
