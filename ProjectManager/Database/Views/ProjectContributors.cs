using Microsoft.Data.SqlClient;

namespace ProjectManager.Database.Views;

public class ProjectContributors
{
    public string ProjectTitle { get; set; }
    public int Contributors { get; set; }

    public ProjectContributors(string projectTitle, int contributors)
    {
        ProjectTitle = projectTitle;
        Contributors = contributors;
    }
}

public static class ViewProjectsContributors
{
    public static List<ProjectContributors> List()
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                "SELECT title, contributors FROM view_projects_contributors_amount",
                conn
            );

            using var reader = command.ExecuteReader();
            var result = new List<ProjectContributors>();

            while (reader.Read())
            {
                result.Add(new ProjectContributors(
                    reader.GetString(0),
                    reader.GetInt32(1)
                ));
            }

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:ViewProjectsContributors:List ERROR: " + e.Message);
            return [];
        }
    }
}