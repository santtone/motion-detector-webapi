
using System;

namespace MotionDetectorWebApi.Utils
{
    public static class AuthHelper
    {
        public static string UsernameFromIdentity(string usernamePostfix, System.Security.Principal.IIdentity identity)
        {
            var identityName = identity.Name;
            return identityName.Substring(0, identityName.IndexOf(usernamePostfix, StringComparison.Ordinal));
        }

        public static string UsernameToAzurePrincipalName(string usernamePostfix, string username)
        {
            return $"{username}{usernamePostfix}";
        }
    }
}
