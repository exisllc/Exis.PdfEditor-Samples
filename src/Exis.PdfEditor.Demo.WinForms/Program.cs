using Exis.PdfEditor.Licensing;

namespace Exis.PdfEditor.Demo.WinForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        ExisLicense.Initialize();
        Application.Run(new MainForm());
    }
}
