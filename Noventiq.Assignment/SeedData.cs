using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoventiqAssignment.DB.Context;
using NoventiqAssignment.DB.Models;

namespace NoventiqAssignment.API
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            #region Seed Users, Roles

            
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();


            string[] roleNames = { "Administrator", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = roleName,
                        Description = $"{roleName} role"
                    });
                }
            }


            var adminEmail = "administrator@Noventiq.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin1",
                    LastName = "User",
                    EmailConfirmed = true,
                    CreatedDate = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrator");
                }
            }
            #endregion

            #region Seed Products
            var context = serviceProvider.GetRequiredService<NoventiqContext>();


            if (!await context.Products.AnyAsync())
            {
                var products = new List<Product>
    {
        new Product
        {
            Price = 19.99m,
            CreatedDate = DateTime.UtcNow,
            Translations = new List<ProductTranslation>
            {
                new ProductTranslation
                {
                    LanguageCode = "en",
                    Name = "Smartphone",
                    Description = "Latest model smartphone with advanced features"
                },
                new ProductTranslation
                {
                    LanguageCode = "fr",
                    Name = "Smartphone",
                    Description = "Smartphone dernier modèle avec des fonctionnalités avancées"
                },
                new ProductTranslation
                {
                    LanguageCode = "hi",
                    Name = "स्मार्टफोन",
                    Description = "उन्नत सुविधाओं वाला नवीनतम मॉडल स्मार्टफोन"
                }
            }
        },
        new Product
        {
            Price = 99.99m,
            CreatedDate = DateTime.UtcNow,
            Translations = new List<ProductTranslation>
            {
                new ProductTranslation
                {
                    LanguageCode = "en",
                    Name = "Laptop",
                    Description = "High-performance laptop for professionals"
                },
                new ProductTranslation
                {
                    LanguageCode = "fr",
                    Name = "Ordinateur portable",
                    Description = "Ordinateur portable haute performance pour les professionnels"
                },
                new ProductTranslation
                {
                    LanguageCode = "hi",
                    Name = "लैपटॉप",
                    Description = "पेशेवरों के लिए उच्च प्रदर्शन वाला लैपटॉप"
                }
            }
        },
        new Product
        {
            Price = 9.99m,
            CreatedDate = DateTime.UtcNow,
            Translations = new List<ProductTranslation>
            {
                new ProductTranslation
                {
                    LanguageCode = "en",
                    Name = "Headphones",
                    Description = "Noise-cancelling wireless headphones"
                },
                new ProductTranslation
                {
                    LanguageCode = "fr",
                    Name = "Écouteurs",
                    Description = "Écouteurs sans fil avec réduction de bruit"
                },
                new ProductTranslation
                {
                    LanguageCode = "hi",
                    Name = "हेडफोन",
                    Description = "शोर रद्द करने वाले वायरलेस हेडफोन"
                }
            }
        }
    };

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }

            #endregion
        }
    }
}
