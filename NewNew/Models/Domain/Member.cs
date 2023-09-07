
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNew.Models.Domain
{
    public class Member
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int memberId { get; set; }
        public string memberFullName { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string memberPassword { get; set; }
        public MemberStatus Status { get; set; }
        public virtual ICollection<BookBorrow> Borrows { get; set; }
        public ICollection<Reservation> reservations { get; set; }
        public ICollection<FinePayment> finePayments { get; set; }
    }
}


public enum MemberStatus
{
    Active,
    Blocked
}


