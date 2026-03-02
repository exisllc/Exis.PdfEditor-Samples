namespace Exis.PdfEditor.Demo.Console.Demos;

internal static class ConsoleHelper
{
    public static void WriteHeader(string text)
    {
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine($"\n{"".PadLeft(60, '═')}");
        System.Console.WriteLine($"  {text}");
        System.Console.WriteLine($"{"".PadLeft(60, '═')}");
        System.Console.ResetColor();
    }

    public static void WriteSuccess(string text)
    {
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine($"  ✓ {text}");
        System.Console.ResetColor();
    }

    public static void WriteError(string text)
    {
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine($"  ✗ {text}");
        System.Console.ResetColor();
    }

    public static void WriteInfo(string text)
    {
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine($"  ℹ {text}");
        System.Console.ResetColor();
    }

    public static void WritePromo()
    {
        System.Console.ForegroundColor = ConsoleColor.Magenta;
        System.Console.WriteLine("\n  ─────────────────────────────────────────────────────────");
        System.Console.WriteLine("  📦 Get Exis.PdfEditor → https://www.nuget.org/packages/Exis.PdfEditor");
        System.Console.WriteLine("  ─────────────────────────────────────────────────────────\n");
        System.Console.ResetColor();
    }
}
