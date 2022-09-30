namespace TodoApi.Services
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using TodoApi.Models;

    public interface SqlGet
    {
        Task<ActionResult<IEnumerable<TodoListItem>>> GetItems(); // get全部

        Task<ActionResult<TodoListItem>> GetItemsById(int id); // 用id来get当行数据

        IQueryable<TodoListItem> GetUserId(int id);

        Task<ActionResult<TodoListItem>> PostItems(TodoListItem item); // post操作

        Task<ActionResult<TodoListItem>> UpdateItems(TodoListItem item); // put 新的数据

        Task<ActionResult<TodoListItem>> UpdateItemsById(int id, TodoListItem item);// put 改动原来的数据

        Task<ActionResult<TodoListItem>> DeleteItemsById(int id);    // 删除所在id的行数据

        Task<ActionResult<TodoListItem>> DeleteItemsByTitle(int userId, string title);
    }
}
