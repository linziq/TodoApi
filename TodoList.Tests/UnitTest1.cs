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
            var optins = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(databaseName: "TodolistItems")
                .Options;

            using (var context = new TodoContext(optins))
            {
                context.TodoListItems.Add(new TodoListItem
                {
                    OrdersId = 1,
                    AddDate = DateTime.Now,
                    IsDone = true,
                    UserID = 1,
                    Title = "sss"
                });
                context.SaveChanges();
            }

            using (var context = new TodoContext(optins))
            {
                //mock service
               SqlHelper sqlHelper= new SqlHelper(context);
               var result=await sqlHelper.GetItemsById(1);
                
                var actualResult = result.Value;

                Assert.IsTrue(1==Convert.ToInt32((actualResult).UserID));
                Assert.IsTrue("sss"==actualResult.Title);
                Assert.IsTrue(actualResult.IsDone);
            }     
        }


    }
}