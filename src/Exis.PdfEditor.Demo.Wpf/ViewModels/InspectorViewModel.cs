namespace Exis.PdfEditor.Demo.Wpf.ViewModels;

using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Exis.PdfEditor.Demo.Wpf.Helpers;
using Exis.PdfEditor;
using Microsoft.Win32;

public class InspectorViewModel : INotifyPropertyChanged
{
    private string _inputFilePath = string.Empty;
    private int _pageCount;
    private string _fileSize = string.Empty;
    private string _author = string.Empty;
    private string _title = string.Empty;
    private string _creationDate = string.Empty;
    private bool _isEncrypted;
    private string _textContent = string.Empty;
    private string _resultMessage = string.Empty;
    private bool _isProcessing;

    public InspectorViewModel()
    {
        BrowseCommand = new RelayCommand(Browse);
        InspectCommand = new RelayCommand(InspectAsync, () => !IsProcessing && !string.IsNullOrWhiteSpace(InputFilePath));
        ExtractTextCommand = new RelayCommand(ExtractTextAsync, () => !IsProcessing && !string.IsNullOrWhiteSpace(InputFilePath));
    }

    public string InputFilePath
    {
        get => _inputFilePath;
        set { _inputFilePath = value; OnPropertyChanged(); }
    }

    public int PageCount
    {
        get => _pageCount;
        set { _pageCount = value; OnPropertyChanged(); }
    }

    public string FileSize
    {
        get => _fileSize;
        set { _fileSize = value; OnPropertyChanged(); }
    }

    public string Author
    {
        get => _author;
        set { _author = value; OnPropertyChanged(); }
    }

    public string Title
    {
        get => _title;
        set { _title = value; OnPropertyChanged(); }
    }

    public string CreationDate
    {
        get => _creationDate;
        set { _creationDate = value; OnPropertyChanged(); }
    }

    public bool IsEncrypted
    {
        get => _isEncrypted;
        set { _isEncrypted = value; OnPropertyChanged(); }
    }

    public string TextContent
    {
        get => _textContent;
        set { _textContent = value; OnPropertyChanged(); }
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
    public ICommand InspectCommand { get; }
    public ICommand ExtractTextCommand { get; }

    private void Browse()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
            Title = "Select a PDF file to inspect"
        };

        if (dialog.ShowDialog() == true)
        {
            InputFilePath = dialog.FileName;
            ResultMessage = string.Empty;
            TextContent = string.Empty;
            ClearMetadata();
        }
    }

    private void ClearMetadata()
    {
        PageCount = 0;
        FileSize = string.Empty;
        Author = string.Empty;
        Title = string.Empty;
        CreationDate = string.Empty;
        IsEncrypted = false;
    }

    private async Task InspectAsync()
    {
        if (string.IsNullOrWhiteSpace(InputFilePath))
            return;

        IsProcessing = true;
        ResultMessage = "Inspecting PDF...";

        try
        {
            var filePath = InputFilePath;

            // File size from filesystem
            var fileInfo = new FileInfo(filePath);
            FileSize = FormatFileSize(fileInfo.Length);

            PdfDocumentInfo info = null!;

            await Task.Run(async () =>
            {
                info = await PdfInspector.InspectAsync(filePath);
            });

            PageCount = info.PageCount;
            Author = info.Author ?? "(not set)";
            Title = info.Title ?? "(not set)";
            CreationDate = info.CreationDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? "(not set)";
            IsEncrypted = info.IsEncrypted;

            ResultMessage = "Inspection complete.";
        }
        catch (Exception ex)
        {
            ResultMessage = $"Error: {ex.Message}";
            ClearMetadata();
        }
        finally
        {
            IsProcessing = false;
        }
    }

    private async Task ExtractTextAsync()
    {
        if (string.IsNullOrWhiteSpace(InputFilePath))
            return;

        IsProcessing = true;
        ResultMessage = "Extracting text content...";

        try
        {
            var filePath = InputFilePath;
            string extractedText = string.Empty;

            await Task.Run(() =>
            {
                var result = PdfTextExtractor.ExtractText(filePath);
                extractedText = result.FullText;
            });

            TextContent = string.IsNullOrWhiteSpace(extractedText)
                ? "(No text content found in this PDF.)"
                : extractedText;

            ResultMessage = $"Extracted {TextContent.Length:N0} characters from the PDF.";
        }
        catch (Exception ex)
        {
            ResultMessage = $"Error: {ex.Message}";
            TextContent = string.Empty;
        }
        finally
        {
            IsProcessing = false;
        }
    }

    private static string FormatFileSize(long bytes)
    {
        string[] suffixes = { "B", "KB", "MB", "GB" };
        int order = 0;
        double size = bytes;

        while (size >= 1024 && order < suffixes.Length - 1)
        {
            order++;
            size /= 1024;
        }

        return $"{size:0.##} {suffixes[order]}";
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
