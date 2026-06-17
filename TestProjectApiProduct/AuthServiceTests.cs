using Api_Integration_Product.Models.AuthModels;
using Api_Integration_Product.Models.UserModels;
using Api_Integration_Product.Services.AuthServices;

namespace TestProjectApiProduct
{
    public class AuthServiceTests
    {
        // Petit utilitaire : construit un AuthService prêt à l'emploi (contexte + config JWT)
        private static AuthService CreateService(out Api_Integration_Product.Data.MyAppContext context)
        {
            context = TestHelpers.CreateInMemoryContext();
            var config = TestHelpers.CreateTestConfiguration();
            return new AuthService(context, config);
        }

        // ─────────────────────────────────────────────────────────────
        // RegisterAsync : cas nominal
        // ─────────────────────────────────────────────────────────────
        [Fact]
        public async Task RegisterAsync_NouvelUtilisateur_CreeEtRenvoieUnToken()
        {
            // Arrange
            var service = CreateService(out var context);
            var form = new RegisterDTO { Username = "zac", Email = "zac@mail.com", Password = "Secret123" };

            // Act
            var resultat = await service.RegisterAsync(form);

            // Assert : réponse correcte
            Assert.Equal("zac", resultat.Username);
            Assert.Equal("User", resultat.Role);             // rôle par défaut
            Assert.False(string.IsNullOrEmpty(resultat.Token)); // un JWT a été généré

            // Assert : l'utilisateur est en base et le mot de passe est HASHÉ (jamais en clair)
            var enBase = context.Users.Single();
            Assert.Equal("zac@mail.com", enBase.Email);
            Assert.NotEqual("Secret123", enBase.PasswordHash);
            Assert.True(BCrypt.Net.BCrypt.Verify("Secret123", enBase.PasswordHash));
        }

        [Fact]
        public async Task RegisterAsync_EmailDejaUtilise_LeveInvalidOperationException()
        {
            // Arrange : un utilisateur existe déjà avec cet email
            var service = CreateService(out var context);
            context.Users.Add(new User { Username = "autre", Email = "zac@mail.com", PasswordHash = "x" });
            await context.SaveChangesAsync();

            var form = new RegisterDTO { Username = "zac", Email = "zac@mail.com", Password = "Secret123" };

            // Act + Assert : l'inscription doit échouer
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.RegisterAsync(form));
            Assert.Equal("Email déja utilisé!", ex.Message);
        }

        [Fact]
        public async Task RegisterAsync_UsernameDejaUtilise_LeveInvalidOperationException()
        {
            // Arrange
            var service = CreateService(out var context);
            context.Users.Add(new User { Username = "zac", Email = "autre@mail.com", PasswordHash = "x" });
            await context.SaveChangesAsync();

            var form = new RegisterDTO { Username = "zac", Email = "zac@mail.com", Password = "Secret123" };

            // Act + Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.RegisterAsync(form));
            Assert.Equal("Ce nom d'utilisateur est déja utilisé!", ex.Message);
        }

        // ─────────────────────────────────────────────────────────────
        // LoginAsync : cas nominal + échecs
        // ─────────────────────────────────────────────────────────────
        [Fact]
        public async Task LoginAsync_IdentifiantsCorrects_RenvoieUnToken()
        {
            // Arrange : on inscrit d'abord un utilisateur
            var service = CreateService(out _);
            await service.RegisterAsync(new RegisterDTO
            {
                Username = "zac",
                Email = "zac@mail.com",
                Password = "Secret123"
            });

            // Act : on se connecte avec les bons identifiants
            var resultat = await service.LoginAsync(new LoginDTO
            {
                Email = "zac@mail.com",
                Password = "Secret123"
            });

            // Assert
            Assert.False(string.IsNullOrEmpty(resultat.Token));
            Assert.Equal("zac", resultat.Username);
        }

        [Fact]
        public async Task LoginAsync_MauvaisMotDePasse_LeveUnauthorizedAccessException()
        {
            // Arrange
            var service = CreateService(out _);
            await service.RegisterAsync(new RegisterDTO
            {
                Username = "zac",
                Email = "zac@mail.com",
                Password = "Secret123"
            });

            // Act + Assert : mauvais mot de passe => 401
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => service.LoginAsync(new LoginDTO { Email = "zac@mail.com", Password = "MAUVAIS" }));
            Assert.Equal("Email ou mot de passe invalide", ex.Message);
        }

        [Fact]
        public async Task LoginAsync_EmailInexistant_LeveUnauthorizedAccessException()
        {
            // Arrange : aucun utilisateur en base
            var service = CreateService(out _);

            // Act + Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => service.LoginAsync(new LoginDTO { Email = "inconnu@mail.com", Password = "peu importe" }));
        }
    }
}
