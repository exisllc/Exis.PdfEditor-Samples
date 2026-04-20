namespace Exis.PdfEditor.Demo.Wpf.ViewModels;

using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Exis.PdfEditor.Demo.Wpf.Helpers;
using Exis.PdfEditor;
using Microsoft.Win32;

public class BatesNumberingViewModel : INotifyPropertyChanged
{
    private string _inputFilePath = string.Empty;
    private string _prefix = "ABC";
    private string _suffix = string.Empty;
    private int _startNumber = 1;
    private int _digits = 6;
    private int _selectedPositionIndex = 5; // BottomRight
    private double _fontSize = 10;
    private string _selectedColor = "Black";
    private string _confidentialityLabel = string.Empty;
    private bool _skipFirstPage;
    private bool _counterAdvancesOnSkippedPages = true;
    private string _resultMessage = string.Empty;
    private bool _isProcessing;

    public BatesNumberingViewModel()
    {
        BrowseCommand = new RelayCommand(Browse);
        ApplyCommand = new RelayCommand(ApplyAsync,
            () => !IsProcessing && !string.IsNullOrWhiteSpace(InputFilePath));
    }

    public string InputFilePath
    {
        get => _inputFilePath;
        set { _inputFilePath = value; OnPropertyChanged(); }
    }

    public string Prefix
    {
        get => _prefix;
        set { _prefix = value; OnPropertyChanged(); }
    }

    public string Suffix
    {
        get => _suffix;
        set { _suffix = value; OnPropertyChanged(); }
    }

    public int StartNumber
    {
        get => _startNumber;
        set { _startNumber = value; OnPropertyChanged(); }
    }

    public int Digits
    {
        get => _digits;
        set { _digits = value; OnPropertyChanged(); }
    }

    public string[] PositionOptions { get; } =
        { "TopLeft", "TopCenter", "TopRight", "BottomLeft", "BottomCenter", "BottomRight" };

    public int SelectedPositionIndex
    {
        get => _selectedPositionIndex;
        set { _selectedPositionIndex = value; OnPropertyChanged(); }
    }

    public double FontSize
    {
        get => _fontSize;
        set { _fontSize = value; OnPropertyChanged(); }
    }

    public string[] ColorOptions { get; } = { "Black", "Red", "Blue", "Green", "Orange" };

    public string SelectedColor
    {
        get => _selectedColor;
        set { _selectedColor = value; OnPropertyChanged(); }
    }

    public string ConfidentialityLabel
    {
        get => _confidentialityLabel;
        set { _confidentialityLabel = value; OnPropertyChanged(); }
    }

    public bool SkipFirstPage
    {
        get => _skipFirstPage;
        set { _skipFirstPage = value; OnPropertyChanged(); }
    }

    public bool CounterAdvancesOnSkippedPages
    {
        get => _counterAdvancesOnSkippedPages;
        set { _counterAdvancesOnSkippedPages = value; OnPropertyChanged(); }
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
    public ICommand ApplyCommand { get; }

    private void Browse()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
            Title = "Select a PDF file for Bates numbering"
        };

        if (dialog.ShowDialog() == true)
        {
            InputFilePath = dialog.FileName;
            ResultMessage = string.Empty;
        }
    }

    private async Task ApplyAsync()
    {
        if (string.IsNullOrWhiteSpace(InputFilePath))
            return;

        var dialog = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Save Bates-stamped PDF as",
            FileName = Path.GetFileNameWithoutExtension(InputFilePath) + "_bates.pdf"
        };

        if (dialog.ShowDialog() != true)
            return;

        IsProcessing = true;
        ResultMessage = "Applying Bates stamp...";

        try
        {
            var inputPath = InputFilePath;
            var outputPath = dialog.FileName;
            var options = new BatesStampOptions
            {
                Prefix = Prefix ?? string.Empty,
                Suffix = Suffix ?? string.Empty,
                StartNumber = StartNumber,
                Digits = Digits,
                Position = (BatesPosition)SelectedPositionIndex,
                FontSize = (float)FontSize,
                TextColor = ResolveColor(SelectedColor),
                ConfidentialityLabel = string.IsNullOrWhiteSpace(ConfidentialityLabel)
                    ? null
                    : ConfidentialityLabel,
                SkipFirstPage = SkipFirstPage,
                CounterAdvancesOnSkippedPages = CounterAdvancesOnSkippedPages
            };

            var result = await Task.Run(() =>
                PdfBatesStamp.ApplyBatesStamp(inputPath, outputPath, options));

            ResultMessage =
                $"Bates stamp applied successfully.\n" +
                $"Pages stamped: {result.PagesStamped}\n" +
                $"First: {result.FirstNumber}\n" +
                $"Last: {result.LastNumber}\n" +
                $"Saved to: {outputPath}";
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

    private static PdfColor? ResolveColor(string colorName)
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

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
