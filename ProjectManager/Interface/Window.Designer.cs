using ProjectManager.Interface.Tabs;

namespace ProjectManager.Interface;

partial class Window
{
    private TabControl _tabControl;
    
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }
    
    private void SetAllControlsFont(Control.ControlCollection ctrls)
    {
        foreach(Control ctrl in ctrls)
        {
            if(ctrl.Controls != null)
                SetAllControlsFont(ctrl.Controls);
            ctrl.Font = new Font("Roboto", ctrl.Font.Size + 3);
        }
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this._tabControl = new TabControl();
        this._tabControl.Dock = DockStyle.Fill;
        
        this._tabControl.TabPages.Add(new ProjectsTab());
        this._tabControl.TabPages.Add(new UsersTab());
        this._tabControl.TabPages.Add(new RolesTab());
        this._tabControl.TabPages.Add(new TasksTab());
        this._tabControl.TabPages.Add(new UsersProjectsTab());

        this.Controls.Add(this._tabControl);
        
        SetAllControlsFont(this.Controls);
        
        SuspendLayout();
        // 
        // Form1
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Text = "Project Manager";
        ResumeLayout(false);
    }

    #endregion
}