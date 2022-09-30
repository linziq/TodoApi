namespace TodoApi.Services
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using TodoApi.Context;
    using TodoApi.Models;

    public class SqlHelper : SqlGet
    {
        private readonly TodoContext context;  // 声明只读字段

        public SqlHelper(TodoContext todoContext) // 构造函数注入上下文
        {
            context = todoContext;
        }

        public DbSet<TodoListItem> TodoListItems { get; set; } = null!;

        // 下面代码实现接口
        public async Task<ActionResult<IEnumerable<TodoListItem>>> Get() // 以IEnumerable<T>来作为数据查询返回对象
        {
            return await context.TodoListItems.ToListAsync(); // 异步枚举 tolist()存在表里提交给数据库
        }

        public async Task<ActionResult<IEnumerable<TodoListItem>>> GetItems()
        {
            return await context.TodoListItems.ToListAsync();
        }

        public async Task<ActionResult<TodoListItem>> GetItemsById(int id)
        {
            var todoListItems = await context.TodoListItems.FindAsync(id);
            return todoListItems;
        }

        public IQueryable<TodoListItem> GetUserId(int id)
        {
            return context.TodoListItems.Where(x => x.UserID == id);
        }

        public async Task<ActionResult<TodoListItem>> PostItems(TodoListItem item)
        {
            context.TodoListItems.Add(item); // 向集合里添加数据
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<ActionResult<TodoListItem>> DeleteItemsById(int id)
        {
            var todoListitem = await context.TodoListItems.FindAsync(id);
            context.TodoListItems.Remove(todoListitem); // 移除id所在列
            await context.SaveChangesAsync();  // 保持更改
            return todoListitem;
        }

        public Task<ActionResult<TodoListItem>> UpdateItems(TodoListItem item)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult<TodoListItem>> UpdateItemsById(int id, TodoListItem item)
        {
            item.OrdersId = id;
            context.TodoListItems.Update(item);
            EntityState state = context.Entry(item).State;
            await context.SaveChangesAsync();  // 保持更改
            return item;
        }

        public async Task<ActionResult<TodoListItem>> DeleteItemsByTitle(int userId, string title)
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

            return null;
        }
    }
}
