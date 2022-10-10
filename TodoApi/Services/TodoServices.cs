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

        public IEnumerable<TodoListItem> GetItemsByUserId(int id)  // 实现查询
        {
            var item = context.TodoListItems.Where(x => x.UserID == id);
            if (item == null)
            {
                throw new Exception();
            }

            return item;
        }

        public async Task<int> CreateItems(TodoListItem item) //实现增加数据
        {
            context.TodoListItems.Add(item);
            await context.SaveChangesAsync();
            return item.OrdersId;
        }

        public async Task UpdateItemsById(int id, TodoListItem item)  // 实现修改数据
        {
            item.OrdersId = id;
            context.TodoListItems.Update(item);
            EntityState state = context.Entry(item).State;
            await context.SaveChangesAsync();
            return ;
        }

        public async Task DeleteItemsByTitle(int userId, int OrdersId)  //实现删除数据
        {  
              var list=await context.TodoListItems.Where(x=>x.OrdersId==OrdersId).ToListAsync();
                    context.TodoListItems.Remove(list);
                    await context.SaveChangesAsync();
                    return ;
            

        }

        public async Task<IEnumerable<TodoListItem>> GetItems()
        {
            return await context.TodoListItems.ToListAsync();
        }
    }
}
