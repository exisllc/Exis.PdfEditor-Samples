namespace Exis.PdfEditor.Demo.WinForms.Forms;

partial class FindReplaceForm
{
    private System.ComponentModel.IContainer components = null;

    // Top panel
    private Panel topPanel;
    private Label lblTitle;
    private Label lblDescription;
    private Panel filePickerPanel;
    private Label lblFile;
    private TextBox txtInputFile;
    private Button btnBrowseInput;

    // Settings panel
    private Panel settingsPanel;
    private Label lblSearch;
    private TextBox txtSearch;
    private Label lblReplace;
    private TextBox txtReplace;
    private CheckBox chkRegex;
    private CheckBox chkAdaptiveFitting;
    private Label lblTextColor;
    private ComboBox cmbTextColor;
    private Label lblHighlightColor;
    private ComboBox cmbHighlightColor;

    // Bottom panel
    private Panel bottomPanel;
    private FlowLayoutPanel actionPanel;
    private Button btnExecute;
    private Button btnSaveAs;
    private ProgressBar progressBar;
    private Label lblResult;
    private TextBox txtResult;

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
        var baseFont = new Font("Segoe UI", 9.5f);
        var headerFont = new Font("Segoe UI", 14f, FontStyle.Bold);
        var labelFont = new Font("Segoe UI", 9f);
        var accentColor = Color.FromArgb(37, 99, 235);

        // ---- Top panel ----
        topPanel = new Panel();
        topPanel.Dock = DockStyle.Top;
        topPanel.Height = 120;
        topPanel.Padding = new Padding(24, 16, 24, 8);
        topPanel.BackColor = Color.White;

        lblTitle = new Label();
        lblTitle.Text = "Find & Replace";
        lblTitle.Font = headerFont;
        lblTitle.ForeColor = Color.FromArgb(15, 23, 42);
        lblTitle.AutoSize = true;
        lblTitle.Location = new Point(24, 16);

        lblDescription = new Label();
        lblDescription.Text = "Search for text in a PDF and replace it, with optional regex, color, and highlight styling.";
        lblDescription.Font = baseFont;
        lblDescription.ForeColor = Color.FromArgb(100, 116, 139);
        lblDescription.AutoSize = true;
        lblDescription.Location = new Point(26, 48);

        // PDF file picker
        filePickerPanel = new Panel();
        filePickerPanel.Location = new Point(24, 80);
        filePickerPanel.Size = new Size(900, 40);
        filePickerPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        lblFile = new Label();
        lblFile.Text = "PDF File:";
        lblFile.Font = baseFont;
        lblFile.ForeColor = Color.FromArgb(51, 65, 85);
        lblFile.Location = new Point(0, 8);
        lblFile.AutoSize = true;

        txtInputFile = new TextBox();
        txtInputFile.Font = baseFont;
        txtInputFile.Location = new Point(70, 4);
        txtInputFile.Size = new Size(640, 28);
        txtInputFile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtInputFile.ReadOnly = true;
        txtInputFile.BackColor = Color.FromArgb(248, 250, 252);
        txtInputFile.BorderStyle = BorderStyle.FixedSingle;

        btnBrowseInput = new Button();
        btnBrowseInput.Text = "Browse...";
        btnBrowseInput.Size = new Size(90, 32);
        btnBrowseInput.Location = new Point(716, 2);
        btnBrowseInput.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBrowseInput.FlatStyle = FlatStyle.Flat;
        btnBrowseInput.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
        btnBrowseInput.BackColor = Color.White;
        btnBrowseInput.ForeColor = Color.FromArgb(51, 65, 85);
        btnBrowseInput.Font = new Font("Segoe UI", 9f);
        btnBrowseInput.Cursor = Cursors.Hand;

        filePickerPanel.Controls.AddRange(new Control[] { lblFile, txtInputFile, btnBrowseInput });
        topPanel.Controls.Add(filePickerPanel);
        topPanel.Controls.Add(lblDescription);
        topPanel.Controls.Add(lblTitle);

        // ---- Settings panel (Fill) ----
        settingsPanel = new Panel();
        settingsPanel.Dock = DockStyle.Fill;
        settingsPanel.Padding = new Padding(24, 16, 24, 8);
        settingsPanel.BackColor = Color.White;

        int y = 8;

        lblSearch = new Label();
        lblSearch.Text = "Search text:";
        lblSearch.Font = labelFont;
        lblSearch.ForeColor = Color.FromArgb(51, 65, 85);
        lblSearch.Location = new Point(0, y);
        lblSearch.AutoSize = true;

        txtSearch = new TextBox();
        txtSearch.Font = baseFont;
        txtSearch.Location = new Point(120, y - 2);
        txtSearch.Size = new Size(500, 28);
        txtSearch.BorderStyle = BorderStyle.FixedSingle;
        txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        y += 40;

        lblReplace = new Label();
        lblReplace.Text = "Replace with:";
        lblReplace.Font = labelFont;
        lblReplace.ForeColor = Color.FromArgb(51, 65, 85);
        lblReplace.Location = new Point(0, y);
        lblReplace.AutoSize = true;

        txtReplace = new TextBox();
        txtReplace.Font = baseFont;
        txtReplace.Location = new Point(120, y - 2);
        txtReplace.Size = new Size(500, 28);
        txtReplace.BorderStyle = BorderStyle.FixedSingle;
        txtReplace.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        y += 48;

