using dotNetShop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;
using dotNetShop.Services;
using System.Text;


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
			var builder = WebApplication.CreateBuilder();

			
			//string connectionString = builder.Configuration["ProdConnectionString"];
			
						
			builder.Services.AddDbContext<ShopDBContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("MysqlConnectionString")));

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			#region
			//Secret Manager
			//builder.Configuration.AddUserSecrets<Program>();
			//string connectionString = builder.Configuration["DevConnectionString"];
			//builder.Services.AddDbContext<ShopDBContext>(options => options.UseMySQL(connectionString));


			//Переменные окружения
			//builder.Configuration.AddEnvironmentVariables();
			//string connectionString = builder.Configuration["ProdConnectionString"];
			//builder.Services.AddDbContext<ShopDBContext>(options => options.UseMySQL(connectionString));


			// Шифрование конфигурационного файла
			//builder.Services.AddDataProtection()
			//	.PersistKeysToFileSystem(new DirectoryInfo(@"C:\Users\Студент\Desktop\dotnetshop\very_secret_dont_worry")) // Укажите путь для хранения ключей
			//	.SetApplicationName("MyUniqueApp"); // Уникальное имя приложения

			//builder.Services.AddSingleton<AppSettingsEncryptionService>();
			#endregion

			var app = builder.Build();

			#region
			//var appSettingsEncryptionService = app.Services.GetRequiredService<AppSettingsEncryptionService>();
			//EncryptAppSettings(appSettingsEncryptionService);


			// Загрузка зашифрованного файла
			//var encryptedJson = File.ReadAllText("appsettings.json");
			//var decryptedJson = appSettingsEncryptionService.Decrypt(encryptedJson);

			//builder.Configuration.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(decryptedJson)));

			#endregion

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


		static void EncryptAppSettings(AppSettingsEncryptionService encryptionService)
		{
			// Путь к файлу appsettings.json
			var filePath = "appsettings.json";

			// Чтение содержимого файла
			var json = File.ReadAllText(filePath);

			// Шифрование содержимого
			var encryptedJson = encryptionService.Encrypt(json);

			// Запись зашифрованного содержимого обратно в файл
			File.WriteAllText(filePath, encryptedJson);
		}
	}
}
