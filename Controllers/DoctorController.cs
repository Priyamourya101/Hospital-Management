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
    public class DoctorController : ControllerBase
    {
        private readonly HospitalContext _context;

        public DoctorController(HospitalContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Doctor>> Register([FromBody] RegisterDoctorRequest request)
        {
            try
            {
                if (await _context.Doctors.AnyAsync(d => d.Email == request.Email))
                {
                    return BadRequest(new { Message = "Email already exists" });
                }

                var doctor = new Doctor
                {
                    Name = request.Name,
                    Email = request.Email,
                    Password = PasswordHasher.HashPassword(request.Password),
                    Phone = request.Phone,
                    Specialization = request.Specialization,
                    Qualification = request.Qualification,
                    Experience = request.Experience,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Doctors.Add(doctor);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetDoctorById), new { id = doctor.Id }, new
                {
                    Id = doctor.Id,
                    Name = doctor.Name,
                    Email = doctor.Email,
                    Specialization = doctor.Specialization,
                    Message = "Doctor registered successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while registering doctor", Error = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllDoctors()
        {
            try
            {
                var doctors = await _context.Doctors
                    .Select(d => new
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Email = d.Email,
                        Phone = d.Phone,
                        Specialization = d.Specialization,
                        Qualification = d.Qualification,
                        Experience = d.Experience,
                        IsAvailable = d.IsAvailable,
                        CreatedAt = d.CreatedAt
                    })
                    .ToListAsync();

                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching doctors", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetDoctorById(int id)
        {
            try
            {
                var doctor = await _context.Doctors
                    .Where(d => d.Id == id)
                    .Select(d => new
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Email = d.Email,
                        Phone = d.Phone,
                        Specialization = d.Specialization,
                        Qualification = d.Qualification,
                        Experience = d.Experience,
                        IsAvailable = d.IsAvailable,
                        CreatedAt = d.CreatedAt
                    })
                    .FirstOrDefaultAsync();

                if (doctor == null)
                {
                    return NotFound(new { Message = "Doctor not found" });
                }

                return Ok(doctor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching doctor", Error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDoctor(int id, [FromBody] UpdateDoctorRequest request)
        {
            try
            {
                var doctor = await _context.Doctors.FindAsync(id);
                if (doctor == null)
                {
                    return NotFound(new { Message = "Doctor not found" });
                }

                doctor.Name = request.Name ?? doctor.Name;
                doctor.Phone = request.Phone ?? doctor.Phone;
                doctor.Specialization = request.Specialization ?? doctor.Specialization;
                doctor.Qualification = request.Qualification ?? doctor.Qualification;
                doctor.Experience = request.Experience;
                doctor.IsAvailable = request.IsAvailable;
                doctor.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { Message = "Doctor updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating doctor", Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDoctor(int id)
        {
            try
            {
                var doctor = await _context.Doctors.FindAsync(id);
                if (doctor == null)
                {
                    return NotFound(new { Message = "Doctor not found" });
                }

                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Doctor deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting doctor", Error = ex.Message });
            }
        }

        [HttpPost("issue-prescription")]
        public async Task<ActionResult<Prescription>> IssuePrescription([FromBody] IssuePrescriptionRequest request)
        {
            try
            {
                var prescription = new Prescription
                {
                    PatientId = request.PatientId,
                    DoctorId = request.DoctorId,
                    Diagnosis = request.Diagnosis,
                    Medications = request.Medications,
                    Instructions = request.Instructions,
                    PrescriptionDate = DateTime.UtcNow,
                    NextVisitDate = request.NextVisitDate,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Prescriptions.Add(prescription);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPrescriptionById), new { id = prescription.Id }, new
                {
                    Id = prescription.Id,
                    PatientId = prescription.PatientId,
                    DoctorId = prescription.DoctorId,
                    Diagnosis = prescription.Diagnosis,
                    Medications = prescription.Medications,
                    Instructions = prescription.Instructions,
                    PrescriptionDate = prescription.PrescriptionDate,
                    NextVisitDate = prescription.NextVisitDate,
                    Message = "Prescription issued successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while issuing prescription", Error = ex.Message });
            }
        }

        [HttpGet("prescription/{id}")]
        public async Task<ActionResult<object>> GetPrescriptionById(int id)
        {
            try
            {
                var prescription = await _context.Prescriptions
                    .Include(p => p.Patient)
                    .Include(p => p.Doctor)
                    .Where(p => p.Id == id)
                    .Select(p => new
                    {
                        Id = p.Id,
                        PatientName = p.Patient.Name,
                        DoctorName = p.Doctor.Name,
                        Diagnosis = p.Diagnosis,
                        Medications = p.Medications,
                        Instructions = p.Instructions,
                        PrescriptionDate = p.PrescriptionDate,
                        NextVisitDate = p.NextVisitDate
                    })
                    .FirstOrDefaultAsync();

                if (prescription == null)
                {
                    return NotFound(new { Message = "Prescription not found" });
                }

                return Ok(prescription);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching prescription", Error = ex.Message });
            }
        }
    }
} 