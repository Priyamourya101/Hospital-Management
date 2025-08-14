using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospital.Data;
using Hospital.Models;
using System.Security.Claims;
using Hospital.DTOs;
using BCrypt.Net;

namespace Hospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly HospitalContext _context;

        public AdminController(HospitalContext context)
        {
            _context = context;
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<object>> Dashboard()
        {
            try
            {
                var totalDoctors = await _context.Doctors.CountAsync();
                var totalPatients = await _context.Patients.CountAsync();
                var totalAppointments = await _context.Appointments.CountAsync();
                var pendingAppointments = await _context.Appointments.CountAsync(a => a.Status == "Pending");
                var totalInventory = await _context.Inventories.CountAsync();
                var lowStockItems = await _context.Inventories.CountAsync(i => i.Quantity <= i.MinimumStock);

                return Ok(new
                {
                    TotalDoctors = totalDoctors,
                    TotalPatients = totalPatients,
                    TotalAppointments = totalAppointments,
                    PendingAppointments = pendingAppointments,
                    TotalInventory = totalInventory,
                    LowStockItems = lowStockItems
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching dashboard data", Error = ex.Message });
            }
        }

        [HttpGet("details")]
        public async Task<ActionResult<object>> Details()
        {
            try
            {
                var userEmail = HttpContext.Session.GetString("UserEmail");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new { Message = "User not authenticated" });
                }

                var admin = await _context.Admins
                    .FirstOrDefaultAsync(a => a.Email == userEmail);

                if (admin == null)
                {
                    return NotFound(new { Message = "Admin not found" });
                }

                return Ok(new
                {
                    Id = admin.Id,
                    Name = admin.Name,
                    Email = admin.Email,
                    Phone = admin.Phone,
                    CreatedAt = admin.CreatedAt
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching admin details", Error = ex.Message });
            }
        }

        [HttpGet("appointments/all")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllAppointments()
        {
            try
            {
                var appointments = await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .OrderByDescending(a => a.CreatedAt)
                    .Select(a => new
                    {
                        Id = a.Id,
                        PatientName = a.Patient.Name,
                        DoctorName = a.Doctor.Name,
                        AppointmentDate = a.AppointmentDate,
                        TimeSlot = a.TimeSlot,
                        Status = a.Status,
                        Symptoms = a.Symptoms,
                        DoctorNotes = a.DoctorNotes,
                        CreatedAt = a.CreatedAt
                    })
                    .ToListAsync();

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching appointments", Error = ex.Message });
            }
        }

        // STAFF CRUD
        [HttpGet("staff")]
        public async Task<ActionResult<IEnumerable<StaffResponse>>> GetAllStaff()
        {
            var staffList = await _context.Staffs.Select(s => new StaffResponse
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                Address = s.Address,
                City = s.City,
                State = s.State,
                Department = s.Department,
                Role = s.Role,
                DateOfBirth = s.DateOfBirth,
                HireDate = s.HireDate,
                Salary = s.Salary,
                EmergencyContact = s.EmergencyContact,
                EmergencyPhone = s.EmergencyPhone,
                UserId = s.UserId
            }).ToListAsync();
            return Ok(staffList);
        }

        [HttpGet("staff/{id}")]
        public async Task<ActionResult<StaffResponse>> GetStaffById(long id)
        {
            var s = await _context.Staffs.FindAsync(id);
            if (s == null) return NotFound();
            var resp = new StaffResponse
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                Address = s.Address,
                City = s.City,
                State = s.State,
                Department = s.Department,
                Role = s.Role,
                DateOfBirth = s.DateOfBirth,
                HireDate = s.HireDate,
                Salary = s.Salary,
                EmergencyContact = s.EmergencyContact,
                EmergencyPhone = s.EmergencyPhone,
                UserId = s.UserId
            };
            return Ok(resp);
        }

        [HttpPost("staff")]
        public async Task<ActionResult> AddStaff([FromBody] RegisterStaffRequest req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var staff = new Staff
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = req.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(req.Password),
                PhoneNumber = req.PhoneNumber,
                Address = req.Address,
                City = req.City,
                State = req.State,
                Department = req.Department,
                Role = req.Role,
                DateOfBirth = req.DateOfBirth,
                HireDate = req.HireDate,
                Salary = req.Salary,
                EmergencyContact = req.EmergencyContact,
                EmergencyPhone = req.EmergencyPhone,
                UserId = req.UserId
            };
            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Staff added successfully" });
        }

        [HttpPut("staff/{id}")]
        public async Task<ActionResult> UpdateStaff(long id, [FromBody] UpdateStaffRequest req)
        {
            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null) return NotFound();
            if (req.FirstName != null) staff.FirstName = req.FirstName;
            if (req.LastName != null) staff.LastName = req.LastName;
            if (req.PhoneNumber != null) staff.PhoneNumber = req.PhoneNumber;
            if (req.Address != null) staff.Address = req.Address;
            if (req.City != null) staff.City = req.City;
            if (req.State != null) staff.State = req.State;
            if (req.Department != null) staff.Department = req.Department;
            if (req.Role != null) staff.Role = req.Role;
            if (req.DateOfBirth.HasValue) staff.DateOfBirth = req.DateOfBirth.Value;
            if (req.HireDate.HasValue) staff.HireDate = req.HireDate.Value;
            if (req.Salary.HasValue) staff.Salary = req.Salary.Value;
            if (req.EmergencyContact != null) staff.EmergencyContact = req.EmergencyContact;
            if (req.EmergencyPhone != null) staff.EmergencyPhone = req.EmergencyPhone;
            // Hash password if provided in update
            var passwordProp = req.GetType().GetProperty("Password");
            if (passwordProp != null)
            {
                var newPassword = passwordProp.GetValue(req) as string;
                if (!string.IsNullOrEmpty(newPassword))
                {
                    staff.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                }
            }
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Staff updated successfully" });
        }

        [HttpDelete("staff/{id}")]
        public async Task<ActionResult> DeleteStaff(long id)
        {
            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null) return NotFound();
            _context.Staffs.Remove(staff);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Staff deleted successfully" });
        }

        // NURSE CRUD
        [HttpGet("nurse")]
        public async Task<ActionResult<IEnumerable<NurseResponse>>> GetAllNurses()
        {
            var nurses = await _context.Nurses.Select(n => new NurseResponse
            {
                Id = n.Id,
                Name = n.Name,
                Email = n.Email,
                Phone = n.Phone,
                Shift = n.Shift,
                Department = n.Department,
                Qualification = n.Qualification
            }).ToListAsync();
            return Ok(nurses);
        }

        [HttpGet("nurse/{id}")]
        public async Task<ActionResult<NurseResponse>> GetNurseById(long id)
        {
            var n = await _context.Nurses.FindAsync(id);
            if (n == null) return NotFound();
            var resp = new NurseResponse
            {
                Id = n.Id,
                Name = n.Name,
                Email = n.Email,
                Phone = n.Phone,
                Shift = n.Shift,
                Department = n.Department,
                Qualification = n.Qualification
            };
            return Ok(resp);
        }

        [HttpPost("nurse")]
        public async Task<ActionResult> AddNurse([FromBody] RegisterNurseRequest req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var nurse = new Nurse
            {
                Name = req.Name,
                Email = req.Email,
                Phone = req.Phone,
                Shift = req.Shift,
                Department = req.Department,
                Qualification = req.Qualification
            };
            _context.Nurses.Add(nurse);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Nurse added successfully" });
        }

        [HttpPut("nurse/{id}")]
        public async Task<ActionResult> UpdateNurse(long id, [FromBody] UpdateNurseRequest req)
        {
            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse == null) return NotFound();
            if (req.Name != null) nurse.Name = req.Name;
            if (req.Phone != null) nurse.Phone = req.Phone;
            if (req.Shift != null) nurse.Shift = req.Shift;
            if (req.Department != null) nurse.Department = req.Department;
            if (req.Qualification != null) nurse.Qualification = req.Qualification;
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Nurse updated successfully" });
        }

        [HttpDelete("nurse/{id}")]
        public async Task<ActionResult> DeleteNurse(long id)
        {
            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse == null) return NotFound();
            _context.Nurses.Remove(nurse);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Nurse deleted successfully" });
        }
    }
} 