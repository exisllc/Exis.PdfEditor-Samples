namespace Exis.PdfEditor.Demo.Wpf.Views;

using System.Windows.Controls;
using Exis.PdfEditor.Demo.Wpf.ViewModels;

public partial class FindReplaceView : UserControl
{
    public FindReplaceView()
    {
        InitializeComponent();
        DataContext = new FindReplaceViewModel();
    }
}
