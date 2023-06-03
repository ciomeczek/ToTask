using System.Data;
using Dapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ToTask.DTOs;

public class UserDTO
{
    [BindNever]
    public int Id { get; set; }
    public string Username { get; set; }
}

public class CreateUserDTO
{
    [BindNever]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class CreateUserValidator : AbstractValidator<CreateUserDTO>
{
    private readonly IDbConnection _dbConnection;
    
    public CreateUserValidator(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;

        RuleFor(dto => dto.Username)
            .Must(BeUniqueUsername).WithMessage("Username must be unique.");
    }

    private bool BeUniqueUsername(string username)
    {
        var query = "SELECT COUNT(*) FROM user WHERE username = @Username";
        var count = _dbConnection.ExecuteScalar<int>(query, new { Username = username });

        return count == 0; 
    }
}