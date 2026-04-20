namespace Exis.PdfEditor.Demo.Wpf.Views;

using System.Windows.Controls;
using Exis.PdfEditor.Demo.Wpf.ViewModels;

public partial class BatesNumberingView : UserControl
{
    public BatesNumberingView()
    {
        InitializeComponent();
        DataContext = new BatesNumberingViewModel();
    }
}
