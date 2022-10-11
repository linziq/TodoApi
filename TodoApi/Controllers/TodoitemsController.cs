namespace TodoApi.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TodoApi.Models;
    using TodoApi.Services;

    [Authorize]
    [Route("api/[controller]")] // 路由
    [ApiController]
    public class TodoitemsController : ControllerBase
    {
        private readonly ITodoServices _ITodoServices;
        public TodoitemsController(ITodoServices IToddoServices) // 构造函数注入接口
        {
            _ITodoServices = IToddoServices;
        }
        //   var identity = HttpContext.User.Identity as ClaimsIdentity;
        [HttpGet]
        public IEnumerable<TodoListItem> GetTodoListItems()
        {
            // HttpContext.User.Claims  Claim 是这些声明对象的列表(以list的方式表现)。       Request
            // int ID = Convert.ToInt32(User.Claims.ToList()[2].Value); // 对应type =="UserId",但申明通常被认为无序，应该按类型
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value); // HttpContext.User
            var result = _ITodoServices.GetItemsByUserId(userId);
            return result;
        }

        [HttpPost] // 添加 
        public async Task<ActionResult> PostTodoitem(TodoListItem todoitem) // 更新PostTodoitem create方法
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            todoitem.UserID = userId;
            await _ITodoServices.CreateItems(todoitem);
            return Ok("添加成功");
        }

        [HttpPut] // 修改
        public async Task<IActionResult> PutTodoitem(int PrimaryID, TodoListItem todoitem)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            todoitem.UserID = userId;
            await _ITodoServices.UpdateItemsById(PrimaryID, todoitem);
            return Ok("修改成功");
        }

        [HttpDelete]
        public async Task<IActionResult> Deleteitem(int PrimaryID)
        {
            await _ITodoServices.DeleteItemsByPrimaryID(PrimaryID);
            return Ok("已成功删除");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllTable")]
        public async Task<ActionResult<IEnumerable<TodoListItem>>> GetItemsAll()
        {
            var result = await _ITodoServices.GetItems();
            return result.ToList();
        }
    }
}
