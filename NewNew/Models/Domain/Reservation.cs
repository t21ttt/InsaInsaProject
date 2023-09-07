
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNew.Models.Domain

{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationId { get; set; }
        public int? memberId { get; set; }
        public Member Member { get; set; }
        public int? bookId { get; set; }
        public Book Book { get; set; }
        [NotMapped]
        public string bookTitle { get; set; }
        [NotMapped]
        public string memberFullName { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
      

        public DateTime reservatonDate { get; set; }
        
            
    }
}