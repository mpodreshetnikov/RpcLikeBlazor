using Microsoft.AspNetCore.Mvc;
using SampleBlazorApplication.Shared.ApiDeclarations;
using SampleBlazorApplication.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleBlazorApplication.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TodoController : ControllerBase, ITodoApi
    {
        private static readonly IList<Todo> Todos = new List<Todo>();

        [HttpGet]
        public Task<IEnumerable<Todo>> GetTodos()
        {
            return Task.FromResult(Todos.AsEnumerable());
        }

        [HttpPost]
        public Task<Todo> AddTodo(Todo todo)
        {
            if (!TryValidateModel(todo))
            {
                return Task.FromResult<Todo>(null);
            }
            todo.Id = Guid.NewGuid();
            Todos.Add(todo);
            return Task.FromResult(todo);
        }

        [HttpDelete]
        public Task<bool> DeleteTodo(Guid id)
        {
            var todo = Todos.SingleOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return Task.FromResult(false);
            }
            Todos.Remove(todo);
            return Task.FromResult(true);
        }

        [HttpPut]
        public Task<bool> MarkTask(Guid id, bool isCompleted)
        {
            var todo = Todos.SingleOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return Task.FromResult(false);
            }
            todo.IsCompleted = isCompleted;
            return Task.FromResult(true);
        }
    }
}
