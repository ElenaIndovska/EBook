using EBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBook.ViewModels
{
    public class BookViewModel
    {
        public IList<Book> Books { get; set; }
        public SelectList Genres { get; set; }
        public string BookGenre { get; set; }
        public string SearchString { get; set; }
    }
}
