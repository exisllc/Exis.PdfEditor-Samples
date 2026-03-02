namespace Exis.PdfEditor.Demo.Console.Demos;

using System.Diagnostics;
using Exis.PdfEditor;

internal static class OptimizationDemo
{
    public static async Task RunAsync()
    {
        ConsoleHelper.WriteHeader("Optimization Demo");
        var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
        Directory.CreateDirectory(outputDir);

        try
        {
            // 1. Create a sample PDF with substantial content for optimization
            ConsoleHelper.WriteInfo("Creating sample PDF with substantial content for optimization...");
            var samplePath = Path.Combine(outputDir, "optimize-sample.pdf");
            var sw = Stopwatch.StartNew();

            var builder = PdfBuilder.Create();
            for (int pageNum = 1; pageNum <= 10; pageNum++)
            {
                int pn = pageNum; // capture for closure
                builder.AddPage(page =>
                {
                    page.Size(PdfPageSize.A4);

                    // Header with company branding (repeated element — good optimization target)
                    page.AddRectangle(0, 800, 595, 42, fill: true,
                        fillRed: 0.145, fillGreen: 0.388, fillBlue: 0.921);
                    page.AddText("Exis Technologies — Monthly Operations Report", 100, 816, 14,
                        o => o.Bold().Color(1.0, 1.0, 1.0));
                    page.AddText($"Page {pn} of 10", 500, 806, 9,
                        o => o.Color(1.0, 1.0, 1.0));

                    // Dense text content
                    page.AddText($"Section {pn}: {GetSectionTitle(pn)}", 50, 770, 14,
                        o => o.Bold().Color(0.196, 0.196, 0.196));

                    double y = 745;
                    for (int line = 0; line < 35; line++)
                    {
                        page.AddText(GetSampleText(pn, line), 50, y, 10);
                        y -= 16;
                    }

                    // Table on each page (repeated structure)
                    y -= 10;
                    page.AddRectangle(50, y, 495, 18, fill: true,
                        fillRed: 0.145, fillGreen: 0.388, fillBlue: 0.921);
                    page.AddText("Metric", 60, y + 4, 9, o => o.Bold().Color(1.0, 1.0, 1.0));
                    page.AddText("Target", 200, y + 4, 9, o => o.Bold().Color(1.0, 1.0, 1.0));
                    page.AddText("Actual", 300, y + 4, 9, o => o.Bold().Color(1.0, 1.0, 1.0));
                    page.AddText("Variance", 420, y + 4, 9, o => o.Bold().Color(1.0, 1.0, 1.0));

                    y -= 18;
                    for (int row = 0; row < 5; row++)
                    {
                        if (row % 2 == 0)
                        {
                            page.AddRectangle(50, y, 495, 16, fill: true,
                                fillRed: 0.953, fillGreen: 0.957, fillBlue: 0.965);
                        }
                        page.AddText($"KPI {pn}.{row + 1}", 60, y + 3, 9);
                        page.AddText($"{90 + row}%", 210, y + 3, 9);
                        page.AddText($"{88 + row + pn}%", 310, y + 3, 9);
                        page.AddText($"+{row + pn - 2}%", 430, y + 3, 9);
                        y -= 16;
                    }

                    // Footer
                    page.AddText("Confidential — Exis Technologies Internal Use Only", 180, 25, 7,
                        o => o.Color(0.588, 0.588, 0.588));
                });
            }
            builder.BuildToFile(samplePath);
            sw.Stop();

            var originalSize = new FileInfo(samplePath).Length;
            ConsoleHelper.WriteSuccess($"Sample PDF created in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Original size: {FormatBytes(originalSize)}");

            // 2. Default optimization
            ConsoleHelper.WriteInfo("\n--- Default Optimization ---");
            sw.Restart();
            var defaultOptPath = Path.Combine(outputDir, "optimized-default.pdf");
            var defaultResult = await PdfOptimizer.OptimizeAsync(samplePath, defaultOptPath);
            sw.Stop();

            var defaultOptSize = new FileInfo(defaultOptPath).Length;
            var defaultReduction = (1.0 - (double)defaultOptSize / originalSize) * 100;
            ConsoleHelper.WriteSuccess($"Default optimization completed in {sw.ElapsedMilliseconds}ms");
            PrintOptimizationResult("Default", originalSize, defaultOptSize, defaultReduction);
            ConsoleHelper.WriteInfo($"    Reduction: {defaultResult.ReductionPercent:F1}% | Saved: {FormatBytes(defaultResult.BytesSaved)}");

            // 3. Aggressive optimization (web-ready)
            ConsoleHelper.WriteInfo("\n--- Aggressive Optimization (Web) ---");
            sw.Restart();
            var aggressivePath = Path.Combine(outputDir, "optimized-aggressive.pdf");
            var aggressiveOptions = new PdfOptimizeOptions
            {
                CompressStreams = true,
                RemoveDuplicateObjects = true,
                RemoveMetadata = false,
                DownsampleImages = true,
                MaxImageDpi = 150
            };
            var aggressiveResult = await PdfOptimizer.OptimizeAsync(samplePath, aggressivePath, aggressiveOptions);
            sw.Stop();

            var aggressiveSize = new FileInfo(aggressivePath).Length;
            var aggressiveReduction = (1.0 - (double)aggressiveSize / originalSize) * 100;
            ConsoleHelper.WriteSuccess($"Aggressive optimization completed in {sw.ElapsedMilliseconds}ms");
            PrintOptimizationResult("Aggressive", originalSize, aggressiveSize, aggressiveReduction);
            ConsoleHelper.WriteInfo($"    Reduction: {aggressiveResult.ReductionPercent:F1}% | Saved: {FormatBytes(aggressiveResult.BytesSaved)}");

            // 4. Print-ready optimization (higher quality)
            ConsoleHelper.WriteInfo("\n--- Print-Ready Optimization ---");
            sw.Restart();
            var printPath = Path.Combine(outputDir, "optimized-print.pdf");
            var printOptions = new PdfOptimizeOptions
            {
                CompressStreams = true,
                RemoveDuplicateObjects = true,
                RemoveMetadata = false,
                DownsampleImages = false
            };
            var printResult = await PdfOptimizer.OptimizeAsync(samplePath, printPath, printOptions);
            sw.Stop();

            var printSize = new FileInfo(printPath).Length;
            var printReduction = (1.0 - (double)printSize / originalSize) * 100;
            ConsoleHelper.WriteSuccess($"Print-ready optimization completed in {sw.ElapsedMilliseconds}ms");
            PrintOptimizationResult("Print-Ready", originalSize, printSize, printReduction);
            ConsoleHelper.WriteInfo($"    Reduction: {printResult.ReductionPercent:F1}% | Saved: {FormatBytes(printResult.BytesSaved)}");

            // 5. Summary comparison
            ConsoleHelper.WriteInfo("\n--- Optimization Summary ---");
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine();
            System.Console.WriteLine($"    {"Profile",-20} {"Original",-14} {"Optimized",-14} {"Reduction",-12}");
            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.WriteLine($"    {"".PadLeft(58, '─')}");

            var files = new[]
            {
                ("Default", defaultOptPath),
                ("Aggressive (Web)", aggressivePath),
                ("Print-Ready", printPath)
            };

            foreach (var (profileName, path) in files)
            {
                var optSize = new FileInfo(path).Length;
                var pctReduction = (1.0 - (double)optSize / originalSize) * 100;

                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.Write($"    {profileName,-20}");
                System.Console.Write($" {FormatBytes(originalSize),-14}");
                System.Console.Write($" {FormatBytes(optSize),-14}");
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine($" {pctReduction:F1}%");
            }
            System.Console.ResetColor();

            ConsoleHelper.WriteSuccess("\nAll optimization demos completed successfully!");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Optimization demo failed: {ex.Message}");
        }
    }

