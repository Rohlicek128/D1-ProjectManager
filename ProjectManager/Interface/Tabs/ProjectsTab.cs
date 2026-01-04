using ProjectManager.Database.Tables;

namespace ProjectManager.Interface.Tabs;

public sealed class ProjectsTab : TabPage
{
    private DataGridView _grid;
    private TextBox _txtTitle, _txtDesc;
    private Button _btnAdd, _btnDelete;

    public ProjectsTab()
    {
        Text = "Projects";
        Initialize();
        LoadProjects();
    }

    private void Initialize()
    {
        _grid = new DataGridView { Dock = DockStyle.Top, Height = 300 };
        _txtTitle = new TextBox { PlaceholderText = "Title", Top = 310, Left = 10 };
        _txtDesc = new TextBox { PlaceholderText = "Description", Top = 310, Left = 200 };

        _btnAdd = new Button { Text = "Add", Top = 350, Left = 10 };
        _btnDelete = new Button { Text = "Delete", Top = 350, Left = 100 };

        _btnAdd.Click += (_, _) =>
        {
            Projects.Create(new Project(_txtTitle.Text, _txtDesc.Text, DateTime.Now));
            LoadProjects();
        };

        _btnDelete.Click += (_, _) =>
        {
            if (_grid.SelectedRows.Count == 0) return;
            Projects.Delete((Project)_grid.SelectedRows[0].DataBoundItem);
            LoadProjects();
        };

        Controls.Add(_grid);
        Controls.Add(_txtTitle);
        Controls.Add(_txtDesc);
        Controls.Add(_btnAdd);
        Controls.Add(_btnDelete);
    }

    private void LoadProjects()
    {
        _grid.DataSource = null;
        _grid.DataSource = Projects.List();
    }
}