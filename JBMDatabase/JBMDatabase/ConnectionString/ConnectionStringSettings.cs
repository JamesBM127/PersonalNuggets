using JBM.DeserializeJson;
using JBMDatabase.ConnectionString.Model;
using JBMDatabase.Validation;
using Microsoft.Extensions.Configuration;
using System.Text;


namespace JBMDatabase.ConnectionString
{
    public static class ConnectionStringSettings
    {
        public static string CreateConnectionString(this ConnectionStringModel connectionStringModel)
        {
            string connectionString = string.Empty;
            try
            {
                if (ConnectionStringIsValid(ref connectionStringModel))
                    connectionString = ComposeConnectionString(ref connectionStringModel);
            }
            catch (ArgumentNullException)
            {
                throw;
            }

            return connectionString;
        }

        public static string CreateConnectionString(this ConnectionStringModel connectionStringModel, IConfiguration configuration, string jsonSection)
        {
            connectionStringModel = configuration.ToCSharp<ConnectionStringModel>(jsonSection);

            return connectionStringModel.CreateConnectionString();
        }

        public static string CreateConnectionString(IConfiguration configuration, string jsonSection)
        {
            return new ConnectionStringModel().CreateConnectionString(configuration, jsonSection);
        }

        private static string ComposeConnectionString(ref ConnectionStringModel connectionString)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"Server={connectionString.ServerFqdn}");

            if (!string.IsNullOrWhiteSpace(connectionString.InstanceName))
                stringBuilder.Append($"\\{connectionString.InstanceName}");

            if (connectionString.Port != null)
                stringBuilder.Append($",{connectionString.Port}; ");
            else
                stringBuilder.Append("; ");

            stringBuilder.Append($"Database={connectionString.Database}; ");
            stringBuilder.Append($"User ID={connectionString.User}; ");
            stringBuilder.Append($"Password={connectionString.Password}; ");
            stringBuilder.Append($"TrustServerCertificate={connectionString.TrustServerCertificate}; ");

            return stringBuilder.ToString();
        }

        private static bool ConnectionStringIsValid(ref ConnectionStringModel connectionStringModel)
        {
            return connectionStringModel.IsValid();
        }
    }
}
