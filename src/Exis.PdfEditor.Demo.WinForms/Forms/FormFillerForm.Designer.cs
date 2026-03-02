namespace Exis.PdfEditor.Demo.WinForms.Forms;

partial class FormFillerForm
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
    private Button btnLoadFields;

    // Center - DataGridView
    private Panel centerPanel;
    private Label lblFields;
    private DataGridView dgvFields;
    private DataGridViewTextBoxColumn colFieldName;
    private DataGridViewTextBoxColumn colFieldType;
    private DataGridViewTextBoxColumn colFieldValue;

    // Bottom panel - action buttons
    private Panel bottomPanel;
    private FlowLayoutPanel actionButtonPanel;
    private Button btnFillAndSave;
    private Button btnFlattenAndSave;
    private Button btnResetValues;
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
        topPanel.Height = 140;
        topPanel.Padding = new Padding(24, 16, 24, 8);
        topPanel.BackColor = Color.White;

        lblTitle = new Label();
        lblTitle.Text = "PDF Form Filler";
        lblTitle.Font = headerFont;
        lblTitle.ForeColor = Color.FromArgb(15, 23, 42);
        lblTitle.AutoSize = true;
        lblTitle.Location = new Point(24, 16);

        lblDescription = new Label();
        lblDescription.Text = "Load a PDF form, view its fields, edit values, and save the filled or flattened result.";
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

        btnLoadFields = new Button();
        btnLoadFields.Text = "Load Fields";
        btnLoadFields.Size = new Size(110, 32);
        btnLoadFields.Location = new Point(716, 2);
        btnLoadFields.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnLoadFields.FlatStyle = FlatStyle.Flat;
        btnLoadFields.FlatAppearance.BorderSize = 0;
        btnLoadFields.BackColor = accentColor;
        btnLoadFields.ForeColor = Color.White;
        btnLoadFields.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
        btnLoadFields.Cursor = Cursors.Hand;

        filePickerPanel.Controls.AddRange(new Control[] { lblFile, txtFilePath, btnBrowse, btnLoadFields });

        topPanel.Controls.Add(filePickerPanel);
        topPanel.Controls.Add(lblDescription);
        topPanel.Controls.Add(lblTitle);

        // ---- Center panel with DataGridView ----
        centerPanel = new Panel();
        centerPanel.Dock = DockStyle.Fill;
        centerPanel.Padding = new Padding(24, 8, 24, 8);
        centerPanel.BackColor = Color.White;

        lblFields = new Label();
        lblFields.Text = "Form Fields";
        lblFields.Font = subHeaderFont;
        lblFields.ForeColor = Color.FromArgb(51, 65, 85);
        lblFields.Dock = DockStyle.Top;
        lblFields.Height = 28;

        dgvFields = new DataGridView();
        dgvFields.Dock = DockStyle.Fill;
        dgvFields.Font = baseFont;
        dgvFields.BackgroundColor = Color.White;
        dgvFields.BorderStyle = BorderStyle.FixedSingle;
        dgvFields.GridColor = Color.FromArgb(226, 232, 240);
        dgvFields.RowHeadersVisible = false;
        dgvFields.AllowUserToAddRows = false;
        dgvFields.AllowUserToDeleteRows = false;
        dgvFields.AllowUserToResizeRows = false;
        dgvFields.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvFields.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvFields.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
        dgvFields.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(51, 65, 85);
        dgvFields.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        dgvFields.ColumnHeadersDefaultCellStyle.Padding = new Padding(4);
        dgvFields.ColumnHeadersHeight = 36;
        dgvFields.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvFields.DefaultCellStyle.Padding = new Padding(4, 2, 4, 2);
        dgvFields.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
        dgvFields.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);
        dgvFields.EnableHeadersVisualStyles = false;
        dgvFields.RowTemplate.Height = 30;

        colFieldName = new DataGridViewTextBoxColumn();
        colFieldName.Name = "FieldName";
        colFieldName.HeaderText = "Field Name";
        colFieldName.ReadOnly = true;
        colFieldName.FillWeight = 35;

        colFieldType = new DataGridViewTextBoxColumn();
        colFieldType.Name = "FieldType";
        colFieldType.HeaderText = "Type";
        colFieldType.ReadOnly = true;
        colFieldType.FillWeight = 20;

        colFieldValue = new DataGridViewTextBoxColumn();
        colFieldValue.Name = "FieldValue";
        colFieldValue.HeaderText = "Value";
        colFieldValue.ReadOnly = false;
        colFieldValue.FillWeight = 45;

        dgvFields.Columns.AddRange(new DataGridViewColumn[] { colFieldName, colFieldType, colFieldValue });

        centerPanel.Controls.Add(dgvFields);
        centerPanel.Controls.Add(lblFields);

        // ---- Bottom panel ----
        bottomPanel = new Panel();
        bottomPanel.Dock = DockStyle.Bottom;
        bottomPanel.Height = 64;
        bottomPanel.Padding = new Padding(24, 8, 24, 12);
        bottomPanel.BackColor = Color.FromArgb(248, 250, 252);

        actionButtonPanel = new FlowLayoutPanel();
        actionButtonPanel.Dock = DockStyle.Left;
        actionButtonPanel.Width = 520;
        actionButtonPanel.FlowDirection = FlowDirection.LeftToRight;
        actionButtonPanel.WrapContents = false;
        actionButtonPanel.Padding = new Padding(0, 4, 0, 0);

        btnFillAndSave = new Button();
        btnFillAndSave.Text = "Fill && Save";
        btnFillAndSave.Size = new Size(130, 36);
        btnFillAndSave.FlatStyle = FlatStyle.Flat;
        btnFillAndSave.FlatAppearance.BorderSize = 0;
        btnFillAndSave.BackColor = accentColor;
        btnFillAndSave.ForeColor = Color.White;
        btnFillAndSave.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        btnFillAndSave.Cursor = Cursors.Hand;
        btnFillAndSave.Margin = new Padding(0, 0, 12, 0);
        btnFillAndSave.Enabled = false;

        btnFlattenAndSave = new Button();
        btnFlattenAndSave.Text = "Flatten && Save";
        btnFlattenAndSave.Size = new Size(140, 36);
        btnFlattenAndSave.FlatStyle = FlatStyle.Flat;
        btnFlattenAndSave.FlatAppearance.BorderSize = 0;
        btnFlattenAndSave.BackColor = Color.FromArgb(22, 163, 74);
        btnFlattenAndSave.ForeColor = Color.White;
        btnFlattenAndSave.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        btnFlattenAndSave.Cursor = Cursors.Hand;
        btnFlattenAndSave.Margin = new Padding(0, 0, 12, 0);
        btnFlattenAndSave.Enabled = false;

        btnResetValues = new Button();
        btnResetValues.Text = "Reset Values";
        btnResetValues.Size = new Size(120, 36);
        btnResetValues.FlatStyle = FlatStyle.Flat;
        btnResetValues.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
        btnResetValues.BackColor = Color.White;
        btnResetValues.ForeColor = Color.FromArgb(51, 65, 85);
        btnResetValues.Font = new Font("Segoe UI", 9.5f);
        btnResetValues.Cursor = Cursors.Hand;
        btnResetValues.Enabled = false;

        actionButtonPanel.Controls.AddRange(new Control[] { btnFillAndSave, btnFlattenAndSave, btnResetValues });

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
        Text = "PDF Form Filler";
        Font = baseFont;
        BackColor = Color.White;

        Controls.Add(centerPanel);
        Controls.Add(bottomPanel);
        Controls.Add(topPanel);

        ResumeLayout(false);
        PerformLayout();
    }
}
