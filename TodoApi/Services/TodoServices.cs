using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Context;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class TodoServices : ITodoServices
    {
        private readonly TodoContext context;  // 声明只读字段

        public TodoServices(TodoContext todoContext) // 构造函数注入上下文
        {
            context = todoContext;
        }

        public IQueryable<TodoListItem> GetItemsByUserId(int id)  // 实现查询
        {
            return context.TodoListItems.Where(x => x.UserID == id);
        }

        public async Task<ActionResult<TodoListItem>> PostItems(TodoListItem item) //实现增加数据
        {
            context.TodoListItems.Add(item); 
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<ActionResult<TodoListItem>> UpdateItemsById(int id, TodoListItem item)  // 实现修改数据
        {
            item.OrdersId = id;
            context.TodoListItems.Update(item);
            EntityState state = context.Entry(item).State;
            await context.SaveChangesAsync();  
            return item;
        }

        public async Task<ActionResult<TodoListItem>> DeleteItemsByTitle(int userId, string title)  //实现删除数据
        {
            var list = await context.TodoListItems.Where(x => x.UserID == userId).ToListAsync();
            foreach (TodoListItem item in list)
            {
                if (item.Title == title)
                {
                    context.TodoListItems.Remove(item);
                    await context.SaveChangesAsync();
                    return item;
                }
            }

#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

    }
}
