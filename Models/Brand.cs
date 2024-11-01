using System.ComponentModel.DataAnnotations;


namespace dotNetShop.Models{
    public class Brand
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
        
        public ICollection<Product> Products { get; set; }
    }
}