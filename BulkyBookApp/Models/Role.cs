using System.ComponentModel.DataAnnotations;

namespace BulkyBookApp.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        public string RoleName { get; set; } // e.g., "Admin" or "User"

        // Navigation property to link users to their roles
        public ICollection<Userclass> Users { get; set; }= new List<Userclass>();   


    }
}
