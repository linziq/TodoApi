namespace TodoApi.Context
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.EntityFrameworkCore;
    using TodoApi.Models;

    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoListItem> TodoListItems { get; set; } = null!; // 可以为空  这里是将todoitem作为一个DbSet表，后面orm框架会自动提交到数据库
    }
}
