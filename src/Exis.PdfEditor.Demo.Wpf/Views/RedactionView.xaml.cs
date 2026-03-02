namespace Exis.PdfEditor.Demo.Wpf.Views;

using System.Windows.Controls;
using Exis.PdfEditor.Demo.Wpf.ViewModels;

public partial class RedactionView : UserControl
{
    public RedactionView()
    {
        InitializeComponent();
        DataContext = new RedactionViewModel();
    }
}
