using dotNetShop.Data;
using Microsoft.EntityFrameworkCore;
using dotNetShop.Services;
using dotNetShop.Models;




// Либы БД
// Microsoft.EntityFrameworkCore
// Microsoft.EntityFrameworkCore.SqlServer
// MySql.EntityFrameworkCore
// 

// dotnet tool install --global dotnet-ef    	Установка dotnet-ef инструменты для миграции
// dotnet ef migrations add InitialCreate		Создание файло миграции First Code
// dotnet ef database update					Применение миграции к базе данных


// Либы шифрование конфигов 
// Microsoft.AspNetCore.DataProtection
// Microsoft.Extensions.Configuration.Json
// Fededim.Extensions.Configuration.Protected
// Microsoft.AspNetCore.Identity.EntityFramework

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
				).AddEntityFrameworkStores<ShopDBContext>();

			// Add services to the container.
			builder.Services.AddControllersWithViews();

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

			//builder.Services.AddEndpointsApiExplorer();
			//builder.Services.AddSwaggerGen();

			var app = builder.Build();


			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
			}

			//app.UseSwagger();
			//app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
			//{
			//	options.SwaggerEndpoint("/swagger/v1/swagger.json", "DemoAPI v1");
			//	//options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
			//	options.RoutePrefix = string.Empty;
			//});

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
