
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNew.Models.Domain
{
    public class BookAuthor
    {
           [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int  bookId { get; set; }
        //public string authorFullName { get; set; }
        //public string bookTitle { get; set; }
        public int authorId { get; set; }
        public virtual Book Book { get; set; }
        public virtual Author Author { get; set; }



    }
}
