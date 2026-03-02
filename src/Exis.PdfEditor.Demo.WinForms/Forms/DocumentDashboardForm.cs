using Exis.PdfEditor;

namespace Exis.PdfEditor.Demo.WinForms.Forms;

public partial class DocumentDashboardForm : Form
{
    private readonly List<PdfFileEntry> _allEntries = new();
    private string? _currentFolderPath;

    public DocumentDashboardForm()
    {
        InitializeComponent();
        WireUpEvents();
        ClearMetadata();
    }

    private void WireUpEvents()
    {
        tsbOpenFolder.Click += OnOpenFolder;
        tsbRefresh.Click += OnRefresh;
        tsbExportReport.Click += OnExportReport;
        tvFiles.AfterSelect += OnFileSelected;
        tstFilter.TextChanged += OnFilterChanged;
    }

    public void LoadSingleFile(string filePath)
    {
        _allEntries.Clear();
        tvFiles.Nodes.Clear();
        _currentFolderPath = Path.GetDirectoryName(filePath);

        var entry = new PdfFileEntry { FilePath = filePath };
        _allEntries.Add(entry);

        var node = tvFiles.Nodes.Add(Path.GetFileName(filePath));
        node.Tag = entry;

        tvFiles.SelectedNode = node;
        UpdateStatusBar();
    }

    private void OnOpenFolder(object? sender, EventArgs e)
    {
        using var fbd = new FolderBrowserDialog
        {
            Description = "Select a folder containing PDF files",
            UseDescriptionForTitle = true
        };

        if (fbd.ShowDialog() == DialogResult.OK)
        {
            _currentFolderPath = fbd.SelectedPath;
            LoadFolder(_currentFolderPath);
        }
    }

