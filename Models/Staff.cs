using System.ComponentModel.DataAnnotations;

namespace Hospital.Models
{
    public class Staff
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(255)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string State { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Department { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Role { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        [Required]
        public double Salary { get; set; }

        [StringLength(255)]
        public string? EmergencyContact { get; set; }

        [StringLength(255)]
        public string? EmergencyPhone { get; set; }

        [Required]
        public long UserId { get; set; }
    }
}
