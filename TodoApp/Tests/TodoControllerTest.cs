using Xunit;
using Microsoft.EntityFrameworkCore;
using TodoApp.Controllers;
using TodoApp.Data;
using TodoApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System; // Dodaj to, potrzebne dla Guid

public class TodoControllerTests
{
    private TodoDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // <- UNIKALNA baza dla każdego testu
            .Options;

        return new TodoDbContext(options);
    }

    private TodoController GetController()
    {
        return new TodoController();
    }

    [Fact]
    public async Task GetAll_ReturnsAllItems()
    {
        // Arrange
        var dbContext = GetDbContext();
        dbContext.TodoItems.Add(new TodoItem { Name = "Test 1", IsDone = false });
        dbContext.TodoItems.Add(new TodoItem { Name = "Test 2", IsDone = true });
        await dbContext.SaveChangesAsync();

        var controller = GetController();

        // Act
        var result = await controller.GetAll(dbContext);

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<TodoItem>>>(result);
        var items = Assert.IsAssignableFrom<IEnumerable<TodoItem>>(actionResult.Value);
        Assert.Equal(2, ((List<TodoItem>)items).Count);
    }

    [Fact]
    public async Task Get_ReturnsItem_WhenItemExists()
    {
        // Arrange
        var dbContext = GetDbContext();
        var item = new TodoItem { Name = "Test item", IsDone = false };
        dbContext.TodoItems.Add(item);
        await dbContext.SaveChangesAsync();

        var controller = GetController();

        // Act
        var result = await controller.Get(item.Id, dbContext);

        // Assert
        var actionResult = Assert.IsType<ActionResult<TodoItem>>(result);
        var retrievedItem = Assert.IsType<TodoItem>(actionResult.Value);
        Assert.Equal(item.Id, retrievedItem.Id);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenItemDoesNotExist()
    {
        // Arrange
        var dbContext = GetDbContext();
        var controller = GetController();

        // Act
        var result = await controller.Get(999, dbContext);

        // Assert
        var actionResult = Assert.IsType<ActionResult<TodoItem>>(result);
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async Task Create_AddsNewItem()
    {
        // Arrange
        var dbContext = GetDbContext();
        var controller = GetController();
        var newItem = new TodoItem { Name = "New Task", IsDone = false };

        // Act
        var result = await controller.Create(newItem, dbContext);

        // Assert
        var actionResult = Assert.IsType<ActionResult<TodoItem>>(result);
        var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var createdItem = Assert.IsType<TodoItem>(createdResult.Value);
        Assert.Equal("New Task", createdItem.Name);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdsDoNotMatch()
    {
        // Arrange
        var dbContext = GetDbContext();
        var controller = GetController();
        var updatedItem = new TodoItem { Id = 2, Name = "Mismatch", IsDone = false };

        // Act
        var result = await controller.Update(1, updatedItem, dbContext);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Delete_RemovesItem_WhenItemExists()
    {
        // Arrange
        var dbContext = GetDbContext();
        var item = new TodoItem { Name = "Task to delete", IsDone = false };
        dbContext.TodoItems.Add(item);
        await dbContext.SaveChangesAsync();

        var controller = GetController();

        // Act
        var result = await controller.Delete(item.Id, dbContext);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Null(await dbContext.TodoItems.FindAsync(item.Id));
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenItemDoesNotExist()
    {
        // Arrange
        var dbContext = GetDbContext();
        var controller = GetController();

        // Act
        var result = await controller.Delete(123, dbContext);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
