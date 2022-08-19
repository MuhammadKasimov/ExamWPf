using ExamWithDesktop.Data.IRepositories;
using ExamWithDesktop.Domain.Entities;

namespace ExamWithDesktop.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
    }
}
