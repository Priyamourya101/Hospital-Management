using Microsoft.AspNetCore.Mvc;
using Hospital.DTOs;
using Hospital.Models;
using Hospital.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;
        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("patient")]
        public async Task<ActionResult<AppointmentResponse>> CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new { Message = "User not authenticated" });

            // Assume you have a method to get patientId by email in your service
            var result = await _appointmentService.CreateAppointmentAsync(request, userEmail);
            if (!result.Success)
                return BadRequest(new { result.Message });
            return CreatedAtAction(nameof(GetAppointmentById), new { id = result.Data.Id }, result.Data);
        }

        [HttpGet("patient")]
        public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetPatientAppointments()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new { Message = "User not authenticated" });
            var appointments = await _appointmentService.GetAppointmentsByPatientEmailAsync(userEmail);
            return Ok(appointments);
        }

        [HttpGet("doctor")]
        public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetDoctorAppointments()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new { Message = "User not authenticated" });
            var appointments = await _appointmentService.GetAppointmentsByDoctorEmailAsync(userEmail);
            return Ok(appointments);
        }

        [HttpPut("status")]
        public async Task<ActionResult> UpdateAppointmentStatus([FromBody] UpdateAppointmentStatusRequest request)
        {
            var result = await _appointmentService.UpdateAppointmentStatusAsync(request);
            if (!result.Success)
                return BadRequest(new { result.Message });
            return Ok(new { Message = result.Message });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentResponse>> GetAppointmentById(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
                return NotFound(new { Message = "Appointment not found" });
            return Ok(appointment);
        }
    }
} 