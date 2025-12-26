using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ProjectManager.Database;

public class ProjectsDatabase
{
    private static ProjectsDatabase _instance;
    public static ProjectsDatabase Instance
    {
        get
        {
            if (_instance == null) _instance = new ProjectsDatabase();
            return _instance;
        }
    }
    
    private readonly string _connectionString;
    public SqlConnection Connection { get; }

    private ProjectsDatabase()
    {
        _connectionString = GetConnectionString();
        Connection = GetConnection()!;
    }

    private string GetConnectionString()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = config.GetConnectionString("Server"),
            UserID = config.GetConnectionString("User"),
            Password = config.GetConnectionString("Password"),
            InitialCatalog = config.GetConnectionString("Database"),
            TrustServerCertificate = true
        };
        return builder.ConnectionString;
    }

    private SqlConnection? GetConnection()
    {
        try
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            Console.WriteLine("[DATABASE] Connected");
            
            return connection;
        }
        catch (SqlException e)
        {
            Console.WriteLine($"SQL Error: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        return null;
    }
}