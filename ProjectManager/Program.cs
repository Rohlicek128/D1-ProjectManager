using ProjectManager.Database;
using ProjectManager.Database.Tables;
using ProjectManager.Interface;

namespace ProjectManager;

static class Program
{
    [STAThread]
    private static void Main()
    {
        //Projects.Create("Tester", "Some desc...");
        foreach (var project in Projects.List())
        {
            Console.WriteLine("{0}, {1}, {2}", project.Title, project.Description, project.CreatedDate);
        }
        
        
        ApplicationConfiguration.Initialize();
        Application.Run(new Window());
    }
}