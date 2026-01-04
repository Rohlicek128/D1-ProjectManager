using ProjectManager.Database;
using ProjectManager.Interface;

namespace ProjectManager;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        try
        {
            var unused = ProjectsDatabase.Instance;
        }
        catch (Exception e)
        {
            MessageBox.Show($"Error: {e.Message}");
            Environment.Exit(1);
        }
        
        ApplicationConfiguration.Initialize();
        Application.Run(new Window());
    }
}