using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace dotNetShop.Models{
    public class Brand
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
        
        public ICollection<Product> Products { get; set; }
    }
}