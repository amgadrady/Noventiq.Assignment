using NoventiqAssignment.Services.DTOModels;
using NoventiqAssignment.Services.Utility;

namespace NoventiqAssignment.Services
{
    public interface IProductService
    {
        Task<GenericResponseModel<ProductDto>> GetProductByIdAsync(int id);
        Task<GenericResponseModel<StatusMessageReturnDTO>> BulkUpdatePricesAsync(decimal percentage);
    }
}
