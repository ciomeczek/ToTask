using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using ToTask.Models;
using Dapper;

namespace ToTask.Data;

public abstract class BaseRepository<T> : IRepository<T> where T : class
{
    protected readonly IDbConnection _dbConnection;
    private readonly string TableName;

    protected BaseRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
        TableName = GetTableName();
    }
    
    public async Task<IEnumerable<T>> GetAll()
    {
        string query = $"SELECT * FROM {TableName}";
        return await _dbConnection.QueryAsync<T>(query);
    }

    public async Task<T> GetById(int id)
    {
        string query = $"SELECT * FROM {TableName} WHERE Id = @Id";
        return await _dbConnection.QuerySingleOrDefaultAsync<T>(query, new { Id = id });
    }
    
    public async Task<T> Add(T obj)
    {
        string columns = GetColumns();
        string values = GetValues();

        string query = $"INSERT INTO {TableName} ({columns}) VALUES ({values}); SELECT LAST_INSERT_ID();";
        var insertedId = await _dbConnection.ExecuteScalarAsync<int>(query, obj);

        return await GetById(insertedId);
    }

    public async Task<T> Update(T obj)
    {
        string updateValues = GetUpdateValues();

        string query = $"UPDATE {TableName} SET {updateValues} WHERE Id = @Id" +
                       "SELECT * FROM Todos WHERE Id = @Id";
        obj = await _dbConnection.QuerySingleOrDefaultAsync(query, obj);

        return obj;
    }

    public async Task<bool> Delete(int id)
    {
        string query = $"DELETE FROM {TableName} WHERE Id = @Id";
        int rowsAffected = await _dbConnection.ExecuteAsync(query, new { Id = id });

        return rowsAffected > 0;
    }

    private string GetColumns()
    {
        Type type = typeof(T);
        var properties = type.GetProperties().Where(p => p.CanRead && !p.GetGetMethod().IsVirtual);

        string columns = string.Join(", ", properties.Select(p => p.Name));
        return columns;
    }

    private string GetValues()
    {
        Type type = typeof(T);
        var properties = type.GetProperties().Where(p => p.CanRead && !p.GetGetMethod().IsVirtual);

        string values = string.Join(", ", properties.Select(p => $"@{p.Name}"));
        return values;
    }
    
    private string GetUpdateValues()
    {
        Type type = typeof(T);
        var properties = type.GetProperties().Where(p => p.CanRead && !p.GetGetMethod().IsVirtual && p.Name != "Id" && !Attribute.IsDefined(p, typeof(EditableAttribute), true));

        string updateValues = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));
        return updateValues;
    }
    
    private string GetTableName()
    {
        Type type = typeof(T);
        var tableAttribute = (TableAttribute)Attribute.GetCustomAttribute(type, typeof(TableAttribute));
        
        if (tableAttribute != null)
            return tableAttribute.Name;

        throw new Exception("Model does not have Table attribute");
    }
}