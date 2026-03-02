using Exis.PdfEditor;

namespace Exis.PdfEditor.Demo.WinForms.Forms;

public partial class FormFillerForm : Form
{
    private string? _loadedFilePath;
    private List<FormFieldInfo> _originalFields = new();

    public FormFillerForm()
    {
        InitializeComponent();
        WireUpEvents();
    }

    private void WireUpEvents()
    {
        btnBrowse.Click += OnBrowse;
        btnLoadFields.Click += OnLoadFields;
        btnFillAndSave.Click += OnFillAndSave;
        btnFlattenAndSave.Click += OnFlattenAndSave;
        btnResetValues.Click += OnResetValues;
    }

    private void OnBrowse(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
            Title = "Select a PDF form"
        };

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            txtFilePath.Text = ofd.FileName;
            _loadedFilePath = ofd.FileName;
        }
    }

    private async void OnLoadFields(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtFilePath.Text))
        {
            MessageBox.Show("Please select a PDF file first.", "No File Selected",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!File.Exists(txtFilePath.Text))
        {
            MessageBox.Show("The selected file does not exist.", "File Not Found",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        _loadedFilePath = txtFilePath.Text;
        SetLoadingState(true);

        try
        {
            var fields = await PdfFormFiller.GetFieldsAsync(_loadedFilePath);

            _originalFields.Clear();
            dgvFields.Rows.Clear();

            foreach (var field in fields)
            {
                var info = new FormFieldInfo
                {
                    Name = field.Name,
                    FieldType = field.Type.ToString(),
                    Value = field.Value ?? string.Empty
                };
                _originalFields.Add(info);
                dgvFields.Rows.Add(info.Name, info.FieldType, info.Value);
            }

            var fieldCount = _originalFields.Count;
            SetStatus($"Loaded {fieldCount} form field(s) from {Path.GetFileName(_loadedFilePath)}.");
            SetActionButtonsEnabled(fieldCount > 0);

            if (fieldCount == 0)
            {
                MessageBox.Show("This PDF does not contain any interactive form fields.",
                    "No Form Fields", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            SetStatus("Failed to load form fields.");
            MessageBox.Show($"Error loading form fields:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private async void OnFillAndSave(object? sender, EventArgs e)
    {
        if (_loadedFilePath == null)
            return;

        using var sfd = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Save filled PDF as",
            FileName = $"filled_{Path.GetFileName(_loadedFilePath)}"
        };

        if (sfd.ShowDialog() != DialogResult.OK)
            return;

        SetLoadingState(true);

        try
        {
            var fieldValues = BuildFieldValuesDictionary();
            await PdfFormFiller.FillAsync(_loadedFilePath, sfd.FileName, fieldValues);

            SetStatus($"Saved filled PDF to {Path.GetFileName(sfd.FileName)}.");
            MessageBox.Show($"Filled PDF saved successfully.\n\n{sfd.FileName}",
                "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            SetStatus("Failed to save filled PDF.");
            MessageBox.Show($"Error saving filled PDF:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private async void OnFlattenAndSave(object? sender, EventArgs e)
    {
        if (_loadedFilePath == null)
            return;

        using var sfd = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Save flattened PDF as",
            FileName = $"flattened_{Path.GetFileName(_loadedFilePath)}"
        };

        if (sfd.ShowDialog() != DialogResult.OK)
            return;

        SetLoadingState(true);

        try
        {
            // First fill the fields with current values, then flatten
            var fieldValues = BuildFieldValuesDictionary();
            await PdfFormFiller.FillAsync(_loadedFilePath, sfd.FileName, fieldValues);
            await PdfFormFiller.FlattenAsync(sfd.FileName, sfd.FileName);

            SetStatus($"Saved flattened PDF to {Path.GetFileName(sfd.FileName)}.");
            MessageBox.Show($"Flattened PDF saved successfully.\nForm fields are now static content.\n\n{sfd.FileName}",
                "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            SetStatus("Failed to save flattened PDF.");
            MessageBox.Show($"Error saving flattened PDF:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private void OnResetValues(object? sender, EventArgs e)
    {
        for (int i = 0; i < dgvFields.Rows.Count && i < _originalFields.Count; i++)
        {
            dgvFields.Rows[i].Cells["FieldValue"].Value = _originalFields[i].Value;
        }
        SetStatus("Field values reset to original.");
    }

    private Dictionary<string, string> BuildFieldValuesDictionary()
    {
        var dict = new Dictionary<string, string>();
        for (int i = 0; i < dgvFields.Rows.Count && i < _originalFields.Count; i++)
        {
            var name = _originalFields[i].Name;
            var value = dgvFields.Rows[i].Cells["FieldValue"].Value?.ToString() ?? string.Empty;
            dict[name] = value;
        }
        return dict;
    }

    private void SetLoadingState(bool loading)
    {
        btnBrowse.Enabled = !loading;
        btnLoadFields.Enabled = !loading;
        btnFillAndSave.Enabled = !loading && _originalFields.Count > 0;
        btnFlattenAndSave.Enabled = !loading && _originalFields.Count > 0;
        btnResetValues.Enabled = !loading && _originalFields.Count > 0;
        dgvFields.ReadOnly = loading;
        Cursor = loading ? Cursors.WaitCursor : Cursors.Default;
    }

    private void SetActionButtonsEnabled(bool enabled)
    {
        btnFillAndSave.Enabled = enabled;
        btnFlattenAndSave.Enabled = enabled;
        btnResetValues.Enabled = enabled;
    }

    private void SetStatus(string text)
    {
        lblStatus.Text = text;
    }

    private class FormFieldInfo
    {
        public string Name { get; set; } = string.Empty;
        public string FieldType { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
