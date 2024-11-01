using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace dotNetShop.Models{

    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }
        
        public ICollection<Product> Products { get; set; }
    }
}