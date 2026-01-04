using Microsoft.Data.SqlClient;

namespace ProjectManager.Database.Tables;

public struct Project
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }

    public Project(string title, string desc, DateTime? createdDate = null, int id = -1)
    {
        Id = id;
        Title = title;
        Description = desc;
        CreatedDate = createdDate ?? DateTime.Now;
    }
}

public static class Projects
{
    public static bool Create(Project project)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"insert into Projects(title, description) values(\'{project.Title}\', \'{project.Description}\')",
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

    public static void Update(Project project)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"update Projects set title = '{project.Title}', description = '{project.Description}' where id = '{project.Id}'",
                conn
            );

            var updatedRows = command.ExecuteNonQuery();
            if (updatedRows > 0)
            {
                Console.WriteLine("[DATABASE]:Projects:Update UPDATED");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Projects:Update ERROR: " + e.Message);
        }
    }
    
    public static void Delete(Project project)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"delete from Projects where id = '{project.Id}'",
                conn
            );

            var deletedRows = command.ExecuteNonQuery();
            if (deletedRows > 0)
            {
                Console.WriteLine("[DATABASE]:Projects:Delete DELETED");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Projects:Delete ERROR: " + e.Message);
        }
    }
    
    public static Project? FindById(int id)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"select id, title, description, created_date from Projects where id = '{id}'",
                conn
            );

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Project(
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetDateTime(3),
                    reader.GetInt32(0)
                );
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Projects:FindByTitle ERROR: " + e.Message);
        }

        return null;
    }

    public static Project? FindByTitle(string title)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"select top 1 id, title, description, created_date from Projects where title = '{title}'",
                conn
            );

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Project(
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetDateTime(3),
                    reader.GetInt32(0)
                );
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Projects:FindByTitle ERROR: " + e.Message);
        }

        return null;
    }

    public static List<Project> List()
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                "select p.id, p.title, p.description, p.created_date from Projects p",
                conn
            );

            using var reader = command.ExecuteReader();

            var result = new List<Project>();
            while (reader.Read())
            {
                result.Add(new Project(
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetDateTime(3),
                    reader.GetInt32(0)
                ));
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