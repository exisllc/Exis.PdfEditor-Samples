namespace Exis.PdfEditor.Demo.Wpf.Views;

using System.Windows.Controls;
using Exis.PdfEditor.Demo.Wpf.ViewModels;

public partial class WatermarkView : UserControl
{
    public WatermarkView()
    {
        InitializeComponent();
        DataContext = new WatermarkViewModel();
    }
}
