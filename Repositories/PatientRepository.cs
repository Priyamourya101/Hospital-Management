using Hospital.Data;
using Hospital.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Hospital.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HospitalContext _context;
        public PatientRepository(HospitalContext context)
        {
            _context = context;
        }

        public async Task<Patient?> GetByEmailAsync(string email)
        {
            return await _context.Patients.FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _context.Patients.FindAsync(id);
        }
    }
}
