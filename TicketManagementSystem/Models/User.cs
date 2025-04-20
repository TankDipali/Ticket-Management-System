using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Models
{
    public class User
    {

        [Key]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public string Role { get; set; }
    }
}