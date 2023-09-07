using NewNew.Models.Domain;

namespace NewNew.Models
{
    public class UpdateAuthorViewModel
    {
        public int authorId { get; set; }
        public string authorFullName { get; set; }
        public string email { get; set; }
        // other properties as needed
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
