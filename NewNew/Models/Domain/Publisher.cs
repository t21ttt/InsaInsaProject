
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNew.Models.Domain

{
    public class Publisher {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PublisherId { get; set; }
        public string publisherName { get; set; }
        public string publisherAddress { get; set; }
        public ICollection<BookCopy> copies { get; set; }

		
	}
}
