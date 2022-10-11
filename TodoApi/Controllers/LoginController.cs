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
        private readonly LoginContext _loginContext;
        private readonly IConfiguration configuration;

        public LoginController(LoginContext loginContext, IConfiguration configuration)
        {
            _loginContext = loginContext;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var list = _loginContext.UserItems.FirstOrDefault(u => u.UserName == model.Username && u.PassWord == model.Password);
            if (list == null)
            {
                return BadRequest("账号和密码错误");
            }

            var authClaims = new List<Claim>
                   {
                    new Claim(ClaimTypes.Role, list.Permission!.ToString().Trim()),
                    new Claim("UserName",list.UserName!.ToString()),
                    new Claim("UserId", list.Id.ToString()),
                   };
            var token = GetToken(authClaims);
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpirationTime = token.ValidTo,
            });
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
