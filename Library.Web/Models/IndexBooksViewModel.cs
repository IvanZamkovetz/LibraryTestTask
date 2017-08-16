using Library.Model;
using PagedList;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models
{
    public class IndexBooksViewModel
    {
        public string Sort { get; set; }
        public string Filter { get; set; }
        public int Page { get; set; }

        public IPagedList<Book> Books { get; set; }
    }
}
