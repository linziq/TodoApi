using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITodoServices  //  TodoService的接口
    {
        IQueryable<TodoListItem> GetItemsByUserId(int id); // 根据UserID进行Get
        Task<ActionResult<TodoListItem>> PostItems(TodoListItem item); // Post一项新的数据
        Task<ActionResult<TodoListItem>> UpdateItemsById(int id, TodoListItem item); // 使用OrderID进行Put
        Task<ActionResult<TodoListItem>> DeleteItemsByTitle(int userId, string title); // 使用Title进行Delect
        Task<ActionResult<IEnumerable<TodoListItem>>> GetItems(); // get全部
    }
}
