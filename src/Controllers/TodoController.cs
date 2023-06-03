using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToTask.DTOs;
using ToTask.Services;
using ToTask.Utils;

namespace ToTask.Controllers;

[ApiController]
[Route("todo/")]
public class TodoController : ControllerBase
{
    private readonly TodoService _todoService;

    public TodoController(TodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<TodoDTO>>> GetAllTodos()
    {
        var todos = await _todoService.GetAllTodos(this.GetUserId());
        return Ok(todos);
    }

    [HttpGet("{id}")]
    [Authorize]
    [IsOwner("Todos")]
    public async Task<ActionResult<TodoDTO>> GetTodoById(int id)
    {
        var todo = await _todoService.GetTodoById(id);

        if (todo == null)
        {
            return NotFound();
        }

        return Ok(todo);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<TodoDTO>> AddTodo(TodoDTO todoDTO)
    {
        todoDTO.UserId = this.GetUserId();
        var newTodo = await _todoService.AddTodo(todoDTO);
        return CreatedAtAction(nameof(GetTodoById), new { id = newTodo.Id }, newTodo);
    }

    [HttpPut("{id}")]
    [Authorize]
    [IsOwner("Todos")]
    public async Task<ActionResult> UpdateTodo(int id, TodoDTO todoDTO)
    {
        todoDTO.Id = id;
        var todo = await _todoService.UpdateTodo(todoDTO);

        if (todo == null)
            return NotFound();
        
        return Ok(todo);
    }

    [HttpDelete("{id}")]
    [Authorize]
    [IsOwner("Todos")]
    public async Task<ActionResult> DeleteTodo(int id)
    {
        var result = await _todoService.DeleteTodo(id);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}