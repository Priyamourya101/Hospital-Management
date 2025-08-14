using System.ComponentModel.DataAnnotations;

namespace Hospital.DTOs
{
    public class RegisterNurseRequest
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(255)]
        public string Phone { get; set; } = string.Empty;
        [Required]
        [StringLength(255)]
        public string Shift { get; set; } = string.Empty;
        [Required]
        [StringLength(255)]
        public string Department { get; set; } = string.Empty;
        [Required]
        [StringLength(255)]
        public string Qualification { get; set; } = string.Empty;
    }

    public class UpdateNurseRequest
    {
        [StringLength(255)]
        public string? Name { get; set; }
        [StringLength(255)]
        public string? Phone { get; set; }
        [StringLength(255)]
        public string? Shift { get; set; }
        [StringLength(255)]
        public string? Department { get; set; }
        [StringLength(255)]
        public string? Qualification { get; set; }
    }

    public class NurseResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
    }
}
