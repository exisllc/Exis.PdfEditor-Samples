namespace Exis.PdfEditor.Demo.WinForms.Forms;

partial class WatermarkForm
{
    private System.ComponentModel.IContainer components = null;

    // Top panel
    private Panel topPanel;
    private Label lblTitle;
    private Label lblDescription;
    private Panel filePickerPanel;
    private Label lblFile;
    private TextBox txtFilePath;
    private Button btnBrowse;

    // Settings panel
    private Panel settingsPanel;
    private Label lblWatermarkTextLabel;
    private TextBox txtWatermarkText;
    private Label lblPosition;
    private ComboBox cboPosition;
    private Label lblFontSize;
    private NumericUpDown nudFontSize;
    private Label lblColor;
    private ComboBox cboColor;
    private Label lblOpacity;
    private TrackBar trkOpacity;
    private Label lblOpacityValue;
    private Label lblPageRange;
    private TextBox txtPageRange;

    // Bottom panel
    private Panel bottomPanel;
    private Button btnAddWatermark;
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
        var labelFont = new Font("Segoe UI", 9f);
        var accentColor = Color.FromArgb(37, 99, 235);

        // ---- Top panel ----
        topPanel = new Panel();
        topPanel.Dock = DockStyle.Top;
        topPanel.Height = 120;
        topPanel.Padding = new Padding(24, 16, 24, 8);
        topPanel.BackColor = Color.White;

        lblTitle = new Label();
        lblTitle.Text = "PDF Watermark";
        lblTitle.Font = headerFont;
        lblTitle.ForeColor = Color.FromArgb(15, 23, 42);
        lblTitle.AutoSize = true;
        lblTitle.Location = new Point(24, 16);

        lblDescription = new Label();
        lblDescription.Text = "Add text watermarks to PDF documents with configurable position, size, color, and opacity.";
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
        txtFilePath.Size = new Size(640, 28);
        txtFilePath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtFilePath.ReadOnly = true;
        txtFilePath.BackColor = Color.FromArgb(248, 250, 252);
        txtFilePath.BorderStyle = BorderStyle.FixedSingle;

        btnBrowse = new Button();
        btnBrowse.Text = "Browse...";
        btnBrowse.Size = new Size(90, 32);
        btnBrowse.Location = new Point(716, 2);
        btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBrowse.FlatStyle = FlatStyle.Flat;
        btnBrowse.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
        btnBrowse.BackColor = Color.White;
        btnBrowse.ForeColor = Color.FromArgb(51, 65, 85);
        btnBrowse.Font = new Font("Segoe UI", 9f);
        btnBrowse.Cursor = Cursors.Hand;

        filePickerPanel.Controls.AddRange(new Control[] { lblFile, txtFilePath, btnBrowse });
        topPanel.Controls.Add(filePickerPanel);
        topPanel.Controls.Add(lblDescription);
        topPanel.Controls.Add(lblTitle);

        // ---- Settings panel ----
        settingsPanel = new Panel();
        settingsPanel.Dock = DockStyle.Fill;
        settingsPanel.Padding = new Padding(24, 16, 24, 8);
        settingsPanel.BackColor = Color.White;

        int y = 8;

        lblWatermarkTextLabel = new Label();
        lblWatermarkTextLabel.Text = "Watermark Text:";
        lblWatermarkTextLabel.Font = labelFont;
        lblWatermarkTextLabel.ForeColor = Color.FromArgb(51, 65, 85);
        lblWatermarkTextLabel.Location = new Point(0, y);
        lblWatermarkTextLabel.AutoSize = true;

        txtWatermarkText = new TextBox();
        txtWatermarkText.Font = baseFont;
        txtWatermarkText.Text = "CONFIDENTIAL";
        txtWatermarkText.Location = new Point(120, y - 2);
        txtWatermarkText.Size = new Size(400, 28);
        txtWatermarkText.BorderStyle = BorderStyle.FixedSingle;

        y += 40;

        lblPosition = new Label();
        lblPosition.Text = "Position:";
        lblPosition.Font = labelFont;
        lblPosition.ForeColor = Color.FromArgb(51, 65, 85);
        lblPosition.Location = new Point(0, y);
        lblPosition.AutoSize = true;

        cboPosition = new ComboBox();
        cboPosition.Font = baseFont;
        cboPosition.DropDownStyle = ComboBoxStyle.DropDownList;
        cboPosition.Items.AddRange(new object[] { "Top", "Bottom", "Center", "Across" });
        cboPosition.SelectedIndex = 3;
        cboPosition.Location = new Point(120, y - 2);
        cboPosition.Size = new Size(150, 28);

        lblFontSize = new Label();
        lblFontSize.Text = "Font Size:";
        lblFontSize.Font = labelFont;
        lblFontSize.ForeColor = Color.FromArgb(51, 65, 85);
        lblFontSize.Location = new Point(300, y);
        lblFontSize.AutoSize = true;