        chkRegex = new CheckBox();
        chkRegex.Text = "Use regular expressions";
        chkRegex.Font = baseFont;
        chkRegex.Location = new Point(0, y);
        chkRegex.AutoSize = true;

        chkAdaptiveFitting = new CheckBox();
        chkAdaptiveFitting.Text = "Shrink text to fit (adaptive)";
        chkAdaptiveFitting.Font = baseFont;
        chkAdaptiveFitting.Location = new Point(220, y);
        chkAdaptiveFitting.AutoSize = true;
        chkAdaptiveFitting.Checked = true;

        y += 40;

        lblTextColor = new Label();
        lblTextColor.Text = "Text color:";
        lblTextColor.Font = labelFont;
        lblTextColor.ForeColor = Color.FromArgb(51, 65, 85);
        lblTextColor.Location = new Point(0, y);
        lblTextColor.AutoSize = true;

        cmbTextColor = new ComboBox();
        cmbTextColor.Font = baseFont;
        cmbTextColor.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbTextColor.Items.AddRange(new object[] { "None", "Red", "Blue", "Green", "Orange", "Magenta", "Cyan", "Black" });
        cmbTextColor.SelectedIndex = 0;
        cmbTextColor.Location = new Point(120, y - 2);
        cmbTextColor.Size = new Size(150, 28);

        lblHighlightColor = new Label();
        lblHighlightColor.Text = "Highlight color:";
        lblHighlightColor.Font = labelFont;
        lblHighlightColor.ForeColor = Color.FromArgb(51, 65, 85);
        lblHighlightColor.Location = new Point(300, y);
        lblHighlightColor.AutoSize = true;

        cmbHighlightColor = new ComboBox();
        cmbHighlightColor.Font = baseFont;
        cmbHighlightColor.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbHighlightColor.Items.AddRange(new object[] { "None", "Yellow", "Cyan", "Green", "Magenta", "Orange", "Red", "Blue" });
        cmbHighlightColor.SelectedIndex = 0;
        cmbHighlightColor.Location = new Point(420, y - 2);
        cmbHighlightColor.Size = new Size(150, 28);

        settingsPanel.Controls.AddRange(new Control[]
        {
            lblSearch, txtSearch,
            lblReplace, txtReplace,
            chkRegex, chkAdaptiveFitting,
            lblTextColor, cmbTextColor,
            lblHighlightColor, cmbHighlightColor
        });

        // ---- Bottom panel ----
        bottomPanel = new Panel();
        bottomPanel.Dock = DockStyle.Bottom;
        bottomPanel.Height = 150;
        bottomPanel.Padding = new Padding(24, 8, 24, 12);
        bottomPanel.BackColor = Color.FromArgb(248, 250, 252);

        actionPanel = new FlowLayoutPanel();
        actionPanel.Dock = DockStyle.Top;
        actionPanel.Height = 44;
        actionPanel.FlowDirection = FlowDirection.LeftToRight;
        actionPanel.WrapContents = false;
        actionPanel.Padding = new Padding(0, 4, 0, 4);

        btnExecute = new Button();
        btnExecute.Text = "Find && Replace";
        btnExecute.Size = new Size(140, 36);
        btnExecute.FlatStyle = FlatStyle.Flat;
        btnExecute.FlatAppearance.BorderSize = 0;
        btnExecute.BackColor = accentColor;
        btnExecute.ForeColor = Color.White;
        btnExecute.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
        btnExecute.Cursor = Cursors.Hand;
        btnExecute.Margin = new Padding(0, 0, 12, 0);

        btnSaveAs = new Button();
        btnSaveAs.Text = "Save As...";
        btnSaveAs.Size = new Size(110, 36);
        btnSaveAs.FlatStyle = FlatStyle.Flat;
        btnSaveAs.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
        btnSaveAs.BackColor = Color.White;
        btnSaveAs.ForeColor = Color.FromArgb(51, 65, 85);
        btnSaveAs.Font = new Font("Segoe UI", 9.5f);
        btnSaveAs.Cursor = Cursors.Hand;

        actionPanel.Controls.AddRange(new Control[] { btnExecute, btnSaveAs });

        progressBar = new ProgressBar();
        progressBar.Dock = DockStyle.Top;
        progressBar.Height = 20;
        progressBar.Style = ProgressBarStyle.Marquee;
        progressBar.Visible = false;

        lblResult = new Label();
        lblResult.Text = "Result";
        lblResult.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
        lblResult.ForeColor = Color.FromArgb(51, 65, 85);
        lblResult.Dock = DockStyle.Top;
        lblResult.Height = 22;

        txtResult = new TextBox();
        txtResult.Font = new Font("Consolas", 9f);
        txtResult.Dock = DockStyle.Fill;
        txtResult.Multiline = true;
        txtResult.ReadOnly = true;
        txtResult.ScrollBars = ScrollBars.Vertical;
        txtResult.BackColor = Color.FromArgb(248, 250, 252);
        txtResult.BorderStyle = BorderStyle.FixedSingle;

        bottomPanel.Controls.Add(txtResult);
        bottomPanel.Controls.Add(lblResult);
        bottomPanel.Controls.Add(progressBar);
        bottomPanel.Controls.Add(actionPanel);

        // ---- Form setup ----
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(960, 700);
        Text = "Find & Replace";
        Font = baseFont;
        BackColor = Color.White;

        Controls.Add(settingsPanel);
        Controls.Add(bottomPanel);
        Controls.Add(topPanel);

        ResumeLayout(false);
        PerformLayout();
    }
}
