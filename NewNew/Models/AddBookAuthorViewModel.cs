using NewNew.Models.Domain;

namespace NewNew.Models
{
    public class AddBookAuthorViewModel
    {
      
        public int bookId { get; set; }
        public int authorId { get; set; }

        public virtual Book Book { get; set; }
        public virtual Author Author { get; set; }
    }
}
