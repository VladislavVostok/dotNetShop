using Microsoft.AspNetCore.DataProtection;
using System.Text.Json;

namespace dotNetShop.Security
{
	public class ProtectedJsonConfigurationProvider : ConfigurationProvider
	{
		// IDataProtector используется для защиты данных, таких как шифрование и дешифрование
		private readonly IDataProtector DataProtector;
        private readonly string FilePath; // Новый член для хранения пути к файлу

                                          
        public static readonly string DataProtectionPurpose = "ProtectedJsonConfiguration";   // Строковая константа, определяющая цель защиты данных

        // Конструктор провайдера ProtectedJsonConfigurationProvider принимает источник конфигурации
        public ProtectedJsonConfigurationProvider(string filePath, ProtectedJsonConfigurationSource source)
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

            FilePath = filePath;
        }

        // Метод Load загружает данные конфигурации и расшифровывает зашифрованные значения
        public override void Load()
        {
            // Проверяет наличие конфигурационного файла, указанного в source.Path
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException($"JSON configuration file '{FilePath}' not found.");
            }

            // Считывает содержимое JSON-файла в строку
            var jsonContent = File.ReadAllText(FilePath);

            // Десериализует JSON-содержимое в словарь
            //var data = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent);
            //if (data == null) return;

            // Используем JsonDocument для обработки сложной структуры JSON
            using (var document = JsonDocument.Parse(jsonContent))
            {
                var data = new Dictionary<string, string>();
                ProcessJsonElement(document.RootElement, data, null);

                // Обрабатывает каждый ключ-значение
                foreach (var kvp in data)
                {
                    var key = kvp.Key;
                    var value = kvp.Value;

                    // Проверяет, зашифрованы ли данные (заключены в "Protected:{}")
                    if (!string.IsNullOrEmpty(value) && value.StartsWith("Protected:{") && value.EndsWith("}"))
                    {
                        var encryptedData = value.Substring(11, value.Length - 12);
                        var decryptedData = DataProtector.Unprotect(encryptedData);
                        Data[key] = decryptedData;
                    }
                    else
                    {
                        Data[key] = value;
                    }
                }
            }

        }

        // Рекурсивно обрабатывает элементы JSON, включая вложенные объекты
        private void ProcessJsonElement(JsonElement element, Dictionary<string, string> data, string? parentKey)
        {
            foreach (var property in element.EnumerateObject())
            {
                var key = parentKey == null ? property.Name : $"{parentKey}:{property.Name}";

                if (property.Value.ValueKind == JsonValueKind.Object)
                {
                    // Рекурсивно обрабатывает вложенные объекты
                    ProcessJsonElement(property.Value, data, key);
                }
                else
                {
                    // Добавляет значение в словарь как строку
                    data[key] = property.Value.ToString();
                }
            }
        }

    }
}
