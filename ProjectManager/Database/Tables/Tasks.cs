using Microsoft.Data.SqlClient;

namespace ProjectManager.Database.Tables;

public class Task
{
    public int Id { get; set; }
    public Project Project { get; set; }
    public User User { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public bool Completed { get; set; }

    public Task(Project project, User user, string title, string desc, string status, string priority, bool completed, int id = -1)
    {
        Id = id;
        Project = project;
        User = user;
        Title = title;
        Description = desc;
        Status = status;
        Priority = priority;
        Completed = completed;
    }

    public override string ToString()
    {
        return Title;
    }
}

public static class Tasks
{
    public static bool Create(Task task)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"insert into Tasks(project_id, assigned_user_id, title, description, status, priority, completed) " +
                $"values({task.Project.Id}, {(task.User.Id > 0 ? task.User.Id.ToString() : "NULL")}, " +
                $"'{task.Title}', '{task.Description}', '{task.Status}', '{task.Priority}', {(task.Completed ? 1 : 0)})",
                conn
            );

            var addedRows = command.ExecuteNonQuery();
            if (addedRows > 0)
            {
                Console.WriteLine("[DATABASE]:Tasks:Create INSERTED");
            }

            return addedRows > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Tasks:Create ERROR: " + e.Message);
            return false;
        }
    }

    public static void Update(Task task)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"update Tasks set " +
                $"project_id = {task.Project.Id}, " +
                $"assigned_user_id = {(task.User.Id > 0 ? task.User.Id.ToString() : "NULL")}, " +
                $"title = '{task.Title}', " +
                $"description = '{task.Description}', " +
                $"status = '{task.Status}', " +
                $"priority = '{task.Priority}', " +
                $"completed = {(task.Completed ? 1 : 0)} " +
                $"where id = {task.Id}",
                conn
            );

            var updatedRows = command.ExecuteNonQuery();
            if (updatedRows > 0)
            {
                Console.WriteLine("[DATABASE]:Tasks:Update UPDATED");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Tasks:Update ERROR: " + e.Message);
        }
    }

    public static void Delete(Task task)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"delete from Tasks where id = {task.Id}",
                conn
            );

            var deletedRows = command.ExecuteNonQuery();
            if (deletedRows > 0)
            {
                Console.WriteLine("[DATABASE]:Tasks:Delete DELETED");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Tasks:Delete ERROR: " + e.Message);
        }
    }

    public static Task? FindById(int id)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                "select t.id, t.project_id, t.assigned_user_id, t.title, t.description, t.status, t.priority, t.completed " +
                "from Tasks t where t.id = " + id,
                conn
            );

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var project = Projects.FindById(reader.GetInt32(1));
                var user = reader.IsDBNull(2) ? new User("", "", DateTime.MinValue, -1) : Users.FindById(reader.GetInt32(2)) ?? new User("", "", DateTime.MinValue, -1);

                return new Task(
                    project ?? new Project("", "", DateTime.MinValue),
                    user,
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetBoolean(7),
                    reader.GetInt32(0)
                );
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Tasks:FindById ERROR: " + e.Message);
        }

        return null;
    }

    public static Task? FindByTitle(string title)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                "select t.id, t.project_id, t.assigned_user_id, t.title, t.description, t.status, t.priority, t.completed " +
                "from Tasks t where t.title = '" + title + "'",
                conn
            );

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var project = Projects.FindById(reader.GetInt32(1));
                var user = reader.IsDBNull(2) ? new User("", "", DateTime.MinValue, -1) : Users.FindById(reader.GetInt32(2)) ?? new User("", "", DateTime.MinValue, -1);

                return new Task(
                    project ?? new Project("", "", DateTime.MinValue),
                    user,
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetBoolean(7),
                    reader.GetInt32(0)
                );
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Tasks:FindByTitle ERROR: " + e.Message);
        }

        return null;
    }

    public static List<Task> List()
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand("""
                SELECT
                    t.id,
                    p.id, p.title, p.description, p.created_date,
                    u.id, u.username, u.email, u.created_date,
                    t.title, t.description, t.status, t.priority, t.completed
                FROM Tasks t
                JOIN Projects p ON p.id = t.project_id
                LEFT JOIN Users u ON u.id = t.assigned_user_id
                """, conn);

            using var reader = command.ExecuteReader();
            var result = new List<Task>();

            while (reader.Read())
            {
                var project = new Project(
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetDateTime(4),
                    reader.GetInt32(1)
                );

                var user = reader.IsDBNull(5)
                    ? new User("", "", DateTime.MinValue, -1)
                    : new User(
                        reader.GetString(6),
                        reader.GetString(7),
                        reader.GetDateTime(8),
                        reader.GetInt32(5)
                    );

                result.Add(new Task(
                    project,
                    user,
                    reader.GetString(9),
                    reader.GetString(10),
                    reader.GetString(11),
                    reader.GetString(12),
                    reader.GetBoolean(13),
                    reader.GetInt32(0)
                ));
            }

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Tasks:List ERROR: " + e.Message);
            return [];
        }
    }
}