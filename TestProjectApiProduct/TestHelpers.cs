using Api_Integration_Product.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TestProjectApiProduct
{
    /// <summary>
    /// Outils partagés par les tests :
    ///  - une base EF Core EN MÉMOIRE (pas de vrai SQL Server nécessaire)
    ///  - une configuration JWT factice (AuthService en a besoin pour générer un token)
    /// </summary>
    public static class TestHelpers
    {
        // Crée un contexte EF Core en mémoire, isolé pour chaque test.
        // Le nom de base unique (Guid) évite que deux tests se partagent les mêmes données.
        public static MyAppContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<MyAppContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new MyAppContext(options);
        }

        // Fournit une configuration minimale avec les clés JWT attendues par AuthService.
        public static IConfiguration CreateTestConfiguration()
        {
            var settings = new Dictionary<string, string?>
            {
                // La clé doit être assez longue pour HmacSha256 (>= 32 caractères / 256 bits)
                ["Jwt:Key"] = "cle_de_test_ultra_secrete_de_32_caracteres_minimum!!",
                ["Jwt:Issuer"] = "TestIssuer",
                ["Jwt:Audience"] = "TestAudience"
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();
        }
    }
}
