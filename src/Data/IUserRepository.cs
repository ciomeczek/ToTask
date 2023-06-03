using ToTask.Models;

namespace ToTask.Data;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetUserByUsername(string username);
}
