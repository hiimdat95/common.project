namespace Utilities.Common
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
        public const string ProfileViewModel = nameof(ProfileViewModel);
        public const string Permissions = nameof(Permissions);
    }

    public static class IocContants
    {
        public const string AppId = "IocApp:AppId";
        public const string Url = "IocApp:Url";
        public const string VerifySsoEndpoint = "IocApp:VerifySsoEndpoint";
        public const string SavedPayloadEndpoint = "IocApp:SavePayloadSsoEndpoint";
    }
}