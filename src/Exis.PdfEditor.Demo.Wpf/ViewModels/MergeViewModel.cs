namespace Exis.PdfEditor.Demo.Wpf.ViewModels;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Exis.PdfEditor.Demo.Wpf.Helpers;
using Exis.PdfEditor;
using Microsoft.Win32;

public class MergeViewModel : INotifyPropertyChanged
{
    private string? _selectedFile;
    private string _outputPath = string.Empty;
    private string _resultMessage = string.Empty;
    private bool _isProcessing;

    public MergeViewModel()
    {
        FileList = new ObservableCollection<string>();

        AddFilesCommand = new RelayCommand(AddFiles);
        RemoveFileCommand = new RelayCommand(RemoveFile, () => SelectedFile is not null);
        MoveUpCommand = new RelayCommand(MoveUp, () => CanMoveUp());
        MoveDownCommand = new RelayCommand(MoveDown, () => CanMoveDown());
        MergeCommand = new RelayCommand(MergeAsync, () => !IsProcessing && FileList.Count >= 2);
        SplitCommand = new RelayCommand(SplitAsync, () => !IsProcessing && FileList.Count >= 1);
    }

    public ObservableCollection<string> FileList { get; }

    public string? SelectedFile
    {
        get => _selectedFile;
        set { _selectedFile = value; OnPropertyChanged(); }
    }

    public string OutputPath
    {
        get => _outputPath;
        set { _outputPath = value; OnPropertyChanged(); }
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

    public ICommand AddFilesCommand { get; }
    public ICommand RemoveFileCommand { get; }
    public ICommand MoveUpCommand { get; }
    public ICommand MoveDownCommand { get; }
    public ICommand MergeCommand { get; }
    public ICommand SplitCommand { get; }

    private void AddFiles()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
            Title = "Select PDF files to merge",
            Multiselect = true
        };

        if (dialog.ShowDialog() == true)
        {
            foreach (var file in dialog.FileNames)
            {
                FileList.Add(file);
            }
            ResultMessage = $"{FileList.Count} file(s) in queue.";
        }
    }

    private void RemoveFile()
    {
        if (SelectedFile is not null && FileList.Contains(SelectedFile))
        {
            FileList.Remove(SelectedFile);
            SelectedFile = FileList.FirstOrDefault();
            ResultMessage = $"{FileList.Count} file(s) in queue.";
        }
    }

    private bool CanMoveUp()
    {
        if (SelectedFile is null) return false;
        var index = FileList.IndexOf(SelectedFile);
        return index > 0;
    }

    private bool CanMoveDown()
    {
        if (SelectedFile is null) return false;
        var index = FileList.IndexOf(SelectedFile);
        return index >= 0 && index < FileList.Count - 1;
    }

    private void MoveUp()
    {
        if (SelectedFile is null) return;
        var index = FileList.IndexOf(SelectedFile);
        if (index <= 0) return;
        var item = SelectedFile;
        FileList.RemoveAt(index);
        FileList.Insert(index - 1, item);
        SelectedFile = item;
    }

    private void MoveDown()
    {
        if (SelectedFile is null) return;
        var index = FileList.IndexOf(SelectedFile);
        if (index < 0 || index >= FileList.Count - 1) return;
        var item = SelectedFile;
        FileList.RemoveAt(index);
        FileList.Insert(index + 1, item);
        SelectedFile = item;
    }

    private async Task MergeAsync()
    {
        if (FileList.Count < 2) return;

        var dialog = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Save merged PDF as",
            FileName = "merged.pdf"
        };

        if (dialog.ShowDialog() != true)
            return;

        OutputPath = dialog.FileName;
        IsProcessing = true;
        ResultMessage = "Merging PDFs...";

        try
        {
            var files = FileList.ToArray();
            var outputPath = OutputPath;

            await Task.Run(async () =>
            {
                await PdfMerger.MergeToFileAsync(files, outputPath);
            });

            ResultMessage = $"Merged {files.Length} files successfully.\nSaved to: {OutputPath}";
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

    private async Task SplitAsync()
    {
        if (FileList.Count < 1) return;

        var dialog = new Microsoft.Win32.SaveFileDialog
        {
            Title = "Select output folder (enter any filename)",
            FileName = "split_output",
            Filter = "All Files (*.*)|*.*"
        };

        if (dialog.ShowDialog() != true)
            return;

        var outputDir = Path.GetDirectoryName(dialog.FileName) ?? Directory.GetCurrentDirectory();
        IsProcessing = true;
        ResultMessage = "Splitting PDF...";

        try
        {
            var inputFile = FileList.First();
            var baseName = Path.GetFileNameWithoutExtension(inputFile);
            var outputPattern = Path.Combine(outputDir, $"{baseName}_part{{0:D3}}.pdf");

            await Task.Run(async () =>
            {
                await PdfSplitter.SplitToFilesAsync(inputFile, outputPattern);
            });

            ResultMessage = $"Split completed. Files saved to: {outputDir}";
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
