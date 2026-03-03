namespace Exis.PdfEditor.Demo.Wpf.ViewModels;

using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Exis.PdfEditor.Demo.Wpf.Helpers;
using Exis.PdfEditor;
using Microsoft.Win32;

public class ImageEditorViewModel : INotifyPropertyChanged
{
    private string _inputFilePath = string.Empty;
    private string _replacementImagePath = string.Empty;
    private string _resultMessage = string.Empty;
    private string _imageListText = string.Empty;
    private bool _isProcessing;
    private bool _replaceByIndex;
    private string _imageIndices = string.Empty;

    public ImageEditorViewModel()
    {
        BrowseCommand = new RelayCommand(Browse);
        BrowseImageCommand = new RelayCommand(BrowseImage);
        FindImagesCommand = new RelayCommand(FindImagesAsync, () => !IsProcessing && !string.IsNullOrWhiteSpace(InputFilePath));
        ReplaceAllCommand = new RelayCommand(ReplaceAllAsync, () => !IsProcessing && !string.IsNullOrWhiteSpace(InputFilePath) && !string.IsNullOrWhiteSpace(ReplacementImagePath));
        ReplaceSelectedCommand = new RelayCommand(ReplaceSelectedAsync, () => !IsProcessing && !string.IsNullOrWhiteSpace(InputFilePath) && !string.IsNullOrWhiteSpace(ReplacementImagePath) && ReplaceByIndex);
    }

    public string InputFilePath
    {
        get => _inputFilePath;
        set { _inputFilePath = value; OnPropertyChanged(); }
    }

    public string ReplacementImagePath
    {
        get => _replacementImagePath;
        set { _replacementImagePath = value; OnPropertyChanged(); }
    }

    public string ResultMessage
    {
        get => _resultMessage;
        set { _resultMessage = value; OnPropertyChanged(); }
    }

    public string ImageListText
    {
        get => _imageListText;
        set { _imageListText = value; OnPropertyChanged(); }
    }

    public bool IsProcessing
    {
        get => _isProcessing;
        set { _isProcessing = value; OnPropertyChanged(); }
    }

    public bool ReplaceByIndex
    {
        get => _replaceByIndex;
        set { _replaceByIndex = value; OnPropertyChanged(); }
    }

    public string ImageIndices
    {
        get => _imageIndices;
        set { _imageIndices = value; OnPropertyChanged(); }
    }

    public ICommand BrowseCommand { get; }
    public ICommand BrowseImageCommand { get; }
    public ICommand FindImagesCommand { get; }
    public ICommand ReplaceAllCommand { get; }
    public ICommand ReplaceSelectedCommand { get; }

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
            ImageListText = string.Empty;
        }
    }

    private void BrowseImage()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All Files (*.*)|*.*",
            Title = "Select a replacement image"
        };

        if (dialog.ShowDialog() == true)
        {
            ReplacementImagePath = dialog.FileName;
        }
    }

    private async Task FindImagesAsync()
    {
        if (string.IsNullOrWhiteSpace(InputFilePath))
            return;

        IsProcessing = true;
        ResultMessage = "Scanning for images...";

        try
        {
            var inputPath = InputFilePath;

            var result = await Task.Run(() => PdfImageEditor.FindImages(inputPath));

            var lines = new System.Text.StringBuilder();
            foreach (var img in result.Images)
            {
                lines.AppendLine($"#{img.Index}: {img.PixelWidth}x{img.PixelHeight}  {img.ColorSpace}  {img.Format}  " +
                    $"Page(s): {string.Join(", ", img.PageNumbers)}");
            }

            ImageListText = lines.ToString();
            ResultMessage = $"Found {result.TotalImages} image(s) across {result.PagesSearched} page(s).";
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

    private async Task ReplaceAllAsync()
    {
        if (string.IsNullOrWhiteSpace(InputFilePath) || string.IsNullOrWhiteSpace(ReplacementImagePath))
            return;

        var dialog = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Save PDF with replaced images",
            FileName = Path.GetFileNameWithoutExtension(InputFilePath) + "_images_replaced.pdf"
        };

        if (dialog.ShowDialog() != true)
            return;

        IsProcessing = true;
        ResultMessage = "Replacing all images...";

        try
        {
            var inputPath = InputFilePath;
            var outputPath = dialog.FileName;
            var imgPath = ReplacementImagePath;

            await Task.Run(() =>
            {
                byte[] replacementImage = File.ReadAllBytes(imgPath);
                PdfImageEditor.ReplaceAll(inputPath, outputPath, replacementImage);
            });

            ResultMessage = $"All images replaced successfully.\nSaved to: {dialog.FileName}";
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

    private async Task ReplaceSelectedAsync()
    {
        if (string.IsNullOrWhiteSpace(InputFilePath) || string.IsNullOrWhiteSpace(ReplacementImagePath))
            return;

        var dialog = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Save PDF with replaced images",
            FileName = Path.GetFileNameWithoutExtension(InputFilePath) + "_images_replaced.pdf"
        };

        if (dialog.ShowDialog() != true)
            return;

        IsProcessing = true;
        ResultMessage = "Replacing selected images...";

        try
        {
            var inputPath = InputFilePath;
            var outputPath = dialog.FileName;
            var imgPath = ReplacementImagePath;
            var indicesText = ImageIndices;

            await Task.Run(() =>
            {
                byte[] replacementImage = File.ReadAllBytes(imgPath);

                // Parse comma-separated indices
                var indices = new List<int>();
                foreach (var part in indicesText.Split(','))
                {
                    if (int.TryParse(part.Trim(), out int idx))
                        indices.Add(idx);
                }

                var options = new PdfImageReplaceOptions
                {
                    ImageIndices = indices.ToArray()
                };

                PdfImageEditor.Replace(inputPath, outputPath, replacementImage, options);
            });

            ResultMessage = $"Selected images replaced successfully.\nSaved to: {dialog.FileName}";
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
