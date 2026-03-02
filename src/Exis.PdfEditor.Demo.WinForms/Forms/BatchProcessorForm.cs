using Exis.PdfEditor;

namespace Exis.PdfEditor.Demo.WinForms.Forms;

public partial class BatchProcessorForm : Form
{
    private CancellationTokenSource? _cts;
    private string _outputFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

    public BatchProcessorForm()
    {
        InitializeComponent();
        txtOutputFolder.Text = _outputFolder;
        WireUpEvents();
        UpdateOptionsVisibility();
    }

    private void WireUpEvents()
    {
        btnAddFiles.Click += OnAddFiles;
        btnRemoveFile.Click += OnRemoveFile;
        btnClearFiles.Click += OnClearFiles;
        btnBrowseOutput.Click += OnBrowseOutput;
        btnProcess.Click += OnProcess;
        btnCancel.Click += OnCancel;
        cmbOperation.SelectedIndexChanged += (_, _) => UpdateOptionsVisibility();
    }

    private void UpdateOptionsVisibility()
    {
        var isPdfA = cmbOperation.SelectedIndex == 2;
        lblPdfAStandard.Visible = isPdfA;
        cmbPdfAStandard.Visible = isPdfA;
    }

    private void OnAddFiles(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Select PDF files to add",
            Multiselect = true
        };

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            foreach (var file in ofd.FileNames)
            {
                if (!lstFiles.Items.Contains(file))
                    lstFiles.Items.Add(file);
            }
            Log($"Added {ofd.FileNames.Length} file(s). Total: {lstFiles.Items.Count}");
        }
    }

    private void OnRemoveFile(object? sender, EventArgs e)
    {
        var selected = lstFiles.SelectedItems.Cast<object>().ToList();
        foreach (var item in selected)
            lstFiles.Items.Remove(item);
        Log($"Removed {selected.Count} file(s). Total: {lstFiles.Items.Count}");
    }

    private void OnClearFiles(object? sender, EventArgs e)
    {
        lstFiles.Items.Clear();
        Log("Cleared all files.");
    }

    private void OnBrowseOutput(object? sender, EventArgs e)
    {
        using var fbd = new FolderBrowserDialog
        {
            Description = "Select output folder",
            SelectedPath = _outputFolder,
            UseDescriptionForTitle = true
        };

        if (fbd.ShowDialog() == DialogResult.OK)
        {
            _outputFolder = fbd.SelectedPath;
            txtOutputFolder.Text = _outputFolder;
        }
    }

    private async void OnProcess(object? sender, EventArgs e)
    {
        if (lstFiles.Items.Count == 0)
        {
            MessageBox.Show("Please add at least one PDF file to process.", "No Files",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var files = lstFiles.Items.Cast<string>().ToArray();
        var operation = cmbOperation.SelectedIndex;

        _cts = new CancellationTokenSource();
        SetProcessingState(true);
        progressBar.Value = 0;
        progressBar.Maximum = files.Length;

        Log($"Starting batch operation: {cmbOperation.Text} on {files.Length} file(s)...");

        try
        {
            switch (operation)
            {
                case 0:
                    await MergeAllAsync(files, _cts.Token);
                    break;
                case 1:
                    await OptimizeAllAsync(files, _cts.Token);
                    break;
                case 2:
                    await ConvertToPdfAAsync(files, _cts.Token);
                    break;
                case 3:
                    await ExtractTextAsync(files, _cts.Token);
                    break;
            }

            Log("Batch operation completed successfully.");

            if (chkOpenWhenDone.Checked && Directory.Exists(_outputFolder))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = _outputFolder,
                    UseShellExecute = true
                });
            }
        }
        catch (OperationCanceledException)
        {
            Log("Operation was cancelled by user.");
        }
        catch (Exception ex)
        {
            Log($"ERROR: {ex.Message}");
            MessageBox.Show($"An error occurred during processing:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetProcessingState(false);
            _cts?.Dispose();
            _cts = null;
        }
    }

    private void OnCancel(object? sender, EventArgs e)
    {
        _cts?.Cancel();
        Log("Cancellation requested...");
    }

    private async Task MergeAllAsync(string[] files, CancellationToken ct)
    {
        var outputPath = Path.Combine(_outputFolder, "merged_output.pdf");
        if (!chkOverwrite.Checked && File.Exists(outputPath))
        {
            outputPath = Path.Combine(_outputFolder,
                $"merged_output_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }

        Log("Merging all PDFs into a single document...");

        for (int i = 0; i < files.Length; i++)
        {
            ct.ThrowIfCancellationRequested();
            Log($"  Adding: {Path.GetFileName(files[i])}");
            UpdateProgress(i + 1);
        }

        Log($"Saving merged document to: {outputPath}");
        await PdfMerger.MergeToFileAsync(files, outputPath);
        Log($"Merge complete: {outputPath}");
    }

    private async Task OptimizeAllAsync(string[] files, CancellationToken ct)
    {
        Log("Optimizing all PDFs...");

        for (int i = 0; i < files.Length; i++)
        {
            ct.ThrowIfCancellationRequested();
            var file = files[i];
            var outputName = $"optimized_{Path.GetFileName(file)}";
            var outputPath = Path.Combine(_outputFolder, outputName);

            if (!chkOverwrite.Checked && File.Exists(outputPath))
            {
                outputPath = Path.Combine(_outputFolder,
                    $"optimized_{DateTime.Now:yyyyMMdd_HHmmss}_{Path.GetFileName(file)}");
            }

            Log($"  Optimizing: {Path.GetFileName(file)}");

            var options = new PdfOptimizeOptions
            {
                CompressStreams = true,
                RemoveDuplicateObjects = true,
                DownsampleImages = true
            };

            var result = await PdfOptimizer.OptimizeAsync(file, outputPath, options);

            Log($"    Saved: {outputPath} (size reduced by {result.ReductionPercent:F1}%)");

            UpdateProgress(i + 1);
        }

        Log("Optimization complete.");
    }

    private async Task ConvertToPdfAAsync(string[] files, CancellationToken ct)
    {
        var standard = cmbPdfAStandard.SelectedIndex switch
        {
            0 => PdfALevel.PdfA1b,
            1 => PdfALevel.PdfA2b,
            2 => PdfALevel.PdfA3b,
            _ => PdfALevel.PdfA2b
        };

        Log($"Converting all PDFs to {cmbPdfAStandard.Text}...");

        for (int i = 0; i < files.Length; i++)
        {
            ct.ThrowIfCancellationRequested();
            var file = files[i];
            var outputName = $"pdfa_{Path.GetFileName(file)}";
            var outputPath = Path.Combine(_outputFolder, outputName);

            if (!chkOverwrite.Checked && File.Exists(outputPath))
            {
                outputPath = Path.Combine(_outputFolder,
                    $"pdfa_{DateTime.Now:yyyyMMdd_HHmmss}_{Path.GetFileName(file)}");
            }

            Log($"  Converting: {Path.GetFileName(file)}");

            await PdfAConverter.ConvertAsync(file, outputPath, standard);

            Log($"    Saved: {outputPath}");
            UpdateProgress(i + 1);
        }

        Log("PDF/A conversion complete.");
    }

    private async Task ExtractTextAsync(string[] files, CancellationToken ct)
    {
        Log("Extracting text from all PDFs...");

        for (int i = 0; i < files.Length; i++)
        {
            ct.ThrowIfCancellationRequested();
            var file = files[i];
            var outputName = $"{Path.GetFileNameWithoutExtension(file)}.txt";
            var outputPath = Path.Combine(_outputFolder, outputName);

            if (!chkOverwrite.Checked && File.Exists(outputPath))
            {
                outputPath = Path.Combine(_outputFolder,
                    $"{Path.GetFileNameWithoutExtension(file)}_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            }

            Log($"  Extracting: {Path.GetFileName(file)}");

            var textResult = PdfTextExtractor.ExtractText(file);
            var text = textResult.FullText;
            await File.WriteAllTextAsync(outputPath, text, ct);

            Log($"    Saved: {outputPath} ({text.Length} characters)");
            UpdateProgress(i + 1);
        }

        Log("Text extraction complete.");
    }

    private void SetProcessingState(bool processing)
    {
        btnProcess.Enabled = !processing;
        btnCancel.Enabled = processing;
        btnAddFiles.Enabled = !processing;
        btnRemoveFile.Enabled = !processing;
        btnClearFiles.Enabled = !processing;
        cmbOperation.Enabled = !processing;
    }

    private void UpdateProgress(int value)
    {
        if (InvokeRequired)
        {
            Invoke(() => UpdateProgress(value));
            return;
        }
        progressBar.Value = Math.Min(value, progressBar.Maximum);
    }

    private void Log(string message)
    {
        if (InvokeRequired)
        {
            Invoke(() => Log(message));
            return;
        }
        txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
    }
}
