using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewNew.Models.Domain
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int adminId { get; set; }
        public string adminFullName { get; set; }
        public string email { get; set; }
        public string phonNumber { get; set; }
        public string gender { get; set; }
        public string password { get; set; }

      
    }
}
