using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Iv.Data
{
    public class ConnectionStringBuilder
    {
        private const string ServerPattern = @"(?:Server|Data Source)=\{0\}";
        private const string IntegratedSecurityPattern = @"Integrated\s+Security\s*=\s*(?:True|SSPI)";
        private const string DatabasePattern = @"(?:Database|Initial Catalog)=\{1\}";
        private const string CredentialsPattern = @"User\s+ID\s*=\s*\{2\}\s*;\s*Password\s*=\s*\{3\}";

        public static string Format(string templateConnectionString, string serverName, string dbName)
        {
            string connectionString = templateConnectionString;
            if (MatchServer(connectionString) && MatchDatabase(connectionString))
            {
                connectionString = String.Format(connectionString, serverName, dbName);
            }
            return connectionString;
        }

        public static string Format(string templateConnectionString, string serverName, string dbName, string userName, string password)
        {
            string connectionString = templateConnectionString;
            if (MatchServer(connectionString) && MatchDatabase(connectionString) && MatchIntegratedSecurity(connectionString))
            {
                return Format(templateConnectionString, serverName, dbName);
            }
            if (MatchServer(connectionString) && MatchDatabase(connectionString) && MatchCredentials(connectionString))
            {
                connectionString = String.Format(connectionString, serverName, dbName, userName, password);
            }
            return connectionString;
        }

        public static string Integrated2Credentials(string templateConnectionString, string serverName, string dbName, string userName, string password)
        {
            string connectionString = templateConnectionString;
            if (MatchServer(connectionString) && MatchDatabase(connectionString))
            {
                connectionString = Regex.Replace(connectionString, IntegratedSecurityPattern, "User ID={2};Password={3}", RegexOptions.IgnoreCase);
            }
            connectionString = Format(connectionString, serverName, dbName, userName, password);
            return connectionString;
        }

        public static string Credentials2Integrated(string templateConnectionString, string serverName, string dbName)
        {
            string connectionString = templateConnectionString;
            if (MatchServer(connectionString) && MatchDatabase(connectionString) && MatchCredentials(connectionString))
            {
                connectionString = Regex.Replace(connectionString, CredentialsPattern, "Integrated Security=SSPI", RegexOptions.IgnoreCase);
            }
            connectionString = Format(connectionString, serverName, dbName);
            return connectionString;
        }

        private static bool MatchServer(string connectionString)
        {
            return Regex.IsMatch(connectionString, ServerPattern, RegexOptions.IgnoreCase);
        }

        private static bool MatchDatabase(string connectionString)
        {
            return Regex.IsMatch(connectionString, DatabasePattern, RegexOptions.IgnoreCase);
        }

        private static bool MatchIntegratedSecurity(string connectionString)
        {
            return Regex.IsMatch(connectionString, IntegratedSecurityPattern, RegexOptions.IgnoreCase);
        }

        private static bool MatchCredentials(string connectionString)
        {
            return Regex.IsMatch(connectionString, CredentialsPattern, RegexOptions.IgnoreCase);
        }

    }
}
