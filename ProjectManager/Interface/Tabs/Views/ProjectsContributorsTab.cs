using ProjectManager.Database.Views;

namespace ProjectManager.Interface.Tabs.Views;

public sealed class ProjectsContributorsTab : TabPage
{
    private DataGridView _grid;
    private Button _btnRefresh;

    public ProjectsContributorsTab()
    {
        Text = "Project Contributors";
        Initialize();
        LoadData();
    }

    private void Initialize()
    {
        _grid = new DataGridView
        {
            Dock = DockStyle.Top,
            Height = 300,
            ReadOnly = true,
            AllowUserToAddRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoGenerateColumns = true
        };

        _btnRefresh = new Button
        {
            Text = "Refresh",
            Top = 310,
            Left = 10
        };

        _btnRefresh.Click += (_, _) => LoadData();

        Controls.Add(_grid);
        Controls.Add(_btnRefresh);
    }

    private void LoadData()
    {
        _grid.DataSource = ViewProjectsContributors.List();
    }
}