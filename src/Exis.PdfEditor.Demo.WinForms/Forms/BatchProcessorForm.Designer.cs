namespace Exis.PdfEditor.Demo.WinForms.Forms;

partial class BatchProcessorForm
{
    private System.ComponentModel.IContainer components = null;

    private SplitContainer mainSplitContainer;

    // Left panel - file list
    private Panel leftPanel;
    private Label lblFileList;
    private ListBox lstFiles;
    private FlowLayoutPanel fileButtonPanel;
    private Button btnAddFiles;
    private Button btnRemoveFile;
    private Button btnClearFiles;

    // Right panel - operations
    private Panel rightPanel;
    private Label lblOperation;
    private ComboBox cmbOperation;
    private GroupBox grpOptions;
    private Label lblOutputFolder;
    private TextBox txtOutputFolder;
    private Button btnBrowseOutput;
    private CheckBox chkOverwrite;
    private CheckBox chkOpenWhenDone;
    private Label lblPdfAStandard;
    private ComboBox cmbPdfAStandard;

    // Bottom panel - progress and log
    private Panel bottomPanel;
    private FlowLayoutPanel processButtonPanel;
    private Button btnProcess;
    private Button btnCancel;
    private ProgressBar progressBar;
    private Label lblLog;
    private TextBox txtLog;

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

        // ---- Main SplitContainer ----
        mainSplitContainer = new SplitContainer();
        mainSplitContainer.Dock = DockStyle.Fill;
        mainSplitContainer.Orientation = Orientation.Horizontal;
        mainSplitContainer.SplitterDistance = 380;
        mainSplitContainer.SplitterWidth = 6;
        mainSplitContainer.BackColor = Color.FromArgb(226, 232, 240);
        mainSplitContainer.Panel1.BackColor = Color.White;
        mainSplitContainer.Panel2.BackColor = Color.White;

        // ===============================
        // TOP HALF — Left/Right split
        // ===============================
        var topSplit = new SplitContainer();
        topSplit.Dock = DockStyle.Fill;
        topSplit.SplitterDistance = 360;
        topSplit.SplitterWidth = 6;
        topSplit.BackColor = Color.FromArgb(226, 232, 240);
        topSplit.Panel1.BackColor = Color.White;
        topSplit.Panel2.BackColor = Color.White;

        // ---- Left panel: File list ----
        leftPanel = new Panel();
        leftPanel.Dock = DockStyle.Fill;
        leftPanel.Padding = new Padding(16);
        leftPanel.BackColor = Color.White;

        lblFileList = new Label();
        lblFileList.Text = "Queued PDF Files";
        lblFileList.Font = headerFont;
        lblFileList.ForeColor = Color.FromArgb(15, 23, 42);
        lblFileList.Dock = DockStyle.Top;
        lblFileList.Height = 32;

        lstFiles = new ListBox();
        lstFiles.Dock = DockStyle.Fill;
        lstFiles.Font = baseFont;
        lstFiles.BorderStyle = BorderStyle.FixedSingle;
        lstFiles.IntegralHeight = false;
        lstFiles.HorizontalScrollbar = true;
        lstFiles.SelectionMode = SelectionMode.MultiExtended;

        fileButtonPanel = new FlowLayoutPanel();
        fileButtonPanel.Dock = DockStyle.Bottom;
        fileButtonPanel.Height = 44;
        fileButtonPanel.FlowDirection = FlowDirection.LeftToRight;
        fileButtonPanel.Padding = new Padding(0, 6, 0, 0);
        fileButtonPanel.WrapContents = false;

        btnAddFiles = new Button();
        btnAddFiles.Text = "Add Files...";
        btnAddFiles.Size = new Size(110, 32);
        btnAddFiles.FlatStyle = FlatStyle.Flat;
        btnAddFiles.FlatAppearance.BorderSize = 0;
        btnAddFiles.BackColor = accentColor;
        btnAddFiles.ForeColor = Color.White;
        btnAddFiles.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
        btnAddFiles.Cursor = Cursors.Hand;
        btnAddFiles.Margin = new Padding(0, 0, 8, 0);

