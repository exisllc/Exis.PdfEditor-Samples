using Exis.PdfEditor;
using Exis.PdfEditor.Demo.WinForms.Forms;

namespace Exis.PdfEditor.Demo.WinForms;

public partial class MainForm : Form
{
    private BatchProcessorForm? _embeddedBatchProcessor;
    private FormFillerForm? _embeddedFormFiller;
    private DocumentDashboardForm? _embeddedDashboard;

    public MainForm()
    {
        InitializeComponent();
        WireUpEvents();
        EmbedSubForms();
        SetStatus("Ready — drag and drop PDF files or choose a tool to get started.");
    }

    private void WireUpEvents()
    {
        openMenuItem.Click += OnOpenPdf;
        exitMenuItem.Click += (_, _) => Close();
        batchProcessMenuItem.Click += (_, _) => mainTabControl.SelectedTab = batchProcessorTab;
        formFillMenuItem.Click += (_, _) => mainTabControl.SelectedTab = formFillerTab;
        dashboardMenuItem.Click += (_, _) => mainTabControl.SelectedTab = dashboardTab;
        aboutMenuItem.Click += OnAbout;

        btnOpenBatchProcessor.Click += (_, _) => mainTabControl.SelectedTab = batchProcessorTab;
        btnOpenFormFiller.Click += (_, _) => mainTabControl.SelectedTab = formFillerTab;
        btnOpenDashboard.Click += (_, _) => mainTabControl.SelectedTab = dashboardTab;
        btnMergePdfs.Click += OnMergePdfs;

        dragDropPanel.FilesDropped += OnFilesDropped;
    }

    private void EmbedSubForms()
    {
        _embeddedBatchProcessor = new BatchProcessorForm
        {
            TopLevel = false,
            FormBorderStyle = FormBorderStyle.None,
            Dock = DockStyle.Fill
        };
        batchProcessorTab.Controls.Add(_embeddedBatchProcessor);
        _embeddedBatchProcessor.Show();

        _embeddedFormFiller = new FormFillerForm
        {
            TopLevel = false,
            FormBorderStyle = FormBorderStyle.None,
            Dock = DockStyle.Fill
        };
        formFillerTab.Controls.Add(_embeddedFormFiller);
        _embeddedFormFiller.Show();

        _embeddedDashboard = new DocumentDashboardForm
        {
            TopLevel = false,
            FormBorderStyle = FormBorderStyle.None,
            Dock = DockStyle.Fill
        };
        dashboardTab.Controls.Add(_embeddedDashboard);
        _embeddedDashboard.Show();
    }

    private void OnOpenPdf(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
            Title = "Open PDF File"
        };

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            SetStatus($"Opened: {Path.GetFileName(ofd.FileName)}");
            _embeddedDashboard?.LoadSingleFile(ofd.FileName);
            mainTabControl.SelectedTab = dashboardTab;
        }
    }

    private async void OnMergePdfs(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Select PDF files to merge",
            Multiselect = true
        };

        if (ofd.ShowDialog() != DialogResult.OK || ofd.FileNames.Length < 2)
        {
            if (ofd.FileNames.Length == 1)
                MessageBox.Show("Please select at least two PDF files to merge.", "Merge PDFs",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var sfd = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Save merged PDF as",
            FileName = "merged.pdf"
        };

        if (sfd.ShowDialog() != DialogResult.OK)
            return;

        try
        {
            SetStatus("Merging PDFs...");
            ShowProgress(true);

            await Task.Run(async () =>
            {
                await PdfMerger.MergeToFileAsync(ofd.FileNames, sfd.FileName);
            });

            SetStatus($"Merged {ofd.FileNames.Length} files into {Path.GetFileName(sfd.FileName)}");
            MessageBox.Show($"Successfully merged {ofd.FileNames.Length} PDF files.",
                "Merge Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            SetStatus("Merge failed.");
            MessageBox.Show($"Error merging PDFs: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            ShowProgress(false);
        }
    }

    private async void OnFilesDropped(object? sender, string[] files)
    {
        if (files.Length == 0)
            return;

        SetStatus($"Processing {files.Length} dropped file(s)...");
        ShowProgress(true);

        try
        {
            var results = new List<string>();
            foreach (var file in files)
            {
                var info = await PdfInspector.InspectAsync(file);
                results.Add($"  {Path.GetFileName(file)}: {info.PageCount} page(s), " +
                            $"Author: {info.Author ?? "N/A"}, Title: {info.Title ?? "N/A"}");
            }

            var summary = $"Processed {files.Length} PDF file(s):\n\n" + string.Join("\n", results);
            SetStatus($"Processed {files.Length} file(s) successfully.");
            MessageBox.Show(summary, "Drop Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            SetStatus("Processing failed.");
            MessageBox.Show($"Error processing dropped files: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            ShowProgress(false);
        }
    }

    private void OnAbout(object? sender, EventArgs e)
    {
        MessageBox.Show(
            "Exis.PdfEditor — WinForms Demo\n\n" +
            "This application demonstrates the capabilities of the Exis.PdfEditor NuGet library " +
            "for .NET PDF manipulation.\n\n" +
            "Features demonstrated:\n" +
            "  - Batch processing (merge, optimize, convert)\n" +
            "  - PDF form filling and flattening\n" +
            "  - Document metadata inspection\n" +
            "  - Drag-and-drop PDF handling\n\n" +
            "Visit https://exis.dev for more information.",
            "About Exis.PdfEditor Demo",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    public void SetStatus(string text)
    {
        if (InvokeRequired)
        {
            Invoke(() => SetStatus(text));
            return;
        }
        statusLabel.Text = text;
    }

    public void ShowProgress(bool visible, int value = 0)
    {
        if (InvokeRequired)
        {
            Invoke(() => ShowProgress(visible, value));
            return;
        }
        statusProgressBar.Visible = visible;
        if (visible)
            statusProgressBar.Value = Math.Clamp(value, 0, 100);
    }
}
