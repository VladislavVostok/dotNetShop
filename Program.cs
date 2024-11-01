using dotNetShop.Data;
using Microsoft.EntityFrameworkCore;


// dotnet tool install --global dotnet-ef    	Установка dotnet-ef инструменты для миграции
// dotnet ef migrations add InitialCreate		Создание файло миграции First Code
// dotnet ef database update					Применение миграции к базе данных

namespace dotNetShop
{
	public class Program
	{
		// Shalom
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddDbContext<ShopDBContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("MySqlConnectionString")));

			// Add services to the container.
			builder.Services.AddControllersWithViews();

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
