using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace dotNetShop.Models{

    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }
        
        public decimal? DiscountedPrice { get; set; }
        
        public int CategoryId { get; set; }
        
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public int BrandId { get; set; }
        
        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }
        

        public ICollection<ProductImage> Images { get; set; }
        
        public ICollection<Color> AvailableColors { get; set; }

        public ICollection<Comment> Comments{get;set;}
    }
}