        btnRemoveFile = new Button();
        btnRemoveFile.Text = "Remove";
        btnRemoveFile.Size = new Size(90, 32);
        btnRemoveFile.FlatStyle = FlatStyle.Flat;
        btnRemoveFile.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
        btnRemoveFile.BackColor = Color.White;
        btnRemoveFile.ForeColor = Color.FromArgb(51, 65, 85);
        btnRemoveFile.Font = new Font("Segoe UI", 9f);
        btnRemoveFile.Cursor = Cursors.Hand;
        btnRemoveFile.Margin = new Padding(0, 0, 8, 0);

        btnClearFiles = new Button();
        btnClearFiles.Text = "Clear All";
        btnClearFiles.Size = new Size(90, 32);
        btnClearFiles.FlatStyle = FlatStyle.Flat;
        btnClearFiles.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
        btnClearFiles.BackColor = Color.White;
        btnClearFiles.ForeColor = Color.FromArgb(51, 65, 85);
        btnClearFiles.Font = new Font("Segoe UI", 9f);
        btnClearFiles.Cursor = Cursors.Hand;

        fileButtonPanel.Controls.AddRange(new Control[] { btnAddFiles, btnRemoveFile, btnClearFiles });

        leftPanel.Controls.Add(lstFiles);
        leftPanel.Controls.Add(fileButtonPanel);
        leftPanel.Controls.Add(lblFileList);

        topSplit.Panel1.Controls.Add(leftPanel);

        // ---- Right panel: Operation settings ----
        rightPanel = new Panel();
        rightPanel.Dock = DockStyle.Fill;
        rightPanel.Padding = new Padding(16);
        rightPanel.BackColor = Color.White;

        lblOperation = new Label();
        lblOperation.Text = "Batch Operation";
        lblOperation.Font = headerFont;
        lblOperation.ForeColor = Color.FromArgb(15, 23, 42);
        lblOperation.Dock = DockStyle.Top;
        lblOperation.Height = 32;

        cmbOperation = new ComboBox();
        cmbOperation.Dock = DockStyle.Top;
        cmbOperation.Font = baseFont;
        cmbOperation.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbOperation.Items.AddRange(new object[] { "Merge All", "Optimize All", "Convert to PDF/A", "Extract Text" });
        cmbOperation.SelectedIndex = 0;
        cmbOperation.Margin = new Padding(0, 8, 0, 0);

        grpOptions = new GroupBox();
        grpOptions.Text = "Options";
        grpOptions.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        grpOptions.ForeColor = Color.FromArgb(51, 65, 85);
        grpOptions.Dock = DockStyle.Fill;
        grpOptions.Padding = new Padding(12, 20, 12, 12);

        lblOutputFolder = new Label();
        lblOutputFolder.Text = "Output Folder:";
        lblOutputFolder.Font = baseFont;
        lblOutputFolder.Location = new Point(16, 32);
        lblOutputFolder.AutoSize = true;

        txtOutputFolder = new TextBox();
        txtOutputFolder.Font = baseFont;
        txtOutputFolder.Location = new Point(16, 56);
        txtOutputFolder.Size = new Size(220, 28);
        txtOutputFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtOutputFolder.ReadOnly = true;
        txtOutputFolder.BackColor = Color.FromArgb(248, 250, 252);

        btnBrowseOutput = new Button();
        btnBrowseOutput.Text = "...";
        btnBrowseOutput.Size = new Size(36, 28);
        btnBrowseOutput.Location = new Point(240, 56);
        btnBrowseOutput.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBrowseOutput.FlatStyle = FlatStyle.Flat;
        btnBrowseOutput.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
        btnBrowseOutput.Cursor = Cursors.Hand;

        chkOverwrite = new CheckBox();
        chkOverwrite.Text = "Overwrite existing files";
        chkOverwrite.Font = baseFont;
        chkOverwrite.Location = new Point(16, 96);
        chkOverwrite.AutoSize = true;

        chkOpenWhenDone = new CheckBox();
        chkOpenWhenDone.Text = "Open output folder when done";
        chkOpenWhenDone.Font = baseFont;
        chkOpenWhenDone.Location = new Point(16, 124);
        chkOpenWhenDone.AutoSize = true;
        chkOpenWhenDone.Checked = true;

        lblPdfAStandard = new Label();
        lblPdfAStandard.Text = "PDF/A Standard:";
        lblPdfAStandard.Font = baseFont;
        lblPdfAStandard.Location = new Point(16, 158);
        lblPdfAStandard.AutoSize = true;
        lblPdfAStandard.Visible = false;

