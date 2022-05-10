namespace SearchEngine.SearchExpressionParser
{
    public interface ISearchQueryParser
    {
        ParseFilterResult ParseSearchQuery(string query);
    }
}