    private void LoadFolder(string folderPath)
    {
        _allEntries.Clear();
        tvFiles.Nodes.Clear();
        ClearMetadata();

        var rootNode = tvFiles.Nodes.Add(Path.GetFileName(folderPath));
        rootNode.Tag = folderPath;

        LoadFolderRecursive(folderPath, rootNode);

        rootNode.Expand();
        UpdateStatusBar();

        if (_allEntries.Count == 0)
        {
            MessageBox.Show("No PDF files found in the selected folder.", "No Files",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void LoadFolderRecursive(string folderPath, TreeNode parentNode)
    {
        try
        {
            var directories = Directory.GetDirectories(folderPath)
                .OrderBy(d => Path.GetFileName(d));

            foreach (var dir in directories)
            {
                var dirNode = parentNode.Nodes.Add(Path.GetFileName(dir));
                dirNode.Tag = dir;
                LoadFolderRecursive(dir, dirNode);

                if (dirNode.Nodes.Count == 0)
                    parentNode.Nodes.Remove(dirNode);
            }

            var pdfFiles = Directory.GetFiles(folderPath, "*.pdf", SearchOption.TopDirectoryOnly)
                .OrderBy(f => Path.GetFileName(f));

            foreach (var file in pdfFiles)
            {
                var entry = new PdfFileEntry { FilePath = file };
                _allEntries.Add(entry);

                var fileNode = parentNode.Nodes.Add(Path.GetFileName(file));
                fileNode.Tag = entry;
            }
        }
        catch (UnauthorizedAccessException)
        {
            // Skip folders we cannot access
        }
    }

    private async void OnFileSelected(object? sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is not PdfFileEntry entry)
        {
            ClearMetadata();
            return;
        }

        await LoadFileMetadataAsync(entry);
    }

    private async Task LoadFileMetadataAsync(PdfFileEntry entry)
    {
        var filePath = entry.FilePath;

        // Set basic file info immediately
        lblFileNameValue.Text = Path.GetFileName(filePath);

        try
        {
            var fileInfo = new FileInfo(filePath);
            lblFileSizeValue.Text = FormatFileSize(fileInfo.Length);
            entry.FileSize = fileInfo.Length;
        }
        catch
        {
            lblFileSizeValue.Text = "N/A";
        }

        // Load PDF-specific properties using PdfInspector
        try
        {
            var info = await PdfInspector.InspectAsync(filePath);

            entry.PageCount = info.PageCount;
            entry.Author = info.Author;
            entry.Title = info.Title;
            entry.CreationDate = info.CreationDate;
            entry.IsEncrypted = info.IsEncrypted;
            entry.PdfVersion = info.Version;
            entry.IsLoaded = true;

            lblPageCountValue.Text = info.PageCount.ToString();
            lblAuthorValue.Text = info.Author ?? "N/A";
            lblTitleValue.Text = info.Title ?? "N/A";
            lblCreatedValue.Text = info.CreationDate?.ToString("g") ?? "N/A";
            lblEncryptedValue.Text = info.IsEncrypted ? "Yes" : "No";
            lblEncryptedValue.ForeColor = info.IsEncrypted
                ? Color.FromArgb(220, 38, 38)
                : Color.FromArgb(22, 163, 74);
            lblPdfVersionValue.Text = info.Version ?? "N/A";

            // Load text preview
            try
            {
                var textResult = PdfTextExtractor.ExtractText(filePath);
                var text = textResult.FullText;
                var preview = text.Length > 5000 ? text[..5000] + "\n\n--- (truncated) ---" : text;
                txtPreview.Text = preview;
            }
            catch
            {
                txtPreview.Text = "(Unable to extract text from this document)";
            }

            UpdateStatusBar();
        }
        catch (Exception ex)
        {
            lblPageCountValue.Text = "Error";
            lblAuthorValue.Text = "\u2014";
            lblTitleValue.Text = "\u2014";
            lblCreatedValue.Text = "\u2014";
            lblEncryptedValue.Text = "\u2014";
            lblEncryptedValue.ForeColor = Color.FromArgb(15, 23, 42);
            lblPdfVersionValue.Text = "\u2014";
            txtPreview.Text = $"Error loading document: {ex.Message}";
        }
    }

    private void OnRefresh(object? sender, EventArgs e)
    {
        if (_currentFolderPath != null && Directory.Exists(_currentFolderPath))
        {
            LoadFolder(_currentFolderPath);
        }
        else if (_allEntries.Count == 1)
        {
            var entry = _allEntries[0];
            tvFiles.Nodes.Clear();
            var node = tvFiles.Nodes.Add(Path.GetFileName(entry.FilePath));
            node.Tag = entry;
            entry.IsLoaded = false;
            tvFiles.SelectedNode = node;
        }
    }

    private async void OnExportReport(object? sender, EventArgs e)
    {
        if (_allEntries.Count == 0)
        {
            MessageBox.Show("No PDF files loaded. Open a folder first.", "No Data",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var sfd = new SaveFileDialog
        {
            Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt",
            Title = "Export Report",
            FileName = "pdf_report.csv"
        };

        if (sfd.ShowDialog() != DialogResult.OK)
            return;

        try
        {
            Cursor = Cursors.WaitCursor;

            // Ensure all entries are loaded
            foreach (var entry in _allEntries.Where(e => !e.IsLoaded))
            {
                try
                {
                    var info = await PdfInspector.InspectAsync(entry.FilePath);
                    entry.PageCount = info.PageCount;
                    entry.Author = info.Author;
                    entry.Title = info.Title;
                    entry.CreationDate = info.CreationDate;
                    entry.IsEncrypted = info.IsEncrypted;
                    entry.PdfVersion = info.Version;
                    entry.FileSize = new FileInfo(entry.FilePath).Length;
                    entry.IsLoaded = true;
                }
                catch
                {
                    // skip files that cannot be loaded
                }
            }

            var lines = new List<string>
            {
                "File Name,File Path,Pages,File Size,Author,Title,Created,Encrypted,PDF Version"
            };

            foreach (var entry in _allEntries)
            {
                var name = Path.GetFileName(entry.FilePath);
                var csvLine = string.Join(",",
                    Escape(name),
                    Escape(entry.FilePath),
                    entry.PageCount,
                    entry.FileSize,
                    Escape(entry.Author ?? ""),
                    Escape(entry.Title ?? ""),
                    Escape(entry.CreationDate?.ToString("o") ?? ""),
                    entry.IsEncrypted ? "Yes" : "No",
                    Escape(entry.PdfVersion ?? ""));
                lines.Add(csvLine);
            }

            await File.WriteAllLinesAsync(sfd.FileName, lines);

            MessageBox.Show($"Report exported successfully.\n{_allEntries.Count} files documented.\n\n{sfd.FileName}",
                "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error exporting report:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void OnFilterChanged(object? sender, EventArgs e)
    {
        var filter = tstFilter.Text.Trim();

        if (string.IsNullOrEmpty(filter))
        {
            if (_currentFolderPath != null)
                LoadFolder(_currentFolderPath);
            return;
        }

        tvFiles.Nodes.Clear();
        ClearMetadata();

        var filtered = _allEntries
            .Where(f => Path.GetFileName(f.FilePath).Contains(filter, StringComparison.OrdinalIgnoreCase))
            .OrderBy(f => Path.GetFileName(f.FilePath));

        foreach (var entry in filtered)
        {
            var node = tvFiles.Nodes.Add(Path.GetFileName(entry.FilePath));
            node.Tag = entry;
        }
    }

    private void ClearMetadata()
    {
        lblFileNameValue.Text = "\u2014";
        lblPageCountValue.Text = "\u2014";
        lblFileSizeValue.Text = "\u2014";
        lblAuthorValue.Text = "\u2014";
        lblTitleValue.Text = "\u2014";
        lblCreatedValue.Text = "\u2014";
        lblEncryptedValue.Text = "\u2014";
        lblEncryptedValue.ForeColor = Color.FromArgb(15, 23, 42);
        lblPdfVersionValue.Text = "\u2014";
        txtPreview.Text = string.Empty;
    }

    private void UpdateStatusBar()
    {
        var totalFiles = _allEntries.Count;
        var totalPages = _allEntries.Where(e => e.IsLoaded).Sum(e => e.PageCount);
        var totalSize = _allEntries.Where(e => e.IsLoaded).Sum(e => e.FileSize);

        tslTotalFiles.Text = $"Files: {totalFiles}";
        tslTotalPages.Text = $"Pages: {totalPages}";
        tslTotalSize.Text = $"Total Size: {FormatFileSize(totalSize)}";
    }

    private static string FormatFileSize(long bytes)
    {
        if (bytes < 1024) return $"{bytes} B";
        if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
        if (bytes < 1024 * 1024 * 1024) return $"{bytes / (1024.0 * 1024.0):F1} MB";
        return $"{bytes / (1024.0 * 1024.0 * 1024.0):F2} GB";
    }

    private static string Escape(string value)
    {
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            return $"\"{value.Replace("\"", "\"\"")}\"";
        return value;
    }

    private class PdfFileEntry
    {
        public string FilePath { get; set; } = string.Empty;
        public int PageCount { get; set; }
        public long FileSize { get; set; }
        public string? Author { get; set; }
        public string? Title { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool IsEncrypted { get; set; }
        public string? PdfVersion { get; set; }
        public bool IsLoaded { get; set; }
    }
}
