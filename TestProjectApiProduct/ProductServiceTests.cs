using Api_Integration_Product.Models.ProductModels;
using Api_Integration_Product.Services.ProductServices;

namespace TestProjectApiProduct
{
    public class ProductServiceTests
    {
        // ─────────────────────────────────────────────────────────────
        // CreateAsync : doit enregistrer le produit et renvoyer un DTO correct
        // ─────────────────────────────────────────────────────────────
        [Fact]
        public async Task CreateAsync_ProduitValide_EnregistreEtRenvoieLeDto()
        {
            // Arrange : base en mémoire + service
            using var context = TestHelpers.CreateInMemoryContext();
            var service = new ProductService(context);

            var nouveauProduit = new CreateProductDto
            {
                Name = "Clavier mécanique",
                Description = "Switches rouges",
                Price = 79.90m,
                Stock = 10
            };

            // Act : on crée le produit pour l'utilisateur 5
            var resultat = await service.CreateAsync(nouveauProduit, userId: 5);

            // Assert : le DTO renvoyé reflète bien les données
            Assert.NotNull(resultat);
            Assert.Equal("Clavier mécanique", resultat.Name);
            Assert.Equal(79.90m, resultat.Price);
            Assert.Equal(10, resultat.Stock);
            Assert.Equal(5, resultat.UserId);
            Assert.True(resultat.Id > 0, "L'Id doit être généré par la base");
        }

        [Fact]
        public async Task CreateAsync_ProduitValide_EstBienPersisteEnBase()
        {
            // Arrange
            using var context = TestHelpers.CreateInMemoryContext();
            var service = new ProductService(context);
            var dto = new CreateProductDto { Name = "Souris", Price = 25m, Stock = 3 };

            // Act
            await service.CreateAsync(dto, userId: 1);

            // Assert : on vérifie directement le contenu de la base
            Assert.Single(context.Products);
            Assert.Equal("Souris", context.Products.First().Name);
        }

        // ─────────────────────────────────────────────────────────────
        // GetAllAsync : doit renvoyer tous les produits, vide au départ
        // ─────────────────────────────────────────────────────────────
        [Fact]
        public async Task GetAllAsync_BaseVide_RenvoieListeVide()
        {
            // Arrange
            using var context = TestHelpers.CreateInMemoryContext();
            var service = new ProductService(context);

            // Act
            var resultat = await service.GetAllAsync();

            // Assert
            Assert.Empty(resultat);
        }

        [Fact]
        public async Task GetAllAsync_AvecPlusieursProduits_LesRenvoieTous()
        {
            // Arrange : on insère 3 produits via le service
            using var context = TestHelpers.CreateInMemoryContext();
            var service = new ProductService(context);
            await service.CreateAsync(new CreateProductDto { Name = "P1", Price = 1m, Stock = 1 }, 1);
            await service.CreateAsync(new CreateProductDto { Name = "P2", Price = 2m, Stock = 2 }, 1);
            await service.CreateAsync(new CreateProductDto { Name = "P3", Price = 3m, Stock = 3 }, 2);

            // Act
            var resultat = await service.GetAllAsync();

            // Assert
            Assert.Equal(3, resultat.Count());
        }
    }
}
