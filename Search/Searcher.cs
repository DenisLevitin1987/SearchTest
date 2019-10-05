using System;
using System.Collections.Generic;
using System.Linq;
using Search.Models;
using Search.SearchExpressionParser;

namespace Search
{
    public class Searcher
    {
        private readonly SearchParser _searchParser = new SearchParser();

        private IReadOnlyDictionary<int, Laptop> FilterSimple(Filter filter)
        {
            if (!filter.IsSimple)
            {
                throw new Exception("filter is not simple, but trying to use as simple");
            }

            var dict = DataStorage.GetDictionaryByFieldName(filter.FieldName);
            var query = filter.Operator == Operator.None
                    ? dict.Where(x => x.Equals(filter.Equals))
                    : dict.Where(x => !x.Equals(filter.Equals));

            return query.ToDictionary(x => x.Value.Id, x => x.Value);
        }

        private IReadOnlyDictionary<int, Laptop> Search(IReadOnlyCollection<Laptop> laptops, Filter filter)
        {
            if (filter.IsSimple)
            {
                var filterSimpledResult = FilterSimple(filter);
                return laptops.Where(x => filterSimpledResult.ContainsKey(x.Id)).ToDictionary(x => x.Id, x => x);
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
                            laptops = Search(laptops, subFilter).Values.ToList();
                        }

                        return laptops.ToDictionary(x => x.Id, x=>x);
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

            var laptops = DataStorage.GetLaptops();

            return new SearcherResult
            {
                Laptops = Search(laptops, filterParseResult.Filter).Values.ToList()
            };
        }
    }
}
