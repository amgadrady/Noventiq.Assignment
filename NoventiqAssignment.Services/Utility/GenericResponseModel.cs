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

        public GenericResponseModel<TResult> ExceptionError(TResult data, string errorMessage, int errorId = 0)
        {
            return new GenericResponseModel<TResult>()
            {
                Data = data,
                Message = errorMessage,
                ErrorList = new List<ErrorListModel>()
                    {
                        new ErrorListModel()
                        {
                            Id = errorId,
                            Message = errorMessage != null ? errorMessage : string.Empty

                        }
                    }
            };
        }

        public GenericResponseModel<StatusMessageReturnDTO> ExceptionError(string errorMessage, int errorId = 0)
        {
            var response = new GenericResponseModel<StatusMessageReturnDTO>()
            {
                Message = errorMessage,
                ErrorList = new List<ErrorListModel>()
                {
                    new ErrorListModel ()
                    {
                        Id = errorId,
                        Message = errorMessage != null ? errorMessage : string.Empty
                    }
                }
            };
            response.Data.status = false;
            return response;
        }
    }
}
