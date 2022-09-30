using Microsoft.EntityFrameworkCore;
using TodoApi.Context;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoList.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task Shuould_Return_All_Itemss_ById()
        {
            //使用内存数据库
            var optins = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(databaseName: "TodolistItems")
                .Options;
            //mock TodoContext
            using var context = new TodoContext(optins);

            context.TodoListItems.Add(new TodoListItem
            {
                OrdersId = 1,
                AddDate = DateTime.Now,
                IsDone = true,
                UserID = 1,
                Title = "sss"
            });
            context.TodoListItems.Add(new TodoListItem
            {
                OrdersId = 2,
                AddDate = DateTime.Now,
                IsDone = false,
                UserID = 2,
                Title = "friday"
            });
            context.SaveChanges();


            //mock service
            SqlHelper sqlHelper = new SqlHelper(context);
            //  var result = await sqlHelper.GetItemsById(1);
            var results = sqlHelper.GetUserId(1);
            //   var actualResult = results.Value;

            //Assert.IsTrue(1 == Convert.ToInt32((actualResult).UserID));
            //Assert.IsTrue("sss" == actualResult.Title);
            //Assert.IsTrue(actualResult.IsDone);
            Assert.Equals(1, results.Count());
            Assert.That(results.ToList()[0].Title, Is.EqualTo("sss"));
        }


    }
}