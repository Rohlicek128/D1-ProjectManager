using ProjectManager.Database.Tables;

namespace ProjectManager.Interface.Tabs;

public sealed class UsersProjectsTab : TabPage
{
    private DataGridView _grid;
    private readonly BindingSource _upBinding = new();

    private ComboBox _cmbProject;
    private ComboBox _cmbUser;
    private ComboBox _cmbRole;
    private Button _btnAdd, _btnDelete, _btnRefresh;

    public UsersProjectsTab()
    {
        Text = "Users + Projects";
        Initialize();
        LoadLookups();
        LoadUserProjects();
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

        _cmbProject = new ComboBox { Top = 310, Left = 10, Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
        _cmbUser = new ComboBox { Top = 310, Left = 200, Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
        _cmbRole = new ComboBox { Top = 310, Left = 390, Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };

        _btnAdd = new Button { Text = "Add", Top = 350, Left = 10 };
        _btnDelete = new Button { Text = "Delete", Top = 350, Left = 100 };
        _btnRefresh = new Button { Text = "Refresh", Top = 350, Left = 190 };

        _btnAdd.Click += AddUserProject;
        _btnDelete.Click += DeleteUserProject;
        _btnRefresh.Click += (_, _) => LoadUserProjects();

        _grid.CellEndEdit += GridCellEndEdit;
        _grid.DataBindingComplete += GridDataBindingComplete;

        Controls.AddRange(new Control[]
        {
            _grid,
            _cmbProject,
            _cmbUser,
            _cmbRole,
            _btnAdd,
            _btnDelete,
            _btnRefresh
        });
    }

    private void LoadLookups()
    {
        _cmbProject.DataSource = Projects.List();
        _cmbProject.DisplayMember = "Title";

        _cmbUser.DataSource = Users.List();
        _cmbUser.DisplayMember = "Username";

        _cmbRole.DataSource = Roles.List();
        _cmbRole.DisplayMember = "Name";
    }

    private void LoadUserProjects()
    {
        _upBinding.DataSource = UsersProjects.List();
        _grid.DataSource = _upBinding;
    }

    private UserProject? SelectedUserProject()
    {
        if (_grid.SelectedRows.Count == 0) return null;
        return (UserProject?)_grid.SelectedRows[0].DataBoundItem;
    }

    private void AddUserProject(object? sender, EventArgs e)
    {
        if (_cmbProject.SelectedItem is not Project project) return;
        if (_cmbUser.SelectedItem is not User user) return;
        if (_cmbRole.SelectedItem is not Role role) return;

        var up = new UserProject(project, user, role);

        try
        {
            UsersProjects.Create(up);
            LoadUserProjects();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error adding user-project: {ex.Message}");
        }
    }

    private void DeleteUserProject(object? sender, EventArgs e)
    {
        var up = SelectedUserProject();
        if (up == null) return;

        try
        {
            UsersProjects.Delete(up);
            LoadUserProjects();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting user-project: {ex.Message}");
        }
    }

    private void GridCellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var up = (UserProject?)_grid.Rows[e.RowIndex].DataBoundItem;
        if (up == null) return;

        try
        {
            UsersProjects.Update(up);
            Window.FlashEditableCells(_grid, e.RowIndex, Color.LightGreen);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating user-project: {ex.Message}");
        }
    }

    private void GridDataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        _grid.Columns["Id"]?.ReadOnly = true;
        _grid.Columns["Id"]?.DefaultCellStyle.BackColor = Color.LightGray;
        
        _grid.Columns["Project"]?.Visible = true;
        _grid.Columns["User"]?.Visible = true;
        _grid.Columns["Role"]?.Visible = true;
    }
}
