using ProjectManager.Database;

namespace ProjectManager;

static class Program
{
    [STAThread]
    private static void Main()
    {
        var db = ProjectsDatabase.Instance;
        
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }
}