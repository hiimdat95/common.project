namespace Utilities.Constants
{
    public static class SystemConstants
    {
        public const string MainConnectionString = "Default";

        public const int MaxAnsiCode = 255;
        public const int MaxValueFiles = 1073741824;

        public static class AppSettings
        {
            public const string SectionAppSettings = nameof(AppSettings);
            public const string DefaultLanguageId = "DefaultLanguageId";
            public const string Token = "Token";
            public const string BaseAddress = "BaseAddress";

            public static string UPLOAD_FOLDER { get; set; } = "AppSettings:UPLOAD_FOLDER";
            public static string URL_IOC_WEATHER { get; set; } = "AppSettings:URL_IOC_WEATHER";
            public static string URL_DOMAIN { get; set; } = nameof(URL_DOMAIN);
        }

        public static class MessageResponse
        {
            public const string DuplicateName = "Tên đã bị trùng. Vui lòng đổi tên khác!";
            public const string NotFound = "Không tồn tại id : ";
            public const string UploadImageFailed = "Tải ảnh lên thất bại.";
            public const string WrongImageType = "Loại ảnh truyền lên không đúng.";
            public const string ConvertToListFailed = "Danh sách truyền vào không hợp lệ.";
            public const string MessageUploadFile = "Dung lượng files tối đa là 20MB";
            public const string ErrorSomethingWentWrong = "Sorry, something went wrong.";
        }

        public static class Roles
        {
            public const string Admin = "Admin";
        }

        public static class TypeFunctionMapping
        {
            public const string Users = "Users";
            public const string Roles = "Roles";
            public const string Group = "Group";
        }

        public static class LdapInformation
        {
            public const string ValidateLDAP = nameof(ValidateLDAP);
        }
    }
}