using Exis.PdfEditor;

namespace Exis.PdfEditor.Demo.WinForms.Forms;

public partial class WatermarkForm : Form
{
    private string? _loadedFilePath;

    public WatermarkForm()
    {
        InitializeComponent();
        WireUpEvents();
    }

    private void WireUpEvents()
    {
        btnBrowse.Click += OnBrowse;
        btnAddWatermark.Click += OnAddWatermark;
        trkOpacity.ValueChanged += (_, _) =>
            lblOpacityValue.Text = $"{trkOpacity.Value}%";
    }

    private void OnBrowse(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
            Title = "Select a PDF file"
        };

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            txtFilePath.Text = ofd.FileName;
            _loadedFilePath = ofd.FileName;
        }
    }

    private async void OnAddWatermark(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtFilePath.Text))
        {
            MessageBox.Show("Please select a PDF file first.", "No File Selected",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(txtWatermarkText.Text))
        {
            MessageBox.Show("Please enter watermark text.", "No Text",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var sfd = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Save watermarked PDF as",
            FileName = $"watermarked_{Path.GetFileName(txtFilePath.Text)}"
        };

        if (sfd.ShowDialog() != DialogResult.OK)
            return;

        SetProcessing(true);

        try
        {
            var options = new PdfWatermarkOptions
            {
                Position = (WatermarkPosition)cboPosition.SelectedIndex,
                FontSize = (double)nudFontSize.Value,
                Opacity = trkOpacity.Value / 100.0,
                TextColor = ResolveColor(cboColor.SelectedItem?.ToString()),
                PageRange = ParsePageRange(txtPageRange.Text)
            };

            var inputPath = txtFilePath.Text;
            var outputPath = sfd.FileName;
            var text = txtWatermarkText.Text;

            var result = await Task.Run(() =>
                PdfWatermark.AddText(inputPath, outputPath, text, options));

            SetStatus($"Watermarked {result.PagesWatermarked} of {result.TotalPages} page(s).");
            MessageBox.Show(
                $"Watermark applied successfully!\n\n" +
                $"Pages watermarked: {result.PagesWatermarked} of {result.TotalPages}\n\n" +
                $"{sfd.FileName}",
                "Watermark Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            SetStatus("Failed to add watermark.");
            MessageBox.Show($"Error adding watermark:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetProcessing(false);
        }
    }

    private static PdfColor? ResolveColor(string? colorName)
    {
        return colorName switch
        {
            "Red" => PdfColor.Red,
            "Blue" => PdfColor.Blue,
            "Green" => PdfColor.Green,
            "Black" => PdfColor.Black,
            "Orange" => PdfColor.Orange,
            _ => null // Light Gray = default
        };
    }

    private static int[]? ParsePageRange(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        var pages = new List<int>();
        foreach (var part in input.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            if (int.TryParse(part.Trim(), out int page) && page > 0)
                pages.Add(page);
        }
        return pages.Count > 0 ? pages.ToArray() : null;
    }

    private void SetProcessing(bool processing)
    {
        btnBrowse.Enabled = !processing;
        btnAddWatermark.Enabled = !processing;
        Cursor = processing ? Cursors.WaitCursor : Cursors.Default;
    }

    private void SetStatus(string text)
    {
        lblStatus.Text = text;
    }
}
