using System;
using System.Collections.Generic;
using System.Text;

namespace Search.SearchExpressionParser
{
    public class ParseFilterResult
    {
        public Filter Filter { get; set; }

        public string Error { get; set; }

        public ParseFilterResult(Filter filter, string error)
        {
            Error = error;
            Filter = filter;
        }
    }
}
