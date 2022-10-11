using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITodoServices  //  TodoService的接口
    {
        IEnumerable<TodoListItem> GetItemsByUserId(int UserID); // 根据UserID进行Get
        Task<int> CreateItems(TodoListItem item); // Post一项新的数据
        Task UpdateItemsById(int PrimaryID, TodoListItem item); // 使用主键进行Put
        Task DeleteItemsByPrimaryID(int PrimaryID); // 使用主键进行Delect
        Task<IEnumerable<TodoListItem>> GetItems(); // get全部
    }
}
