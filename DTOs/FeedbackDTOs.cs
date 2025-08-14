using System.ComponentModel.DataAnnotations;

namespace Hospital.DTOs
{
    public class SubmitFeedbackRequest
    {
        [Required(ErrorMessage = "Subject is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Subject must be between 5 and 100 characters")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Message is required")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Message must be between 10 and 1000 characters")]
        public string Message { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }
    }
} 