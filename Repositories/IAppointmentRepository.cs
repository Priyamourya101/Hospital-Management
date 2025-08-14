using Hospital.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.Repositories
{
    public interface IAppointmentRepository
    {
        Task<Appointment> AddAsync(Appointment appointment);
        Task<Appointment?> GetByIdAsync(int id);
        Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId);
        Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId);
        Task UpdateAsync(Appointment appointment);
    }
}
