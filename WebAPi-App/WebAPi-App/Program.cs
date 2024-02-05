using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPi_App.Data;
using WebAPi_App.Models;
using WebAPi_App.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//CORS
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddDbContext<MyDBContext>(option =>
{
	option.UseSqlServer(builder.Configuration.GetConnectionString("MyDB"));
});

// Repository Pattern

//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

//inmemory static Tai 1 thoi diem 1 inteface chi co tuong ung co 1
builder.Services.AddScoped<ICategoryRepository, CategoryRepositoryInMemory>();

builder.Services.AddScoped<IItemRepository, ItemRepository>();

//Configure appsettings.json map voi appsetting.cs de dung secretKey
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSettings"));

var secretKey = builder.Configuration["AppSettings:SecretKey"];
var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
	opt.TokenValidationParameters = new TokenValidationParameters
	{
		//Tu cap token
		ValidateIssuer = false,
		ValidateAudience = false,

		//ký vào token
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

		ClockSkew = TimeSpan.Zero
	};
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
