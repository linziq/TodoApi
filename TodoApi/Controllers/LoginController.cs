// <copyright file="LoginController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TodoApi.Controllers
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using TodoApi.Context;
    using TodoApi.Models;

    public class LoginController : ControllerBase
    {
        private readonly LoginContext loginContext;
        private readonly IConfiguration configuration;

        public LoginController(LoginContext loginContext, IConfiguration configuration)
        {
            this.loginContext = loginContext;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            List<UserItem> userItem = await this.loginContext.UserItems.ToListAsync();

            foreach (var item in userItem)
            {
                if (model.Username == item.UserName && model.Password == item.PassWord)
                {                   
                    var authClaims = new List<Claim>
                   {
                    new Claim(ClaimTypes.Role, item.Permission!.ToString().Trim()),
                     //new Claim("Role", item.Permission!.ToString().Trim()),
                    new Claim("UserName",item.UserName!.ToString()),
                    new Claim("UserId", item.Id.ToString()),
                   };
                    var token = GetToken(authClaims);
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        ExpirationTime = token.ValidTo,
                    });
                }
            }

            return BadRequest("不存在此用户，或者账号和密码错误");
        }

        // 拿到token字符串
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authScereKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])); // 登录秘钥从configuration里拿

            var token = new JwtSecurityToken(
                issuer: this.configuration["JWT:ValidIssuer"], // 签名地址
                audience: this.configuration["JWT:ValidAudience"], // 受众
                expires: DateTime.Now.AddHours(30), // 过期时间
                claims: authClaims, // 头部带的参数
                signingCredentials: new SigningCredentials(authScereKey, SecurityAlgorithms.HmacSha256)); // 签名证书
            return token;
        }
    }
}
