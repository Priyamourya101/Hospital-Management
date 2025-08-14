using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospital.Data;
using Hospital.DTOs;
using Hospital.Helpers;
using Hospital.Models;
using System.Security.Claims;

namespace Hospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly HospitalContext _context;
        private readonly JwtHelper _jwtHelper;

        public AuthController(HospitalContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                // Check Admin
                var admin = await _context.Admins
                    .FirstOrDefaultAsync(a => a.Email == request.Email);

                if (admin != null && PasswordHasher.VerifyPassword(request.Password, admin.Password))
                {
                    var token = _jwtHelper.GenerateToken(admin.Email, "Admin", admin.Id);
                    
                    // Store in session
                    HttpContext.Session.SetString("UserRole", "Admin");
                    HttpContext.Session.SetString("UserEmail", admin.Email);
                    HttpContext.Session.SetInt32("UserId", admin.Id);

                    return Ok(new LoginResponse
                    {
                        Token = token,
                        Role = "Admin",
                        Email = admin.Email,
                        UserId = admin.Id,
                        Message = "Login successful"
                    });
                }

                // Check Doctor
                var doctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.Email == request.Email);

                if (doctor != null && PasswordHasher.VerifyPassword(request.Password, doctor.Password))
                {
                    var token = _jwtHelper.GenerateToken(doctor.Email, "Doctor", doctor.Id);
                    
                    // Store in session
                    HttpContext.Session.SetString("UserRole", "Doctor");
                    HttpContext.Session.SetString("UserEmail", doctor.Email);
                    HttpContext.Session.SetInt32("UserId", doctor.Id);

                    return Ok(new LoginResponse
                    {
                        Token = token,
                        Role = "Doctor",
                        Email = doctor.Email,
                        UserId = doctor.Id,
                        Message = "Login successful"
                    });
                }

                // Check Patient
                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.Email == request.Email);

                if (patient != null && PasswordHasher.VerifyPassword(request.Password, patient.Password))
                {
                    var token = _jwtHelper.GenerateToken(patient.Email, "Patient", patient.Id);
                    
                    // Store in session
                    HttpContext.Session.SetString("UserRole", "Patient");
                    HttpContext.Session.SetString("UserEmail", patient.Email);
                    HttpContext.Session.SetInt32("UserId", patient.Id);

                    return Ok(new LoginResponse
                    {
                        Token = token,
                        Role = "Patient",
                        Email = patient.Email,
                        UserId = patient.Id,
                        Message = "Login successful"
                    });
                }

                return Unauthorized(new { Message = "Invalid email or password" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred during login", Error = ex.Message });
            }
        }

        [HttpPost("logout")]
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok(new { Message = "Logout successful" });
        }
    }
} 