        nudFontSize = new NumericUpDown();
        nudFontSize.Font = baseFont;
        nudFontSize.Minimum = 8;
        nudFontSize.Maximum = 200;
        nudFontSize.Value = 48;
        nudFontSize.Location = new Point(380, y - 2);
        nudFontSize.Size = new Size(80, 28);

        y += 40;

        lblColor = new Label();
        lblColor.Text = "Color:";
        lblColor.Font = labelFont;
        lblColor.ForeColor = Color.FromArgb(51, 65, 85);
        lblColor.Location = new Point(0, y);
        lblColor.AutoSize = true;

        cboColor = new ComboBox();
        cboColor.Font = baseFont;
        cboColor.DropDownStyle = ComboBoxStyle.DropDownList;
        cboColor.Items.AddRange(new object[] { "Light Gray", "Red", "Blue", "Green", "Black", "Orange" });
        cboColor.SelectedIndex = 0;
        cboColor.Location = new Point(120, y - 2);
        cboColor.Size = new Size(150, 28);

        y += 40;

        lblOpacity = new Label();
        lblOpacity.Text = "Opacity:";
        lblOpacity.Font = labelFont;
        lblOpacity.ForeColor = Color.FromArgb(51, 65, 85);
        lblOpacity.Location = new Point(0, y);
        lblOpacity.AutoSize = true;

        trkOpacity = new TrackBar();
        trkOpacity.Minimum = 0;
        trkOpacity.Maximum = 100;
        trkOpacity.Value = 30;
        trkOpacity.TickFrequency = 10;
        trkOpacity.Location = new Point(120, y - 6);
        trkOpacity.Size = new Size(300, 45);

        lblOpacityValue = new Label();
        lblOpacityValue.Text = "30%";
        lblOpacityValue.Font = baseFont;
        lblOpacityValue.ForeColor = Color.FromArgb(51, 65, 85);
        lblOpacityValue.Location = new Point(430, y);
        lblOpacityValue.AutoSize = true;

        y += 50;

        lblPageRange = new Label();
        lblPageRange.Text = "Pages (optional):";
        lblPageRange.Font = labelFont;
        lblPageRange.ForeColor = Color.FromArgb(51, 65, 85);
        lblPageRange.Location = new Point(0, y);
        lblPageRange.AutoSize = true;

        txtPageRange = new TextBox();
        txtPageRange.Font = baseFont;
        txtPageRange.Location = new Point(120, y - 2);
        txtPageRange.Size = new Size(200, 28);
        txtPageRange.BorderStyle = BorderStyle.FixedSingle;
        txtPageRange.PlaceholderText = "e.g. 1,2,3 (empty = all)";

        settingsPanel.Controls.AddRange(new Control[]
        {
            lblWatermarkTextLabel, txtWatermarkText,
            lblPosition, cboPosition, lblFontSize, nudFontSize,
            lblColor, cboColor,
            lblOpacity, trkOpacity, lblOpacityValue,
            lblPageRange, txtPageRange
        });

        // ---- Bottom panel ----
        bottomPanel = new Panel();
        bottomPanel.Dock = DockStyle.Bottom;
        bottomPanel.Height = 64;
        bottomPanel.Padding = new Padding(24, 8, 24, 12);
        bottomPanel.BackColor = Color.FromArgb(248, 250, 252);

        btnAddWatermark = new Button();
        btnAddWatermark.Text = "Add Watermark";
        btnAddWatermark.Size = new Size(150, 36);
        btnAddWatermark.Location = new Point(24, 12);
        btnAddWatermark.FlatStyle = FlatStyle.Flat;
        btnAddWatermark.FlatAppearance.BorderSize = 0;
        btnAddWatermark.BackColor = accentColor;
        btnAddWatermark.ForeColor = Color.White;
        btnAddWatermark.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        btnAddWatermark.Cursor = Cursors.Hand;

        lblStatus = new Label();
        lblStatus.Dock = DockStyle.Right;
        lblStatus.Width = 400;
        lblStatus.TextAlign = ContentAlignment.MiddleRight;
        lblStatus.Font = baseFont;
        lblStatus.ForeColor = Color.FromArgb(100, 116, 139);
        lblStatus.Text = "No file loaded.";

        bottomPanel.Controls.Add(btnAddWatermark);
        bottomPanel.Controls.Add(lblStatus);

        // ---- Form setup ----
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(960, 640);
        Text = "PDF Watermark";
        Font = baseFont;
        BackColor = Color.White;

        Controls.Add(settingsPanel);
        Controls.Add(bottomPanel);
        Controls.Add(topPanel);

        ResumeLayout(false);
        PerformLayout();
    }
}
