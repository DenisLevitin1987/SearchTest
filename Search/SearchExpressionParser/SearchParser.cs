using System;
using System.Linq;

namespace Search.SearchExpressionParser
{
    public class SearchParser
    {
        private ParseFilterResult ParseSimpleFilter(string query)
        {
            var result = new Filter();

            var splited = query.Split("=", StringSplitOptions.RemoveEmptyEntries);
            if (splited.Length == 0)
            {
                splited = query.Split(new string[] {"not", "!"}, StringSplitOptions.RemoveEmptyEntries);
                if (splited.Length == 0)
                {
                    return new ParseFilterResult(null, $"incorrect parsing of simple filter, term is = {query}");
                }
                else
                {
                    result.Operator = Operator.Not;
                }
            }
            else
            {
                result.Operator = Operator.None;
            }

            if (splited.Length != 2)
            {
                return new ParseFilterResult(null, $"incorrect parsing of simple filter, term is {query}");
            }

            result.FieldName = splited[0].Trim();
            result.Equals = splited[1].Trim();

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
