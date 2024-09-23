using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBookApp.Models
{
    public class Userclass
    {
        [Key]
        public int Id { get; set; }
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Phone   { get; set; }

        public ICollection<Role>Roles { get; set; }=new List<Role>();

    }
}
