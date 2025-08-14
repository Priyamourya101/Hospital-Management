using Hospital.Data;
using Hospital.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Hospital.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HospitalContext _context;
        public DoctorRepository(HospitalContext context)
        {
            _context = context;
        }

        public async Task<Doctor?> GetByEmailAsync(string email)
        {
            return await _context.Doctors.FirstOrDefaultAsync(d => d.Email == email);
        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            return await _context.Doctors.FindAsync(id);
        }
    }
}
