namespace Exis.PdfEditor.Demo.Console.Demos;

using System.Diagnostics;
using Exis.PdfEditor;

internal static class BatesNumberingDemo
{
    public static async Task RunAsync()
    {
        ConsoleHelper.WriteHeader("Bates Numbering Demo");
        var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
        Directory.CreateDirectory(outputDir);

        try
        {
            ConsoleHelper.WriteInfo("Creating sample 5-page legal document...");
            var samplePath = Path.Combine(outputDir, "bates-sample.pdf");
            var builder = PdfBuilder.Create();
            for (int i = 1; i <= 5; i++)
            {
                int pageNumber = i;
                builder.AddPage(page =>
                {
                    page.Size(PdfPageSize.Letter);
                    page.AddText($"Case File — Document {pageNumber}", 72, 720, 16,
                        o => o.Bold().Color(0.1, 0.2, 0.5));
                    page.AddText("Produced for discovery review.", 72, 690, 11);
                    page.AddText($"Exhibit content for page {pageNumber}.", 72, 660, 11);
                });
            }
            builder.BuildToFile(samplePath);
            ConsoleHelper.WriteSuccess($"Sample PDF created: {samplePath}");

            // Demo 1 — Default Bates stamping (bottom-right, ABC000001 style)
            ConsoleHelper.WriteInfo("\n--- Demo 1: Default Bates Stamp ---");
            var defaultPath = Path.Combine(outputDir, "bates-default.pdf");
            var sw = Stopwatch.StartNew();
            var result = await Task.Run(() =>
                PdfBatesStamp.ApplyBatesStamp(samplePath, defaultPath));
            sw.Stop();
            ConsoleHelper.WriteSuccess(
                $"Stamped {result.PagesStamped} pages " +
                $"({result.FirstNumber} → {result.LastNumber}) in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Output: {defaultPath}");

            // Demo 2 — Custom prefix, starting number, and confidentiality label
            ConsoleHelper.WriteInfo("\n--- Demo 2: Legal Production Format ---");
            var legalPath = Path.Combine(outputDir, "bates-legal.pdf");
            sw.Restart();
            result = await Task.Run(() =>
                PdfBatesStamp.ApplyBatesStamp(samplePath, legalPath, new BatesStampOptions
                {
                    Prefix = "SMITH-",
                    Suffix = "-A",
                    StartNumber = 1001,
                    Digits = 5,
                    Position = BatesPosition.BottomRight,
                    FontSize = 10,
                    TextColor = PdfColor.Black,
                    MarginInches = 0.5f,
                    ConfidentialityLabel = "CONFIDENTIAL"
                }));
            sw.Stop();
            ConsoleHelper.WriteSuccess(
                $"Stamped {result.PagesStamped} pages " +
                $"({result.FirstNumber} → {result.LastNumber}) in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Output: {legalPath}");

            // Demo 3 — Skip first page (cover sheet), counter advances on skipped pages
            ConsoleHelper.WriteInfo("\n--- Demo 3: Skip Cover Sheet ---");
            var skipPath = Path.Combine(outputDir, "bates-skip-cover.pdf");
            sw.Restart();
            result = await Task.Run(() =>
                PdfBatesStamp.ApplyBatesStamp(samplePath, skipPath, new BatesStampOptions
                {
                    Prefix = "DOC",
                    StartNumber = 1,
                    Digits = 6,
                    Position = BatesPosition.TopRight,
                    FontSize = 9,
                    TextColor = PdfColor.Red,
                    SkipFirstPage = true,
                    CounterAdvancesOnSkippedPages = false
                }));
            sw.Stop();
            ConsoleHelper.WriteSuccess(
                $"Stamped {result.PagesStamped} pages " +
                $"({result.FirstNumber} → {result.LastNumber}) in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Output: {skipPath}");

            if (result.Warnings is { Count: > 0 })
            {
                foreach (var warning in result.Warnings)
                    ConsoleHelper.WriteInfo($"Warning: {warning}");
            }

            ConsoleHelper.WriteSuccess("\nBates numbering demo completed successfully!");
            ConsoleHelper.WriteInfo("Use cases: legal discovery production, document control, audit trails.");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Bates numbering demo failed: {ex.Message}");
        }
    }
}
