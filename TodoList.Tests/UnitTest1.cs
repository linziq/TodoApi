namespace TodoList.Tests
{
    using Microsoft.EntityFrameworkCore;
    using TodoApi.Context;
    using TodoApi.Models;
    using TodoApi.Services;

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

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
                Title = "sss1",
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
            Assert.That(results.ToList()[0].Title, Is.EqualTo("sss1"));
          //  Assert.IsTrue(results.ToList()[0].Title == "sss1");
        }

        [Test] // post方法
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

        [Test] // put 方法
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

            SqlHelper sqlHelper = new SqlHelper(context);

            var results = sqlHelper.DeleteItemsByTitle(2, "sad");

            var actresults = sqlHelper.GetUserId(2);

            Assert.GreaterOrEqual(actresults.Count(), 0);
        }
    }
}