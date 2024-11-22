using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotNetShop.Models
{
	public class FormContactMessage
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[MaxLength(32)]
		public string Name { get; set; }
		[Required]
		[MaxLength(255)]
		public string Email { get; set; }
		[Required]
		[MaxLength(155)]
		public string Subject { get; set; }
		[Required]
		[MaxLength(2048)]
		public string Message { get; set; }

		public bool IsModeration { get; set; } = false;
	}
}
