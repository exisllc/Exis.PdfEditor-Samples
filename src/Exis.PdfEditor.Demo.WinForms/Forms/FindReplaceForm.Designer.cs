namespace Exis.PdfEditor.Demo.WinForms.Forms;

partial class FindReplaceForm
{
    private System.ComponentModel.IContainer components = null;

    // Input
    private Label lblInputFile;
    private TextBox txtInputFile;
    private Button btnBrowseInput;

    // Search/Replace
    private GroupBox grpSearchReplace;
    private Label lblSearch;
    private TextBox txtSearch;
    private Label lblReplace;
    private TextBox txtReplace;

    // Options
    private GroupBox grpOptions;
    private CheckBox chkRegex;
    private CheckBox chkAdaptiveFitting;
    private Label lblTextColor;
    private ComboBox cmbTextColor;
    private Label lblHighlightColor;
    private ComboBox cmbHighlightColor;

    // Actions
    private FlowLayoutPanel actionPanel;
    private Button btnExecute;
    private Button btnSaveAs;

    // Result
    private Label lblResult;
    private TextBox txtResult;
    private ProgressBar progressBar;

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
        var headerFont = new Font("Segoe UI", 11f, FontStyle.Bold);
        var accentColor = Color.FromArgb(37, 99, 235);

        var mainPanel = new Panel();
        mainPanel.Dock = DockStyle.Fill;
        mainPanel.Padding = new Padding(24);
        mainPanel.BackColor = Color.White;
        mainPanel.AutoScroll = true;

        var contentPanel = new Panel();
        contentPanel.Dock = DockStyle.Top;
        contentPanel.Height = 560;
        contentPanel.MaximumSize = new Size(720, 0);

        int y = 0;

        // ---- Input File ----
        lblInputFile = new Label();
        lblInputFile.Text = "Input PDF File";
        lblInputFile.Font = headerFont;
        lblInputFile.ForeColor = Color.FromArgb(15, 23, 42);
        lblInputFile.Location = new Point(0, y);
        lblInputFile.AutoSize = true;
        y += 30;

        txtInputFile = new TextBox();
        txtInputFile.Font = baseFont;
        txtInputFile.Location = new Point(0, y);
        txtInputFile.Size = new Size(580, 28);
        txtInputFile.ReadOnly = true;
        txtInputFile.BackColor = Color.FromArgb(248, 250, 252);
        txtInputFile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        btnBrowseInput = new Button();
        btnBrowseInput.Text = "Browse...";
        btnBrowseInput.Size = new Size(90, 28);
        btnBrowseInput.Location = new Point(588, y);
        btnBrowseInput.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBrowseInput.FlatStyle = FlatStyle.Flat;
        btnBrowseInput.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
        btnBrowseInput.Cursor = Cursors.Hand;
        btnBrowseInput.Font = baseFont;
        y += 44;

        // ---- Search & Replace ----
        grpSearchReplace = new GroupBox();
        grpSearchReplace.Text = "Search & Replace";
        grpSearchReplace.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        grpSearchReplace.ForeColor = Color.FromArgb(51, 65, 85);
        grpSearchReplace.Location = new Point(0, y);
        grpSearchReplace.Size = new Size(680, 140);
        grpSearchReplace.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        lblSearch = new Label();
        lblSearch.Text = "Search text:";
        lblSearch.Font = baseFont;
        lblSearch.Location = new Point(16, 28);
        lblSearch.AutoSize = true;

        txtSearch = new TextBox();
        txtSearch.Font = baseFont;
        txtSearch.Location = new Point(16, 50);
        txtSearch.Size = new Size(640, 28);
        txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        lblReplace = new Label();
        lblReplace.Text = "Replace with:";
        lblReplace.Font = baseFont;
        lblReplace.Location = new Point(16, 84);
        lblReplace.AutoSize = true;

        txtReplace = new TextBox();
        txtReplace.Font = baseFont;
        txtReplace.Location = new Point(16, 106);
        txtReplace.Size = new Size(640, 28);
        txtReplace.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        grpSearchReplace.Controls.AddRange(new Control[] { lblSearch, txtSearch, lblReplace, txtReplace });
        y += 152;

