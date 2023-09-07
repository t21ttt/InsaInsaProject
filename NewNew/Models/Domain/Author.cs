
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNew.Models.Domain
{

    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int authorId { get; set; }
        public string authorFullName { get; set; }
        public string email { get; set; }
        // other properties as needed
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
