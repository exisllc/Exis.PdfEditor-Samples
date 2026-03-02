namespace Exis.PdfEditor.Demo.Wpf.Views;

using System.Windows.Controls;
using Exis.PdfEditor.Demo.Wpf.ViewModels;

public partial class InspectorView : UserControl
{
    public InspectorView()
    {
        InitializeComponent();
        DataContext = new InspectorViewModel();
    }
}
