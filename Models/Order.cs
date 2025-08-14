using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        public int ProductId { get; set; } // Now refers to InventoryId
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public decimal TotalAmount { get; set; }
        
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Shipped, Delivered, Cancelled
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? DeliveryDate { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; } = null!;
        
        // [ForeignKey("ProductId")]
        // public virtual Product Product { get; set; } = null!; // Removed
    }
} 