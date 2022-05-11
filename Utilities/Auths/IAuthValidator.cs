namespace Utilities.Auths
{
    public interface IAuthValidator
    {
        bool ValidateUser(string username, string password);

        bool ValidateUserLive(string username, string password, string strLdapSrv1, string strLdapSrv2);
    }
}