using System.Data;
using Dapper;
using ToTask.Models;

namespace ToTask.Data;

public class TodoRepository : BaseRepository<Todo>, ITodoRepository
{
    // private readonly IDbConnection _dbConnection;
    //
    // public TodoRepository(IDbConnection dbConnection)
    // {
    //     _dbConnection = dbConnection;
    // }
    
    public TodoRepository(IDbConnection dbConnection) : base(dbConnection) { }

    // public async Task<IEnumerable<Todo>> GetAll()
    // {
    //     string query = "SELECT * FROM Todos";
    //     return await _dbConnection.QueryAsync<Todo>(query);
    // }
    //
    //
    public async Task<IEnumerable<Todo>> GetAllForUser(int userId)
    {
        string query = "SELECT * FROM Todos WHERE UserId = @UserId";
        return await _dbConnection.QueryAsync<Todo>(query, new { UserId = userId });
    }
    //
    // public async Task<Todo> GetById(int id)
    // {
    //     string query = "SELECT * FROM Todos WHERE Id = @Id";
    //     return await _dbConnection.QuerySingleOrDefaultAsync<Todo>(query, new { Id = id });
    // }
    //
    // public async Task<Todo> Add(Todo todo)
    // {
    //     string query = "INSERT INTO Todos (Title, Content, UserId) VALUES (@Title, @Content, @UserId); " +
    //                    "SELECT * FROM Todos WHERE id = LAST_INSERT_ID();";
    //     Todo newTodo = await _dbConnection.QuerySingleOrDefaultAsync<Todo>(query, todo);
    //
    //     return newTodo;
    // }
    //
    // public async Task<Todo> Update(Todo todo)
    // {
    //     string query = "UPDATE Todos SET Title = @Title, Content = @Content WHERE Id = @Id;" +
    //                    "SELECT * FROM Todos WHERE Id = @Id";
    //     todo = await _dbConnection.QuerySingleOrDefaultAsync(query, todo);
    //     return todo;
    // }
    //
    // public async Task<bool> Delete(int id)
    // {
    //     string query = "DELETE FROM Todos WHERE Id = @Id";
    //     int affectedRows = await _dbConnection.ExecuteAsync(query, new { Id = id });
    //     return affectedRows > 0;
    // }
}
