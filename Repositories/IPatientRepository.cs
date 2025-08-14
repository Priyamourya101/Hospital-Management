using Hospital.Models;
using System.Threading.Tasks;

namespace Hospital.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient?> GetByEmailAsync(string email);
        Task<Patient?> GetByIdAsync(int id);
    }
}
