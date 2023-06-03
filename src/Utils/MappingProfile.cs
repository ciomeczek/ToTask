using AutoMapper;
using ToTask.DTOs;
using ToTask.Models;

namespace ToTask.Utils;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Configure your mappings here
        CreateMap<Todo, TodoDTO>();
        CreateMap<TodoDTO, Todo>();
        CreateMap<User, UserDTO>();
        CreateMap<UserDTO, User>();
        CreateMap<User, CreateUserDTO>();
        CreateMap<CreateUserDTO, User>();
    }
}
