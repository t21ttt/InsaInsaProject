using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNew.Models.Domain
{
    public class Fine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public int FineId { get; set; }

        public int FineAmount { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
       

        public DateTime FineDate { get; set; }

        public int? borrowBookId { get; set; }

        public BookBorrow bookBorrow { get; set; }
        public string bookTitle { get; set; }
        public FinePayment FinePayment { get; set; }

        [NotMapped]
        public virtual (int?, DateTime) UniqueKey => (borrowBookId, FineDate);
    }
}
