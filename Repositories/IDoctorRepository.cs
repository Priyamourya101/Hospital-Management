using Hospital.Models;
using System.Threading.Tasks;

namespace Hospital.Repositories
{
    public interface IDoctorRepository
    {
        Task<Doctor?> GetByEmailAsync(string email);
        Task<Doctor?> GetByIdAsync(int id);
    }
}
