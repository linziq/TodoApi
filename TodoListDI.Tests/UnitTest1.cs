namespace TodoListDI.Tests
{
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using TodoApi.Context;
    using TodoApi.Models;
    using TodoApi.Services;

    public class UnitTest1
    {
        [Fact] // get
        public void GetItem_ByUserID()
        {
            var dbContext = GetDbContext();
            // 添加一条UserId为1，title为Test的数据
            dbContext.TodoListItems.Add(new TodoListItem
            {
                PrimaryID = 1,
                AddDate = DateTime.Now,
                IsDone = true,
                UserID = 1,
                Title = "test",
            });
            dbContext.SaveChanges();

            TodoServices services = new TodoServices(dbContext);
            var results = services.GetItemsByUserId(1);

            Assert.Equal("test", results.ToList()[0].Title);
        }

        [Fact] // Post 
        public void CreateItem_By_UserID_Items()
        {
            var dbContext = GetDbContext();
            TodoServices services = new TodoServices(dbContext);

            var results = services.CreateItems(new TodoListItem
            {
                PrimaryID = 2,
                AddDate = DateTime.Now,
                IsDone = false,
                UserID = 2,
                Title = "happy",
            });
            var actresults = services.GetItemsByUserId(2);

            Assert.Equal("happy", actresults.ToList()[0].Title);
        }

        [Fact] //Put
        public  void PutItem_ByPrimaryID()
        {
            var dbContext = GetDbContext();
            dbContext.TodoListItems.Add(new TodoListItem
            {
                PrimaryID = 3,
                AddDate = DateTime.Now,
                IsDone = false,
                UserID = 3,
                Title = "cloudy",
            });
            dbContext.SaveChanges();
            TodoServices services = new TodoServices(dbContext);

            var method = services.UpdateItemsById(3, new TodoListItem
            {
           
                AddDate = DateTime.UtcNow,
                IsDone = true,
                UserID = 3,
                Title = "string",
            });
            var actresult = services.GetItemsByUserId(3);

            Assert.Equal("string", actresult.ToList()[0].Title);

        }

        [Fact] // Delect
        public async void Delete_By_UserID_Title()
        {
            var dbContext = GetDbContext();
            dbContext.TodoListItems.Add(new TodoListItem
            {
                PrimaryID = 4,
                AddDate = DateTime.Now,
                IsDone = false,
                UserID = 4,
                Title = "Wednesday",
            });
            dbContext.SaveChanges();
            TodoServices services = new TodoServices(dbContext);

            await  services.DeleteItemsByPrimaryID(4);
            var results = services.GetItemsByUserId(4);

            Assert.Equal(0, results.Count());
        }


        private TodoContext GetDbContext()
        {
            var optins = new DbContextOptionsBuilder<TodoContext>()
             .UseInMemoryDatabase(databaseName: "Testdb")
             .Options;
            return new TodoContext(optins);
        }
    }
}