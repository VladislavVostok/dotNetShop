using Microsoft.AspNetCore.DataProtection;

namespace dotNetShop.Security
{
    public class AppSettingsEncryptionService
    {
        private readonly IDataProtector _protector;

        public AppSettingsEncryptionService(IDataProtectionProvider provider)
        {
            // Создание протектора с уникальным идентификатором
            _protector = provider.CreateProtector("MyUniqueProtector");
        }

        public string Encrypt(string plainText)
        {
            return _protector.Protect(plainText);
        }

        public string Decrypt(string cipherText)
        {
            return _protector.Unprotect(cipherText);
        }
    }
}
