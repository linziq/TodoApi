namespace TodoApi.Context
{
    using Microsoft.EntityFrameworkCore;
    using TodoApi.Models;

    public class LoginContext : DbContext
    {
        public LoginContext(DbContextOptions<LoginContext> options)
            : base(options)
        {
        }

        public DbSet<UserItem> UserItems { get; set; } = null!; // 可以为空  这里是将todoitem作为一个DbSet表，后面orm框架会自动提交到数据库
    }
}
