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
        public IQueryable<TodoListItem> GetTodoListItems()
        {
            // HttpContext.User.Claims  Claim 是这些声明对象的列表(以list的方式表现)。       Request
            // int identityID = Convert.ToInt32(User.Claims.ToList()[2].Value); // 对应type =="UserId",但申明通常被认为无序，应该按类型
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value); // HttpContext.User
            return _ITodoServices.GetItemsByUserId(userId);
        }

        [HttpPost] // 添加 
        public async Task<ActionResult<TodoListItem>> PostTodoitem(TodoListItem todoitem) // 更新PostTodoitem create方法
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            todoitem.UserID = userId;
            await _ITodoServices.PostItems(todoitem);
            return Ok("添加成功");
        }

        [HttpPut] // 修改
        public async Task<IActionResult> PutTodoitem(int id, TodoListItem todoitem)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            todoitem.UserID = userId;
            await _ITodoServices.UpdateItemsById(id, todoitem);
            return Ok("修改成功");
        }

        [HttpDelete]
        public async Task<IActionResult> Deleteitem(string tiele)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

            var result = await _ITodoServices.DeleteItemsByTitle(userId, tiele);
            if (result == null)
            {
                return NotFound("请输入正确的值");
            }
            else
            {
                return Ok("已成功删除");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllTable")]
        public async Task<ActionResult<IEnumerable<TodoListItem>>> GetItemsAll()
        {
            return await _ITodoServices.GetItems();
        }
    }
}
