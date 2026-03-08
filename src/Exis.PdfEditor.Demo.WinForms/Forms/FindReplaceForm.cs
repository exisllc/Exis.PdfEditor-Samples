using Exis.PdfEditor;

namespace Exis.PdfEditor.Demo.WinForms.Forms;

public partial class FindReplaceForm : Form
{
    private string _inputFilePath = string.Empty;

    public FindReplaceForm()
    {
        InitializeComponent();
        WireUpEvents();
    }

    public void LoadFile(string filePath)
    {
        _inputFilePath = filePath;
        txtInputFile.Text = filePath;
        txtResult.Clear();
    }

    private void WireUpEvents()
    {
        btnBrowseInput.Click += OnBrowseInput;
        btnExecute.Click += OnExecute;
        btnSaveAs.Click += OnSaveAs;
    }

    private void OnBrowseInput(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
            Title = "Select a PDF file"
        };

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            LoadFile(ofd.FileName);
        }
    }

    private async void OnExecute(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_inputFilePath) || string.IsNullOrWhiteSpace(txtSearch.Text))
        {
            MessageBox.Show("Please select an input PDF and enter search text.", "Missing Input",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var outputPath = GenerateOutputPath(_inputFilePath, "_replaced");
        await RunFindReplaceAsync(outputPath);
    }

    private async void OnSaveAs(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_inputFilePath))
        {
            MessageBox.Show("Please select an input PDF first.", "Missing Input",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var sfd = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Save modified PDF as",
            FileName = Path.GetFileNameWithoutExtension(_inputFilePath) + "_replaced.pdf"
        };

        if (sfd.ShowDialog() == DialogResult.OK)
        {
            await RunFindReplaceAsync(sfd.FileName);
        }
    }

    private async Task RunFindReplaceAsync(string outputPath)
    {
        SetProcessing(true);
        txtResult.Text = "Processing find & replace...";

        try
        {
            var searchText = txtSearch.Text;
            var replaceText = txtReplace.Text;
            var useRegex = chkRegex.Checked;
            var useAdaptive = chkAdaptiveFitting.Checked;
            var textColor = ResolvePdfColor(cmbTextColor.SelectedItem?.ToString());
            var highlightColor = ResolvePdfColor(cmbHighlightColor.SelectedItem?.ToString());

            var result = await Task.Run(async () =>
            {
                var options = new PdfFindReplaceOptions
                {
                    UseRegex = useRegex,
                    TextFitting = useAdaptive ? TextFittingMode.Adaptive : TextFittingMode.None,
                    ReplacementTextColor = textColor,
                    ReplacementHighlightColor = highlightColor
                };

                return await PdfFindReplace.ExecuteAsync(_inputFilePath, outputPath, searchText, replaceText, options);
            });

            txtResult.Text = $"Completed: {result.TotalReplacements} replacement(s) across {result.PagesModified} page(s).\r\nSaved to: {outputPath}";
        }
        catch (Exception ex)
        {
            txtResult.Text = $"Error: {ex.Message}";
        }
        finally
        {
            SetProcessing(false);
        }
    }

    private void SetProcessing(bool processing)
    {
        btnExecute.Enabled = !processing;
        btnSaveAs.Enabled = !processing;
        btnBrowseInput.Enabled = !processing;
        progressBar.Visible = processing;
    }

    private static PdfColor? ResolvePdfColor(string? colorName) => colorName switch
    {
        "Red" => PdfColor.Red,
        "Blue" => PdfColor.Blue,
        "Green" => PdfColor.Green,
        "Orange" => PdfColor.Orange,
        "Magenta" => PdfColor.Magenta,
        "Cyan" => PdfColor.Cyan,
        "Black" => PdfColor.Black,
        "Yellow" => PdfColor.Yellow,
        _ => null
    };

    private static string GenerateOutputPath(string inputPath, string suffix)
    {
        var directory = Path.GetDirectoryName(inputPath) ?? string.Empty;
        var name = Path.GetFileNameWithoutExtension(inputPath);
        var extension = Path.GetExtension(inputPath);
        return Path.Combine(directory, $"{name}{suffix}{extension}");
    }
}
