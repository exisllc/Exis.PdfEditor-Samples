namespace Exis.PdfEditor.Demo.Wpf.ViewModels;

using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Exis.PdfEditor.Demo.Wpf.Helpers;
using Exis.PdfEditor;
using Microsoft.Win32;

public class FindReplaceViewModel : INotifyPropertyChanged
{
    private string _inputFilePath = string.Empty;
    private string _searchText = string.Empty;
    private string _replaceText = string.Empty;
    private bool _useRegex;
    private bool _useAdaptiveFitting = true;
    private string _selectedTextColor = "None";
    private string _selectedHighlightColor = "None";
    private string _resultMessage = string.Empty;
    private bool _isProcessing;

    public FindReplaceViewModel()
    {
        BrowseCommand = new RelayCommand(Browse);
        ExecuteCommand = new RelayCommand(ExecuteAsync, () => !IsProcessing && !string.IsNullOrWhiteSpace(InputFilePath) && !string.IsNullOrWhiteSpace(SearchText));
        SaveAsCommand = new RelayCommand(SaveAsAsync, () => !IsProcessing && !string.IsNullOrWhiteSpace(InputFilePath));
    }

    public string InputFilePath
    {
        get => _inputFilePath;
        set { _inputFilePath = value; OnPropertyChanged(); }
    }

    public string SearchText
    {
        get => _searchText;
        set { _searchText = value; OnPropertyChanged(); }
    }

    public string ReplaceText
    {
        get => _replaceText;
        set { _replaceText = value; OnPropertyChanged(); }
    }

    public bool UseRegex
    {
        get => _useRegex;
        set { _useRegex = value; OnPropertyChanged(); }
    }

    public bool UseAdaptiveFitting
    {
        get => _useAdaptiveFitting;
        set { _useAdaptiveFitting = value; OnPropertyChanged(); }
    }

    public string[] ColorOptions { get; } = { "None", "Red", "Blue", "Green", "Orange", "Magenta", "Cyan", "Black" };

    public string SelectedTextColor
    {
        get => _selectedTextColor;
        set { _selectedTextColor = value; OnPropertyChanged(); }
    }

    public string SelectedHighlightColor
    {
        get => _selectedHighlightColor;
        set { _selectedHighlightColor = value; OnPropertyChanged(); }
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
    public ICommand ExecuteCommand { get; }
    public ICommand SaveAsCommand { get; }

    private void Browse()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
            Title = "Select a PDF file"
        };

        if (dialog.ShowDialog() == true)
        {
            InputFilePath = dialog.FileName;
            ResultMessage = string.Empty;
        }
    }

    private async Task ExecuteAsync()
    {
        if (string.IsNullOrWhiteSpace(InputFilePath) || string.IsNullOrWhiteSpace(SearchText))
            return;

        IsProcessing = true;
        ResultMessage = "Processing find & replace...";

        try
        {
            var outputPath = GenerateOutputPath(InputFilePath, "_replaced");
            var searchText = SearchText;
            var replaceText = ReplaceText;
            var useRegex = UseRegex;
            var useAdaptive = UseAdaptiveFitting;
            var textColor = ResolvePdfColor(SelectedTextColor);
            var highlightColor = ResolvePdfColor(SelectedHighlightColor);

            await Task.Run(async () =>
            {
                var options = new PdfFindReplaceOptions
                {
                    UseRegex = useRegex,
                    TextFitting = useAdaptive ? TextFittingMode.Adaptive : TextFittingMode.None,
                    ReplacementTextColor = textColor,
                    ReplacementHighlightColor = highlightColor
                };

                await PdfFindReplace.ExecuteAsync(InputFilePath, outputPath, searchText, replaceText, options);
            });

            ResultMessage = $"Find & replace completed successfully.\nSaved to: {outputPath}";
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

    private async Task SaveAsAsync()
    {
        if (string.IsNullOrWhiteSpace(InputFilePath))
            return;

        var dialog = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Save modified PDF as",
            FileName = Path.GetFileNameWithoutExtension(InputFilePath) + "_replaced.pdf"
        };

        if (dialog.ShowDialog() != true)
            return;

        IsProcessing = true;
        ResultMessage = "Saving...";

        try
        {
            var searchText = SearchText;
            var replaceText = ReplaceText;
            var useRegex = UseRegex;
            var useAdaptive = UseAdaptiveFitting;
            var textColor = ResolvePdfColor(SelectedTextColor);
            var highlightColor = ResolvePdfColor(SelectedHighlightColor);
            var outputPath = dialog.FileName;

            await Task.Run(async () =>
            {
                var options = new PdfFindReplaceOptions
                {
                    UseRegex = useRegex,
                    TextFitting = useAdaptive ? TextFittingMode.Adaptive : TextFittingMode.None,
                    ReplacementTextColor = textColor,
                    ReplacementHighlightColor = highlightColor
                };

                await PdfFindReplace.ExecuteAsync(InputFilePath, outputPath, searchText, replaceText, options);
            });

            ResultMessage = $"File saved to: {dialog.FileName}";
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

    private static PdfColor? ResolvePdfColor(string colorName) => colorName switch
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

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
