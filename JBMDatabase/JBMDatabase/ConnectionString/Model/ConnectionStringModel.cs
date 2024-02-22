namespace JBMDatabase.ConnectionString.Model
{
    public class ConnectionStringModel
    {
        public string ServerFqdn { get; set; }
        public string InstanceName { get; set; }
        public int? Port { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool TrustServerCertificate { get; set; }

        public ConnectionStringModel()
        {
        }

        public ConnectionStringModel(string serverFqdn, string instanceName, int? port, string database, string user, string password, bool trustServerCertificate)
        {
            ServerFqdn = serverFqdn;
            InstanceName = instanceName;
            Port = port;
            Database = database;
            User = user;
            Password = password;
            TrustServerCertificate = trustServerCertificate;
        }
    }
}
