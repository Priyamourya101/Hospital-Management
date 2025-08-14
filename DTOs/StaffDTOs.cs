using System.ComponentModel.DataAnnotations;

namespace Hospital.DTOs
{
    public class RegisterStaffRequest
    {
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
        public string? EmergencyContact { get; set; }
        public string? EmergencyPhone { get; set; }
        [Required]
        public long UserId { get; set; }
    }

    public class UpdateStaffRequest
    {
        [StringLength(255)]
        public string? FirstName { get; set; }
        [StringLength(255)]
        public string? LastName { get; set; }
        [StringLength(255)]
        public string? PhoneNumber { get; set; }
        [StringLength(255)]
        public string? Address { get; set; }
        [StringLength(255)]
        public string? City { get; set; }
        [StringLength(255)]
        public string? State { get; set; }
        [StringLength(255)]
        public string? Department { get; set; }
        [StringLength(255)]
        public string? Role { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? HireDate { get; set; }
        public double? Salary { get; set; }
        public string? EmergencyContact { get; set; }
        public string? EmergencyPhone { get; set; }
    }

    public class StaffResponse
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public double Salary { get; set; }
        public string? EmergencyContact { get; set; }
        public string? EmergencyPhone { get; set; }
        public long UserId { get; set; }
    }
}
