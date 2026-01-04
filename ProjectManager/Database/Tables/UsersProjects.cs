using Microsoft.Data.SqlClient;

namespace ProjectManager.Database.Tables;

public struct UserProject
{
    public int Id { get; set; }
    public Project Project { get; set; }
    public User User { get; set; }
    public Role Role { get; set; }

    public UserProject(Project project, User user, Role role, int id = -1)
    {
        Id = id;
        Project = project;
        User = user;
        Role = role;
    }
}

public static class UsersProjects
{
    public static bool Create(UserProject userProject)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"insert into UsersProjects(user_id, project_id, role_id) " +
                $"values({userProject.User.Id}, {userProject.Project.Id}, {userProject.Role.Id})",
                conn
            );

            var addedRows = command.ExecuteNonQuery();
            if (addedRows > 0)
            {
                Console.WriteLine("[DATABASE]:UsersProjects:Create INSERTED");
            }

            return addedRows > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:UsersProjects:Create ERROR: " + e.Message);
            return false;
        }
    }

    public static void Update(UserProject userProject)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"update UsersProjects set " +
                $"user_id = {userProject.User.Id}, " +
                $"project_id = {userProject.Project.Id}, " +
                $"role_id = {userProject.Role.Id} " +
                $"where id = {userProject.Id}",
                conn
            );

            var updatedRows = command.ExecuteNonQuery();
            if (updatedRows > 0)
            {
                Console.WriteLine("[DATABASE]:UsersProjects:Update UPDATED");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:UsersProjects:Update ERROR: " + e.Message);
        }
    }

    public static void Delete(UserProject userProject)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"delete from UsersProjects where id = {userProject.Id}",
                conn
            );

            var deletedRows = command.ExecuteNonQuery();
            if (deletedRows > 0)
            {
                Console.WriteLine("[DATABASE]:UsersProjects:Delete DELETED");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:UsersProjects:Delete ERROR: " + e.Message);
        }
    }

    public static UserProject? FindById(int id)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"select id, user_id, project_id, role_id from UsersProjects where id = {id}",
                conn
            );

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var user = Users.FindById(reader.GetInt32(1)) ?? new User("", "", DateTime.MinValue, -1);
                var project = Projects.FindById(reader.GetInt32(2)) ?? new Project("", "", DateTime.MinValue);
                var role = Roles.FindById(reader.GetInt32(3)) ?? new Role("", -1);

                return new UserProject(project, user, role, reader.GetInt32(0));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:UsersProjects:FindById ERROR: " + e.Message);
        }

        return null;
    }

    public static List<UserProject> ListByProject(Project project)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"select id, user_id, project_id, role_id from UsersProjects where project_id = {project.Id}",
                conn
            );

            using var reader = command.ExecuteReader();
            var result = new List<UserProject>();

            while (reader.Read())
            {
                var user = Users.FindById(reader.GetInt32(1)) ?? new User("", "", DateTime.MinValue, -1);
                var role = Roles.FindById(reader.GetInt32(3)) ?? new Role("", -1);

                result.Add(new UserProject(project, user, role, reader.GetInt32(0)));
            }

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:UsersProjects:ListByProject ERROR: " + e.Message);
            return [];
        }
    }

    public static List<UserProject> ListByUser(User user)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"select id, user_id, project_id, role_id from UsersProjects where user_id = {user.Id}",
                conn
            );

            using var reader = command.ExecuteReader();
            var result = new List<UserProject>();

            while (reader.Read())
            {
                var project = Projects.FindById(reader.GetInt32(2)) ?? new Project("", "", DateTime.MinValue);
                var role = Roles.FindById(reader.GetInt32(3)) ?? new Role("", -1);

                result.Add(new UserProject(project, user, role, reader.GetInt32(0)));
            }

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:UsersProjects:ListByUser ERROR: " + e.Message);
            return [];
        }
    }

    public static List<UserProject> List()
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                "select id, user_id, project_id, role_id from UsersProjects",
                conn
            );

            using var reader = command.ExecuteReader();
            var result = new List<UserProject>();

            while (reader.Read())
            {
                var user = Users.FindById(reader.GetInt32(1)) ?? new User("", "", DateTime.MinValue, -1);
                var project = Projects.FindById(reader.GetInt32(2)) ?? new Project("", "", DateTime.MinValue);
                var role = Roles.FindById(reader.GetInt32(3)) ?? new Role("", -1);

                result.Add(new UserProject(project, user, role, reader.GetInt32(0)));
            }

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:UsersProjects:List ERROR: " + e.Message);
            return [];
        }
    }
}