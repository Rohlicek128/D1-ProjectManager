using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ProjectManager.Database;

public class ProjectsDatabase
{
    public static ProjectsDatabase Instance
    {
        get
        {
            if (field == null) field = new ProjectsDatabase();
            return field;
        }
    } = null!;

    private readonly string _connectionString;
    public SqlConnection Connection { get; }

    private ProjectsDatabase()
    {
        _connectionString = GetConnectionString()!;
        Connection = GetConnection()!;
    }

    private string? GetConnectionString()
    {
        try
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
            //Console.WriteLine(_connectionString);
            return builder.ConnectionString;
        }
        catch (SqlException e)
        {
            Console.WriteLine($"[DATABASE]:ConnectionString SQL Error: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"[DATABASE]:ConnectionString ERROR: {e.Message}");
        }
        
        Environment.Exit(87); // ERROR_INVALID_PARAMETER
        return null;
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
            Console.WriteLine($"[DATABASE]:Connection SQL Error: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"[DATABASE]:Connection ERROR: {e.Message}");
        }

        Environment.Exit(1);
        return null;
    }
}