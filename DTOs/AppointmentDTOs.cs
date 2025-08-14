using System.ComponentModel.DataAnnotations;

namespace Hospital.DTOs
{
    public class CreateAppointmentRequest
    {
        [Required(ErrorMessage = "Doctor ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Doctor ID must be a positive number")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Appointment date is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Time slot is required")]
        [StringLength(20, ErrorMessage = "Time slot cannot exceed 20 characters")]
        [RegularExpression(@"^(09:00|10:00|11:00|12:00|14:00|15:00|16:00|17:00)$", 
            ErrorMessage = "Time slot must be one of: 09:00, 10:00, 11:00, 12:00, 14:00, 15:00, 16:00, 17:00")]
        public string TimeSlot { get; set; } = string.Empty;

        [StringLength(500, MinimumLength = 10, ErrorMessage = "Symptoms must be between 10 and 500 characters")]
        public string Symptoms { get; set; } = string.Empty;
    }

    public class UpdateAppointmentStatusRequest
    {
        [Required(ErrorMessage = "Appointment ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Appointment ID must be a positive number")]
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        [RegularExpression(@"^(Scheduled|Confirmed|Completed|Cancelled|No-Show)$", 
            ErrorMessage = "Status must be one of: Scheduled, Confirmed, Completed, Cancelled, No-Show")]
        public string Status { get; set; } = string.Empty;

        [StringLength(500, MinimumLength = 5, ErrorMessage = "Doctor notes must be between 5 and 500 characters")]
        public string? DoctorNotes { get; set; }
    }

    public class AppointmentResponse
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
        public string Symptoms { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? DoctorNotes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 