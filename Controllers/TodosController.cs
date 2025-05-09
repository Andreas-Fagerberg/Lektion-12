using Microsoft.AspNetCore.Mvc;
using static TodoEntity;

namespace Lektion_12;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly List<TodoEntity> _todos = new List<TodoEntity>();

    [HttpPost("create")]
    public IActionResult CreateTodo([FromBody] TodoDto request)
    {
        var currentUser = UsersController.CurrentUser;
        try
        {
            if (string.IsNullOrEmpty(request.Title))
            {
                return BadRequest("Title is required");
            }

            var newId = _todos.Count > 0 ? _todos.Max(t => t.Id) + 1 : 1;

            var todo = new TodoEntity()
            {
                Id = newId,
                Title = request.Title,
                Description = request.Description,
                IsCompleted = false,
                CreatedAt = DateTime.Now,
                UserId = currentUser.Id,
            };

            _todos.Add(todo);

            return Ok($"Todo: {todo.Title} Created at: {todo.CreatedAt}");
        }
        catch (Exception ex)
        {
            return BadRequest($"Something went wrong: {ex.Message}");
        }
    }
}
