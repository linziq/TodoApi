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
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            return _ITodoServices.GetItemsByUserId(userId);
        }

        [HttpPost] // 添加
        public async Task<ActionResult<TodoListItem>> PostTodoitem(TodoListItem todoitem) // 更新PostTodoitem create方法
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            todoitem.UserID = userId;
            return await _ITodoServices.PostItems(todoitem);
        }

        [HttpPut] // 修改

        public async Task<IActionResult> PutTodoitem(int id, TodoListItem todoitem)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            todoitem.UserID = userId;
            await _ITodoServices.UpdateItemsById(id, todoitem);
            return Ok(id);
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
    }
}
