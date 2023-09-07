using NewNew.Models.Domain;

namespace NewNew.Models
{
    public class UpdateFineViewModel
    {
        public int FineId { get; set; }
        public int FineAmount { get; set; }
        public DateTime FineDate { get; set; }
        public int? borrowBookId { get; set; }
        public BookBorrow BookBorrow { get; set; }
        public FinePayment FinePayment { get; set; }
    }
}