        // ---- Options ----
        grpOptions = new GroupBox();
        grpOptions.Text = "Options";
        grpOptions.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        grpOptions.ForeColor = Color.FromArgb(51, 65, 85);
        grpOptions.Location = new Point(0, y);
        grpOptions.Size = new Size(680, 160);
        grpOptions.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        chkRegex = new CheckBox();
        chkRegex.Text = "Use regular expressions";
        chkRegex.Font = baseFont;
        chkRegex.Location = new Point(16, 28);
        chkRegex.AutoSize = true;

        chkAdaptiveFitting = new CheckBox();
        chkAdaptiveFitting.Text = "Shrink text to fit (adaptive)";
        chkAdaptiveFitting.Font = baseFont;
        chkAdaptiveFitting.Location = new Point(220, 28);
        chkAdaptiveFitting.AutoSize = true;
        chkAdaptiveFitting.Checked = true;

        lblTextColor = new Label();
        lblTextColor.Text = "Replacement text color:";
        lblTextColor.Font = baseFont;
        lblTextColor.Location = new Point(16, 64);
        lblTextColor.AutoSize = true;

        cmbTextColor = new ComboBox();
        cmbTextColor.Font = baseFont;
        cmbTextColor.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbTextColor.Items.AddRange(new object[] { "None", "Red", "Blue", "Green", "Orange", "Magenta", "Cyan", "Black" });
        cmbTextColor.SelectedIndex = 0;
        cmbTextColor.Location = new Point(16, 88);
        cmbTextColor.Size = new Size(180, 28);

        lblHighlightColor = new Label();
        lblHighlightColor.Text = "Highlight color:";
        lblHighlightColor.Font = baseFont;
        lblHighlightColor.Location = new Point(240, 64);
        lblHighlightColor.AutoSize = true;

        cmbHighlightColor = new ComboBox();
        cmbHighlightColor.Font = baseFont;
        cmbHighlightColor.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbHighlightColor.Items.AddRange(new object[] { "None", "Yellow", "Cyan", "Green", "Magenta", "Orange", "Red", "Blue" });
        cmbHighlightColor.SelectedIndex = 0;
        cmbHighlightColor.Location = new Point(240, 88);
        cmbHighlightColor.Size = new Size(180, 28);

        grpOptions.Controls.AddRange(new Control[]
        {
            chkRegex, chkAdaptiveFitting,
            lblTextColor, cmbTextColor,
            lblHighlightColor, cmbHighlightColor
        });
        y += 172;

        // ---- Action Buttons ----
        actionPanel = new FlowLayoutPanel();
        actionPanel.Location = new Point(0, y);
        actionPanel.Size = new Size(680, 44);
        actionPanel.FlowDirection = FlowDirection.LeftToRight;
        actionPanel.WrapContents = false;

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
        y += 50;

        // ---- Progress ----
        progressBar = new ProgressBar();
        progressBar.Location = new Point(0, y);
        progressBar.Size = new Size(680, 20);
        progressBar.Style = ProgressBarStyle.Marquee;
        progressBar.Visible = false;
        progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        y += 28;

        // ---- Result ----
        lblResult = new Label();
        lblResult.Text = "Result";
        lblResult.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
        lblResult.ForeColor = Color.FromArgb(51, 65, 85);
        lblResult.Location = new Point(0, y);
        lblResult.AutoSize = true;
        y += 24;

        txtResult = new TextBox();
        txtResult.Font = new Font("Consolas", 9f);
        txtResult.Location = new Point(0, y);
        txtResult.Size = new Size(680, 60);
        txtResult.Multiline = true;
        txtResult.ReadOnly = true;
        txtResult.ScrollBars = ScrollBars.Vertical;
        txtResult.BackColor = Color.FromArgb(248, 250, 252);
        txtResult.BorderStyle = BorderStyle.FixedSingle;
        txtResult.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        contentPanel.Controls.AddRange(new Control[]
        {
            lblInputFile, txtInputFile, btnBrowseInput,
            grpSearchReplace, grpOptions, actionPanel,
            progressBar, lblResult, txtResult
        });

        mainPanel.Controls.Add(contentPanel);

        // ---- Form setup ----
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(960, 700);
        Text = "Find & Replace";
        Font = baseFont;
        BackColor = Color.White;
        Padding = new Padding(0);

        Controls.Add(mainPanel);

        ResumeLayout(false);
        PerformLayout();
    }
}
