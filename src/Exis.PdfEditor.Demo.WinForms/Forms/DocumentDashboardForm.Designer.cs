namespace Exis.PdfEditor.Demo.WinForms.Forms;

partial class DocumentDashboardForm
{
    private System.ComponentModel.IContainer components = null;

    // Toolbar
    private ToolStrip toolStrip;
    private ToolStripButton tsbOpenFolder;
    private ToolStripButton tsbRefresh;
    private ToolStripSeparator tsSep1;
    private ToolStripButton tsbExportReport;
    private ToolStripSeparator tsSep2;
    private ToolStripLabel tslFilter;
    private ToolStripTextBox tstFilter;

    // Main split
    private SplitContainer mainSplitContainer;

    // Left - TreeView
    private TreeView tvFiles;
    private ImageList treeImageList;

    // Right split (top: metadata, bottom: text preview)
    private SplitContainer rightSplitContainer;

    // Right top - metadata panel
    private Panel metadataPanel;
    private Label lblMetadataTitle;
    private TableLayoutPanel metadataTable;
    private Label lblFileNameLabel;
    private Label lblFileNameValue;
    private Label lblPageCountLabel;
    private Label lblPageCountValue;
    private Label lblFileSizeLabel;
    private Label lblFileSizeValue;
    private Label lblAuthorLabel;
    private Label lblAuthorValue;
    private Label lblTitleLabel;
    private Label lblTitleValue;
    private Label lblCreatedLabel;
    private Label lblCreatedValue;
    private Label lblEncryptedLabel;
    private Label lblEncryptedValue;
    private Label lblPdfVersionLabel;
    private Label lblPdfVersionValue;

    // Right bottom - text preview
    private Panel textPreviewPanel;
    private Label lblTextPreview;
    private TextBox txtPreview;

    // Status bar
    private StatusStrip dashboardStatusStrip;
    private ToolStripStatusLabel tslTotalFiles;
    private ToolStripStatusLabel tslTotalPages;
    private ToolStripStatusLabel tslTotalSize;

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
        var metaLabelFont = new Font("Segoe UI", 9f, FontStyle.Bold);
        var metaValueFont = new Font("Segoe UI", 9f);
        var accentColor = Color.FromArgb(37, 99, 235);
        var labelColor = Color.FromArgb(100, 116, 139);
        var valueColor = Color.FromArgb(15, 23, 42);

        // ---- ToolStrip ----
        toolStrip = new ToolStrip();
        toolStrip.GripStyle = ToolStripGripStyle.Hidden;
        toolStrip.BackColor = Color.White;
        toolStrip.Padding = new Padding(8, 4, 8, 4);
        toolStrip.Font = baseFont;
        toolStrip.RenderMode = ToolStripRenderMode.System;

        tsbOpenFolder = new ToolStripButton("Open Folder");
        tsbOpenFolder.ToolTipText = "Open a folder containing PDF files";
        tsbOpenFolder.DisplayStyle = ToolStripItemDisplayStyle.Text;
        tsbOpenFolder.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
        tsbOpenFolder.ForeColor = accentColor;

        tsbRefresh = new ToolStripButton("Refresh");
        tsbRefresh.ToolTipText = "Reload all files";
        tsbRefresh.DisplayStyle = ToolStripItemDisplayStyle.Text;

        tsSep1 = new ToolStripSeparator();

        tsbExportReport = new ToolStripButton("Export Report");
        tsbExportReport.ToolTipText = "Export a metadata summary report";
        tsbExportReport.DisplayStyle = ToolStripItemDisplayStyle.Text;

        tsSep2 = new ToolStripSeparator();

        tslFilter = new ToolStripLabel("Filter:");
        tstFilter = new ToolStripTextBox();
        tstFilter.Size = new Size(180, 25);
        tstFilter.ToolTipText = "Type to filter files by name";
        tstFilter.BorderStyle = BorderStyle.FixedSingle;

        toolStrip.Items.AddRange(new ToolStripItem[]
        {
            tsbOpenFolder, tsbRefresh, tsSep1, tsbExportReport, tsSep2, tslFilter, tstFilter
        });

        // ---- Tree ImageList ----
        treeImageList = new ImageList(components);
        treeImageList.ColorDepth = ColorDepth.Depth32Bit;
        treeImageList.ImageSize = new Size(16, 16);

