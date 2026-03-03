using Exis.PdfEditor;

namespace Exis.PdfEditor.Demo.WinForms.Forms;

public partial class ImageEditorForm : Form
{
    private string? _loadedFilePath;

    public ImageEditorForm()
    {
        InitializeComponent();
        WireUpEvents();
    }

    private void WireUpEvents()
    {
        btnBrowse.Click += OnBrowse;
        btnBrowseImage.Click += OnBrowseImage;
        btnFindImages.Click += OnFindImages;
        btnReplaceAll.Click += OnReplaceAll;
        btnReplaceSelected.Click += OnReplaceSelected;
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

    private void OnBrowseImage(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All Files (*.*)|*.*",
            Title = "Select a replacement image"
        };

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            txtImagePath.Text = ofd.FileName;
        }
    }

    private async void OnFindImages(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtFilePath.Text))
        {
            MessageBox.Show("Please select a PDF file first.", "No File Selected",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _loadedFilePath = txtFilePath.Text;
        SetProcessing(true);

        try
        {
            var result = await PdfImageEditor.FindImagesAsync(_loadedFilePath);

            dgvImages.Rows.Clear();
            foreach (var img in result.Images)
            {
                dgvImages.Rows.Add(
                    img.Index,
                    $"{img.PixelWidth}x{img.PixelHeight}",
                    img.ColorSpace,
                    img.Format,
                    string.Join(", ", img.PageNumbers));
            }

            SetStatus($"Found {result.TotalImages} image(s) across {result.PagesSearched} page(s).");
            SetActionButtonsEnabled(result.TotalImages > 0);
        }
        catch (Exception ex)
        {
            SetStatus("Failed to find images.");
            MessageBox.Show($"Error finding images:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetProcessing(false);
        }
    }

    private async void OnReplaceAll(object? sender, EventArgs e)
    {
        if (_loadedFilePath == null || string.IsNullOrWhiteSpace(txtImagePath.Text))
        {
            MessageBox.Show("Please select both a PDF file and a replacement image.", "Missing Input",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var sfd = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Save PDF with replaced images",
            FileName = $"images_replaced_{Path.GetFileName(_loadedFilePath)}"
        };

        if (sfd.ShowDialog() != DialogResult.OK)
            return;

        SetProcessing(true);

        try
        {
            byte[] replacementImage = await File.ReadAllBytesAsync(txtImagePath.Text);
            var result = await PdfImageEditor.ReplaceAllAsync(_loadedFilePath, sfd.FileName, replacementImage);

            SetStatus($"Replaced {result.ImagesReplaced} of {result.ImagesFound} image(s).");
            MessageBox.Show($"Replaced {result.ImagesReplaced} image(s) successfully.\n\n{sfd.FileName}",
                "Replace Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            SetStatus("Failed to replace images.");
            MessageBox.Show($"Error replacing images:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetProcessing(false);
        }
    }

    private async void OnReplaceSelected(object? sender, EventArgs e)
    {
        if (_loadedFilePath == null || string.IsNullOrWhiteSpace(txtImagePath.Text))
        {
            MessageBox.Show("Please select both a PDF file and a replacement image.", "Missing Input",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Get selected indices from the DataGridView
        var selectedIndices = new List<int>();
        foreach (DataGridViewRow row in dgvImages.SelectedRows)
        {
            if (row.Cells["colIndex"].Value is int idx)
                selectedIndices.Add(idx);
        }

        if (selectedIndices.Count == 0)
        {
            MessageBox.Show("Please select one or more images from the list.", "No Selection",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var sfd = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Save PDF with replaced images",
            FileName = $"images_replaced_{Path.GetFileName(_loadedFilePath)}"
        };

        if (sfd.ShowDialog() != DialogResult.OK)
            return;

        SetProcessing(true);

        try
        {
            byte[] replacementImage = await File.ReadAllBytesAsync(txtImagePath.Text);
            var options = new PdfImageReplaceOptions { ImageIndices = selectedIndices.ToArray() };
            var result = await PdfImageEditor.ReplaceAsync(_loadedFilePath, sfd.FileName, replacementImage, options);

            SetStatus($"Replaced {result.ImagesReplaced} of {result.ImagesFound} image(s).");
            MessageBox.Show($"Replaced {result.ImagesReplaced} selected image(s) successfully.\n\n{sfd.FileName}",
                "Replace Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            SetStatus("Failed to replace images.");
            MessageBox.Show($"Error replacing images:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetProcessing(false);
        }
    }

    private void SetProcessing(bool processing)
    {
        btnBrowse.Enabled = !processing;
        btnBrowseImage.Enabled = !processing;
        btnFindImages.Enabled = !processing;
        btnReplaceAll.Enabled = !processing;
        btnReplaceSelected.Enabled = !processing;
        Cursor = processing ? Cursors.WaitCursor : Cursors.Default;
    }

    private void SetActionButtonsEnabled(bool enabled)
    {
        btnReplaceAll.Enabled = enabled;
        btnReplaceSelected.Enabled = enabled;
    }

    private void SetStatus(string text)
    {
        lblStatus.Text = text;
    }
}
