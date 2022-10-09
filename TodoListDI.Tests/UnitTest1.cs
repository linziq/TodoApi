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

            _todoContext = new TodoContext(optins);

            _sqlHelper = new SqlHelper(_todoContext);
        }

        [Fact]
        public void GetItem_ByUserID()
        {
            // ���һ��UserIdΪ1��titleΪTest������
            _todoContext.TodoListItems.Add(new TodoListItem
            {
                OrdersId = 1,
                AddDate = DateTime.Now,
                IsDone = true,
                UserID = 1,
                Title = "test",
            });

            // ���һ��UserIdΪ1��titleΪFriday������
            _todoContext.TodoListItems.Add(new TodoListItem
            {
                OrdersId = 2,
                AddDate = DateTime.Now,
                IsDone = false,
                UserID = 2,
                Title = "friday",
            });

            _todoContext.SaveChanges();

            var results = _sqlHelper.GetUserId(1);

            Assert.Equal("test", results.ToList()[0].Title);
        }

        [Fact] // Post 
        public void PostItem_By_UserID_Items()
        {
            var results = _sqlHelper.PostItems(new TodoListItem
            {
                OrdersId = 3,
                AddDate = DateTime.Now,
                IsDone = false,
                UserID = 3,
                Title = "sad",
            });

            var actresults = _sqlHelper.GetUserId(3);

            Assert.Equal("sad", actresults.ToList()[0].Title);
        }

        [Fact] //Put
        public void PutItem_ByOrdersID()
        {
            var result = _sqlHelper.UpdateItemsById(1, new TodoListItem
            {
                OrdersId = 1,
                AddDate = DateTime.Now,
                IsDone = false,
                UserID = 1,
                Title = "happy",
            });

            var actresult = _sqlHelper.GetUserId(1);

            Assert.Equal("happy", actresult.ToList()[0].Title);
           
        }

        [Fact] // Delect
        public void Delete_By_UserID_Title()
        {
            var methods = _sqlHelper.DeleteItemsByTitle(2, "friday");

            var results = _sqlHelper.GetUserId(2);

            Assert.Equal(0, results.Count());
        }
    }
}