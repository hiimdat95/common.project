using cm.Utilities.Models.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.DirectoryServices.Protocols;
using System.Net;

namespace cm.Application.AuthServices
{
    public class AuthValidator : IAuthValidator
    {
        private readonly AuthServerSettings _authServerSettings;

        public AuthValidator(IOptions<AuthServerSettings> authServerSettings, ILogger<AuthValidator> logger)
        {
            _authServerSettings = authServerSettings.Value;
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
    }
}