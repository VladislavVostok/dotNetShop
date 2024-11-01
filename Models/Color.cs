using System.ComponentModel.DataAnnotations;


namespace dotNetShop.Models
{


    public class Color
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public string HexCode { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}