namespace TodoList.Tests
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using TodoApi.Context;
    using TodoApi.Models;
    using TodoApi.Services;

    public class Tests
    {
        [Test] // Get方法
        public void Shuould_Return_All_Itemss_ById()
        {
            // 使用内存数据库
            var optins = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(databaseName: "TodolistItems")
                .Options;

            // mock TodoContext
            using var context = new TodoContext(optins);

            context.TodoListItems.Add(new TodoListItem
            {
                OrdersId = 1,
                AddDate = DateTime.Now,
                IsDone = true,
                UserID = 1,
                Title = "test",
            });
            context.TodoListItems.Add(new TodoListItem
            {
                OrdersId = 2,
                AddDate = DateTime.Now,
                IsDone = false,
                UserID = 2,
                Title = "friday1",
            });
            context.SaveChanges();

            // mock service
            TodoServices TodoServices = new TodoServices(context);

            var results = TodoServices.GetItemsByUserId(1);

            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.ToList()[0].Title, Is.EqualTo("test"));
        }

        [Test] // post方法
        public void PostItems_ByUserId()
        {
            var optins = new DbContextOptionsBuilder<TodoContext>()
               .UseInMemoryDatabase(databaseName: "TodolistItems")
               .Options;

            using var context = new TodoContext(optins);

            TodoServices TodoServices = new TodoServices(context);

            var results = TodoServices.PostItems(new TodoListItem
            {
                OrdersId = 3,
                AddDate = DateTime.Now,
                IsDone = false,
                UserID = 3,
                Title = "sad",
            });

            var actresults = TodoServices.GetItemsByUserId(3);

            Assert.That(actresults.ToList()[0].Title, Is.EqualTo("sad"));
        }

        [Test] // put 方法
        public void Put_ByTitle()
        {
            var optins = new DbContextOptionsBuilder<TodoContext>()
               .UseInMemoryDatabase(databaseName: "TodolistItems")
               .Options;

            using var context = new TodoContext(optins);

            context.TodoListItems.Add(new TodoListItem
            {
                OrdersId = 4,
                AddDate = DateTime.Now,
                IsDone = false,
                UserID = 4,
                Title = "sad",
            }) ;
            context.SaveChanges();

            TodoServices TodoServices = new TodoServices(context);

            var results = TodoServices.UpdateItemsById(4, new TodoListItem
            {
                OrdersId = 4,
                AddDate = DateTime.Now,
                IsDone = false,
                UserID = 4,
                Title = "sadly",
            });

            var actresults = TodoServices.GetItemsByUserId(4);

            Assert.That(actresults.ToList()[0].Title, Is.EqualTo("sad"));
        }

        [Test]
        public void Delete_ByTitle()
        {
            var optins = new DbContextOptionsBuilder<TodoContext>()
              .UseInMemoryDatabase(databaseName: "TodolistItems")
              .Options;

            using var context = new TodoContext(optins);

            context.TodoListItems.Add(new TodoListItem
            {
                OrdersId = 2,
                AddDate = DateTime.Now,
                IsDone = true,
                UserID = 2,
                Title = "test",
            });
            context.SaveChanges();

            TodoServices TodoServices = new TodoServices(context);

            var methods = TodoServices.DeleteItemsByTitle(2, "test");

            var results = TodoServices.GetItemsByUserId(2);

            Assert.GreaterOrEqual(results.Count(), 0);
        }
    }
}
