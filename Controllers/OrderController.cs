using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospital.Data;
using Hospital.DTOs;
using Hospital.Models;

namespace Hospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly HospitalContext _context;

        public OrderController(HospitalContext context)
        {
            _context = context;
        }

        [HttpPost("place-order")]
        public async Task<ActionResult<Order>> PlaceOrder([FromBody] PlaceOrderRequest request)
        {
            try
            {
                var userEmail = HttpContext.Session.GetString("UserEmail");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new { Message = "User not authenticated" });
                }

                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == userEmail);
                if (patient == null)
                {
                    return Unauthorized(new { Message = "Patient not found" });
                }

                var inventory = await _context.Inventories.FindAsync(request.ProductId); // ProductId now refers to InventoryId
                if (inventory == null)
                {
                    return BadRequest(new { Message = "Inventory item not found" });
                }

                if (!inventory.IsAvailable || inventory.Quantity < request.Quantity)
                {
                    return BadRequest(new { Message = "Inventory item is not available or insufficient quantity" });
                }

                var totalAmount = inventory.Price * request.Quantity;

                var order = new Order
                {
                    PatientId = patient.Id,
                    ProductId = request.ProductId, // Still using ProductId field, but refers to InventoryId
                    Quantity = request.Quantity,
                    TotalAmount = totalAmount,
                    Status = "Pending",
                    Notes = request.Notes,
                    OrderDate = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };

                inventory.Quantity -= request.Quantity;
                if (inventory.Quantity == 0) inventory.IsAvailable = false;

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, new
                {
                    Id = order.Id,
                    InventoryName = inventory.Name,
                    Quantity = order.Quantity,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    Message = "Order placed successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while placing order", Error = ex.Message });
            }
        }

        [HttpGet("patient")]
        public async Task<ActionResult<IEnumerable<object>>> GetPatientOrders()
        {
            try
            {
                var userEmail = HttpContext.Session.GetString("UserEmail");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new { Message = "User not authenticated" });
                }

                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == userEmail);
                if (patient == null)
                {
                    return Unauthorized(new { Message = "Patient not found" });
                }

                var orders = await _context.Orders
                    .Where(o => o.PatientId == patient.Id)
                    .OrderByDescending(o => o.CreatedAt)
                    .Select(o => new
                    {
                        Id = o.Id,
                        InventoryName = _context.Inventories.Where(i => i.Id == o.ProductId).Select(i => i.Name).FirstOrDefault(),
                        Quantity = o.Quantity,
                        TotalAmount = o.TotalAmount,
                        Status = o.Status,
                        OrderDate = o.OrderDate,
                        DeliveryDate = o.DeliveryDate
                    })
                    .ToListAsync();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching patient orders", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetOrderById(int id)
        {
            try
            {
                var order = await _context.Orders
                    .Where(o => o.Id == id)
                    .Select(o => new
                    {
                        Id = o.Id,
                        PatientName = _context.Patients.Where(p => p.Id == o.PatientId).Select(p => p.Name).FirstOrDefault(),
                        PatientEmail = _context.Patients.Where(p => p.Id == o.PatientId).Select(p => p.Email).FirstOrDefault(),
                        InventoryName = _context.Inventories.Where(i => i.Id == o.ProductId).Select(i => i.Name).FirstOrDefault(),
                        InventoryCategory = _context.Inventories.Where(i => i.Id == o.ProductId).Select(i => i.Category).FirstOrDefault(),
                        Quantity = o.Quantity,
                        TotalAmount = o.TotalAmount,
                        Status = o.Status,
                        Notes = o.Notes,
                        OrderDate = o.OrderDate,
                        DeliveryDate = o.DeliveryDate
                    })
                    .FirstOrDefaultAsync();

                if (order == null)
                {
                    return NotFound(new { Message = "Order not found" });
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching order", Error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, [FromBody] UpdateOrderRequest request)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    return NotFound(new { Message = "Order not found" });
                }

                order.Status = request.Status;
                order.Notes = request.Notes;
                order.DeliveryDate = request.DeliveryDate;
                order.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { Message = "Order updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating order", Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    return NotFound(new { Message = "Order not found" });
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Order deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting order", Error = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllOrders()
        {
            try
            {
                var orders = await _context.Orders
                    .OrderByDescending(o => o.CreatedAt)
                    .Select(o => new
                    {
                        Id = o.Id,
                        PatientName = _context.Patients.Where(p => p.Id == o.PatientId).Select(p => p.Name).FirstOrDefault(),
                        PatientEmail = _context.Patients.Where(p => p.Id == o.PatientId).Select(p => p.Email).FirstOrDefault(),
                        InventoryName = _context.Inventories.Where(i => i.Id == o.ProductId).Select(i => i.Name).FirstOrDefault(),
                        Quantity = o.Quantity,
                        TotalAmount = o.TotalAmount,
                        Status = o.Status,
                        OrderDate = o.OrderDate,
                        DeliveryDate = o.DeliveryDate
                    })
                    .ToListAsync();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching orders", Error = ex.Message });
            }
        }
    }
} 