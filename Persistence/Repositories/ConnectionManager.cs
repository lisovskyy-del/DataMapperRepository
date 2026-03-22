using System.Configuration;
using System.Data.Common;

namespace RepositoryPatternDemo.Persistence.Repositories;

internal class ConnectionManager
{
    //DbProviderFactories.RegisterFactory("System.Data.SQLite", SqliteFactory.Instance);
    private readonly string _connectionString;
    private readonly string _providerName;

    public DbProviderFactory DbProviderFactory { get; init; }

    public ConnectionManager()
    {
        string provider = ConfigurationManager.AppSettings["DbProvider"];
        (_connectionString, _providerName) = provider switch
        {
            "SQLite" => (ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString,
                         "System.Data.SQLite"),
            "SqlServer" => (ConfigurationManager.ConnectionStrings["SqlServerConnection"].ConnectionString,
                            "System.Data.SqlClient"),
            _ => throw new Exception("Unknown provider")
        };
    }

    public DbConnection GetConnection()
    {
        var factory = DbProviderFactories.GetFactory(_providerName);
        var conn = factory.CreateConnection();
        conn.ConnectionString = _connectionString;
        return conn;
    }
}
