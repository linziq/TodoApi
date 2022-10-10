using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITodoServices  //  TodoService的接口
    {
        IEnumerable<TodoListItem> GetItemsByUserId(int userId); // 根据UserID进行Get
        Task<int> CreateItems(TodoListItem item); // Post一项新的数据
        Task UpdateItemsById(int id, TodoListItem item); // 使用OrderID进行Put
        Task DeleteItemsByTitle(int userId,int OrdersId); // 使用Title进行Delect
        Task<IEnumerable<TodoListItem>> GetItems(); // get全部
    }
}
