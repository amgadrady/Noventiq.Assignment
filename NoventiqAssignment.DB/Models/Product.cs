
namespace NoventiqAssignment.DB.Models
{
    public class Product
    {
        public Product()
        {
            Translations = new HashSet<ProductTranslation>();
        }
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<ProductTranslation> Translations { get; set; }
    }
}
