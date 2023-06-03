using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToTask.Models;

[Table("Users")]
public class User
{
    [Editable(false)]
    public int Id { get; set; }
    public string Username { get; set; }
    [Editable(false)]
    public string Password { get; set; }
}