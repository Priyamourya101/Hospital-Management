using System.ComponentModel.DataAnnotations;

namespace Hospital.Models
{
    public class Nurse
    {
        [Key]
        public long Id { get; set; }

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
        public string Shift { get; set; } = string.Empty; // Morning, Evening, Night

        [Required]
        [StringLength(255)]
        public string Department { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Qualification { get; set; } = string.Empty;
    }
}