        // ---- Main SplitContainer ----
        mainSplitContainer = new SplitContainer();
        mainSplitContainer.Dock = DockStyle.Fill;
        mainSplitContainer.SplitterDistance = 280;
        mainSplitContainer.SplitterWidth = 6;
        mainSplitContainer.BackColor = Color.FromArgb(226, 232, 240);
        mainSplitContainer.Panel1.BackColor = Color.White;
        mainSplitContainer.Panel2.BackColor = Color.White;
        mainSplitContainer.Panel1MinSize = 200;

        // ---- Left: TreeView ----
        tvFiles = new TreeView();
        tvFiles.Dock = DockStyle.Fill;
        tvFiles.Font = baseFont;
        tvFiles.BorderStyle = BorderStyle.None;
        tvFiles.ShowLines = true;
        tvFiles.ShowPlusMinus = true;
        tvFiles.ShowRootLines = true;
        tvFiles.FullRowSelect = true;
        tvFiles.HideSelection = false;
        tvFiles.ItemHeight = 24;
        tvFiles.Indent = 20;
        tvFiles.BackColor = Color.White;
        tvFiles.ImageList = treeImageList;
        tvFiles.Padding = new Padding(4);

        mainSplitContainer.Panel1.Padding = new Padding(8);
        mainSplitContainer.Panel1.Controls.Add(tvFiles);

        // ---- Right SplitContainer ----
        rightSplitContainer = new SplitContainer();
        rightSplitContainer.Dock = DockStyle.Fill;
        rightSplitContainer.Orientation = Orientation.Horizontal;
        rightSplitContainer.SplitterDistance = 300;
        rightSplitContainer.SplitterWidth = 6;
        rightSplitContainer.BackColor = Color.FromArgb(226, 232, 240);
        rightSplitContainer.Panel1.BackColor = Color.White;
        rightSplitContainer.Panel2.BackColor = Color.White;

        // ---- Right top: Metadata panel ----
        metadataPanel = new Panel();
        metadataPanel.Dock = DockStyle.Fill;
        metadataPanel.Padding = new Padding(20, 16, 20, 16);
        metadataPanel.BackColor = Color.White;

        lblMetadataTitle = new Label();
        lblMetadataTitle.Text = "Document Properties";
        lblMetadataTitle.Font = headerFont;
        lblMetadataTitle.ForeColor = Color.FromArgb(15, 23, 42);
        lblMetadataTitle.Dock = DockStyle.Top;
        lblMetadataTitle.Height = 36;

        metadataTable = new TableLayoutPanel();
        metadataTable.Dock = DockStyle.Fill;
        metadataTable.ColumnCount = 2;
        metadataTable.RowCount = 8;
        metadataTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
        metadataTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        for (int i = 0; i < 8; i++)
            metadataTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        metadataTable.Padding = new Padding(0, 4, 0, 0);
        metadataTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;

        lblFileNameLabel = CreateMetaLabel("File Name:", metaLabelFont, labelColor);
        lblFileNameValue = CreateMetaValue("—", metaValueFont, valueColor);
        lblPageCountLabel = CreateMetaLabel("Page Count:", metaLabelFont, labelColor);
        lblPageCountValue = CreateMetaValue("—", metaValueFont, valueColor);
        lblFileSizeLabel = CreateMetaLabel("File Size:", metaLabelFont, labelColor);
        lblFileSizeValue = CreateMetaValue("—", metaValueFont, valueColor);
        lblAuthorLabel = CreateMetaLabel("Author:", metaLabelFont, labelColor);
        lblAuthorValue = CreateMetaValue("—", metaValueFont, valueColor);
        lblTitleLabel = CreateMetaLabel("Title:", metaLabelFont, labelColor);
        lblTitleValue = CreateMetaValue("—", metaValueFont, valueColor);
        lblCreatedLabel = CreateMetaLabel("Created:", metaLabelFont, labelColor);
        lblCreatedValue = CreateMetaValue("—", metaValueFont, valueColor);
        lblEncryptedLabel = CreateMetaLabel("Encrypted:", metaLabelFont, labelColor);
        lblEncryptedValue = CreateMetaValue("—", metaValueFont, valueColor);
        lblPdfVersionLabel = CreateMetaLabel("PDF Version:", metaLabelFont, labelColor);
        lblPdfVersionValue = CreateMetaValue("—", metaValueFont, valueColor);

