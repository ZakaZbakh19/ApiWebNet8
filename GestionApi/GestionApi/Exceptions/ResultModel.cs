namespace GestionApi.Exceptions
{
    public class ResultModel<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public CustomException Exception { get; set; }

        public static ResultModel<T> SuccessResult(T data, CustomException customException = null)
        {
            return new ResultModel<T> { Success = true, Data = data, Exception = customException };
        }

        public static ResultModel<T> Failure(CustomException customException = null)
        {
            return new ResultModel<T> { Success = false, Exception = customException };
        }
    }
}
