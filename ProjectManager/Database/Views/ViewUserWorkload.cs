using Microsoft.Data.SqlClient;

namespace ProjectManager.Database.Views;

public class UserWorkload
{
    public string Username { get; set; }
    public int Assignments { get; set; }

    public UserWorkload(string username, int assignments)
    {
        Username = username;
        Assignments = assignments;
    }
}

public static class ViewUsersWorkload
{
    public static List<UserWorkload> List()
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                "SELECT username, assignments FROM view_users_workload",
                conn
            );

            using var reader = command.ExecuteReader();
            var result = new List<UserWorkload>();

            while (reader.Read())
            {
                result.Add(new UserWorkload(
                    reader.GetString(0),
                    reader.GetInt32(1)
                ));
            }

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:ViewUsersWorkload:List ERROR: " + e.Message);
            return [];
        }
    }
}