using System;

namespace cm.Utilities.Models.Settings
{
    public class AuthConfig
    {
        public String LdapAddress1 { get; set; }
        public String LdapAddress2 { get; set; }
        public String LdapAddress3 { get; set; }

        public String LdapAddress4 { get; set; }
        public AuthSettings authSettings { get; set; }
    }
}