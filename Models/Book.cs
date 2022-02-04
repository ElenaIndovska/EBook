using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EBook.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(30)]
        public string Genre { get; set; }

        public int? Year { get; set; }

        [Display(Name = "Image")]
        public string ImgFile { get; set; }

        public ICollection<BookAuthor> Authors { get; set; }
    }
}
