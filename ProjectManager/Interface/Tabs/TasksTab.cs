using ProjectManager.Database.Tables;
using Task = ProjectManager.Database.Tables.Task;

namespace ProjectManager.Interface.Tabs;

public sealed class TasksTab : TabPage
{
    private DataGridView _grid;
    private readonly BindingSource _taskBinding = new();

    private ComboBox _cmbProject;
    private ComboBox _cmbUser;
    private ComboBox _cmbStatus;
    private ComboBox _cmbPriority;
    private TextBox _txtTitle;
    private TextBox _txtDescription;

    private Button _btnAdd, _btnDelete, _btnRefresh;

    public TasksTab()
    {
        Text = "Tasks";
        Initialize();
        LoadLookups();
        LoadTasks();
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

        _cmbProject = new ComboBox { Top = 310, Left = 10, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
        _cmbUser = new ComboBox { Top = 310, Left = 170, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };

        _txtTitle = new TextBox { PlaceholderText = "Title", Top = 310, Left = 330, Width = 180 };
        _txtDescription = new TextBox { PlaceholderText = "Description", Top = 310, Left = 520, Width = 250 };

        _cmbStatus = new ComboBox { Top = 350, Left = 10, Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
        _cmbPriority = new ComboBox { Top = 350, Left = 140, Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };

        _btnAdd = new Button { Text = "Add", Top = 350, Left = 280 };
        _btnDelete = new Button { Text = "Delete", Top = 350, Left = 360 };
        _btnRefresh = new Button { Text = "Refresh", Top = 350, Left = 450 };

        _btnAdd.Click += AddTask;
        _btnDelete.Click += DeleteTask;
        _btnRefresh.Click += (_, _) => LoadTasks();

        _grid.CellEndEdit += GridCellEndEdit;
        _grid.DataBindingComplete += GridDataBindingComplete;

        Controls.AddRange([
            _grid,
            _cmbProject,
            _cmbUser,
            _txtTitle,
            _txtDescription,
            _cmbStatus,
            _cmbPriority,
            _btnAdd,
            _btnDelete,
            _btnRefresh
        ]);
    }

    private void LoadLookups()
    {
        _cmbProject.DataSource = Projects.List();
        _cmbProject.DisplayMember = "Title";

        _cmbUser.DataSource = Users.List();
        _cmbUser.DisplayMember = "Username";

        _cmbStatus.Items.AddRange("todo", "in_progress", "done");
        _cmbPriority.Items.AddRange("low", "medium", "high");

        _cmbStatus.SelectedIndex = 0;
        _cmbPriority.SelectedIndex = 1;
    }

    private void LoadTasks()
    {
        _taskBinding.DataSource = Tasks.List();
        _grid.DataSource = _taskBinding;
    }

    private Task? SelectedTask()
    {
        if (_grid.SelectedRows.Count == 0) return null;
        return (Task?)_grid.SelectedRows[0].DataBoundItem;
    }

    private void AddTask(object? sender, EventArgs e)
    {
        if (_cmbProject.SelectedItem is not Project project) return;
        if (_cmbUser.SelectedItem is not User user) return;

        var task = new Task(
            project,
            user,
            _txtTitle.Text,
            _txtDescription.Text,
            _cmbStatus.Text,
            _cmbPriority.Text,
            false
        );

        if (!ValidateTask(task)) return;

        Tasks.Create(task);
        LoadTasks();
    }

    private void DeleteTask(object? sender, EventArgs e)
    {
        var task = SelectedTask();
        if (task == null) return;

        Tasks.Delete(task);
        LoadTasks();
    }

    private bool ValidateTask(Task task)
    {
        if (string.IsNullOrWhiteSpace(task.Title))
        {
            MessageBox.Show("Task title cannot be empty.");
            return false;
        }

        return true;
    }

    private void GridCellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var task = (Task?)_grid.Rows[e.RowIndex].DataBoundItem!;
        if (!ValidateTask(task)) return;

        try
        {
            Tasks.Update(task);
            Window.FlashEditableCells(_grid, e.RowIndex, Color.LightGreen);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating task: {ex.Message}");
        }
        
        LoadTasks();
    }

    private void GridDataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        _grid.Columns["Id"]?.ReadOnly = true;
        _grid.Columns["Id"]?.DefaultCellStyle.BackColor = Color.LightGray;
    }
}