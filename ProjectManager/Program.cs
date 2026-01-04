using ProjectManager.Database.Tables;
using ProjectManager.Interface;

namespace ProjectManager;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        //Users.Create(new User("Me", "me@mail.com"));
        //Roles.Create(new Role("Developer"));
        
        //UsersProjects.Create(new UserProject(Projects.FindById(1).Value, Users.FindById(1).Value, Roles.FindById(1).Value));
        foreach (var user in Users.List())
        {
            Console.WriteLine("{0}, {1}, {2}, {3}", user.Username, user.Email, user.CreatedDate, user.Id);
        }
        foreach (var project in Projects.List())
        {
            Console.WriteLine("{0}, {1}, {2}, {3}", project.Id, project.Title, project.Description, project.CreatedDate);
        }
        
        
        
        
        ApplicationConfiguration.Initialize();
        Application.Run(new Window());
    }
}