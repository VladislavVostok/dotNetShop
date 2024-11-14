using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration.Json;
using System.Text.RegularExpressions;

namespace dotNetShop.Security
{
	public class ProtectedJsonConfigurationSource : JsonConfigurationSource
	{
		// Сервисный провайдер, используемый для получения IDataProtector
		public IServiceProvider? ServiceProvider { get; set; }

		// Действие для настройки IDataProtectionBuilder, если ServiceProvider отсутствует
		public Action<IDataProtectionBuilder>? DataProtectionBuildAction { get; set; }

		// Регулярное выражение, используемое для поиска и извлечения защищенных данных в формате Protected:{data}
		public Regex ProtectedRegex { get; set; } = new Regex(@"Protected:{(?<protectedData>.+?)}", RegexOptions.Compiled);

		// Метод Build создает и возвращает экземпляр ProtectedJsonConfigurationProvider
		public override IConfigurationProvider Build(IConfigurationBuilder builder)
		{
			// Проверяет, что указан либо ServiceProvider, либо DataProtectionBuildAction
			if (ServiceProvider == null && DataProtectionBuildAction == null)
			{
				// Если ни ServiceProvider, ни DataProtectionBuildAction не указаны, вызывает исключение
				throw new InvalidOperationException("Необходимо указать либо ServiceProvider, либо DataProtectionBuildAction для ProtectedJsonConfigurationSource.");
			}

			// Возвращает новый провайдер конфигурации с поддержкой защиты данных
			return new ProtectedJsonConfigurationProvider(Path, this);
		}
	}
}
