namespace Exis.PdfEditor.Demo.Wpf.ViewModels;

using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Exis.PdfEditor.Demo.Wpf.Helpers;
using Exis.PdfEditor;
using Microsoft.Win32;

public class BuilderViewModel : INotifyPropertyChanged
{
    private string _documentTitle = "Sample Document";
    private string _authorName = "Exis Software";
    private string _bodyText = "This document was generated using Exis.PdfEditor's PdfBuilder API.\n\nThe PdfBuilder provides a fluent interface for constructing PDF documents from scratch, including support for text layout, fonts, images, and structured content.";
    private string _outputPath = string.Empty;
    private string _selectedTemplate = "Report";
    private string _resultMessage = string.Empty;
    private bool _isProcessing;

    public BuilderViewModel()
    {
        Templates = new[] { "Letterhead", "Report", "Certificate" };
        GenerateCommand = new RelayCommand(GenerateAsync, () => !IsProcessing);
        BrowseOutputCommand = new RelayCommand(BrowseOutput);
    }

    public string[] Templates { get; }

    public string DocumentTitle
    {
        get => _documentTitle;
        set { _documentTitle = value; OnPropertyChanged(); }
    }

    public string AuthorName
    {
        get => _authorName;
        set { _authorName = value; OnPropertyChanged(); }
    }

    public string BodyText
    {
        get => _bodyText;
        set { _bodyText = value; OnPropertyChanged(); }
    }

    public string OutputPath
    {
        get => _outputPath;
        set { _outputPath = value; OnPropertyChanged(); }
    }

    public string SelectedTemplate
    {
        get => _selectedTemplate;
        set { _selectedTemplate = value; OnPropertyChanged(); }
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

    public ICommand GenerateCommand { get; }
    public ICommand BrowseOutputCommand { get; }

    private void BrowseOutput()
    {
        var dialog = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Save generated PDF as",
            FileName = $"{SelectedTemplate.ToLowerInvariant()}_output.pdf"
        };

        if (dialog.ShowDialog() == true)
        {
            OutputPath = dialog.FileName;
        }
    }

    private async Task GenerateAsync()
    {
        if (string.IsNullOrWhiteSpace(OutputPath))
        {
            BrowseOutput();
            if (string.IsNullOrWhiteSpace(OutputPath))
                return;
        }

        IsProcessing = true;
        ResultMessage = $"Generating {SelectedTemplate} document...";

        try
        {
            var title = DocumentTitle;
            var author = AuthorName;
            var body = BodyText;
            var template = SelectedTemplate;
            var output = OutputPath;

            await Task.Run(() =>
            {
                var builder = PdfBuilder.Create();

                switch (template)
                {
                    case "Letterhead":
                        BuildLetterhead(builder, title, author, body);
                        break;
                    case "Report":
                        BuildReport(builder, title, author, body);
                        break;
                    case "Certificate":
                        BuildCertificate(builder, title, author, body);
                        break;
                }

                builder.BuildToFile(output);
            });

            ResultMessage = $"{SelectedTemplate} document generated successfully.\nSaved to: {OutputPath}";
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

    private static void BuildLetterhead(PdfBuilder builder, string title, string author, string body)
    {
        builder.AddPage(page =>
        {
            page.Size(PdfPageSize.A4);

            // Company header
            page.AddText(author, 72, 72, 20, o => o.Bold());
            page.AddText("123 Business Avenue, Suite 100", 72, 96, 10);
            page.AddText("contact@example.com | www.example.com", 72, 110, 10);
            page.AddLine(72, 130, 540, 130);

            // Date
            page.AddText(DateTime.Now.ToString("MMMM dd, yyyy"), 72, 160, 11);

            // Body content
            page.AddText(title, 72, 200, 16, o => o.Bold());
            page.AddText(body, 72, 230, 11);

            // Footer
            page.AddLine(72, 740, 540, 740);
            page.AddText("Generated with Exis.PdfEditor", 72, 752, 8);
        });
    }

    private static void BuildReport(PdfBuilder builder, string title, string author, string body)
    {
        // Title page
        builder.AddPage(page =>
        {
            page.Size(PdfPageSize.A4);

            page.AddText(title, 72, 300, 28, o => o.Bold());
            page.AddText($"Prepared by {author}", 72, 340, 14);
            page.AddText(DateTime.Now.ToString("MMMM yyyy"), 72, 365, 12);
        });

        // Content page
        builder.AddPage(page =>
        {
            page.Size(PdfPageSize.A4);

            page.AddText("1. Introduction", 72, 72, 18, o => o.Bold());
            page.AddLine(72, 94, 540, 94);
            page.AddText(body, 72, 110, 11);

            page.AddText("2. Summary", 72, 400, 18, o => o.Bold());
            page.AddLine(72, 422, 540, 422);
            page.AddText(
                "This report was generated programmatically using the Exis.PdfEditor PdfBuilder API. " +
                "The builder supports multi-page documents, text formatting, geometric shapes, and metadata.",
                72, 438, 11);

            // Footer on content page
            page.AddText("Page 2", 290, 760, 9);
        });
    }

    private static void BuildCertificate(PdfBuilder builder, string title, string author, string body)
    {
        builder.AddPage(page =>
        {
            // Landscape orientation
            page.Size(PdfPageSize.A4);

            // Border rectangles
            page.AddRectangle(30, 30, 732, 552);
            page.AddRectangle(40, 40, 712, 532);

            // Header
            page.AddText("CERTIFICATE", 396, 100, 36, o => o.Bold());
            page.AddText("OF ACHIEVEMENT", 396, 140, 18);

            page.AddLine(200, 170, 592, 170);

            // Recipient
            page.AddText("This certifies that", 396, 210, 14);
            page.AddText(title, 396, 250, 26, o => o.Bold());

            // Body
            page.AddText(body, 396, 300, 12);

            // Signature area
            page.AddLine(200, 460, 370, 460);
            page.AddText("Date", 285, 472, 10);
            page.AddText(DateTime.Now.ToString("MM/dd/yyyy"), 285, 445, 11);

            page.AddLine(422, 460, 592, 460);
            page.AddText("Authorized Signature", 507, 472, 10);
            page.AddText(author, 507, 445, 11);
        });
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
