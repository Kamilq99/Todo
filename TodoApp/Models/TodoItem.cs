using System.ComponentModel.DataAnnotations.Schema;
namespace TodoApp.Models;

[Table("tasks")]
public class TodoItem
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsDone { get; set; }
}