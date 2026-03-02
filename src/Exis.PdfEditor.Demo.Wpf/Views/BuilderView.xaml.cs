namespace Exis.PdfEditor.Demo.Wpf.Views;

using System.Windows.Controls;
using Exis.PdfEditor.Demo.Wpf.ViewModels;

public partial class BuilderView : UserControl
{
    public BuilderView()
    {
        InitializeComponent();
        DataContext = new BuilderViewModel();
    }
}
