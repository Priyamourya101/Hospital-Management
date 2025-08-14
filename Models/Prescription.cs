using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Models
{
    public class Prescription
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        public int DoctorId { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Diagnosis { get; set; } = string.Empty;
        
        [Required]
        [StringLength(1000)]
        public string Medications { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Instructions { get; set; } = string.Empty;
        
        public DateTime PrescriptionDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? NextVisitDate { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; } = null!;
        
        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; } = null!;
    }
} 