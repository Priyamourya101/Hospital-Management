using System.ComponentModel.DataAnnotations;

namespace Hospital.DTOs
{
    public class PlaceOrderRequest
    {
        [Required(ErrorMessage = "Inventory ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Inventory ID must be a positive number")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }
    }

    public class UpdateOrderRequest
    {
        [Required(ErrorMessage = "Status is required")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        [RegularExpression(@"^(Pending|Confirmed|Processing|Shipped|Delivered|Cancelled)$", 
            ErrorMessage = "Status must be one of: Pending, Confirmed, Processing, Shipped, Delivered, Cancelled")]
        public string Status { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DeliveryDate { get; set; }
    }
} 