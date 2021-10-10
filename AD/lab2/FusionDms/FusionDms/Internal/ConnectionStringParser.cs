using System;
using System.IO;
using System.Linq;

using FusionDms.Core;
using FusionDms.Exceptions;

namespace FusionDms.Internal
{
    internal class ConnectionStringParser
    {
        private static readonly string[] RequiredParts =
        {
            "Db", "Access"
        };

        internal static ConnectionInfo ParseConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString) ||
                string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidConnectionStringException("Connection string can't be null, empty or whitespace.");
            }

            var connectionParts = connectionString.Split(';');

            foreach (var requiredPart in RequiredParts)
            {
                if (!connectionParts.Any(x => x.Contains(requiredPart)))
                {
                    throw new InvalidConnectionStringException(
                        "The given connection string isn't contains a required part: " + requiredPart);
                }
            }

            var filePath = connectionParts.First(x => x.Contains("Db=")).AsSpan()[3..].ToString();
            var accessMode = connectionParts.First(x => x.Contains("Access=")).AsSpan()[7..].ToString();

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified database file isn't exists.");
            }

            return new ConnectionInfo(filePath, accessMode);
        }

        internal static string GetFilePath(string connectionString)
        {
            return connectionString.Split(';')
                .First(x => x.Contains("Db="))
                .AsSpan()[3..]
                .ToString();
        }
    }
}