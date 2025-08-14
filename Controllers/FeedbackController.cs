using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospital.Data;
using Hospital.DTOs;
using Hospital.Models;

namespace Hospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly HospitalContext _context;

        public FeedbackController(HospitalContext context)
        {
            _context = context;
        }

        [HttpPost("submit")]
        public async Task<ActionResult<Feedback>> SubmitFeedback([FromBody] SubmitFeedbackRequest request)
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

                var feedback = new Feedback
                {
                    PatientId = patient.Id,
                    Subject = request.Subject,
                    Message = request.Message,
                    Rating = request.Rating,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetFeedbackById), new { id = feedback.Id }, new
                {
                    Id = feedback.Id,
                    Subject = feedback.Subject,
                    Rating = feedback.Rating,
                    Message = "Feedback submitted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while submitting feedback", Error = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllFeedback()
        {
            try
            {
                var feedback = await _context.Feedbacks
                    .Include(f => f.Patient)
                    .OrderByDescending(f => f.CreatedAt)
                    .Select(f => new
                    {
                        Id = f.Id,
                        PatientName = f.Patient.Name,
                        PatientEmail = f.Patient.Email,
                        Subject = f.Subject,
                        Message = f.Message,
                        Rating = f.Rating,
                        CreatedAt = f.CreatedAt
                    })
                    .ToListAsync();

                return Ok(feedback);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching feedback", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetFeedbackById(int id)
        {
            try
            {
                var feedback = await _context.Feedbacks
                    .Include(f => f.Patient)
                    .Where(f => f.Id == id)
                    .Select(f => new
                    {
                        Id = f.Id,
                        PatientName = f.Patient.Name,
                        PatientEmail = f.Patient.Email,
                        Subject = f.Subject,
                        Message = f.Message,
                        Rating = f.Rating,
                        CreatedAt = f.CreatedAt
                    })
                    .FirstOrDefaultAsync();

                if (feedback == null)
                {
                    return NotFound(new { Message = "Feedback not found" });
                }

                return Ok(feedback);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching feedback", Error = ex.Message });
            }
        }

        [HttpGet("patient")]
        public async Task<ActionResult<IEnumerable<object>>> GetPatientFeedback()
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

                var feedback = await _context.Feedbacks
                    .Where(f => f.PatientId == patient.Id)
                    .OrderByDescending(f => f.CreatedAt)
                    .Select(f => new
                    {
                        Id = f.Id,
                        Subject = f.Subject,
                        Message = f.Message,
                        Rating = f.Rating,
                        CreatedAt = f.CreatedAt
                    })
                    .ToListAsync();

                return Ok(feedback);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching feedback", Error = ex.Message });
            }
        }
    }
} 