using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospital.Data;
using Hospital.DTOs;
using Hospital.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly HospitalContext _context;

        public InventoryController(HospitalContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Inventory>> AddInventory([FromBody] AddInventoryRequest request)
        {
            try
            {
                var inventory = new Inventory
                {
                    Name = request.Name,
                    Description = request.Description,
                    Quantity = request.Quantity,
                    Price = request.Price,
                    Category = request.Category,
                    Manufacturer = request.Manufacturer,
                    ExpiryDate = request.ExpiryDate,
                    MinimumStock = request.MinimumStock,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Inventories.Add(inventory);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetInventoryById), new { id = inventory.Id }, new
                {
                    Id = inventory.Id,
                    Name = inventory.Name,
                    Quantity = inventory.Quantity,
                    Price = inventory.Price,
                    Message = "Inventory item added successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while adding inventory", Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllInventory()
        {
            try
            {
                var inventory = await _context.Inventories
                    .Select(i => new
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Description = i.Description,
                        Quantity = i.Quantity,
                        Price = i.Price,
                        Category = i.Category,
                        Manufacturer = i.Manufacturer,
                        ExpiryDate = i.ExpiryDate,
                        MinimumStock = i.MinimumStock,
                        IsAvailable = i.IsAvailable,
                        CreatedAt = i.CreatedAt
                    })
                    .ToListAsync();

                return Ok(inventory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching inventory", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetInventoryById(int id)
        {
            try
            {
                var inventory = await _context.Inventories
                    .Where(i => i.Id == id)
                    .Select(i => new
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Description = i.Description,
                        Quantity = i.Quantity,
                        Price = i.Price,
                        Category = i.Category,
                        Manufacturer = i.Manufacturer,
                        ExpiryDate = i.ExpiryDate,
                        MinimumStock = i.MinimumStock,
                        IsAvailable = i.IsAvailable,
                        CreatedAt = i.CreatedAt
                    })
                    .FirstOrDefaultAsync();

                if (inventory == null)
                {
                    return NotFound(new { Message = "Inventory item not found" });
                }

                return Ok(inventory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching inventory", Error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateInventory(int id, [FromBody] UpdateInventoryRequest request)
        {
            try
            {
                var inventory = await _context.Inventories.FindAsync(id);
                if (inventory == null)
                {
                    return NotFound(new { Message = "Inventory item not found" });
                }

                inventory.Name = request.Name ?? inventory.Name;
                inventory.Description = request.Description ?? inventory.Description;
                inventory.Quantity = request.Quantity;
                inventory.Price = request.Price;
                inventory.Category = request.Category ?? inventory.Category;
                inventory.Manufacturer = request.Manufacturer ?? inventory.Manufacturer;
                inventory.ExpiryDate = request.ExpiryDate;
                inventory.MinimumStock = request.MinimumStock;
                inventory.IsAvailable = request.IsAvailable;
                inventory.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { Message = "Inventory item updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating inventory", Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInventory(int id)
        {
            try
            {
                var inventory = await _context.Inventories.FindAsync(id);
                if (inventory == null)
                {
                    return NotFound(new { Message = "Inventory item not found" });
                }

                _context.Inventories.Remove(inventory);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Inventory item deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting inventory", Error = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<object>>> SearchInventory([FromQuery] string query)
        {
            try
            {
                var inventory = await _context.Inventories
                    .Where(i => i.Name.Contains(query) || i.Description.Contains(query) || i.Category.Contains(query))
                    .Select(i => new
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Description = i.Description,
                        Quantity = i.Quantity,
                        Price = i.Price,
                        Category = i.Category,
                        Manufacturer = i.Manufacturer,
                        ExpiryDate = i.ExpiryDate,
                        IsAvailable = i.IsAvailable
                    })
                    .ToListAsync();

                return Ok(inventory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while searching inventory", Error = ex.Message });
            }
        }

        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<object>>> GetLowStockItems()
        {
            try
            {
                var lowStockItems = await _context.Inventories
                    .Where(i => i.Quantity <= i.MinimumStock)
                    .Select(i => new
                    {
                        Id = i.Id,
                        Name = i.Name,
                        CurrentQuantity = i.Quantity,
                        MinimumStock = i.MinimumStock,
                        Category = i.Category,
                        Manufacturer = i.Manufacturer
                    })
                    .ToListAsync();

                return Ok(lowStockItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching low stock items", Error = ex.Message });
            }
        }

        [HttpGet("expiring")]
        public async Task<ActionResult<IEnumerable<object>>> GetExpiringItems()
        {
            try
            {
                var thirtyDaysFromNow = DateTime.UtcNow.AddDays(30);
                var expiringItems = await _context.Inventories
                    .Where(i => i.ExpiryDate <= thirtyDaysFromNow)
                    .Select(i => new
                    {
                        Id = i.Id,
                        Name = i.Name,
                        ExpiryDate = i.ExpiryDate,
                        DaysUntilExpiry = (i.ExpiryDate - DateTime.UtcNow).Days,
                        Quantity = i.Quantity,
                        Category = i.Category
                    })
                    .OrderBy(i => i.ExpiryDate)
                    .ToListAsync();

                return Ok(expiringItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching expiring items", Error = ex.Message });
            }
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<object>> GetInventoryDashboard()
        {
            try
            {
                var totalItems = await _context.Inventories.CountAsync();
                var lowStockItems = await _context.Inventories.CountAsync(i => i.Quantity <= i.MinimumStock);
                var expiringItems = await _context.Inventories.CountAsync(i => i.ExpiryDate <= DateTime.UtcNow.AddDays(30));
                var totalValue = await _context.Inventories.SumAsync(i => i.Quantity * i.Price);
                var categories = await _context.Inventories.GroupBy(i => i.Category)
                    .Select(g => new { Category = g.Key, Count = g.Count() })
                    .ToListAsync();

                return Ok(new
                {
                    TotalItems = totalItems,
                    LowStockItems = lowStockItems,
                    ExpiringItems = expiringItems,
                    TotalValue = totalValue,
                    Categories = categories
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching dashboard data", Error = ex.Message });
            }
        }

        [HttpGet("public")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<object>>> GetAvailableInventory()
        {
            var inventory = await _context.Inventories
                .Where(i => i.IsAvailable && i.Quantity > 0)
                .Select(i => new {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    Price = i.Price,
                    Category = i.Category,
                    Manufacturer = i.Manufacturer,
                    ExpiryDate = i.ExpiryDate,
                    Quantity = i.Quantity
                }).ToListAsync();
            return Ok(inventory);
        }
    }
} 