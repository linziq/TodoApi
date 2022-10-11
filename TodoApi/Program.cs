using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TodoApi.Context;
using TodoApi.Services;

var builder = WebApplication.CreateBuilder(args);  // 程序初始化设置
ConfigurationManager configuration = builder.Configuration;

var connectionString = builder.Configuration.GetConnectionString("SqlConn"); // build configuration
builder.Services.AddDbContext<TodoContext>(opt => opt.UseSqlServer(connectionString));
builder.Services.AddDbContext<LoginContext>(opt => opt.UseSqlServer(connectionString));

// authorization授权
builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("adminOrSuper", policy => policy.RequireRole("Admin", "SuperAdmin"));
});

// authentication身份验证
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
    };
});

//builder.Services.AddControllers()
//    .AddNewtonsoftJson(options =>
//    {
//        options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; // 格式化时间
//    });

builder.Services.AddScoped<ITodoServices, TodoServices>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "TodoApi",
        Version = "v1",
    });
    c.AddSecurityDefinition("JwtBearer", new OpenApiSecurityScheme()
    {
        Description = "在这里输入token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
    });
    var scheme = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference() { Type = ReferenceType.SecurityScheme, Id = "JwtBearer" }
    };
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        [scheme] = new string[0]
    });
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApi v1"));
}

app.UseHttpsRedirection();

app.UseAuthentication(); // 在前

app.UseAuthorization(); // 在后

app.MapControllers();

app.Run(); // 运行