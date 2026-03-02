namespace Exis.PdfEditor.Demo.Wpf;

using System.Windows;
using Exis.PdfEditor.Licensing;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ExisLicense.Initialize();
    }
}
