namespace Exis.PdfEditor.Demo.Wpf.ViewModels;

using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Exis.PdfEditor.Demo.Wpf.Helpers;
using Exis.PdfEditor;
using Microsoft.Win32;

public class WatermarkViewModel : INotifyPropertyChanged
{
    private string _inputFilePath = string.Empty;
    private string _watermarkText = "CONFIDENTIAL";
    private int _selectedPositionIndex = 3; // Across
    private double _fontSize = 48;
    private double _opacity = 0.3;
    private string _selectedColor = "Light Gray";
    private string _pageRange = string.Empty;
    private string _resultMessage = string.Empty;
    private bool _isProcessing;

    public WatermarkViewModel()
    {
        BrowseCommand = new RelayCommand(Browse);
        AddWatermarkCommand = new RelayCommand(AddWatermarkAsync, () => !IsProcessing && !string.IsNullOrWhiteSpace(InputFilePath));
    }

    public string InputFilePath
    {
        get => _inputFilePath;
        set { _inputFilePath = value; OnPropertyChanged(); }
    }

    public string WatermarkText
    {
        get => _watermarkText;
        set { _watermarkText = value; OnPropertyChanged(); }
    }

    public string[] PositionOptions { get; } = { "Top", "Bottom", "Center", "Across" };

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

    public double Opacity
    {
        get => _opacity;
        set { _opacity = value; OnPropertyChanged(); OnPropertyChanged(nameof(OpacityPercent)); }
    }

    public string OpacityPercent => $"{Opacity * 100:F0}%";

    public string[] ColorOptions { get; } = { "Light Gray", "Red", "Blue", "Green", "Black", "Orange" };

    public string SelectedColor
    {
        get => _selectedColor;
        set { _selectedColor = value; OnPropertyChanged(); }
    }

    public string PageRange
    {
        get => _pageRange;
        set { _pageRange = value; OnPropertyChanged(); }
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
    public ICommand AddWatermarkCommand { get; }

    private void Browse()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
            Title = "Select a PDF file for watermarking"
        };

        if (dialog.ShowDialog() == true)
        {
            InputFilePath = dialog.FileName;
            ResultMessage = string.Empty;
        }
    }

    private async Task AddWatermarkAsync()
    {
        if (string.IsNullOrWhiteSpace(InputFilePath))
            return;

        if (string.IsNullOrWhiteSpace(WatermarkText))
        {
            ResultMessage = "Please enter watermark text.";
            return;
        }

        var dialog = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Save watermarked PDF as",
            FileName = Path.GetFileNameWithoutExtension(InputFilePath) + "_watermarked.pdf"
        };

        if (dialog.ShowDialog() != true)
            return;

        IsProcessing = true;
        ResultMessage = "Adding watermark...";

        try
        {
            var inputPath = InputFilePath;
            var outputPath = dialog.FileName;
            var text = WatermarkText;
            var posIndex = SelectedPositionIndex;
            var fontSize = FontSize;
            var opacity = Opacity;
            var color = ResolveColor(SelectedColor);
            var pageRange = ParsePageRange(PageRange);

            var result = await Task.Run(() =>
            {
                var options = new PdfWatermarkOptions
                {
                    Position = (WatermarkPosition)posIndex,
                    FontSize = fontSize,
                    Opacity = opacity,
                    TextColor = color,
                    PageRange = pageRange
                };

                return PdfWatermark.AddText(inputPath, outputPath, text, options);
            });

            ResultMessage = $"Watermark applied successfully.\n" +
                            $"Pages watermarked: {result.PagesWatermarked} of {result.TotalPages}\n" +
                            $"Saved to: {dialog.FileName}";
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

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
