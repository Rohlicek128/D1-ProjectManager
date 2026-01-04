using Microsoft.Data.SqlClient;

namespace ProjectManager.Database.Tables;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public DateTime CreatedDate { get; set; }

    public User(string username, string email, DateTime? createdDate = null, int id = -1)
    {
        Id = id;
        Username = username;
        Email = email;
        CreatedDate = createdDate ?? DateTime.Now;
    }
}

public static class Users
{
    public static bool Create(User user)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"insert into Users(username, email) values('{user.Username}', '{user.Email}')",
                conn
            );

            var addedRows = command.ExecuteNonQuery();
            if (addedRows > 0)
            {
                Console.WriteLine("[DATABASE]:Users:Create INSERTED");
            }

            return addedRows > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Users:Create ERROR: " + e.Message);
            return false;
        }
    }

    public static void Update(User user)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"update Users set username = '{user.Username}', email = '{user.Email}' where id = {user.Id}",
                conn
            );

            var updatedRows = command.ExecuteNonQuery();
            if (updatedRows > 0)
            {
                Console.WriteLine("[DATABASE]:Users:Update UPDATED");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Users:Update ERROR: " + e.Message);
        }
    }

    public static void Delete(User user)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"delete from Users where id = {user.Id}",
                conn
            );

            var deletedRows = command.ExecuteNonQuery();
            if (deletedRows > 0)
            {
                Console.WriteLine("[DATABASE]:Users:Delete DELETED");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Users:Delete ERROR: " + e.Message);
        }
    }

    public static User? FindById(int id)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"select id, username, email, created_date from Users where id = {id}",
                conn
            );

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new User(
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetDateTime(3),
                    reader.GetInt32(0)
                );
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Users:FindById ERROR: " + e.Message);
        }

        return null;
    }

    public static User? FindByUsername(string username)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"select id, username, email, created_date from Users where username = '{username}'",
                conn
            );

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new User(
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetDateTime(3),
                    reader.GetInt32(0)
                );
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Users:FindByUsername ERROR: " + e.Message);
        }

        return null;
    }

    public static List<User> List()
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                "select id, username, email, created_date from Users",
                conn
            );

            using var reader = command.ExecuteReader();
            var result = new List<User>();

            while (reader.Read())
            {
                result.Add(new User(
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
            Console.WriteLine("[DATABASE]:Users:List ERROR: " + e.Message);
            return [];
        }
    }
}