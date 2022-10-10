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

        [Fact] // get
        public void GetItem_ByUserID()
        {
           var dbContext =  GetDbContext();

            // ���һ��UserIdΪ1��titleΪTest������
            dbContext.TodoListItems.Add(new TodoListItem
            {
                OrdersId = 1,
                AddDate = DateTime.Now,
                IsDone = true,
                UserID = 1,
                Title = "test",
            });

            // ���һ��UserIdΪ2��titleΪFriday������
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

        private TodoContext GetDbContext()
        {
            var optins = new DbContextOptionsBuilder<TodoContext>()
             .UseInMemoryDatabase(databaseName: "TodolistItems")
             .Options;
           return new TodoContext(optins);
        }

        [Fact] // Post 
        public void CreateItem_By_UserID_Items()
        {
            var results = _TodoServices.CreateItems(new TodoListItem
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
            var methods = _TodoServices.DeleteItemsByTitle(2,1);

            var results = _TodoServices.GetItemsByUserId(2);

            Assert.Equal(0, results.Count());
        }
    }
}