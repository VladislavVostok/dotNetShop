using Microsoft.AspNetCore.DataProtection;

namespace dotNetShop.Security
{
	public class ProtectedJsonConfigurationProvider : ConfigurationProvider
	{
		// IDataProtector используется для защиты данных, таких как шифрование и дешифрование
		private readonly IDataProtector DataProtector;

		// Строковая константа, определяющая цель защиты данных
		public static readonly string DataProtectionPurpose = "ProtectedJsonConfiguration";

		// Конструктор провайдера ProtectedJsonConfigurationProvider принимает источник конфигурации
		public ProtectedJsonConfigurationProvider(ProtectedJsonConfigurationSource source)
		{
			// Если источник имеет действие по настройке защиты данных
			if (source.DataProtectionBuildAction != null)
			{
				// Создает новую коллекцию сервисов
				var services = new ServiceCollection();

				// Настраивает защиту данных с помощью предоставленного действия
				source.DataProtectionBuildAction(services.AddDataProtection());

				// Создает провайдер сервисов из коллекции, содержащей защиту данных
				source.ServiceProvider = services.BuildServiceProvider();
			}
			else if (source.ServiceProvider == null)
			{
				// Если нет провайдера сервисов или действия для настройки защиты данных, бросает исключение
				throw new ArgumentNullException(nameof(source.ServiceProvider));
			}

			// Получает IDataProtector из провайдера сервисов и задает цель защиты данных
			DataProtector = source.ServiceProvider
				.GetRequiredService<IDataProtectionProvider>()
				.CreateProtector(DataProtectionPurpose);
		}

		// Метод Load загружает данные конфигурации и расшифровывает зашифрованные значения
		public override void Load()
		{
			base.Load();

			// Проходит по всем ключам и проверяет наличие зашифрованных данных
			foreach (var key in Data.Keys.ToList())
			{
				var value = Data[key];

				// Проверяет, начинается ли значение с "Protected:{" и заканчивается "}", что указывает на зашифрованные данные
				if (!string.IsNullOrEmpty(value) && value.StartsWith("Protected:{") && value.EndsWith("}"))
				{
					// Извлекает зашифрованные данные, удаляя префикс и суффикс
					var encryptedData = value.Substring(10, value.Length - 11);

					// Расшифровывает данные с помощью IDataProtector
					var decryptedData = DataProtector.Unprotect(encryptedData);

					// Заменяет зашифрованные данные расшифрованными значениями в конфигурации
					Data[key] = decryptedData;
				}
			}
		}
	}
}
