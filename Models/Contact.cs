using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotNetShop.Models
{
	public class Contact
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[MaxLength(255)]
		public string Address { get; set; }

		[MaxLength(32)]
		public string Phone { get; set; }

		[MaxLength(255)]
		public string Email { get; set; }
		public ICollection<Contact> Contacts { get; set; }
	}
}
