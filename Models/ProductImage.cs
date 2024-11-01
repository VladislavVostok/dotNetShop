using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace dotNetShop.Models{
    
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Url { get; set; }
        
        public int ProductId { get; set; }
        
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}