using System;
using System.Collections.Generic;
using System.Linq;
using Search.Models;
using Search.SearchExpressionParser;

namespace Search
{
    public class Searcher
    {
        private readonly SearchQueryParser _searchParser;
        private readonly DataStorage _dataStorage;

        public Searcher(SearchQueryParser searchParser, DataStorage dataStorage)
        {
            _searchParser = searchParser;
            _dataStorage = dataStorage;
        }

        public IReadOnlyDictionary<int, Laptop> GetEquals(IReadOnlyDictionary<int, Laptop> laptops, string equal)
        {
            if (_dataStorage.GetCache().TryGetValue(equal, out var ids))
            {
                var res = new Dictionary<int, Laptop>();
                foreach (var id in ids)
                {
                    if (laptops.TryGetValue(id, out var laptop))
                    {
                        res.Add(id, laptop);
                    }
                }

                return res;
            }
            else
            {
                return new Dictionary<int, Laptop>();
            }
        }

        public IReadOnlyDictionary<int, Laptop> GetNotEquals(IReadOnlyDictionary<int, Laptop> laptops, string equal)
        {
            var res = new Dictionary<int, Laptop>();
            if (_dataStorage.GetCache().TryGetValue(equal, out var ids))
            {
                foreach (var laptop in laptops)
                {
                    if (!ids.Contains(laptop.Key))
                    {
                        res.Add(laptop.Key, laptop.Value);
                    }
                }

                return res;
            }
            else
            {
                return laptops;
            }
        }

        private IReadOnlyDictionary<int, Laptop> FilterSimple(IReadOnlyDictionary<int, Laptop> laptops, Filter filter)
        {
            if (!filter.IsSimple)
            {
                throw new Exception("filter is not simple, but trying to use as simple");
            }

            return filter.Operator == Operator.None ? GetEquals(laptops, filter.Equals) : GetNotEquals(laptops, filter.Equals);
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
