using dotNetShop.Data;
using Microsoft.EntityFrameworkCore;
using dotNetShop.Services;
using Microsoft.Extensions.Options;

// Подключает API для защиты данных (например, шифрование конфиденциальной информации)
using Microsoft.AspNetCore.DataProtection;
using dotNetShop.Security;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using System.Text.RegularExpressions;



// Либы БД
// Microsoft.EntityFrameworkCore
// Microsoft.EntityFrameworkCore.SqlServer
// 
// 

// dotnet tool install --global dotnet-ef    	Установка dotnet-ef инструменты для миграции
// dotnet ef migrations add InitialCreate		Создание файло миграции First Code
// dotnet ef database update					Применение миграции к базе данных


// Либы шифрование конфигов 
// Microsoft.AspNetCore.DataProtection
// Microsoft.Extensions.Configuration.Json
// Fededim.Extensions.Configuration.Protected


namespace dotNetShop
{
	public static class Program
	{
		// Shalom
		public static void Main(string[] args)
		{
			// Создает и настраивает службу защиты данных
			var servicesDataProtection = new ServiceCollection();
			ConfigureDataProtection(servicesDataProtection.AddDataProtection());
			var serviceProviderDataProtection = servicesDataProtection.BuildServiceProvider();

			// Создает защитник данных для конкретной цели (используется для шифрования/дешифрования)
			var dataProtector = serviceProviderDataProtection.GetRequiredService<IDataProtectionProvider>()
				.CreateProtector(ProtectedJsonConfigurationProvider.DataProtectionPurpose);

			// Шифрует данные в JSON-файлах перед загрузкой конфигурации
			dataProtector.ProtectFiles(".");

			// Создает и настраивает конфигурацию приложения с использованием зашифрованных JSON-файлов
			var configuration = new ConfigurationBuilder()
				.AddCommandLine(args) // Добавляет поддержку аргументов командной строки
				.AddProtectedJsonFile("appsettings.json", ConfigureDataProtection) // Добавляет зашифрованный JSON
				.AddProtectedJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT")}.json", ConfigureDataProtection) // Добавляет зашифрованный JSON для конкретного окружения
				.AddEnvironmentVariables() // Добавляет поддержку переменных окружения
				.Build(); // Строит окончательную конфигурацию

			// Создает контейнер сервисов DI и регистрирует конфигурацию AppSettings
			var services = new ServiceCollection();
			services.Configure<AppSettings>(configuration); // Привязывает конфигурацию к типу AppSettings
			var serviceProvider = services.BuildServiceProvider();

			// Извлекает привязанные настройки AppSettings из контейнера сервисов
			var appSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value;

			var builder = WebApplication.CreateBuilder();

			// Повторная настройка защиты данных
			IDataProtectionBuilder dataProtectionBuilder = servicesDataProtection.AddDataProtection();
			ConfigureDataProtection(dataProtectionBuilder);

			builder.Services.AddDbContext<ShopDBContext>(options => 
						options.UseSqlServer(
							configuration.GetConnectionString("SQLSERVERConnectionString")
						)
			);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			# region Secret Manager, Переменные окружения
			//Secret Manager
			//builder.Configuration.AddUserSecrets<Program>();
			//string connectionString = builder.Configuration["DevConnectionString"];
			//builder.Services.AddDbContext<ShopDBContext>(options => options.UseMySQL(connectionString));


			//Переменные окружения
			//builder.Configuration.AddEnvironmentVariables();
			//string connectionString = builder.Configuration["ProdConnectionString"];
			//builder.Services.AddDbContext<ShopDBContext>(options => options.UseMySQL(connectionString));


			#endregion

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


		// Метод для настройки защиты данных
		private static void ConfigureDataProtection(IDataProtectionBuilder builder)
		{
			builder.UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
			{
				EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC, // Использует AES 256 для шифрования
				ValidationAlgorithm = ValidationAlgorithm.HMACSHA256, // Использует HMAC SHA256 для проверки подлинности
			}).SetDefaultKeyLifetime(TimeSpan.FromDays(365 * 15)) // Устанавливает срок действия ключей на 15 лет
			  .PersistKeysToFileSystem(new DirectoryInfo("..\\Keys")); // Указывает путь для хранения ключей
		}


		// Метод расширения для добавления зашифрованных JSON-файлов в конфигурацию
		public static IConfigurationBuilder AddProtectedJsonFile(this IConfigurationBuilder builder, string path, Action<IDataProtectionBuilder> configureDataProtection)
		{
			var source = new ProtectedJsonConfigurationSource
			{
				Path = path, // Указывает путь к JSON-файлу
				Optional = false, // Указывает, что файл обязателен
				ReloadOnChange = true, // Обновляет конфигурацию при изменении файла
				DataProtectionBuildAction = configureDataProtection // Настройка защиты данных
			};

			return builder.Add(source); // Добавляет источник конфигурации
		}


		// Метод расширения для шифрования данных в JSON-файлах
		public static void ProtectFiles(this IDataProtector protector, string directoryPath)
		{
			var files = Directory.GetFiles(directoryPath, "*.json"); // Получает все JSON-файлы в директории

			foreach (var file in files)
			{
				var fileContent = File.ReadAllText(file); // Читает содержимое файла
				var regex = new Regex(@"Protect:{(?<protectedData>.+?)}", RegexOptions.Compiled); // Паттер для поиска данных в Protected

				var protectedContent = regex.Replace(fileContent, match =>
				{
					var dataToEncrypt = match.Groups["protectedData"].Value; // Извлекает данные для шифрования
					var encryptedData = protector.Protect(dataToEncrypt); // Шифрует данные
					return $"Protected:{{{encryptedData}}}"; // Вставляет зашифрованные данные
				});

				File.WriteAllText(file, protectedContent); // Записывает обратно зашифрованные данные
			}
		}

	}
}
