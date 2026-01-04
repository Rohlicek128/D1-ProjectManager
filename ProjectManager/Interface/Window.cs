using Timer = System.Windows.Forms.Timer;

namespace ProjectManager.Interface;

public partial class Window : Form
{
    public Window()
    {
        InitializeComponent();
    }
    
    public static void FlashEditableCells(DataGridView grid, int rowIndex, Color color, int interval = 1000)
    {
        foreach (DataGridViewCell cell in grid.Rows[rowIndex].Cells)
        {
            if (!cell.OwningColumn!.ReadOnly) cell.Style.BackColor = color;
        }
        var timer = new Timer { Interval = interval };
        timer.Tick += (_, _) =>
        {
            foreach (DataGridViewCell cell in grid.Rows[rowIndex].Cells)
            {
                if (!cell.OwningColumn!.ReadOnly)
                    cell.Style.BackColor = Color.White;
            }
            timer.Stop();
        };
        timer.Start();
    }
}