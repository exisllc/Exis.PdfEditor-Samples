namespace Exis.PdfEditor.Demo.WinForms.Forms;

partial class ImageEditorForm
{
    private System.ComponentModel.IContainer components = null;

    // Top panel - file selection
    private Panel topPanel;
    private Label lblTitle;
    private Label lblDescription;
    private Panel filePickerPanel;
    private Label lblFile;
    private TextBox txtFilePath;
    private Button btnBrowse;
    private Button btnFindImages;

    // Image picker
    private Panel imagePickerPanel;
    private Label lblImage;
    private TextBox txtImagePath;
    private Button btnBrowseImage;

    // Center - DataGridView
    private Panel centerPanel;
    private Label lblImages;
    private DataGridView dgvImages;
    private DataGridViewTextBoxColumn colIndex;
    private DataGridViewTextBoxColumn colDimensions;
    private DataGridViewTextBoxColumn colColorSpace;
    private DataGridViewTextBoxColumn colFormat;
    private DataGridViewTextBoxColumn colPages;

    // Bottom panel - action buttons
    private Panel bottomPanel;
    private FlowLayoutPanel actionButtonPanel;
    private Button btnReplaceAll;
    private Button btnReplaceSelected;
    private Label lblStatus;

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
        var subHeaderFont = new Font("Segoe UI", 10f, FontStyle.Bold);
        var accentColor = Color.FromArgb(37, 99, 235);

        // ---- Top panel ----
        topPanel = new Panel();
        topPanel.Dock = DockStyle.Top;
        topPanel.Height = 180;
        topPanel.Padding = new Padding(24, 16, 24, 8);
        topPanel.BackColor = Color.White;

        lblTitle = new Label();
        lblTitle.Text = "PDF Image Editor";
        lblTitle.Font = headerFont;
        lblTitle.ForeColor = Color.FromArgb(15, 23, 42);
        lblTitle.AutoSize = true;
        lblTitle.Location = new Point(24, 16);

        lblDescription = new Label();
        lblDescription.Text = "Find all images in a PDF, then replace all or specific images with a new JPEG or PNG.";
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

        txtFilePath = new TextBox();
        txtFilePath.Font = baseFont;
        txtFilePath.Location = new Point(70, 4);
        txtFilePath.Size = new Size(540, 28);
        txtFilePath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtFilePath.ReadOnly = true;
        txtFilePath.BackColor = Color.FromArgb(248, 250, 252);
        txtFilePath.BorderStyle = BorderStyle.FixedSingle;

        btnBrowse = new Button();
        btnBrowse.Text = "Browse...";
        btnBrowse.Size = new Size(90, 32);
        btnBrowse.Location = new Point(618, 2);
        btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBrowse.FlatStyle = FlatStyle.Flat;
        btnBrowse.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
        btnBrowse.BackColor = Color.White;
        btnBrowse.ForeColor = Color.FromArgb(51, 65, 85);
        btnBrowse.Font = new Font("Segoe UI", 9f);
        btnBrowse.Cursor = Cursors.Hand;

        btnFindImages = new Button();
        btnFindImages.Text = "Find Images";
        btnFindImages.Size = new Size(110, 32);
        btnFindImages.Location = new Point(716, 2);
        btnFindImages.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnFindImages.FlatStyle = FlatStyle.Flat;
        btnFindImages.FlatAppearance.BorderSize = 0;
        btnFindImages.BackColor = accentColor;
        btnFindImages.ForeColor = Color.White;
        btnFindImages.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
        btnFindImages.Cursor = Cursors.Hand;

        filePickerPanel.Controls.AddRange(new Control[] { lblFile, txtFilePath, btnBrowse, btnFindImages });

        // Image picker
        imagePickerPanel = new Panel();
        imagePickerPanel.Location = new Point(24, 130);
        imagePickerPanel.Size = new Size(900, 40);
        imagePickerPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        lblImage = new Label();
        lblImage.Text = "Replace:";
        lblImage.Font = baseFont;
        lblImage.ForeColor = Color.FromArgb(51, 65, 85);
        lblImage.Location = new Point(0, 8);
        lblImage.AutoSize = true;

        txtImagePath = new TextBox();
        txtImagePath.Font = baseFont;
        txtImagePath.Location = new Point(70, 4);
        txtImagePath.Size = new Size(640, 28);
        txtImagePath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtImagePath.ReadOnly = true;
        txtImagePath.BackColor = Color.FromArgb(248, 250, 252);
        txtImagePath.BorderStyle = BorderStyle.FixedSingle;

        btnBrowseImage = new Button();
        btnBrowseImage.Text = "Browse...";
        btnBrowseImage.Size = new Size(90, 32);
        btnBrowseImage.Location = new Point(716, 2);
        btnBrowseImage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBrowseImage.FlatStyle = FlatStyle.Flat;
        btnBrowseImage.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
        btnBrowseImage.BackColor = Color.White;
        btnBrowseImage.ForeColor = Color.FromArgb(51, 65, 85);
        btnBrowseImage.Font = new Font("Segoe UI", 9f);
        btnBrowseImage.Cursor = Cursors.Hand;

        imagePickerPanel.Controls.AddRange(new Control[] { lblImage, txtImagePath, btnBrowseImage });

        topPanel.Controls.Add(imagePickerPanel);
        topPanel.Controls.Add(filePickerPanel);
        topPanel.Controls.Add(lblDescription);
        topPanel.Controls.Add(lblTitle);

        // ---- Center panel with DataGridView ----
        centerPanel = new Panel();
        centerPanel.Dock = DockStyle.Fill;
        centerPanel.Padding = new Padding(24, 8, 24, 8);
        centerPanel.BackColor = Color.White;

