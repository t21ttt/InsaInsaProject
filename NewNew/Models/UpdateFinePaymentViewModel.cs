using NewNew.Models.Domain;

namespace NewNew.Models
{
    public class UpdateFinePaymentViewModel
    {
        public int finePaymentId { get; set; }
        public int finePaymentAmount { get; set; }
        public DateTime finePaymentDate { get; set; }
        public int? memberId { get; set; }
        public Member Member { get; set; }
        public int FineId { get; set; }
        public Fine Fine { get; set; }
    }
}
