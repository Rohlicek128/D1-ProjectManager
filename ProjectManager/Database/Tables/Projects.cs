using Microsoft.Data.SqlClient;

namespace ProjectManager.Database.Tables;

public struct Project(string title, string desc, DateTime createdDate)
{
    public string Title = title;
    public string Description = desc;
    public DateTime CreatedDate = createdDate;
}

public static class Projects
{
    public static bool Create(string title, string description)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"insert into Projects(title, description) values(\'{title}\', \'{description}\')",
                conn
            );

            var addedRows = command.ExecuteNonQuery();
            if (addedRows > 0)
            {
                Console.WriteLine("[DATABASE]:Projects:Create INSERTED");
            }
            
            return addedRows > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Projects:Create ERROR: " + e.Message);
            return false;
        }
    }

    public static List<Project> List()
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                "select p.title, p.description, p.created_date from Projects p",
                conn
            );

            var reader = command.ExecuteReader();

            var result = new List<Project>();
            while (reader.Read())
            {
                result.Add(new Project(reader.GetString(0), reader.GetString(1), reader.GetDateTime(2)));
            }
            
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Projects:List ERROR: " + e.Message);
            return [];
        }
    }
}