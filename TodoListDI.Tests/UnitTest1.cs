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
        private readonly TodoServices _TodoServices;
        public UnitTest1()
        {
            var optins = new DbContextOptionsBuilder<TodoContext>()
              .UseInMemoryDatabase(databaseName: "TodolistItems")
              .Options;

            _todoContext = new TodoContext(optins);

            _TodoServices = new TodoServices(_todoContext);
        }

        [Fact]
        public void GetItem_ByUserID()
        {
            // 添加一条UserId为1，title为Test的数据
            _todoContext.TodoListItems.Add(new TodoListItem
            {
                OrdersId = 1,
                AddDate = DateTime.Now,
                IsDone = true,
                UserID = 1,
                Title = "test",
            });

            // 添加一条UserId为2，title为Friday的数据
            _todoContext.TodoListItems.Add(new TodoListItem
            {
                OrdersId = 2,
                AddDate = DateTime.Now,
                IsDone = false,
                UserID = 2,
                Title = "friday",
            });

            _todoContext.SaveChanges();

            var results = _TodoServices.GetItemsByUserId(1);

            Assert.Equal("test", results.ToList()[0].Title);
        }

        [Fact] // Post 
        public void PostItem_By_UserID_Items()
        {
            var results = _TodoServices.PostItems(new TodoListItem
            {
                OrdersId = 3,
                AddDate = DateTime.Now,
                IsDone = false,
                UserID = 3,
                Title = "sad",
            });

            var actresults = _TodoServices.GetItemsByUserId(3);

            Assert.Equal("sad", actresults.ToList()[0].Title);
        }

        [Fact] //Put
        public void PutItem_ByOrdersID()
        {
            var result = _TodoServices.UpdateItemsById(3, new TodoListItem
            {
                AddDate = DateTime.Now,
                IsDone = false,
                UserID = 3,
                Title = "happy",
            });

            var actresult = _TodoServices.GetItemsByUserId(3);

            Assert.Equal("happy", actresult.ToList()[0].Title);

        }

        [Fact] // Delect
        public void Delete_By_UserID_Title()
        {
            var methods = _TodoServices.DeleteItemsByTitle(2, "friday");

            var results = _TodoServices.GetItemsByUserId(2);

            Assert.Equal(0, results.Count());
        }
    }
}