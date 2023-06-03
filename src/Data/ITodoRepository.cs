using ToTask.Models;

namespace ToTask.Data;

public interface ITodoRepository : IRepository<Todo>
{
    Task<IEnumerable<Todo>> GetAllForUser(int userId);
}