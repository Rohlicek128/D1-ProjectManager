using ProjectManager.Database.Tables;

namespace ProjectManager.Interface.Tabs.Views;

public sealed class ViewsTab : TabPage
{
    private TabControl _tabs;

    public ViewsTab()
    {
        Text = "Views";
        Initialize();
    }

    private void Initialize()
    {
        _tabs = new TabControl
        {
            Dock = DockStyle.Fill
        };

        _tabs.TabPages.Add(new UsersWorkloadTab());
        _tabs.TabPages.Add(new ProjectsContributorsTab());

        Controls.Add(_tabs);
    }
}