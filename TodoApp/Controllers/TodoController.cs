using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;
using TodoApp.Data;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    [HttpGet]
public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll([FromServices] TodoDbContext db)
{
    return await db.TodoItems.ToListAsync();
}

[HttpGet("{id}")]
public async Task<ActionResult<TodoItem>> Get(int id, [FromServices] TodoDbContext db)
{
    var item = await db.TodoItems.FindAsync(id);
    return item is null ? NotFound() : item;
}

[HttpPost]
public async Task<ActionResult<TodoItem>> Create(TodoItem item, [FromServices] TodoDbContext db)
{
    db.TodoItems.Add(item);
    await db.SaveChangesAsync();
    return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
}

[HttpPut("{id}")]
public async Task<IActionResult> Update(int id, TodoItem item, [FromServices] TodoDbContext db)
{
    if (id != item.Id) return BadRequest();

    db.Entry(item).State = EntityState.Modified;
    await db.SaveChangesAsync();
    return NoContent();
}

[HttpDelete("{id}")]
public async Task<IActionResult> Delete(int id, [FromServices] TodoDbContext db)
{
    var item = await db.TodoItems.FindAsync(id);
    if (item is null) return NotFound();

    db.TodoItems.Remove(item);
    await db.SaveChangesAsync();
    return NoContent();
}

}