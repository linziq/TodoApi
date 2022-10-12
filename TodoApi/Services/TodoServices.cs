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

        public IEnumerable<TodoListItem> GetItemsByUserId(int UserID)  // 实现查询
        {
            var item = context.TodoListItems.Where(x => x.UserID == UserID);
            if (item == null)
            {
                throw new Exception("该用户没有数据，请新增数据");
            }
            return item;
        }

        public async Task<int> CreateItems(TodoListItem item) //实现增加数据
        {
            if (item == null)
            {
                throw new Exception("请勿输入空值");
            }
            context.TodoListItems.Add(item);
            await context.SaveChangesAsync();
            return item.PrimaryID;
        }

        public async Task UpdateItemsById(int PrimaryID, TodoListItem item)  // 实现修改数据
        {
            var toUpdate = context.TodoListItems.FirstOrDefault(it => it.PrimaryID == PrimaryID);
            if (toUpdate == null)
            {
                throw new Exception("不存在此主键ID所关联的数据，修改失败");
            }
            toUpdate.AddDate = item.AddDate;
            toUpdate.Title = item.Title;
            toUpdate.IsDone = item.IsDone;
            toUpdate.Content= item.Content;

            await context.SaveChangesAsync();
            return;
        }

        public async Task DeleteItemsByPrimaryID(int PrimaryID)  //实现删除数据
        {
            var list = context.TodoListItems.FirstOrDefault(x => x.PrimaryID == PrimaryID);
            if (list == null)
            {
                throw new Exception("不存在此数据,删除失败");
            }
            context.TodoListItems.RemoveRange(list);
            await context.SaveChangesAsync();
            return;
        }

        public async Task<IEnumerable<TodoListItem>> GetItems()
        {
            return await context.TodoListItems.ToListAsync();
        }
    }
}
