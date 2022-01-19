namespace cm.Utilities.Common
{
    public class ApiSuccessResult<T> : ApiResult<T>
    {
        public ApiSuccessResult(T resultObj, string message)
        {
            IsSuccessed = true;
            ResultObj = resultObj;
            Message = message;
        }

        public ApiSuccessResult(string message)
        {
            IsSuccessed = true;
            Message = message;
        }
    }
}