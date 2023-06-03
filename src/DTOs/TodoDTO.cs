using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ToTask.DTOs;

public class TodoDTO
{
    [BindNever]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    [BindNever]
    public int UserId { get; set; }
    [BindNever]
    public DateTime CreatedAt { get; set; }
}