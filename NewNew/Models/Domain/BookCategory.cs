
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNew.Models.Domain
{

    public class BookCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int bookCategroryId { get; set; } 
        public string categoryName { get; set; }
        public ICollection<Book> Books { get; set; }

		
	}
}
