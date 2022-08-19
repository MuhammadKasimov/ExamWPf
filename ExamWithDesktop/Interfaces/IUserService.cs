using ExamWithDesktop.Domain.Entities;
using ExamWithDesktop.WPF.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExamWithDesktop.WPF.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateAsync(UserForCreation userForCreation);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetAsync(long id);
        Task<User> UpdateAsync(long id, UserForCreation userForCreation);
        Task UploadImageAsync(long id, string imagePath, string passportPath);
    }
}
