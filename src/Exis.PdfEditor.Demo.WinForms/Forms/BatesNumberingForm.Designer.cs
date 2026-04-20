namespace Exis.PdfEditor.Demo.WinForms.Forms;

partial class BatesNumberingForm
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
    private Label lblPrefix;
    private TextBox txtPrefix;
    private Label lblSuffix;
    private TextBox txtSuffix;
    private Label lblStartNumber;
    private NumericUpDown nudStartNumber;
    private Label lblDigits;
    private NumericUpDown nudDigits;
    private Label lblPosition;
    private ComboBox cboPosition;
    private Label lblFontSize;
    private NumericUpDown nudFontSize;
    private Label lblColor;
    private ComboBox cboColor;
    private Label lblConfidentiality;
    private TextBox txtConfidentiality;
    private CheckBox chkSkipFirstPage;
    private CheckBox chkCounterAdvancesOnSkipped;

    // Bottom panel
    private Panel bottomPanel;
    private Button btnApply;
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
        lblTitle.Text = "Bates Numbering";
        lblTitle.Font = headerFont;
        lblTitle.ForeColor = Color.FromArgb(15, 23, 42);
        lblTitle.AutoSize = true;
        lblTitle.Location = new Point(24, 16);

        lblDescription = new Label();
        lblDescription.Text = "Apply sequential Bates identifiers to a PDF for legal discovery and document production.";
        lblDescription.Font = baseFont;
        lblDescription.ForeColor = Color.FromArgb(100, 116, 139);
        lblDescription.AutoSize = true;
        lblDescription.Location = new Point(26, 48);

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

        lblPrefix = new Label();
        lblPrefix.Text = "Prefix:";
        lblPrefix.Font = labelFont;
        lblPrefix.ForeColor = Color.FromArgb(51, 65, 85);
        lblPrefix.Location = new Point(0, y);
        lblPrefix.AutoSize = true;

        txtPrefix = new TextBox();
        txtPrefix.Font = baseFont;
        txtPrefix.Text = "ABC";
        txtPrefix.Location = new Point(130, y - 2);
        txtPrefix.Size = new Size(150, 28);
        txtPrefix.BorderStyle = BorderStyle.FixedSingle;

        lblSuffix = new Label();
        lblSuffix.Text = "Suffix:";
        lblSuffix.Font = labelFont;
        lblSuffix.ForeColor = Color.FromArgb(51, 65, 85);
        lblSuffix.Location = new Point(300, y);
        lblSuffix.AutoSize = true;

        txtSuffix = new TextBox();
        txtSuffix.Font = baseFont;
        txtSuffix.Location = new Point(380, y - 2);
        txtSuffix.Size = new Size(150, 28);
        txtSuffix.BorderStyle = BorderStyle.FixedSingle;

        y += 40;

        lblStartNumber = new Label();
        lblStartNumber.Text = "Start Number:";
        lblStartNumber.Font = labelFont;
        lblStartNumber.ForeColor = Color.FromArgb(51, 65, 85);
        lblStartNumber.Location = new Point(0, y);
        lblStartNumber.AutoSize = true;

        nudStartNumber = new NumericUpDown();
        nudStartNumber.Font = baseFont;
        nudStartNumber.Minimum = 1;
        nudStartNumber.Maximum = 999999999;
        nudStartNumber.Value = 1;
        nudStartNumber.Location = new Point(130, y - 2);
        nudStartNumber.Size = new Size(120, 28);

        lblDigits = new Label();
        lblDigits.Text = "Digits:";
        lblDigits.Font = labelFont;
        lblDigits.ForeColor = Color.FromArgb(51, 65, 85);
        lblDigits.Location = new Point(300, y);
        lblDigits.AutoSize = true;

        nudDigits = new NumericUpDown();
        nudDigits.Font = baseFont;
        nudDigits.Minimum = 1;
        nudDigits.Maximum = 12;
        nudDigits.Value = 6;
        nudDigits.Location = new Point(380, y - 2);
        nudDigits.Size = new Size(80, 28);

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
        cboPosition.Items.AddRange(new object[] { "TopLeft", "TopCenter", "TopRight", "BottomLeft", "BottomCenter", "BottomRight" });
        cboPosition.SelectedIndex = 5;
        cboPosition.Location = new Point(130, y - 2);
        cboPosition.Size = new Size(150, 28);

        lblFontSize = new Label();
        lblFontSize.Text = "Font Size:";
        lblFontSize.Font = labelFont;
        lblFontSize.ForeColor = Color.FromArgb(51, 65, 85);
        lblFontSize.Location = new Point(300, y);
        lblFontSize.AutoSize = true;

        nudFontSize = new NumericUpDown();
        nudFontSize.Font = baseFont;
        nudFontSize.Minimum = 6;
        nudFontSize.Maximum = 48;
        nudFontSize.Value = 10;
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
        cboColor.Items.AddRange(new object[] { "Black", "Red", "Blue", "Green", "Orange" });
        cboColor.SelectedIndex = 0;
        cboColor.Location = new Point(130, y - 2);
        cboColor.Size = new Size(150, 28);

        y += 40;

        lblConfidentiality = new Label();
        lblConfidentiality.Text = "Confidentiality:";
        lblConfidentiality.Font = labelFont;
        lblConfidentiality.ForeColor = Color.FromArgb(51, 65, 85);
        lblConfidentiality.Location = new Point(0, y);
        lblConfidentiality.AutoSize = true;

        txtConfidentiality = new TextBox();
        txtConfidentiality.Font = baseFont;
        txtConfidentiality.Location = new Point(130, y - 2);
        txtConfidentiality.Size = new Size(300, 28);
        txtConfidentiality.BorderStyle = BorderStyle.FixedSingle;
        txtConfidentiality.PlaceholderText = "optional — e.g. CONFIDENTIAL";

        y += 40;

        chkSkipFirstPage = new CheckBox();
        chkSkipFirstPage.Text = "Skip first page (cover sheet)";
        chkSkipFirstPage.Font = labelFont;
        chkSkipFirstPage.ForeColor = Color.FromArgb(51, 65, 85);
        chkSkipFirstPage.Location = new Point(0, y);
        chkSkipFirstPage.AutoSize = true;

        y += 28;

        chkCounterAdvancesOnSkipped = new CheckBox();
        chkCounterAdvancesOnSkipped.Text = "Counter advances on skipped pages";
        chkCounterAdvancesOnSkipped.Font = labelFont;
        chkCounterAdvancesOnSkipped.ForeColor = Color.FromArgb(51, 65, 85);
        chkCounterAdvancesOnSkipped.Location = new Point(0, y);
        chkCounterAdvancesOnSkipped.AutoSize = true;
        chkCounterAdvancesOnSkipped.Checked = true;

        settingsPanel.Controls.AddRange(new Control[]
        {
            lblPrefix, txtPrefix, lblSuffix, txtSuffix,
            lblStartNumber, nudStartNumber, lblDigits, nudDigits,
            lblPosition, cboPosition, lblFontSize, nudFontSize,
            lblColor, cboColor,
            lblConfidentiality, txtConfidentiality,
            chkSkipFirstPage, chkCounterAdvancesOnSkipped
        });

        // ---- Bottom panel ----
        bottomPanel = new Panel();
        bottomPanel.Dock = DockStyle.Bottom;
        bottomPanel.Height = 64;
        bottomPanel.Padding = new Padding(24, 8, 24, 12);
        bottomPanel.BackColor = Color.FromArgb(248, 250, 252);

        btnApply = new Button();
        btnApply.Text = "Apply Bates Stamp";
        btnApply.Size = new Size(170, 36);
        btnApply.Location = new Point(24, 12);
        btnApply.FlatStyle = FlatStyle.Flat;
        btnApply.FlatAppearance.BorderSize = 0;
        btnApply.BackColor = accentColor;
        btnApply.ForeColor = Color.White;
        btnApply.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        btnApply.Cursor = Cursors.Hand;

        lblStatus = new Label();
        lblStatus.Dock = DockStyle.Right;
        lblStatus.Width = 420;
        lblStatus.TextAlign = ContentAlignment.MiddleRight;
        lblStatus.Font = baseFont;
        lblStatus.ForeColor = Color.FromArgb(100, 116, 139);
        lblStatus.Text = "No file loaded.";

        bottomPanel.Controls.Add(btnApply);
        bottomPanel.Controls.Add(lblStatus);

        // ---- Form setup ----
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(960, 640);
        Text = "Bates Numbering";
        Font = baseFont;
        BackColor = Color.White;

        Controls.Add(settingsPanel);
        Controls.Add(bottomPanel);
        Controls.Add(topPanel);

        ResumeLayout(false);
        PerformLayout();
    }
}
