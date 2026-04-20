using Exis.PdfEditor;

namespace Exis.PdfEditor.Demo.WinForms.Forms;

public partial class BatesNumberingForm : Form
{
    public BatesNumberingForm()
    {
        InitializeComponent();
        WireUpEvents();
    }

    private void WireUpEvents()
    {
        btnBrowse.Click += OnBrowse;
        btnApply.Click += OnApply;
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
        }
    }

    private async void OnApply(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtFilePath.Text))
        {
            MessageBox.Show("Please select a PDF file first.", "No File Selected",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var sfd = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Save Bates-stamped PDF as",
            FileName = $"bates_{Path.GetFileName(txtFilePath.Text)}"
        };

        if (sfd.ShowDialog() != DialogResult.OK)
            return;

        SetProcessing(true);

        try
        {
            var options = new BatesStampOptions
            {
                Prefix = txtPrefix.Text ?? string.Empty,
                Suffix = txtSuffix.Text ?? string.Empty,
                StartNumber = (int)nudStartNumber.Value,
                Digits = (int)nudDigits.Value,
                Position = (BatesPosition)cboPosition.SelectedIndex,
                FontSize = (float)nudFontSize.Value,
                TextColor = ResolveColor(cboColor.SelectedItem?.ToString()),
                ConfidentialityLabel = string.IsNullOrWhiteSpace(txtConfidentiality.Text)
                    ? null
                    : txtConfidentiality.Text,
                SkipFirstPage = chkSkipFirstPage.Checked,
                CounterAdvancesOnSkippedPages = chkCounterAdvancesOnSkipped.Checked
            };

            var inputPath = txtFilePath.Text;
            var outputPath = sfd.FileName;

            var result = await Task.Run(() =>
                PdfBatesStamp.ApplyBatesStamp(inputPath, outputPath, options));

            SetStatus($"Stamped {result.PagesStamped} page(s): {result.FirstNumber} → {result.LastNumber}");
            MessageBox.Show(
                $"Bates stamp applied successfully!\n\n" +
                $"Pages stamped: {result.PagesStamped}\n" +
                $"First: {result.FirstNumber}\n" +
                $"Last: {result.LastNumber}\n\n" +
                $"{sfd.FileName}",
                "Bates Stamp Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            SetStatus("Failed to apply Bates stamp.");
            MessageBox.Show($"Error applying Bates stamp:\n{ex.Message}", "Error",
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
            "Orange" => PdfColor.Orange,
            _ => PdfColor.Black
        };
    }

    private void SetProcessing(bool processing)
    {
        btnBrowse.Enabled = !processing;
        btnApply.Enabled = !processing;
        Cursor = processing ? Cursors.WaitCursor : Cursors.Default;
    }

    private void SetStatus(string text)
    {
        lblStatus.Text = text;
    }
}
