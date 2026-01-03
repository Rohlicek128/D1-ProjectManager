using Microsoft.Data.SqlClient;

namespace ProjectManager.Database.Tables;

public struct Role(string name, int id = -1)
{
    public int Id = id;
    public string Name = name;
}

public static class Roles
{
    public static bool Create(Role role)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"insert into Roles(name) values('{role.Name}')",
                conn
            );

            var addedRows = command.ExecuteNonQuery();
            if (addedRows > 0)
            {
                Console.WriteLine("[DATABASE]:Roles:Create INSERTED");
            }

            return addedRows > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Roles:Create ERROR: " + e.Message);
            return false;
        }
    }

    public static void Update(Role role)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"update Roles set name = '{role.Name}' where id = {role.Id}",
                conn
            );

            var updatedRows = command.ExecuteNonQuery();
            if (updatedRows > 0)
            {
                Console.WriteLine("[DATABASE]:Roles:Update UPDATED");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Roles:Update ERROR: " + e.Message);
        }
    }

    public static void Delete(Role role)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"delete from Roles where id = {role.Id}",
                conn
            );

            var deletedRows = command.ExecuteNonQuery();
            if (deletedRows > 0)
            {
                Console.WriteLine("[DATABASE]:Roles:Delete DELETED");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Roles:Delete ERROR: " + e.Message);
        }
    }

    public static Role? FindById(int id)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"select id, name from Roles where id = {id}",
                conn
            );

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Role(
                    reader.GetString(1),
                    reader.GetInt32(0)
                );
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Roles:FindById ERROR: " + e.Message);
        }

        return null;
    }

    public static Role? FindByName(string name)
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                $"select id, name from Roles where name = '{name}'",
                conn
            );

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Role(
                    reader.GetString(1),
                    reader.GetInt32(0)
                );
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Roles:FindByName ERROR: " + e.Message);
        }

        return null;
    }

    public static List<Role> List()
    {
        try
        {
            var conn = ProjectsDatabase.Instance.Connection;

            using var command = new SqlCommand(
                "select id, name from Roles",
                conn
            );

            using var reader = command.ExecuteReader();
            var result = new List<Role>();

            while (reader.Read())
            {
                result.Add(new Role(
                    reader.GetString(1),
                    reader.GetInt32(0)
                ));
            }

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine("[DATABASE]:Roles:List ERROR: " + e.Message);
            return [];
        }
    }
}