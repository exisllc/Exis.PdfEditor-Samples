namespace Exis.PdfEditor.Demo.Wpf.ViewModels;

using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Exis.PdfEditor.Demo.Wpf.Helpers;
using Exis.PdfEditor;
using Microsoft.Win32;

public class RedactionViewModel : INotifyPropertyChanged
{
    private string _inputFilePath = string.Empty;
    private string _redactionText = string.Empty;
    private bool _useRegex;
    private bool _isAreaRedaction;
    private double _areaX;
    private double _areaY;
    private double _areaWidth = 200;
    private double _areaHeight = 50;
    private string _resultMessage = string.Empty;
    private bool _isProcessing;

    public RedactionViewModel()
    {
        BrowseCommand = new RelayCommand(Browse);
        RedactCommand = new RelayCommand(RedactAsync, () => !IsProcessing && !string.IsNullOrWhiteSpace(InputFilePath));
    }

    public string InputFilePath
    {
        get => _inputFilePath;
        set { _inputFilePath = value; OnPropertyChanged(); }
    }

    public string RedactionText
    {
        get => _redactionText;
        set { _redactionText = value; OnPropertyChanged(); }
    }

    public bool UseRegex
    {
        get => _useRegex;
        set { _useRegex = value; OnPropertyChanged(); }
    }

    public bool IsAreaRedaction
    {
        get => _isAreaRedaction;
        set { _isAreaRedaction = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsTextRedaction)); }
    }

    public bool IsTextRedaction => !IsAreaRedaction;

    public double AreaX
    {
        get => _areaX;
        set { _areaX = value; OnPropertyChanged(); }
    }

    public double AreaY
    {
        get => _areaY;
        set { _areaY = value; OnPropertyChanged(); }
    }

    public double AreaWidth
    {
        get => _areaWidth;
        set { _areaWidth = value; OnPropertyChanged(); }
    }

    public double AreaHeight
    {
        get => _areaHeight;
        set { _areaHeight = value; OnPropertyChanged(); }
    }

    public string ResultMessage
    {
        get => _resultMessage;
        set { _resultMessage = value; OnPropertyChanged(); }
    }

    public bool IsProcessing
    {
        get => _isProcessing;
        set { _isProcessing = value; OnPropertyChanged(); }
    }

    public ICommand BrowseCommand { get; }
    public ICommand RedactCommand { get; }

    private void Browse()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
            Title = "Select a PDF file for redaction"
        };

        if (dialog.ShowDialog() == true)
        {
            InputFilePath = dialog.FileName;
            ResultMessage = string.Empty;
        }
    }

    private async Task RedactAsync()
    {
        if (string.IsNullOrWhiteSpace(InputFilePath))
            return;

        if (!IsAreaRedaction && string.IsNullOrWhiteSpace(RedactionText))
        {
            ResultMessage = "Please enter text to redact.";
            return;
        }

        var dialog = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Save redacted PDF as",
            FileName = Path.GetFileNameWithoutExtension(InputFilePath) + "_redacted.pdf"
        };

        if (dialog.ShowDialog() != true)
            return;

        IsProcessing = true;
        ResultMessage = "Applying redaction...";

        try
        {
            var inputPath = InputFilePath;
            var outputPath = dialog.FileName;
            var isArea = IsAreaRedaction;
            var text = RedactionText;
            var regex = UseRegex;
            var x = AreaX;
            var y = AreaY;
            var w = AreaWidth;
            var h = AreaHeight;

            await Task.Run(async () =>
            {
                var redactions = new List<PdfRedaction>();

                if (isArea)
                {
                    redactions.Add(new PdfRedaction
                    {
                        Area = new PdfRect(x, y, w, h),
                        PageNumber = 1
                    });
                }
                else
                {
                    redactions.Add(new PdfRedaction
                    {
                        Text = text,
                        IsRegex = regex,
                        CaseSensitive = false,
                        ReplaceWith = "[REDACTED]"
                    });
                }

                await PdfRedactor.RedactAsync(inputPath, outputPath, redactions);
            });

            var mode = isArea ? "Area" : "Text";
            ResultMessage = $"{mode} redaction applied successfully.\nSaved to: {dialog.FileName}";
        }
        catch (Exception ex)
        {
            ResultMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsProcessing = false;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
