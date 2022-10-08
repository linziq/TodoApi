namespace TodoList.Tests
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using TodoApi.Context;
    using TodoApi.Models;
    using TodoApi.Services;

    public class Tests
    {
        [Test] // Get
        public void Shuould_Return_All_Itemss_ById()
        {
          
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
            SqlHelper sqlHelper = new SqlHelper(context);

            var results = sqlHelper.GetUserId(1);

            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.ToList()[0].Title, Is.EqualTo("test"));
        }

        [Test] // post����
        public void PostItems_ByUserId()
        {
            var optins = new DbContextOptionsBuilder<TodoContext>()
               .UseInMemoryDatabase(databaseName: "TodolistItems")
               .Options;

            using var context = new TodoContext(optins);

            SqlHelper sqlHelper = new SqlHelper(context);

            var results = sqlHelper.PostItems(new TodoListItem
            {
                OrdersId = 2,
                AddDate = DateTime.Now,
                IsDone = false,
                UserID = 2,
                Title = "sad",
            });

            var actresults = sqlHelper.GetUserId(2);

            Assert.That(actresults.ToList()[0].Title, Is.EqualTo("sad"));
        }

        [Test] // put ����

        public void Put_ByTitle()
        {
            var optins = new DbContextOptionsBuilder<TodoContext>()
               .UseInMemoryDatabase(databaseName: "TodolistItems")
               .Options;

            using var context = new TodoContext(optins);

            SqlHelper sqlHelper = new SqlHelper(context);

            var results = sqlHelper.UpdateItemsById(2, new TodoListItem
            {
                OrdersId = 2,
                AddDate = DateTime.Now,
                IsDone = false,
                UserID = 2,
                Title = "sadly",
            });

            var actresults = sqlHelper.GetUserId(2);

            Assert.That(actresults.ToList()[0].Title, Is.EqualTo("sadly"));
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

            SqlHelper sqlHelper = new SqlHelper(context);

            var methods = sqlHelper.DeleteItemsByTitle(2, "test");

            var results = sqlHelper.GetUserId(2);

            Assert.GreaterOrEqual(results.Count(), 0);  
        }
    }
}
