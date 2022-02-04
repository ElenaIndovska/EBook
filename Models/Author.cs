using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EBook.Models
{
    public class Author
    {
		public int Id { get; set; }

		[Required]
		[StringLength(50)]
		public string Name { get; set; }


		[Display(Name = "Birth Date")]
		public DateTime BirthDate { get; set; }

		public ICollection<BookAuthor> Books { get; set; }
	}
}
