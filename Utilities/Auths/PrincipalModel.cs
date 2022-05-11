using System;

namespace Utilities.Auths
{
    public class PrincipalModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Permission { get; set; }
    }
}