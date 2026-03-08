namespace Exis.PdfEditor.Demo.WinForms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    private MenuStrip menuStrip;
    private ToolStripMenuItem fileMenu;
    private ToolStripMenuItem openMenuItem;
    private ToolStripMenuItem exitMenuItem;
    private ToolStripMenuItem toolsMenu;
    private ToolStripMenuItem findReplaceMenuItem;
    private ToolStripMenuItem batchProcessMenuItem;
    private ToolStripMenuItem formFillMenuItem;
    private ToolStripMenuItem dashboardMenuItem;
    private ToolStripMenuItem helpMenu;
    private ToolStripMenuItem aboutMenuItem;

    private StatusStrip statusStrip;
    private ToolStripStatusLabel statusLabel;
    private ToolStripProgressBar statusProgressBar;

    private TabControl mainTabControl;
    private TabPage quickActionsTab;
    private TabPage findReplaceTab;
    private TabPage batchProcessorTab;
    private TabPage formFillerTab;
    private TabPage dashboardTab;
    private TabPage imageEditorTab;
    private TabPage watermarkTab;

    // Quick Actions tab controls
    private Panel quickActionsTopPanel;
    private Label quickActionsTitle;
    private Label quickActionsSubtitle;
    private FlowLayoutPanel quickActionsButtonPanel;
    private Button btnOpenFindReplace;
    private Button btnOpenBatchProcessor;
    private Button btnOpenFormFiller;
    private Button btnOpenDashboard;
    private Button btnMergePdfs;
    private Controls.DragDropPanel dragDropPanel;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();

        // ---- MenuStrip ----
        menuStrip = new MenuStrip();
        fileMenu = new ToolStripMenuItem("&File");
        openMenuItem = new ToolStripMenuItem("&Open PDF...", null, null, Keys.Control | Keys.O);
        exitMenuItem = new ToolStripMenuItem("E&xit", null, null, Keys.Alt | Keys.F4);
        fileMenu.DropDownItems.AddRange(new ToolStripItem[] { openMenuItem, new ToolStripSeparator(), exitMenuItem });

        toolsMenu = new ToolStripMenuItem("&Tools");
        findReplaceMenuItem = new ToolStripMenuItem("Find && &Replace...");
        batchProcessMenuItem = new ToolStripMenuItem("&Batch Processor...");
        formFillMenuItem = new ToolStripMenuItem("&Form Filler...");
        dashboardMenuItem = new ToolStripMenuItem("&Document Dashboard...");
        toolsMenu.DropDownItems.AddRange(new ToolStripItem[] { findReplaceMenuItem, batchProcessMenuItem, formFillMenuItem, dashboardMenuItem });

        helpMenu = new ToolStripMenuItem("&Help");
        aboutMenuItem = new ToolStripMenuItem("&About Exis.PdfEditor Demo");
        helpMenu.DropDownItems.Add(aboutMenuItem);

        menuStrip.Items.AddRange(new ToolStripItem[] { fileMenu, toolsMenu, helpMenu });
        menuStrip.Font = new Font("Segoe UI", 9.5f);
        menuStrip.BackColor = Color.White;
        menuStrip.Padding = new Padding(8, 2, 0, 2);

        // ---- StatusStrip ----
        statusStrip = new StatusStrip();
        statusStrip.BackColor = Color.FromArgb(248, 250, 252);
        statusStrip.SizingGrip = true;
        statusStrip.Font = new Font("Segoe UI", 9f);

        statusLabel = new ToolStripStatusLabel("Ready");
        statusLabel.Spring = true;
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;

        statusProgressBar = new ToolStripProgressBar();
        statusProgressBar.Size = new Size(200, 18);
        statusProgressBar.Visible = false;

        statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel, statusProgressBar });

        // ---- TabControl ----
        mainTabControl = new TabControl();
        mainTabControl.Dock = DockStyle.Fill;
        mainTabControl.Font = new Font("Segoe UI", 10f);
        mainTabControl.Padding = new Point(16, 6);
        mainTabControl.Margin = new Padding(0);

        // ---- Quick Actions Tab ----
        quickActionsTab = new TabPage("Quick Actions");
        quickActionsTab.BackColor = Color.White;
        quickActionsTab.Padding = new Padding(24);

        quickActionsTopPanel = new Panel();
        quickActionsTopPanel.Dock = DockStyle.Top;
        quickActionsTopPanel.Height = 120;
        quickActionsTopPanel.Padding = new Padding(0, 8, 0, 8);

        quickActionsTitle = new Label();
        quickActionsTitle.Text = "Exis.PdfEditor — Quick Actions";
        quickActionsTitle.Font = new Font("Segoe UI", 18f, FontStyle.Bold);
        quickActionsTitle.ForeColor = Color.FromArgb(15, 23, 42);
        quickActionsTitle.AutoSize = true;
        quickActionsTitle.Location = new Point(0, 8);

        quickActionsSubtitle = new Label();
        quickActionsSubtitle.Text = "Launch a tool below or drag-and-drop PDF files for instant processing.";
        quickActionsSubtitle.Font = new Font("Segoe UI", 10.5f);
        quickActionsSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
        quickActionsSubtitle.AutoSize = true;
        quickActionsSubtitle.Location = new Point(2, 48);

        quickActionsTopPanel.Controls.Add(quickActionsSubtitle);
        quickActionsTopPanel.Controls.Add(quickActionsTitle);

        // Action buttons
        quickActionsButtonPanel = new FlowLayoutPanel();
        quickActionsButtonPanel.Dock = DockStyle.Top;
        quickActionsButtonPanel.Height = 60;
        quickActionsButtonPanel.FlowDirection = FlowDirection.LeftToRight;
        quickActionsButtonPanel.Padding = new Padding(0, 4, 0, 4);
        quickActionsButtonPanel.WrapContents = false;

        btnOpenFindReplace = CreateAccentButton("Find & Replace");
        btnOpenBatchProcessor = CreateAccentButton("Batch Processor");
        btnOpenFormFiller = CreateAccentButton("Form Filler");
        btnOpenDashboard = CreateAccentButton("Document Dashboard");
        btnMergePdfs = CreateAccentButton("Merge PDFs...");

        quickActionsButtonPanel.Controls.Add(btnOpenFindReplace);
        quickActionsButtonPanel.Controls.Add(btnOpenBatchProcessor);
        quickActionsButtonPanel.Controls.Add(btnOpenFormFiller);
        quickActionsButtonPanel.Controls.Add(btnOpenDashboard);
        quickActionsButtonPanel.Controls.Add(btnMergePdfs);

        // Drag-drop panel
        dragDropPanel = new Controls.DragDropPanel();
        dragDropPanel.Dock = DockStyle.Fill;
        dragDropPanel.MinimumSize = new Size(200, 150);

        quickActionsTab.Controls.Add(dragDropPanel);
        quickActionsTab.Controls.Add(quickActionsButtonPanel);
        quickActionsTab.Controls.Add(quickActionsTopPanel);

        // ---- Find & Replace Tab ----
        findReplaceTab = new TabPage("Find & Replace");
        findReplaceTab.BackColor = Color.White;

        // ---- Batch Processor Tab ----
        batchProcessorTab = new TabPage("Batch Processor");
        batchProcessorTab.BackColor = Color.White;

        // ---- Form Filler Tab ----
        formFillerTab = new TabPage("Form Filler");
        formFillerTab.BackColor = Color.White;

        // ---- Dashboard Tab ----
        dashboardTab = new TabPage("Dashboard");
        dashboardTab.BackColor = Color.White;

        // ---- Image Editor Tab ----
        imageEditorTab = new TabPage("Image Editor");
        imageEditorTab.BackColor = Color.White;

        // ---- Watermark Tab ----
        watermarkTab = new TabPage("Watermark");
        watermarkTab.BackColor = Color.White;

        // ---- Add tabs ----
        mainTabControl.TabPages.AddRange(new TabPage[]
        {
            quickActionsTab,
            findReplaceTab,
            batchProcessorTab,
            formFillerTab,
            dashboardTab,
            imageEditorTab,
            watermarkTab
        });

        // ---- Form setup ----
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1024, 768);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Exis.PdfEditor \u2014 WinForms Demo";
        Font = new Font("Segoe UI", 9.5f);
        BackColor = Color.White;
        MinimumSize = new Size(800, 600);

        MainMenuStrip = menuStrip;
        Controls.Add(mainTabControl);
        Controls.Add(statusStrip);
        Controls.Add(menuStrip);

        ResumeLayout(false);
        PerformLayout();
    }

    private static Button CreateAccentButton(string text)
    {
        var btn = new Button();
        btn.Text = text;
        btn.Size = new Size(160, 40);
        btn.Margin = new Padding(0, 0, 12, 0);
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.BackColor = Color.FromArgb(37, 99, 235);
        btn.ForeColor = Color.White;
        btn.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        btn.Cursor = Cursors.Hand;
        return btn;
    }
}
