using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace cm.Utilities.Common
{
    public static class MappingTypeCode
    {
        public static readonly string UserRole = "UserRole";
        public static readonly string UserInGroup = "UserInGroup";
        public static readonly string GroupRole = "GroupRole";
        public static readonly string GroupArea = "GroupArea";
    }

    public static class Claims
    {
        public const string UserId = nameof(UserId);
        public const string CanBoViewModel = nameof(CanBoViewModel);
        public const string Permissions = nameof(Permissions);
    }

    public static class IocContants
    {
        public const string AppId = "IocApp:AppId";
        public const string Url = "IocApp:Url";
        public const string VerifySsoEndpoint = "IocApp:VerifySsoEndpoint";
        public const string SavedPayloadEndpoint = "IocApp:SavePayloadSsoEndpoint";
    }

    [DataContract]
    public class ValidationResult
    {
        [DataMember]
        public bool IsValid { get; private set; }

        [DataMember]
        public IEnumerable<string> Messages { get; private set; }

        public static ValidationResult Succeeded() => new ValidationResult { IsValid = true };

        public static ValidationResult Failed(string message) => Failed(new string[] { message });

        public static ValidationResult Failed(IEnumerable<string> messages) => new ValidationResult { IsValid = false, Messages = messages };
    }

    [DataContract]
    public class ServiceResult<T>
    {
        [DataMember]
        public IEnumerable<string> Messages { get; private set; }

        [DataMember]
        public T Data { get; private set; }

        [DataMember]
        public ValidationResult ValidationResult { get; private set; }

        [DataMember]
        public bool IsSuccess { get; private set; }

        public static ServiceResult<T> Succeeded(T data = default) => new ServiceResult<T> { Data = data, IsSuccess = true };

        public static ServiceResult<T> Succeeded(string message)
        {
            return new ServiceResult<T>
            {
                Messages = new[] { message },
                IsSuccess = true
            };
        }

        public static ServiceResult<T> Succeeded(T data, string message)
        {
            return new ServiceResult<T>
            {
                Data = data,
                Messages = new[] { message },
                IsSuccess = true
            };
        }

        public static ServiceResult<T> Failed(string message) => Failed(null, message);

        public static ServiceResult<T> Failed(ValidationResult validationResult) => Failed(validationResult, message: null);

        public static ServiceResult<T> Failed(ValidationResult validationResult, string message) => Failed(validationResult, new[] { message });

        public static ServiceResult<T> Failed(ValidationResult validationResult, IEnumerable<string> messages)
        {
            return new ServiceResult<T>
            {
                Messages = messages,
                ValidationResult = validationResult,
                IsSuccess = false
            };
        }

        public IEnumerable<string> GetMessages()
        {
            if (Messages == null)
            {
                return Enumerable.Empty<string>();
            }

            var messages = Messages.ToList();
            messages.AddRange(ValidationResult?.Messages ?? System.Array.Empty<string>());
            return messages;
        }
    }
}