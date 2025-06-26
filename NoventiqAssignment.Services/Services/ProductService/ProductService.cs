using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NoventiqAssignment.DB.Context;
using NoventiqAssignment.Services.DTOModels;
using NoventiqAssignment.Services.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoventiqAssignment.Services
{
    public class ProductService : IProductService
    {
        private readonly NoventiqContext noventiqContext;
        private readonly IMultiLanguageService multiLanguageService;

        public ProductService(NoventiqContext noventiqContext, IMultiLanguageService multiLanguageService)
        {
            this.noventiqContext = noventiqContext;
            this.multiLanguageService = multiLanguageService;
        }

        public async Task<GenericResponseModel<ProductDto>> GetProductByIdAsync(int id)
        {
            var responseModel = new GenericResponseModel<ProductDto>();
            var languageCode = multiLanguageService.GetCurrentLanguage();

            var product = await noventiqContext.Products
            .AsNoTracking()
            .Include(p => p.Translations
                .Where(t => t.LanguageCode == languageCode))
            .FirstOrDefaultAsync(p => p.Id == id);


            if (product == null)
            {
                return GenericResponseModel<ProductDto>.ErrorResponse(
                    "Product not found");
            }
            
            responseModel.Data = new ProductDto
            {
                Id = product.Id,
                Name = product.Translations.FirstOrDefault().Name,
                Description = product.Translations.FirstOrDefault().Description,
                Price = product.Price

            };

            return responseModel;

        }
    }
}