    private static void PrintOptimizationResult(string profile, long originalSize, long optimizedSize, double reduction)
    {
        System.Console.ForegroundColor = ConsoleColor.White;
        System.Console.WriteLine($"    Profile:   {profile}");
        System.Console.WriteLine($"    Original:  {FormatBytes(originalSize)}");
        System.Console.WriteLine($"    Optimized: {FormatBytes(optimizedSize)}");
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine($"    Reduction: {reduction:F1}%");
        System.Console.ResetColor();
    }

    private static string FormatBytes(long bytes)
    {
        if (bytes >= 1_048_576)
            return $"{bytes / 1_048_576.0:F2} MB";
        if (bytes >= 1024)
            return $"{bytes / 1024.0:F1} KB";
        return $"{bytes} bytes";
    }

    private static string GetSectionTitle(int pageNum) => pageNum switch
    {
        1 => "Executive Overview",
        2 => "Revenue Performance",
        3 => "Customer Acquisition",
        4 => "Product Development",
        5 => "Infrastructure & Ops",
        6 => "Human Resources",
        7 => "Marketing Analytics",
        8 => "Partner Ecosystem",
        9 => "Risk Assessment",
        10 => "Forward Guidance",
        _ => "General"
    };

    private static string GetSampleText(int page, int line)
    {
        var texts = new[]
        {
            "This section presents detailed analysis of quarterly performance metrics across all business units.",
            "Key performance indicators demonstrate continued momentum in enterprise customer acquisition.",
            "Revenue growth accelerated in the latter half of the quarter, driven by large contract signings.",
            "The engineering team delivered 34 new features while maintaining 99.97% platform availability.",
            "Customer satisfaction scores reached an all-time high with NPS increasing 6 points quarter-over-quarter.",
            "Infrastructure costs decreased 12% through optimization of cloud resource allocation.",
            "The sales pipeline remains healthy with $45M in qualified opportunities for the upcoming quarter.",
            "Cross-functional initiatives delivered measurable improvements in operational efficiency.",
            "Strategic partnerships contributed $3.2M in co-sell revenue during the reporting period.",
            "Talent acquisition remains on track with 28 new hires across engineering and go-to-market teams."
        };
        return texts[(page + line) % texts.Length];
    }
}