        lblImages = new Label();
        lblImages.Text = "Images Found";
        lblImages.Font = subHeaderFont;
        lblImages.ForeColor = Color.FromArgb(51, 65, 85);
        lblImages.Dock = DockStyle.Top;
        lblImages.Height = 28;

        dgvImages = new DataGridView();
        dgvImages.Dock = DockStyle.Fill;
        dgvImages.Font = baseFont;
        dgvImages.BackgroundColor = Color.White;
        dgvImages.BorderStyle = BorderStyle.FixedSingle;
        dgvImages.GridColor = Color.FromArgb(226, 232, 240);
        dgvImages.RowHeadersVisible = false;
        dgvImages.AllowUserToAddRows = false;
        dgvImages.AllowUserToDeleteRows = false;
        dgvImages.AllowUserToResizeRows = false;
        dgvImages.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvImages.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvImages.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
        dgvImages.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(51, 65, 85);
        dgvImages.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        dgvImages.ColumnHeadersDefaultCellStyle.Padding = new Padding(4);
        dgvImages.ColumnHeadersHeight = 36;
        dgvImages.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvImages.DefaultCellStyle.Padding = new Padding(4, 2, 4, 2);
        dgvImages.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
        dgvImages.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);
        dgvImages.EnableHeadersVisualStyles = false;
        dgvImages.RowTemplate.Height = 30;
        dgvImages.ReadOnly = true;
        dgvImages.MultiSelect = true;

        colIndex = new DataGridViewTextBoxColumn();
        colIndex.Name = "colIndex";
        colIndex.HeaderText = "#";
        colIndex.FillWeight = 10;

        colDimensions = new DataGridViewTextBoxColumn();
        colDimensions.Name = "colDimensions";
        colDimensions.HeaderText = "Dimensions";
        colDimensions.FillWeight = 25;

        colColorSpace = new DataGridViewTextBoxColumn();
        colColorSpace.Name = "colColorSpace";
        colColorSpace.HeaderText = "Color Space";
        colColorSpace.FillWeight = 20;

        colFormat = new DataGridViewTextBoxColumn();
        colFormat.Name = "colFormat";
        colFormat.HeaderText = "Format";
        colFormat.FillWeight = 20;

        colPages = new DataGridViewTextBoxColumn();
        colPages.Name = "colPages";
        colPages.HeaderText = "Pages";
        colPages.FillWeight = 25;

        dgvImages.Columns.AddRange(new DataGridViewColumn[] { colIndex, colDimensions, colColorSpace, colFormat, colPages });

        centerPanel.Controls.Add(dgvImages);
        centerPanel.Controls.Add(lblImages);

        // ---- Bottom panel ----
        bottomPanel = new Panel();
        bottomPanel.Dock = DockStyle.Bottom;
        bottomPanel.Height = 64;
        bottomPanel.Padding = new Padding(24, 8, 24, 12);
        bottomPanel.BackColor = Color.FromArgb(248, 250, 252);

        actionButtonPanel = new FlowLayoutPanel();
        actionButtonPanel.Dock = DockStyle.Left;
        actionButtonPanel.Width = 400;
        actionButtonPanel.FlowDirection = FlowDirection.LeftToRight;
        actionButtonPanel.WrapContents = false;
        actionButtonPanel.Padding = new Padding(0, 4, 0, 0);

        btnReplaceAll = new Button();
        btnReplaceAll.Text = "Replace All";
        btnReplaceAll.Size = new Size(130, 36);
        btnReplaceAll.FlatStyle = FlatStyle.Flat;
        btnReplaceAll.FlatAppearance.BorderSize = 0;
        btnReplaceAll.BackColor = accentColor;
        btnReplaceAll.ForeColor = Color.White;
        btnReplaceAll.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        btnReplaceAll.Cursor = Cursors.Hand;
        btnReplaceAll.Margin = new Padding(0, 0, 12, 0);
        btnReplaceAll.Enabled = false;

        btnReplaceSelected = new Button();
        btnReplaceSelected.Text = "Replace Selected";
        btnReplaceSelected.Size = new Size(150, 36);
        btnReplaceSelected.FlatStyle = FlatStyle.Flat;
        btnReplaceSelected.FlatAppearance.BorderSize = 0;
        btnReplaceSelected.BackColor = Color.FromArgb(22, 163, 74);
        btnReplaceSelected.ForeColor = Color.White;
        btnReplaceSelected.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        btnReplaceSelected.Cursor = Cursors.Hand;
        btnReplaceSelected.Margin = new Padding(0, 0, 12, 0);
        btnReplaceSelected.Enabled = false;

        actionButtonPanel.Controls.AddRange(new Control[] { btnReplaceAll, btnReplaceSelected });

        lblStatus = new Label();
        lblStatus.Dock = DockStyle.Right;
        lblStatus.Width = 300;
        lblStatus.TextAlign = ContentAlignment.MiddleRight;
        lblStatus.Font = baseFont;
        lblStatus.ForeColor = Color.FromArgb(100, 116, 139);
        lblStatus.Text = "No file loaded.";

        bottomPanel.Controls.Add(actionButtonPanel);
        bottomPanel.Controls.Add(lblStatus);

        // ---- Form setup ----
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(960, 640);
        Text = "PDF Image Editor";
        Font = baseFont;
        BackColor = Color.White;

        Controls.Add(centerPanel);
        Controls.Add(bottomPanel);
        Controls.Add(topPanel);

        ResumeLayout(false);
        PerformLayout();
    }
}
