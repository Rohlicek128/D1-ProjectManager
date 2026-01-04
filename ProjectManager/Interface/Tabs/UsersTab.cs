using ProjectManager.Database.Tables;

namespace ProjectManager.Interface.Tabs;

public sealed class UsersTab : TabPage
{
    private DataGridView _grid;
    private readonly BindingSource _userBinding = new();
    
    private TextBox _txtUsername;
    private TextBox _txtEmail;
    private Button _btnAdd, _btnDelete, _btnRefresh, _btnLoadCsv;

    public UsersTab()
    {
        Text = "Users";
        Initialize();
        LoadUsers();
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

        _txtUsername = new TextBox { PlaceholderText = "Username", Top = 310, Left = 10 };
        _txtEmail = new TextBox { PlaceholderText = "Email", Top = 310, Left = 200 };

        _btnAdd = new Button { Text = "Add", Top = 350, Left = 10 };
        _btnDelete = new Button { Text = "Delete", Top = 350, Left = 100 };
        _btnRefresh = new Button { Text = "Refresh", Top = 350, Left = 190 };
        _btnLoadCsv = new Button { Text = "Load from CSV", Top = 350, Left = 280, Width = 200};

        _btnAdd.Click += AddUser;
        _btnDelete.Click += DeleteUser;
        _btnRefresh.Click += (_, _) => LoadUsers();
        _btnLoadCsv.Click += LoadFromCsv;
        
        _grid.CellEndEdit += GridCellEndEdit;
        _grid.DataBindingComplete += GridDataBindingComplete;

        Controls.Add(_grid);
        Controls.Add(_txtUsername);
        Controls.Add(_txtEmail);
        Controls.Add(_btnAdd);
        Controls.Add(_btnDelete);
        Controls.Add(_btnRefresh);
        Controls.Add(_btnLoadCsv);
    }

    private void LoadUsers()
    {
        _userBinding.DataSource = Users.List();
        _grid.DataSource = _userBinding;
        
        _grid.Columns["Id"]?.ReadOnly = true;
        _grid.Columns["CreatedDate"]?.ReadOnly = true;
    }

    private User? SelectedUser()
    {
        if (_grid.SelectedRows.Count == 0) return null;
        return (User?)_grid.SelectedRows[0].DataBoundItem;
    }

    private void AddUser(object? sender, EventArgs e)
    {
        var user = new User(
            _txtUsername.Text,
            _txtEmail.Text,
            DateTime.Now
        );
        if (!ValidateUser(user)) return;
        
        Users.Create(user);
        LoadUsers();
    }

    private void DeleteUser(object? sender, EventArgs e)
    {
        var user = SelectedUser();
        if (user == null) return;

        Users.Delete(user);
        LoadUsers();
    }
    
    private bool ValidateUser(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Username))
        {
            MessageBox.Show("Username cannot be empty.");
            return false;
        }
        if (string.IsNullOrWhiteSpace(user.Email))
        {
            MessageBox.Show("Email cannot be empty.");
            return false;
        }

        return true;
    }
    
    
    private void GridCellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var user = (User?)_grid.Rows[e.RowIndex].DataBoundItem!;
        if (!ValidateUser(user)) return;

        try
        {
            Users.Update(user);
            Window.FlashEditableCells(_grid, e.RowIndex, Color.LightGreen);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating user: {ex.Message}");
        }
    }
    
    private void GridDataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        _grid.Columns["Id"]?.ReadOnly = true;
        _grid.Columns["CreatedDate"]?.ReadOnly = true;
            
        _grid.Columns["Id"]?.DefaultCellStyle.BackColor = Color.LightGray;
        _grid.Columns["CreatedDate"]?.DefaultCellStyle.BackColor = Color.LightGray;
    }
    
    private void LoadFromCsv(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Filter = "CSV files|*.csv",
            Title = "Select CSV file"
        };

        if (dlg.ShowDialog() != DialogResult.OK) return;

        try
        {
            var lines = File.ReadAllLines(dlg.FileName);

            var isHeader = true;
            foreach (var line in lines)
            {
                if (isHeader)
                {
                    isHeader = false;
                    continue;
                }
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                var parts = line.Split(',');
                if (parts.Length < 1) continue;

                var username = parts[0].Trim();
                var email = parts.Length > 1 ? parts[1].Trim() : "";

                if (string.IsNullOrWhiteSpace(username)) continue;

                var user = new User(username, email);
                Users.Create(user);
            }

            LoadUsers();
            MessageBox.Show("CSV import completed successfully!");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading CSV: {ex.Message}");
        }
    }
}