namespace cm.Utilities.Common
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public ApiErrorResult(T resultObj, string message)
        {
            IsSuccessed = false;
            ResultObj = resultObj;
            Message = message;
        }

        public ApiErrorResult(string message)
        {
            IsSuccessed = false;
            Message = message;
        }
    }
}