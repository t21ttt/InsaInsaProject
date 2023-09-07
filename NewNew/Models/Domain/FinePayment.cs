
using NewNew.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNew.Models.Domain

{
    public class FinePayment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int finePaymentId { get; set; }
        public int finePaymentAmount { get; set; }
        public DateTime finePaymentDate { get; set; }
        public int? memberId { get; set; }
        public Member Member { get; set; }
		public int FineId { get; set; }
		public Fine Fine { get; set; }
        public bool isPaid { get; set; }


	}
}

