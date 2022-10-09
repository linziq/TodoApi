namespace TodoListDI.Tests
{
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.EntityFrameworkCore;
    using TodoApi.Context;
    using TodoApi.Models;
    using TodoApi.Services;
    

    public class UnitTest1
    {
        private readonly TodoContext _todoContext;
        private readonly SqlHelper _sqlHelper;
        public UnitTest1()
        {
            var optins = new DbContextOptionsBuilder<TodoContext>()
              .UseInMemoryDatabase(databaseName: "TodolistItems")
              .Options;

            // 替代了链接SQL数据库，使用内存数据库模拟
            _todoContext = new TodoContext(optins);

            // 添加一条UserId为1，title为Test的数据
            _todoContext.TodoListItems.Add(new TodoListItem
            {
                OrdersId = 1,
                AddDate = DateTime.Now,
                IsDone = true,
                UserID = 1,
                Title = "test",
            });

            // 添加一条UserId为1，title为Friday的数据
            _todoContext.TodoListItems.Add(new TodoListItem
            {
                OrdersId = 2,
                AddDate = DateTime.Now,
                IsDone = false,
                UserID = 1,
                Title = "friday",
            });

            _todoContext.SaveChanges();

            _sqlHelper = new SqlHelper(_todoContext);
        }
        [Fact]
        public void GetItem_ByUserID()
        {
            var results = _sqlHelper.GetUserId(1);

            Assert.Equal("test", results.ToList()[0].Title);
        }
        [Fact] // Post 
        public void PostItem_ByUserID()
        {
            var results = _sqlHelper.PostItems(new TodoListItem
            {
                OrdersId = 3,
                AddDate = DateTime.Now,
                IsDone = false,
                UserID = 2,
                Title = "sad",
            });

            var actresults = _sqlHelper.GetUserId(2);

            Assert.Equal("sad", actresults.ToList()[0].Title);
        }

        [Fact] //Put
        public void PutItem_ByOrdersID()
        {
            var result = _sqlHelper.UpdateItemsById(2, new TodoListItem
            {
                OrdersId = 2,
                AddDate = DateTime.Now,
                IsDone = false,
                UserID = 1,
                Title = "happy",
            });

            var actresult = _sqlHelper.GetUserId(1);

            Assert.Equal("happy", actresult.ToList()[1].Title);
        }

        [Fact]
        public void Delete_ByTitle()
        {
            var methods = _sqlHelper.DeleteItemsByTitle(2, "sad");

            var results = _sqlHelper.GetUserId(2);

            Assert.Equal(0, results.Count());
        }
    }
}