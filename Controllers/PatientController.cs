using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospital.Data;
using Hospital.DTOs;
using Hospital.Helpers;
using Hospital.Models;

namespace Hospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly HospitalContext _context;

        public PatientController(HospitalContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Patient>> Register([FromBody] RegisterPatientRequest request)
        {
            try
            {
                if (await _context.Patients.AnyAsync(p => p.Email == request.Email))
                {
                    return BadRequest(new { Message = "Email already exists" });
                }

                var patient = new Patient
                {
                    Name = request.Name,
                    Email = request.Email,
                    Password = PasswordHasher.HashPassword(request.Password),
                    Phone = request.Phone,
                    DateOfBirth = request.DateOfBirth,
                    Gender = request.Gender,
                    Address = request.Address,
                    BloodGroup = request.BloodGroup,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPatientById), new { id = patient.Id }, new
                {
                    Id = patient.Id,
                    Name = patient.Name,
                    Email = patient.Email,
                    Message = "Patient registered successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while registering patient", Error = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllPatients()
        {
            try
            {
                var patients = await _context.Patients
                    .Select(p => new
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Email = p.Email,
                        Phone = p.Phone,
                        DateOfBirth = p.DateOfBirth,
                        Gender = p.Gender,
                        Address = p.Address,
                        BloodGroup = p.BloodGroup,
                        CreatedAt = p.CreatedAt
                    })
                    .ToListAsync();

                return Ok(patients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching patients", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetPatientById(int id)
        {
            try
            {
                var patient = await _context.Patients
                    .Where(p => p.Id == id)
                    .Select(p => new
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Email = p.Email,
                        Phone = p.Phone,
                        DateOfBirth = p.DateOfBirth,
                        Gender = p.Gender,
                        Address = p.Address,
                        BloodGroup = p.BloodGroup,
                        CreatedAt = p.CreatedAt
                    })
                    .FirstOrDefaultAsync();

                if (patient == null)
                {
                    return NotFound(new { Message = "Patient not found" });
                }

                return Ok(patient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching patient", Error = ex.Message });
            }
        }

        [HttpGet("details")]
        public async Task<ActionResult<object>> GetPatientDetails()
        {
            try
            {
                var userEmail = HttpContext.Session.GetString("UserEmail");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new { Message = "User not authenticated" });
                }

                var patient = await _context.Patients
                    .Where(p => p.Email == userEmail)
                    .Select(p => new
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Email = p.Email,
                        Phone = p.Phone,
                        DateOfBirth = p.DateOfBirth,
                        Gender = p.Gender,
                        Address = p.Address,
                        BloodGroup = p.BloodGroup,
                        CreatedAt = p.CreatedAt
                    })
                    .FirstOrDefaultAsync();

                if (patient == null)
                {
                    return NotFound(new { Message = "Patient not found" });
                }

                return Ok(patient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching patient details", Error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePatient(int id, [FromBody] UpdatePatientRequest request)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);
                if (patient == null)
                {
                    return NotFound(new { Message = "Patient not found" });
                }

                patient.Name = request.Name ?? patient.Name;
                patient.Phone = request.Phone ?? patient.Phone;
                patient.Address = request.Address ?? patient.Address;
                patient.BloodGroup = request.BloodGroup ?? patient.BloodGroup;
                patient.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { Message = "Patient updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating patient", Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePatient(int id)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);
                if (patient == null)
                {
                    return NotFound(new { Message = "Patient not found" });
                }

                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Patient deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting patient", Error = ex.Message });
            }
        }
    }
} 