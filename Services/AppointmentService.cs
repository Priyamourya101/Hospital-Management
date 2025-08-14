using Hospital.DTOs;
using Hospital.Models;
using Hospital.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Services
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }

    public class AppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        public AppointmentService(IAppointmentRepository appointmentRepository, IPatientRepository patientRepository, IDoctorRepository doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<ServiceResult<AppointmentResponse>> CreateAppointmentAsync(CreateAppointmentRequest request, string patientEmail)
        {
            var patient = await _patientRepository.GetByEmailAsync(patientEmail);
            if (patient == null)
                return new ServiceResult<AppointmentResponse> { Success = false, Message = "Patient not found" };
            var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);
            if (doctor == null)
                return new ServiceResult<AppointmentResponse> { Success = false, Message = "Doctor not found" };
            if (!doctor.IsAvailable)
                return new ServiceResult<AppointmentResponse> { Success = false, Message = "Doctor is not available" };
            var appointment = new Appointment
            {
                PatientId = patient.Id,
                DoctorId = request.DoctorId,
                AppointmentDate = request.AppointmentDate,
                TimeSlot = request.TimeSlot,
                Symptoms = request.Symptoms,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };
            await _appointmentRepository.AddAsync(appointment);
            var response = new AppointmentResponse
            {
                Id = appointment.Id,
                PatientId = patient.Id,
                PatientName = patient.Name,
                DoctorId = doctor.Id,
                DoctorName = doctor.Name,
                AppointmentDate = appointment.AppointmentDate,
                TimeSlot = appointment.TimeSlot,
                Symptoms = appointment.Symptoms,
                Status = appointment.Status,
                CreatedAt = appointment.CreatedAt
            };
            return new ServiceResult<AppointmentResponse> { Success = true, Data = response };
        }

        public async Task<IEnumerable<AppointmentResponse>> GetAppointmentsByPatientEmailAsync(string patientEmail)
        {
            var patient = await _patientRepository.GetByEmailAsync(patientEmail);
            if (patient == null) return Enumerable.Empty<AppointmentResponse>();
            var appointments = await _appointmentRepository.GetByPatientIdAsync(patient.Id);
            return appointments.Select(a => new AppointmentResponse
            {
                Id = a.Id,
                PatientId = a.PatientId,
                PatientName = a.Patient?.Name ?? string.Empty,
                DoctorId = a.DoctorId,
                DoctorName = a.Doctor?.Name ?? string.Empty,
                AppointmentDate = a.AppointmentDate,
                TimeSlot = a.TimeSlot,
                Symptoms = a.Symptoms,
                Status = a.Status,
                DoctorNotes = a.DoctorNotes,
                CreatedAt = a.CreatedAt
            });
        }

        public async Task<IEnumerable<AppointmentResponse>> GetAppointmentsByDoctorEmailAsync(string doctorEmail)
        {
            var doctor = await _doctorRepository.GetByEmailAsync(doctorEmail);
            if (doctor == null) return Enumerable.Empty<AppointmentResponse>();
            var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctor.Id);
            return appointments.Select(a => new AppointmentResponse
            {
                Id = a.Id,
                PatientId = a.PatientId,
                PatientName = a.Patient?.Name ?? string.Empty,
                DoctorId = a.DoctorId,
                DoctorName = a.Doctor?.Name ?? string.Empty,
                AppointmentDate = a.AppointmentDate,
                TimeSlot = a.TimeSlot,
                Symptoms = a.Symptoms,
                Status = a.Status,
                DoctorNotes = a.DoctorNotes,
                CreatedAt = a.CreatedAt
            });
        }

        public async Task<ServiceResult<bool>> UpdateAppointmentStatusAsync(UpdateAppointmentStatusRequest request)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);
            if (appointment == null)
                return new ServiceResult<bool> { Success = false, Message = "Appointment not found" };
            appointment.Status = request.Status;
            appointment.DoctorNotes = request.DoctorNotes;
            appointment.UpdatedAt = DateTime.UtcNow;
            await _appointmentRepository.UpdateAsync(appointment);
            return new ServiceResult<bool> { Success = true, Message = "Appointment status updated successfully", Data = true };
        }

        public async Task<AppointmentResponse?> GetAppointmentByIdAsync(int id)
        {
            var a = await _appointmentRepository.GetByIdAsync(id);
            if (a == null) return null;
            return new AppointmentResponse
            {
                Id = a.Id,
                PatientId = a.PatientId,
                PatientName = a.Patient?.Name ?? string.Empty,
                DoctorId = a.DoctorId,
                DoctorName = a.Doctor?.Name ?? string.Empty,
                AppointmentDate = a.AppointmentDate,
                TimeSlot = a.TimeSlot,
                Symptoms = a.Symptoms,
                Status = a.Status,
                DoctorNotes = a.DoctorNotes,
                CreatedAt = a.CreatedAt
            };
        }
    }
}
