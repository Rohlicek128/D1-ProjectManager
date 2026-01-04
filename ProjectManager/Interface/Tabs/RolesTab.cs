using ProjectManager.Database.Tables;

namespace ProjectManager.Interface.Tabs;

public sealed class RolesTab : TabPage
{
    private DataGridView _grid;
    private readonly BindingSource _roleBinding = new();

    private TextBox _txtName;
    private Button _btnAdd, _btnDelete, _btnRefresh;

    public RolesTab()
    {
        Text = "Roles";
        Initialize();
        LoadRoles();
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

        _txtName = new TextBox
        {
            PlaceholderText = "Role name",
            Top = 310,
            Left = 10,
            Width = 200
        };

        _btnAdd = new Button { Text = "Add", Top = 350, Left = 10 };
        _btnDelete = new Button { Text = "Delete", Top = 350, Left = 100 };
        _btnRefresh = new Button { Text = "Refresh", Top = 350, Left = 190 };

        _btnAdd.Click += AddRole;
        _btnDelete.Click += DeleteRole;
        _btnRefresh.Click += (_, _) => LoadRoles();

        _grid.CellEndEdit += GridCellEndEdit;
        _grid.DataBindingComplete += GridDataBindingComplete;

        Controls.Add(_grid);
        Controls.Add(_txtName);
        Controls.Add(_btnAdd);
        Controls.Add(_btnDelete);
        Controls.Add(_btnRefresh);
    }

    private void LoadRoles()
    {
        _roleBinding.DataSource = Roles.List();
        _grid.DataSource = _roleBinding;
    }

    private Role? SelectedRole()
    {
        if (_grid.SelectedRows.Count == 0) return null;
        return (Role?)_grid.SelectedRows[0].DataBoundItem;
    }

    private void AddRole(object? sender, EventArgs e)
    {
        var role = new Role(_txtName.Text);
        if (!ValidateRole(role)) return;

        Roles.Create(role);
        LoadRoles();
    }

    private void DeleteRole(object? sender, EventArgs e)
    {
        var role = SelectedRole();
        if (role == null) return;

        Roles.Delete(role);
        LoadRoles();
    }

    private bool ValidateRole(Role role)
    {
        if (string.IsNullOrWhiteSpace(role.Name))
        {
            MessageBox.Show("Role name cannot be empty.");
            return false;
        }

        return true;
    }

    private void GridCellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var role = (Role?)_grid.Rows[e.RowIndex].DataBoundItem!;
        if (!ValidateRole(role)) return;

        try
        {
            Roles.Update(role);
            Window.FlashEditableCells(_grid, e.RowIndex, Color.LightGreen);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating role: {ex.Message}");
        }
    }

    private void GridDataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        _grid.Columns["Id"]?.ReadOnly = true;
        _grid.Columns["Id"]?.DefaultCellStyle.BackColor = Color.LightGray;
    }
}
