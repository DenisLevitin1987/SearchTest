namespace SearchWeb
{
    public class SearchResponse
    {
        public LaptopDTO[] Laptops { get; set; }
    }

    public class LaptopDTO
    {
        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Полная название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Аттрибуты из названия
        /// </summary>
        public string[] Attributes { get; set; }
    }
}