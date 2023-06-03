using AutoMapper;
using ToTask.Data;
using ToTask.DTOs;
using ToTask.Models;

namespace ToTask.Services;

public class TodoService
{
    private readonly ITodoRepository _todoRepository;
    private readonly IMapper _mapper;

    public TodoService(ITodoRepository todoRepository, IMapper mapper)
    {
        _todoRepository = todoRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TodoDTO>> GetAllTodos(int userId)
    {
        IEnumerable<Todo> todos = await _todoRepository.GetAllForUser(userId);
        return _mapper.Map<IEnumerable<TodoDTO>>(todos);
    }

    public async Task<TodoDTO> GetTodoById(int id)
    {
        Todo todo = await _todoRepository.GetById(id);
        return _mapper.Map<TodoDTO>(todo);
    }

    public async Task<Todo> AddTodo(TodoDTO todoDTO)
    {
        Todo todo = _mapper.Map<Todo>(todoDTO);

        Todo newTodo = await _todoRepository.Add(todo);
        return newTodo;
    }

    public async Task<TodoDTO?> UpdateTodo(TodoDTO todoDTO)
    {
        Todo existingTodo = await _todoRepository.GetById(todoDTO.Id);

        if (existingTodo == null)
        {
            return null;
        }

        existingTodo = await _todoRepository.Update(_mapper.Map<Todo>(todoDTO));

        return _mapper.Map<TodoDTO>(existingTodo);
    }

    public async Task<bool> DeleteTodo(int id)
    {
        return await _todoRepository.Delete(id);
    }
}