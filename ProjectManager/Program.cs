using ProjectManager.Database.Tables;
using ProjectManager.Interface;

namespace ProjectManager;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Users.Create(new User("Me", "me@mail.com"));
        foreach (var user in Users.List())
        {
            Console.WriteLine("{0}, {1}, {2}", user.Username, user.Email, user.CreatedDate);
        }
        
        foreach (var project in Projects.List())
        {
            Console.WriteLine("{0}, {1}, {2}", project.Title, project.Description, project.CreatedDate);
        }
        
        
        ApplicationConfiguration.Initialize();
        Application.Run(new Window());
    }
}