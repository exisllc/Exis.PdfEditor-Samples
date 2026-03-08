namespace Exis.PdfEditor.Demo.Console.Demos;

using System.Diagnostics;
using Exis.PdfEditor;

internal static class WatermarkDemo
{
    public static async Task RunAsync()
    {
        ConsoleHelper.WriteHeader("Watermark Demo");
        var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
        Directory.CreateDirectory(outputDir);

        try
        {
            // 1. Create a sample 2-page PDF
            ConsoleHelper.WriteInfo("Creating sample PDF...");
            var samplePath = Path.Combine(outputDir, "watermark-sample.pdf");
            PdfBuilder.Create()
                .AddPage(page =>
                {
                    page.Size(PdfPageSize.A4);
                    page.AddText("Company Report — Page 1", 72, 750, 18,
                        o => o.Bold().Color(0.1, 0.3, 0.8));
                    page.AddText("This document contains important data.", 72, 700, 12);
                })
                .AddPage(page =>
                {
                    page.Size(PdfPageSize.A4);
                    page.AddText("Company Report — Page 2", 72, 750, 18,
                        o => o.Bold().Color(0.1, 0.3, 0.8));
                    page.AddText("Financial summary and projections.", 72, 700, 12);
                })
                .BuildToFile(samplePath);
            ConsoleHelper.WriteSuccess($"Sample PDF created: {samplePath}");

            // 2. Diagonal "CONFIDENTIAL" watermark (default options)
            ConsoleHelper.WriteInfo("\n--- Demo 1: Diagonal Watermark ---");
            var diagonalPath = Path.Combine(outputDir, "watermark-diagonal.pdf");
            var sw = Stopwatch.StartNew();
            var result = await PdfWatermark.AddTextAsync(samplePath, diagonalPath, "CONFIDENTIAL");
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Watermarked {result.PagesWatermarked}/{result.TotalPages} pages in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Output: {diagonalPath}");

            // 3. Top watermark in red with 50% opacity
            ConsoleHelper.WriteInfo("\n--- Demo 2: Top Watermark (Red, 50% Opacity) ---");
            var topPath = Path.Combine(outputDir, "watermark-top-red.pdf");
            sw.Restart();
            result = await PdfWatermark.AddTextAsync(samplePath, topPath, "DRAFT", new PdfWatermarkOptions
            {
                Position = WatermarkPosition.Top,
                FontSize = 36,
                TextColor = PdfColor.Red,
                Opacity = 0.5
            });
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Watermarked {result.PagesWatermarked}/{result.TotalPages} pages in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Output: {topPath}");

            // 4. Watermark specific pages only
            ConsoleHelper.WriteInfo("\n--- Demo 3: Page-Specific Watermark ---");
            var selectivePath = Path.Combine(outputDir, "watermark-page1-only.pdf");
            sw.Restart();
            result = await PdfWatermark.AddTextAsync(samplePath, selectivePath, "PAGE 1 ONLY", new PdfWatermarkOptions
            {
                Position = WatermarkPosition.Center,
                FontSize = 48,
                Opacity = 0.2,
                PageRange = new[] { 1 }
            });
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Watermarked {result.PagesWatermarked}/{result.TotalPages} pages in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Output: {selectivePath}");

            ConsoleHelper.WriteSuccess("\nWatermark demo completed successfully!");
            ConsoleHelper.WriteInfo("Use cases: confidentiality stamps, draft markings, branding overlays.");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Watermark demo failed: {ex.Message}");
        }
    }
}