        metadataTable.Controls.Add(lblFileNameLabel, 0, 0);
        metadataTable.Controls.Add(lblFileNameValue, 1, 0);
        metadataTable.Controls.Add(lblPageCountLabel, 0, 1);
        metadataTable.Controls.Add(lblPageCountValue, 1, 1);
        metadataTable.Controls.Add(lblFileSizeLabel, 0, 2);
        metadataTable.Controls.Add(lblFileSizeValue, 1, 2);
        metadataTable.Controls.Add(lblAuthorLabel, 0, 3);
        metadataTable.Controls.Add(lblAuthorValue, 1, 3);
        metadataTable.Controls.Add(lblTitleLabel, 0, 4);
        metadataTable.Controls.Add(lblTitleValue, 1, 4);
        metadataTable.Controls.Add(lblCreatedLabel, 0, 5);
        metadataTable.Controls.Add(lblCreatedValue, 1, 5);
        metadataTable.Controls.Add(lblEncryptedLabel, 0, 6);
        metadataTable.Controls.Add(lblEncryptedValue, 1, 6);
        metadataTable.Controls.Add(lblPdfVersionLabel, 0, 7);
        metadataTable.Controls.Add(lblPdfVersionValue, 1, 7);

        metadataPanel.Controls.Add(metadataTable);
        metadataPanel.Controls.Add(lblMetadataTitle);

        rightSplitContainer.Panel1.Controls.Add(metadataPanel);

        // ---- Right bottom: Text preview ----
        textPreviewPanel = new Panel();
        textPreviewPanel.Dock = DockStyle.Fill;
        textPreviewPanel.Padding = new Padding(20, 12, 20, 12);
        textPreviewPanel.BackColor = Color.White;

        lblTextPreview = new Label();
        lblTextPreview.Text = "Text Preview";
        lblTextPreview.Font = headerFont;
        lblTextPreview.ForeColor = Color.FromArgb(15, 23, 42);
        lblTextPreview.Dock = DockStyle.Top;
        lblTextPreview.Height = 32;

        txtPreview = new TextBox();
        txtPreview.Dock = DockStyle.Fill;
        txtPreview.Multiline = true;
        txtPreview.ReadOnly = true;
        txtPreview.ScrollBars = ScrollBars.Both;
        txtPreview.Font = new Font("Consolas", 9f);
        txtPreview.BackColor = Color.FromArgb(248, 250, 252);
        txtPreview.BorderStyle = BorderStyle.FixedSingle;
        txtPreview.WordWrap = false;

        textPreviewPanel.Controls.Add(txtPreview);
        textPreviewPanel.Controls.Add(lblTextPreview);

        rightSplitContainer.Panel2.Controls.Add(textPreviewPanel);

        mainSplitContainer.Panel2.Controls.Add(rightSplitContainer);

        // ---- Dashboard StatusStrip ----
        dashboardStatusStrip = new StatusStrip();
        dashboardStatusStrip.BackColor = Color.FromArgb(248, 250, 252);
        dashboardStatusStrip.Font = baseFont;
        dashboardStatusStrip.SizingGrip = false;

        tslTotalFiles = new ToolStripStatusLabel("Files: 0");
        tslTotalFiles.BorderSides = ToolStripStatusLabelBorderSides.Right;
        tslTotalFiles.BorderStyle = Border3DStyle.Etched;
        tslTotalFiles.Padding = new Padding(4, 0, 12, 0);

        tslTotalPages = new ToolStripStatusLabel("Pages: 0");
        tslTotalPages.BorderSides = ToolStripStatusLabelBorderSides.Right;
        tslTotalPages.BorderStyle = Border3DStyle.Etched;
        tslTotalPages.Padding = new Padding(4, 0, 12, 0);

        tslTotalSize = new ToolStripStatusLabel("Total Size: 0 B");
        tslTotalSize.Padding = new Padding(4, 0, 4, 0);

        dashboardStatusStrip.Items.AddRange(new ToolStripItem[] { tslTotalFiles, tslTotalPages, tslTotalSize });

        // ---- Form setup ----
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 680);
        Text = "Document Dashboard";
        Font = baseFont;
        BackColor = Color.White;

        Controls.Add(mainSplitContainer);
        Controls.Add(dashboardStatusStrip);
        Controls.Add(toolStrip);

        ResumeLayout(false);
        PerformLayout();
    }

    private static Label CreateMetaLabel(string text, Font font, Color color)
    {
        return new Label
        {
            Text = text,
            Font = font,
            ForeColor = color,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            AutoSize = false
        };
    }

    private static Label CreateMetaValue(string text, Font font, Color color)
    {
        return new Label
        {
            Text = text,
            Font = font,
            ForeColor = color,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            AutoSize = false,
            AutoEllipsis = true
        };
    }
}
