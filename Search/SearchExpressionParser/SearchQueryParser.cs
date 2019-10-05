using System;
using System.Linq;

namespace Search.SearchExpressionParser
{
    public class SearchQueryParser
    {
        private ParseFilterResult ParseSimpleFilter(string query)
        {
            var result = new Filter();

            if (query.Split(" ", StringSplitOptions.RemoveEmptyEntries).Length > 2)
            {
                return new ParseFilterResult(null, $"incorrect filter parsing, query={query}");
            }

            if (query.IndexOf('!') > -1 || query.IndexOf("not") > -1)
            {
                result.Operator = Operator.Not;
                result.Equals = query.Replace("!", "").Replace(" not ", " ");
            }
            else
            {
                result.Equals = query.Trim();
            }

            return new ParseFilterResult(result, null);
        }

        public ParseFilterResult ParseSearchQuery(string query)
        {
            var filter = new Filter();
            query = query.ToLower();

            /// TODO: Парсинг скобочек

            var splitedByOr = query.Split(new string[] {"or", "|"}, StringSplitOptions.RemoveEmptyEntries);
            if (splitedByOr.Length > 1)
            {
                filter.Operator = Operator.Or;
                var parsedResult = splitedByOr.Select(x => ParseSearchQuery(x.Trim())).ToArray();
                var parsedWithError = parsedResult.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Error));
                if (!string.IsNullOrWhiteSpace(parsedWithError?.Error))
                {
                    return new ParseFilterResult(null, parsedWithError.Error);
                }

                filter.SubFilters = parsedResult.Select(x => x.Filter).ToArray();
            }
            else
            {
                var splitedByAnd = query.Split(new string[] {"and", "&"}, StringSplitOptions.RemoveEmptyEntries);
                if (splitedByAnd.Length > 1)
                {
                    filter.Operator = Operator.And;
                    var parsedResult = splitedByAnd.Select(x => ParseSearchQuery(x.Trim())).ToArray();
                    var parsedWithError = parsedResult.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Error));
                    if (!string.IsNullOrWhiteSpace(parsedWithError?.Error))
                    {
                        return new ParseFilterResult(null, parsedWithError.Error);
                    }

                    filter.SubFilters = parsedResult.Select(x => x.Filter).ToArray();
                }
                else
                {
                    var parseSimpleResult = ParseSimpleFilter(query);
                    if (string.IsNullOrWhiteSpace(parseSimpleResult.Error))
                    {
                        filter = parseSimpleResult.Filter;
                    }
                    else
                    {
                        return new ParseFilterResult(null, parseSimpleResult.Error);
                    }
                }
            }

            return new ParseFilterResult(filter, null);
        }
    }
}
