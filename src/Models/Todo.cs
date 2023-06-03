using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToTask.Models;

[Table("Todos")]
public class Todo
{
    [Editable(false)]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    [Editable(false)]
    public int UserId { get; set; }
    [Editable(false)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
