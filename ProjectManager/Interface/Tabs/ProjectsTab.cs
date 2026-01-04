using ProjectManager.Database.Tables;

namespace ProjectManager.Interface.Tabs;

public sealed class ProjectsTab : TabPage
{
    private DataGridView _grid;
    private readonly BindingSource _projectBinding = new();

    private TextBox _txtTitle;
    private TextBox _txtDescription;
    private Button _btnAdd, _btnDelete, _btnRefresh;

    public ProjectsTab()
    {
        Text = "Projects";
        Initialize();
        LoadProjects();
    }

    private void Initialize()
    {
        _grid = new DataGridView
        {
            Dock = DockStyle.Top,
            Height = 300,
            ReadOnly = false,
            AllowUserToAddRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoGenerateColumns = true
        };

        _txtTitle = new TextBox
        {
            PlaceholderText = "Title",
            Top = 310,
            Left = 10,
            Width = 180
        };

        _txtDescription = new TextBox
        {
            PlaceholderText = "Description",
            Top = 310,
            Left = 200,
            Width = 300
        };

        _btnAdd = new Button { Text = "Add", Top = 350, Left = 10 };
        _btnDelete = new Button { Text = "Delete", Top = 350, Left = 100 };
        _btnRefresh = new Button { Text = "Refresh", Top = 350, Left = 190 };

        _btnAdd.Click += AddProject;
        _btnDelete.Click += DeleteProject;
        _btnRefresh.Click += (_, _) => LoadProjects();

        _grid.CellEndEdit += GridCellEndEdit;
        _grid.DataBindingComplete += GridDataBindingComplete;

        Controls.Add(_grid);
        Controls.Add(_txtTitle);
        Controls.Add(_txtDescription);
        Controls.Add(_btnAdd);
        Controls.Add(_btnDelete);
        Controls.Add(_btnRefresh);
    }

    private void LoadProjects()
    {
        _projectBinding.DataSource = Projects.List();
        _grid.DataSource = _projectBinding;
    }

    private Project? SelectedProject()
    {
        if (_grid.SelectedRows.Count == 0) return null;
        return (Project?)_grid.SelectedRows[0].DataBoundItem;
    }

    private void AddProject(object? sender, EventArgs e)
    {
        var project = new Project(
            _txtTitle.Text,
            _txtDescription.Text
        );

        if (!ValidateProject(project)) return;

        Projects.Create(project);
        LoadProjects();
    }

    private void DeleteProject(object? sender, EventArgs e)
    {
        var project = SelectedProject();
        if (project == null) return;

        Projects.Delete(project);
        LoadProjects();
    }

    private bool ValidateProject(Project project)
    {
        if (string.IsNullOrWhiteSpace(project.Title))
        {
            MessageBox.Show("Title cannot be empty.");
            return false;
        }

        return true;
    }

    private void GridCellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var project = (Project?)_grid.Rows[e.RowIndex].DataBoundItem!;
        if (!ValidateProject(project)) return;

        try
        {
            Projects.Update(project);
            Window.FlashEditableCells(_grid, e.RowIndex, Color.LightGreen);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating project: {ex.Message}");
        }
    }

    private void GridDataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        _grid.Columns["Id"]?.ReadOnly = true;
        _grid.Columns["CreatedDate"]?.ReadOnly = true;

        _grid.Columns["Id"]?.DefaultCellStyle.BackColor = Color.LightGray;
        _grid.Columns["CreatedDate"]?.DefaultCellStyle.BackColor = Color.LightGray;
    }
}
