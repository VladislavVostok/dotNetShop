using dotNetShop.Data;
using Microsoft.EntityFrameworkCore;
using dotNetShop.Services;
using dotNetShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;




// Либы БД
// dotnet add package Microsoft.EntityFrameworkCore
// dotnet add package Microsoft.EntityFrameworkCore.SqlServer
// dotnet add package MySql.EntityFrameworkCore
// 

// dotnet tool install --global dotnet-ef    	Установка dotnet-ef инструменты для миграции
// dotnet ef migrations add InitialCreate		Создание файло миграции First Code
// dotnet ef database update					Применение миграции к базе данных


// dotnet add package Microsoft.AspNetCore.Identity.EntityFramework		// Для MVC авторизации
// dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer		// Для JWT авторизации

namespace dotNetShop
{
	public static class Program
	{
		// Shalom
		public static void Main(string[] args)
		{

			// Создает и настраивает конфигурацию приложения с использованием зашифрованных JSON-файлов
			var configuration = new ConfigurationBuilder()
				.AddCommandLine(args) // Добавляет поддержку аргументов командной строки
				.AddJsonFile("appsettings.json", optional: true)
				.AddEnvironmentVariables() // Добавляет поддержку переменных окружения
				.Build(); // Строит окончательную конфигурацию
						
			var builder = WebApplication.CreateBuilder();




			builder.Services.AddDbContext<ShopDBContext>(options =>
						options.UseMySQL
						(
							builder.Configuration.GetConnectionString("MYSQLConnectionString")
						)
			);

			builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(
				options =>
				{
					options.Password.RequireNonAlphanumeric = true; //пароль должен содержать не буквенно-цифровые символы (например, подчеркивание)
					options.Password.RequireDigit = true; //пароль должен содержать цифры
					options.Password.RequiredLength = 10; //минимальная длина пароля
				}
				).AddEntityFrameworkStores<ShopDBContext>()
				.AddDefaultTokenProviders();            //Добавляем провайдер JWT


			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            }); ;


            builder.Services.AddAuthorization();

            // Add services to the container.
            builder.Services.AddControllersWithViews() ;

			#region Secret Manager, Переменные окружения
			//Secret Manager
			//builder.Configuration.AddUserSecrets<Program>();
			//string connectionString = builder.Configuration["DevConnectionString"];
			//builder.Services.AddDbContext<ShopDBContext>(options => options.UseMySQL(connectionString));

			//Переменные окружения
			//builder.Configuration.AddEnvironmentVariables();
			//string connectionString = builder.Configuration["ProdConnectionString"];
			//builder.Services.AddDbContext<ShopDBContext>(options => options.UseMySQL(connectionString));
			#endregion

			// Регистрация сервиса для работы с товарами
			builder.Services.AddScoped<IShopService, ShopService>();
			builder.Services.AddScoped<IContactService, ContactService>();
			builder.Services.AddScoped<ITokenService, TokenService>();


			var app = builder.Build();


			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
			}


			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");


			app.Run();
		}
	}
}
