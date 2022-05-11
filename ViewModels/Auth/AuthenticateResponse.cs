using System.Collections.Generic;
using ViewModels.Permissions;

namespace ViewModels.Auth
{
    public class AuthenticateResponse : AuthResponse
    {
        public List<PermissionViewModel> ListPermissions { get; set; }
    }
}