using JBMDatabase.ConnectionString.Model;
using System.Text;

namespace JBMDatabase.Validation
{
    public static class ConnectionStringValidation
    {
        public static bool IsValid(this ConnectionStringModel connectionStringModel)
        {
            StringBuilder errorMessages = new StringBuilder();
            byte numberOfErrors = 0;

            if (string.IsNullOrWhiteSpace(connectionStringModel.ServerFqdn))
            {
                numberOfErrors++;
                errorMessages.Append($"|{numberOfErrors}:ServerFqdn| ");
            }

            if (string.IsNullOrWhiteSpace(connectionStringModel.Database))
            {
                numberOfErrors++;
                errorMessages.Append($"|{numberOfErrors}:Database| ");
            }

            if (string.IsNullOrWhiteSpace(connectionStringModel.User))
            {
                numberOfErrors++;
                errorMessages.Append($"|{numberOfErrors}:User| ");
            }

            if (string.IsNullOrWhiteSpace(connectionStringModel.Password))
            {
                numberOfErrors++;
                errorMessages.Append($"|{numberOfErrors}:Password|");
            }

            if (errorMessages.Length > 0)
            {
                throw new ArgumentNullException($"{errorMessages.ToString()}");
            }

            return true;
        }

        public static bool IsValid(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("Invalid Connectionstring");
            }

            return true;
        }
    }
}
