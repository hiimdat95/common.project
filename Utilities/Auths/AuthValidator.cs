using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;
using Utilities.Models.Settings;

namespace Utilities.Auths
{
    public class AuthValidator : IAuthValidator
    {
        private readonly AuthServerSettings _authServerSettings;
        private readonly ILogger<AuthValidator> _logger;

        public AuthValidator(IOptions<AuthServerSettings> authServerSettings, ILogger<AuthValidator> logger)
        {
            _authServerSettings = authServerSettings.Value;
            _logger = logger;
        }

        public bool ValidateUser(string username, string password)
        {
            var authenticated = true;

            try
            {
                var credentials = new NetworkCredential(username, password, _authServerSettings.Domain);
                var serverId = new LdapDirectoryIdentifier(_authServerSettings.Url);

                using var connection = new LdapConnection(serverId, credentials);
                connection.Bind();
            }
            catch (Exception)
            {
                authenticated = false;
            }

            return authenticated;
        }

        public bool ValidateUserLive(string username, string password, string strLdapSrv1, string strLdapSrv2)
        {
            DirectoryEntry Entry = new DirectoryEntry("LDAP://" + strLdapSrv1);
            string strUser = username, strPass = password;
            try
            {
                Entry.Username = strUser;
                Entry.Password = strPass;
                Entry.AuthenticationType = AuthenticationTypes.Secure;
                DirectorySearcher search = new DirectorySearcher(Entry);
                if (strUser.IndexOf("@") >= 0)
                    strUser = strUser.Substring(0, strUser.IndexOf("@"));
                search.Filter = "(SAMAccountName=" + strUser + ")";
                SearchResult searchResult;
                searchResult = search.FindOne();
                if (searchResult == null)
                    return false;
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                Entry = new DirectoryEntry("LDAP://" + strLdapSrv2);
                try
                {
                    Entry.Username = strUser;
                    Entry.Password = strPass;
                    Entry.AuthenticationType = AuthenticationTypes.Secure;
                    DirectorySearcher search = new DirectorySearcher(Entry);
                    if (strUser.IndexOf("@") >= 0)
                        strUser = strUser.Substring(0, strUser.IndexOf("@"));
                    search.Filter = "(SAMAccountName=" + strUser + ")";
                    SearchResult searchResult;
                    searchResult = search.FindOne();
                    if (searchResult == null)
                        return false;
                    else
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}