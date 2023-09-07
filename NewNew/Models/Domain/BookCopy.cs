using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace NewNew.Models.Domain
{
    public class BookCopy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int bookCopyId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
       

        public DateTime yearOfPublished { get; set; }
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }
        public int bookId { get; set; }
        public Book Book { get; set; }
        [NotMapped]
        public string publisherName { get; set; }
        [NotMapped]
        public string bookTitle { get; set; }
        public ICollection<BookBorrow> bookBorrows { get; set; }
    }
}
