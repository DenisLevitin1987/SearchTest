using Search.SearchExpressionParser;

namespace Search
{
    public class Filter
    {
        public Operator Operator { get; set; }

        public string Equals { get; set; }

        public Filter[] SubFilters { get; set; }

        public bool IsSimple
        {
            get { return (SubFilters == null || SubFilters.Length == 0) && (Operator == Operator.None || Operator == Operator.Not); }
        }
    }
}
