
namespace NoventiqAssignment.DB.Models
{
    public class ProductTranslation
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public required string LanguageCode { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        public Product Product { get; set; }
    }
}
