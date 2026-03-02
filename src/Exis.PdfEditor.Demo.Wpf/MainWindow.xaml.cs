namespace Exis.PdfEditor.Demo.Wpf;

using System.Windows;
using Exis.PdfEditor.Demo.Wpf.ViewModels;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}
