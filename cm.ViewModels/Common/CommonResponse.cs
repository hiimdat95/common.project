using System;

namespace cm.ViewModels.Common
{
    public class SsoVerifyResponse<T>
    {
        public int StatusCode { get; set; }
        public bool Succeeded { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class CommonResponse
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}