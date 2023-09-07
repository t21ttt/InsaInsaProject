using NewNew.Models.Domain;

namespace NewNew.Models
{
    public class UpdateReservationViewModel
    {
        public int ReservationId { get; set; }
        public int? memberId { get; set; }
        public Member Member { get; set; }
        public int? bookId { get; set; }
        public Book Book { get; set; }
        public DateTime reservatonDate { get; set; }
    }
}
