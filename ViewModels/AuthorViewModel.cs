using EBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBook.ViewModels
{
    public class AuthorViewModel
    {
        public IList<Author> Authors { get; set; }
        public string SearchString { get; set; }
    }
}
