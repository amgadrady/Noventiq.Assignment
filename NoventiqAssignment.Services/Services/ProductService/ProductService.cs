using Microsoft.EntityFrameworkCore;
using NoventiqAssignment.DB.Context;
using NoventiqAssignment.DB.Models;
using NoventiqAssignment.Services.DTOModels;
using NoventiqAssignment.Services.Utility;
using NoventiqAssignment.UnitOfWork;
using System.Linq.Expressions;

namespace NoventiqAssignment.Services
{
    public class ProductService : IProductService
    {
        private readonly IMultiLanguageService multiLanguageService;
        private readonly IUnitOfWork unitOfWork;

        public ProductService(IMultiLanguageService multiLanguageService, IUnitOfWork unitOfWork)
        {

            this.multiLanguageService = multiLanguageService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<GenericResponseModel<ProductDto>> GetProductByIdAsync(int id)
        {
            var responseModel = new GenericResponseModel<ProductDto>();
            var languageCode = multiLanguageService.GetCurrentLanguage();

            var expressions = new List<Expression<Func<Product, bool>>>();
            expressions.Add(x => x.Id == id);
            var includes = new List<Func<IQueryable<Product>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Product, object>>> { c => 
            c.Include(c => c.Translations
            .Where(t => t.LanguageCode == languageCode)) };

            var product = await unitOfWork.Repository<Product>().GetData(filters: expressions, include: includes);


            if (product.Count() == 0)
            {
                return GenericResponseModel<ProductDto>.ErrorResponse(
                    "Product not found");
            }
            
            var firstProduct = product.FirstOrDefault();
                var translation = firstProduct.Translations.FirstOrDefault();
                responseModel.Data = new ProductDto
                {
                    Id = firstProduct.Id,
                    Name = translation?.Name ?? string.Empty,
                    Description = translation?.Description ?? string.Empty,
                    Price = firstProduct.Price
                };

            return responseModel;

        }

        public async Task<GenericResponseModel<StatusMessageReturnDTO>> BulkUpdatePricesAsync(decimal percentage)
        {
            try
            {
                var responseModel = new GenericResponseModel<StatusMessageReturnDTO>();
                
                string sql = "UPDATE Products SET Price = Price * {0}";
                int affectedRows = await unitOfWork.Repository<Product>().ExecuteRawSqlAsync(sql, percentage);

                
                await unitOfWork.SaveChangesAsync();
                responseModel.Data.Status = true;
                return responseModel;
            }
            catch (Exception ex)
            {
                return GenericResponseModel<StatusMessageReturnDTO>.ErrorResponseForStatus(
                    "Error updating product prices");
            }
        }
    }
}
