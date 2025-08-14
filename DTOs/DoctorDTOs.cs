using System.ComponentModel.DataAnnotations;

namespace Hospital.DTOs
{
    public class RegisterDoctorRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
     ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
        public string Password { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "Phone number must be between 10 and 20 characters")]
        [RegularExpression(@"^[\+]?[0-9\s\-\(\)]+$", ErrorMessage = "Invalid phone number format")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialization is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Specialization must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Specialization can only contain letters and spaces")]
        public string Specialization { get; set; } = string.Empty;

        [Required(ErrorMessage = "Qualification is required")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Qualification must be between 5 and 500 characters")]
        public string Qualification { get; set; } = string.Empty;

        [Range(0, 50, ErrorMessage = "Experience must be between 0 and 50 years")]
        public int Experience { get; set; }
    }

    public class UpdateDoctorRequest
    {
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces")]
        public string? Name { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "Phone number must be between 10 and 20 characters")]
        [RegularExpression(@"^[\+]?[0-9\s\-\(\)]+$", ErrorMessage = "Invalid phone number format")]
        public string? Phone { get; set; }

        [StringLength(100, MinimumLength = 2, ErrorMessage = "Specialization must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Specialization can only contain letters and spaces")]
        public string? Specialization { get; set; }

        [StringLength(500, MinimumLength = 5, ErrorMessage = "Qualification must be between 5 and 500 characters")]
        public string? Qualification { get; set; }

        [Range(0, 50, ErrorMessage = "Experience must be between 0 and 50 years")]
        public int Experience { get; set; }

        public bool IsAvailable { get; set; }
    }

    public class IssuePrescriptionRequest
    {
        [Required(ErrorMessage = "Patient ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive number")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Doctor ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Doctor ID must be a positive number")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Diagnosis is required")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Diagnosis must be between 5 and 500 characters")]
        public string Diagnosis { get; set; } = string.Empty;

        [Required(ErrorMessage = "Medications are required")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "Medications must be between 5 and 1000 characters")]
        public string Medications { get; set; } = string.Empty;

        [Required(ErrorMessage = "Instructions are required")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "Instructions must be between 5 and 1000 characters")]
        public string Instructions { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? NextVisitDate { get; set; }
    }
} 