namespace Exis.PdfEditor.Demo.Console;

using Exis.PdfEditor.Demo.Console.Demos;
using Exis.PdfEditor.Licensing;

internal class Program
{
    private static async Task Main(string[] args)
    {
        ExisLicense.Initialize();

        while (true)
        {
            PrintBanner();
            PrintMenu();

            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.Write("  Select an option: ");
            System.Console.ResetColor();

            var input = System.Console.ReadLine()?.Trim();

            if (input == "0")
            {
                System.Console.ForegroundColor = ConsoleColor.Cyan;
                System.Console.WriteLine("\n  Thank you for exploring Exis.PdfEditor!");
                System.Console.ResetColor();
                ConsoleHelper.WritePromo();
                break;
            }

            System.Console.Clear();

            switch (input)
            {
                case "1":
                    await FindReplaceDemo.RunAsync();
                    break;
                case "2":
                    await MergeSplitDemo.RunAsync();
                    break;
                case "3":
                    await BuilderDemo.RunAsync();
                    break;
                case "4":
                    await ReportGeneratorDemo.RunAsync();
                    break;
                case "5":
                    await FormFillingDemo.RunAsync();
                    break;
                case "6":
                    await RedactionDemo.RunAsync();
                    break;
                case "7":
                    await OptimizationDemo.RunAsync();
                    break;
                case "8":
                    await TextExtractionDemo.RunAsync();
                    break;
                case "9":
                    await PdfADemo.RunAsync();
                    break;
                case "10":
                    await DigitalSignatureDemo.RunAsync();
                    break;
                case "11":
                    await FullPipelineDemo.RunAsync();
                    break;
                default:
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine("\n  Invalid selection. Please try again.");
                    System.Console.ResetColor();
                    continue;
            }

            ConsoleHelper.WritePromo();

            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.WriteLine("  Press any key to return to the menu...");
            System.Console.ResetColor();
            System.Console.ReadKey(true);
            System.Console.Clear();
        }
    }

    private static void PrintBanner()
    {
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine();
        System.Console.WriteLine("  ╔══════════════════════════════════════════════════════════╗");
        System.Console.WriteLine("  ║                                                          ║");
        System.Console.WriteLine("  ║          E X I S . P D F E D I T O R                     ║");
        System.Console.WriteLine("  ║          Demo Suite v1.5.2                                ║");
        System.Console.WriteLine("  ║                                                          ║");
        System.Console.WriteLine("  ║    Professional PDF Processing for .NET                   ║");
        System.Console.WriteLine("  ║                                                          ║");
        System.Console.WriteLine("  ╚══════════════════════════════════════════════════════════╝");
        System.Console.ResetColor();
        System.Console.WriteLine();
    }

    private static void PrintMenu()
    {
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("  ┌──────────────────────────────────────────────────────────┐");
        System.Console.WriteLine("  │                    DEMO MENU                              │");
        System.Console.WriteLine("  ├──────────────────────────────────────────────────────────┤");
        System.Console.ResetColor();

        PrintMenuItem(" 1", "Find & Replace", "Template-based text replacement");
        PrintMenuItem(" 2", "Merge & Split", "Combine and split PDF documents");
        PrintMenuItem(" 3", "PDF Builder", "Create documents from scratch");
        PrintMenuItem(" 4", "Report Generator", "Multi-page business reports  ★");
        PrintMenuItem(" 5", "Form Filling", "Read, fill, and flatten PDF forms");
        PrintMenuItem(" 6", "Redaction", "Securely remove sensitive content");
        PrintMenuItem(" 7", "Optimization", "Compress and reduce file sizes");
        PrintMenuItem(" 8", "Text Extraction", "Extract structured text content");
        PrintMenuItem(" 9", "PDF/A Compliance", "Validate and convert to PDF/A");
        PrintMenuItem("10", "Digital Signatures", "Sign and verify PDF documents");
        PrintMenuItem("11", "Full Pipeline Demo", "End-to-end enterprise workflow");

        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("  ├──────────────────────────────────────────────────────────┤");
        System.Console.ResetColor();

        PrintMenuItem(" 0", "Exit", "");

        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("  └──────────────────────────────────────────────────────────┘");
        System.Console.ResetColor();
        System.Console.WriteLine();
    }

    private static void PrintMenuItem(string number, string title, string description)
    {
        System.Console.ForegroundColor = ConsoleColor.White;
        System.Console.Write($"  │  ");
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.Write($"[{number}]");
        System.Console.ForegroundColor = ConsoleColor.White;
        System.Console.Write($"  {title,-24}");
        System.Console.ForegroundColor = ConsoleColor.DarkGray;
        System.Console.Write($"{description,-26}");
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("│");
        System.Console.ResetColor();
    }
}
