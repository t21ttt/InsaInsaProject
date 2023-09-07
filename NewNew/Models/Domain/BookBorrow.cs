
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNew.Models.Domain
{
    public class BookBorrow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int borrowBookId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
       

        public DateTime borrowBookDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
      

        public DateTime borrowDueDate { get; set; }
        public int? bookCopyId { get; set; }
        public BookCopy BookCopy { get; set; }
        public int?  memberId { get; set; }
        [NotMapped]
        public string memberFullName { get; set; }
        [NotMapped]
        public string bookTitle { get; set; }
        public Member Member { get; set; }
        public bool isReturned { get; set; }
        public bool isNotified { get; set; }


		public ICollection<Fine> fines { get; set; }
    }
    

}
