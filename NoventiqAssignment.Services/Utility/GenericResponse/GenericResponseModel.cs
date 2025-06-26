using NoventiqAssignment.Services.DTOModels;

namespace NoventiqAssignment.Services.Utility
{
    public class GenericResponseModel<TResult> : BaseResponseModel
    {
        public GenericResponseModel()
        {
            Type t = typeof(TResult);
            if (t.GetConstructor(Type.EmptyTypes) != null)
                Data = Activator.CreateInstance<TResult>();
        }

        public TResult? Data { get; set; }

        public static GenericResponseModel<TResult> ErrorResponse(string errorMessage, int errorId = 1)
        {
            return new GenericResponseModel<TResult>
            {
                Message = errorMessage,
                ErrorList = new List<ErrorListModel>
            {
                new ErrorListModel
                {
                    Id = errorId,
                    Message = errorMessage ?? string.Empty
                }
            }
            };
        }

        public static GenericResponseModel<StatusMessageReturnDTO> ErrorResponseForStatus(string errorMessage, int errorId = 1)
        {
            return new GenericResponseModel<StatusMessageReturnDTO>
            {
                Data = new StatusMessageReturnDTO { Status = false },
                Message = errorMessage,
                ErrorList = new List<ErrorListModel>
            {
                new ErrorListModel
                {
                    Id = errorId,
                    Message = errorMessage ?? string.Empty
                }
            }
            };
        }


    }
}