        cmbPdfAStandard = new ComboBox();
        cmbPdfAStandard.Font = baseFont;
        cmbPdfAStandard.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbPdfAStandard.Items.AddRange(new object[] { "PDF/A-1b", "PDF/A-2b", "PDF/A-3b" });
        cmbPdfAStandard.SelectedIndex = 1;
        cmbPdfAStandard.Location = new Point(16, 182);
        cmbPdfAStandard.Size = new Size(180, 28);
        cmbPdfAStandard.Visible = false;

        grpOptions.Controls.AddRange(new Control[]
        {
            lblOutputFolder, txtOutputFolder, btnBrowseOutput,
            chkOverwrite, chkOpenWhenDone,
            lblPdfAStandard, cmbPdfAStandard
        });

        rightPanel.Controls.Add(grpOptions);
        rightPanel.Controls.Add(cmbOperation);
        rightPanel.Controls.Add(lblOperation);

        topSplit.Panel2.Controls.Add(rightPanel);

        mainSplitContainer.Panel1.Controls.Add(topSplit);

        // ===============================
        // BOTTOM HALF — Progress and log
        // ===============================
        bottomPanel = new Panel();
        bottomPanel.Dock = DockStyle.Fill;
        bottomPanel.Padding = new Padding(16);
        bottomPanel.BackColor = Color.White;

        processButtonPanel = new FlowLayoutPanel();
        processButtonPanel.Dock = DockStyle.Top;
        processButtonPanel.Height = 48;
        processButtonPanel.FlowDirection = FlowDirection.LeftToRight;
        processButtonPanel.Padding = new Padding(0, 0, 0, 8);
        processButtonPanel.WrapContents = false;

        btnProcess = new Button();
        btnProcess.Text = "Process Batch";
        btnProcess.Size = new Size(140, 36);
        btnProcess.FlatStyle = FlatStyle.Flat;
        btnProcess.FlatAppearance.BorderSize = 0;
        btnProcess.BackColor = accentColor;
        btnProcess.ForeColor = Color.White;
        btnProcess.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
        btnProcess.Cursor = Cursors.Hand;
        btnProcess.Margin = new Padding(0, 0, 12, 0);

        btnCancel = new Button();
        btnCancel.Text = "Cancel";
        btnCancel.Size = new Size(90, 36);
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
        btnCancel.BackColor = Color.White;
        btnCancel.ForeColor = Color.FromArgb(51, 65, 85);
        btnCancel.Font = new Font("Segoe UI", 9.5f);
        btnCancel.Cursor = Cursors.Hand;
        btnCancel.Enabled = false;

        processButtonPanel.Controls.AddRange(new Control[] { btnProcess, btnCancel });

        progressBar = new ProgressBar();
        progressBar.Dock = DockStyle.Top;
        progressBar.Height = 24;
        progressBar.Style = ProgressBarStyle.Continuous;

        lblLog = new Label();
        lblLog.Text = "Processing Log";
        lblLog.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
        lblLog.ForeColor = Color.FromArgb(51, 65, 85);
        lblLog.Dock = DockStyle.Top;
        lblLog.Height = 28;
        lblLog.Padding = new Padding(0, 6, 0, 2);

        txtLog = new TextBox();
        txtLog.Dock = DockStyle.Fill;
        txtLog.Multiline = true;
        txtLog.ReadOnly = true;
        txtLog.ScrollBars = ScrollBars.Vertical;
        txtLog.Font = new Font("Consolas", 9f);
        txtLog.BackColor = Color.FromArgb(248, 250, 252);
        txtLog.BorderStyle = BorderStyle.FixedSingle;
        txtLog.WordWrap = false;

        bottomPanel.Controls.Add(txtLog);
        bottomPanel.Controls.Add(lblLog);
        bottomPanel.Controls.Add(progressBar);
        bottomPanel.Controls.Add(processButtonPanel);

        mainSplitContainer.Panel2.Controls.Add(bottomPanel);

        // ---- Form setup ----
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(960, 700);
        Text = "Batch Processor";
        Font = baseFont;
        BackColor = Color.White;
        Padding = new Padding(0);

        Controls.Add(mainSplitContainer);

        ResumeLayout(false);
        PerformLayout();
    }
}
