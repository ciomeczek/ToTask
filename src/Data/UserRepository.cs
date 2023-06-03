using System.Data;
using Dapper;
using ToTask.Models;

namespace ToTask.Data;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    // private readonly IDbConnection _dbConnection;

    // public UserRepository(IDbConnection dbConnection) : base(dbConnection)
    // {
    //     _dbConnection = dbConnection;
    // }
    
    public UserRepository(IDbConnection dbConnection) : base(dbConnection) { }

    // public async Task<IEnumerable<User>> GetAll()
    // {
    //     string query = "SELECT * FROM Users";
    //     return await _dbConnection.QueryAsync<User>(query);
    // }

    public async Task<User> GetUserByUsername(string username)
    {
        string query = "SELECT * FROM Users WHERE Username = @Username";
        var parameters = new { Username = username };
        return await _dbConnection.QuerySingleOrDefaultAsync<User>(query, parameters);
    }

    // public async Task<User> GetById(int id)
    // {
    //     string query = "SELECT * FROM Users WHERE Id = @Id";
    //     return await _dbConnection.QuerySingleOrDefaultAsync<User>(query, new { Id = id });
    // }
    //
    // public async Task<User> Add(User user)
    // {
    //     string query = "INSERT INTO Users (Id, Username, Password) VALUES (@Id, @Username, @password); SELECT * FROM Users WHERE Id = LAST_INSERT_ID();";
    //     User newUser = await _dbConnection.QuerySingleOrDefaultAsync<User>(query, user);
    //
    //     return newUser;
    // }
    //
    // public async Task<User> Update(User user)
    // {
    //     string query = "UPDATE Users SET Username = @Username, Password = @Password WHERE Id = @Id" +
    //                    "SELECT * FROM Users WHERE Id = @Id";
    //     user = await _dbConnection.QuerySingleOrDefaultAsync(query, user);
    //     return user;
    // }
    //
    // public async Task<bool> Delete(int id)
    // {
    //     string query = "DELETE FROM Users WHERE Id = @Id";
    //     int affectedRows = await _dbConnection.ExecuteAsync(query, new { Id = id });
    //     return affectedRows > 0;
    // }
}