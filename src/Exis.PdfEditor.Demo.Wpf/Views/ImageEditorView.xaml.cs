namespace Exis.PdfEditor.Demo.Wpf.Views;

using System.Windows.Controls;
using Exis.PdfEditor.Demo.Wpf.ViewModels;

public partial class ImageEditorView : UserControl
{
    public ImageEditorView()
    {
        InitializeComponent();
        DataContext = new ImageEditorViewModel();
    }
}
