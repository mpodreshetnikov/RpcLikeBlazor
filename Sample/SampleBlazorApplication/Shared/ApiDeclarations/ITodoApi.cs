using RpcLikeBlazor.ApiAttributes;
using SampleBlazorApplication.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleBlazorApplication.Shared.ApiDeclarations
{
    [ApiInterface]
    public interface ITodoApi
    {
        [ApiHttpMethod(Method.Get)]
        Task<IEnumerable<Todo>> GetTodos();

        [ApiHttpMethod(Method.Post)]
        Task<Todo> AddTodo(Todo todo);

        [ApiHttpMethod(Method.Delete)]
        Task<bool> DeleteTodo(Guid id);

        [ApiHttpMethod(Method.Put)]
        Task<bool> MarkTask(Guid id, bool isCompleted);
    }
}
