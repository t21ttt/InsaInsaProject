using NewNew.Models.Domain;

namespace NewNew.Models
{
    public class UpdateBookCategoryViewModel
    {
        public int bookCategroryId { get; set; }
        public string categoryName { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
