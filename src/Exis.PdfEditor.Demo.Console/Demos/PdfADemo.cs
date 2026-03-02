namespace Exis.PdfEditor.Demo.Console.Demos;

using System.Diagnostics;
using Exis.PdfEditor;

internal static class PdfADemo
{
    public static async Task RunAsync()
    {
        ConsoleHelper.WriteHeader("PDF/A Compliance Demo");
        var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
        Directory.CreateDirectory(outputDir);

        try
        {
            // 1. Create a sample PDF (non-compliant by default)
            ConsoleHelper.WriteInfo("Creating sample PDF for PDF/A compliance testing...");
            var samplePath = Path.Combine(outputDir, "pdfa-sample.pdf");
            var sw = Stopwatch.StartNew();

            var builder = PdfBuilder.Create();
            builder.WithMetadata(m => m.Title("PDF/A Compliance Test Document").Author("Exis Technologies — Demo Suite"));

            builder.AddPage(page =>
            {
                page.Size(PdfPageSize.A4);

                page.AddRectangle(0, 790, 595, 52, fill: true,
                    fillRed: 0.145, fillGreen: 0.388, fillBlue: 0.921);
                page.AddText("Archival Document — Compliance Test", 100, 810, 20,
                    o => o.Bold().Color(1.0, 1.0, 1.0));

                page.AddText("Document Metadata", 50, 750, 14, o => o.Bold());

                var metadataLines = new[]
                {
                    $"Title:           PDF/A Compliance Test Document",
                    $"Author:          Exis Technologies — Demo Suite",
                    $"Created:         {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                    $"Generator:       Exis.PdfEditor v1.5.2",
                    $"Classification:  Internal — Archival Testing",
                    $"Reference ID:    PDFA-{DateTime.Now:yyyyMMdd}-001"
                };

                double y = 725;
                foreach (var line in metadataLines)
                {
                    page.AddText(line, 50, y, 11);
                    y -= 18;
                }

                y -= 20;
                page.AddText("Compliance Requirements", 50, y, 14, o => o.Bold());
                y -= 25;

                var requirements = new[]
                {
                    "PDF/A is an ISO-standardized version of the Portable Document Format (PDF)",
                    "designed for long-term archiving of electronic documents. It differs from regular",
                    "PDF by prohibiting features unsuitable for long-term preservation, including:",
                    "",
                    "  - JavaScript and executable file launches",
                    "  - Audio and video content",
                    "  - External content references",
                    "  - Encryption and password protection",
                    "  - Transparency (in PDF/A-1)",
                    "",
                    "PDF/A requires that all fonts used in the document be embedded, and that device-",
                    "independent color spaces be used. XMP metadata is also required.",
                    "",
                    "Exis.PdfEditor supports validation against and conversion to the following standards:",
                    "  - PDF/A-1b (ISO 19005-1, Level B conformance)",
                    "  - PDF/A-2b (ISO 19005-2, Level B conformance)",
                    "  - PDF/A-3b (ISO 19005-3, Level B conformance)"
                };

                foreach (var line in requirements)
                {
                    page.AddText(line, 50, y, 11);
                    y -= 16;
                }
            });

            builder.AddPage(page =>
            {
                page.Size(PdfPageSize.A4);
                page.AddText("Page 2 — Additional content for multi-page compliance testing.", 50, 780, 11);
                page.AddText("This page verifies that compliance conversion handles all pages correctly.", 50, 760, 11);
                page.AddText("Font embedding, color space, and metadata requirements apply across the entire document.", 50, 740, 11);
            });

            builder.BuildToFile(samplePath);
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Sample PDF created in {sw.ElapsedMilliseconds}ms: {samplePath}");

            // 2. Validate original PDF against PDF/A-2b
            ConsoleHelper.WriteInfo("\n--- Validate Original PDF Against PDF/A-2b ---");
            sw.Restart();
            var validation = await PdfAConverter.ValidateAsync(samplePath, PdfALevel.PdfA2b);
            sw.Stop();

            if (validation.IsCompliant)
            {
                ConsoleHelper.WriteSuccess($"Document IS compliant with PDF/A-2b ({sw.ElapsedMilliseconds}ms)");
            }
            else
            {
                ConsoleHelper.WriteInfo($"Document is NOT compliant with PDF/A-2b ({sw.ElapsedMilliseconds}ms)");
                ConsoleHelper.WriteInfo($"Violations found: {validation.Violations.Count}");

                int issueNum = 1;
                foreach (var violation in validation.Violations.Take(10))
                {
                    System.Console.ForegroundColor = ConsoleColor.DarkGray;
                    System.Console.Write($"    {issueNum,2}. ");
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                    System.Console.WriteLine(violation);
                    System.Console.ResetColor();
                    issueNum++;
                }

                if (validation.Violations.Count > 10)
                {
                    ConsoleHelper.WriteInfo($"  ... and {validation.Violations.Count - 10} more violations");
                }
            }

            // 3. Convert to PDF/A-1b
            ConsoleHelper.WriteInfo("\n--- Convert to PDF/A-1b ---");
            sw.Restart();
            var pdfA1Path = Path.Combine(outputDir, "pdfa-converted-1b.pdf");
            await PdfAConverter.ConvertAsync(samplePath, pdfA1Path, PdfALevel.PdfA1b);
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Converted to PDF/A-1b in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Output: {pdfA1Path}");

            // 4. Re-validate the converted PDF
            ConsoleHelper.WriteInfo("\n--- Re-Validate Converted PDF Against PDF/A-1b ---");
            sw.Restart();
            var revalidation = await PdfAConverter.ValidateAsync(pdfA1Path, PdfALevel.PdfA1b);
            sw.Stop();

            if (revalidation.IsCompliant)
            {
                ConsoleHelper.WriteSuccess($"Converted document IS compliant with PDF/A-1b ({sw.ElapsedMilliseconds}ms)");
            }
            else
            {
                ConsoleHelper.WriteError($"Converted document still has {revalidation.Violations.Count} violations");
                foreach (var violation in revalidation.Violations.Take(5))
                {
                    ConsoleHelper.WriteInfo($"  {violation}");
                }
            }

            // 5. Convert to PDF/A-2b
            ConsoleHelper.WriteInfo("\n--- Convert to PDF/A-2b ---");
            sw.Restart();
            var pdfA2Path = Path.Combine(outputDir, "pdfa-converted-2b.pdf");
            await PdfAConverter.ConvertAsync(samplePath, pdfA2Path, PdfALevel.PdfA2b);
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Converted to PDF/A-2b in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Output: {pdfA2Path}");

            // 6. Validate PDF/A-2b
            ConsoleHelper.WriteInfo("\n--- Validate PDF/A-2b ---");
            sw.Restart();
            var val2b = await PdfAConverter.ValidateAsync(pdfA2Path, PdfALevel.PdfA2b);
            sw.Stop();

            if (val2b.IsCompliant)
            {
                ConsoleHelper.WriteSuccess($"Document IS compliant with PDF/A-2b ({sw.ElapsedMilliseconds}ms)");
            }
            else
            {
                ConsoleHelper.WriteError($"Document has {val2b.Violations.Count} compliance violations");
            }

            // 7. Convert to PDF/A-3b
            ConsoleHelper.WriteInfo("\n--- Convert to PDF/A-3b ---");
            sw.Restart();
            var pdfA3Path = Path.Combine(outputDir, "pdfa-converted-3b.pdf");
            await PdfAConverter.ConvertAsync(samplePath, pdfA3Path, PdfALevel.PdfA3b);
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Converted to PDF/A-3b in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Output: {pdfA3Path}");

            // Summary
            ConsoleHelper.WriteInfo("\n--- PDF/A Compliance Summary ---");
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine();
            System.Console.WriteLine($"    {"Standard",-15} {"Output File",-35} {"Status"}");
            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.WriteLine($"    {"".PadLeft(60, '─')}");

            var standards = new[]
            {
                ("PDF/A-1b", "pdfa-converted-1b.pdf"),
                ("PDF/A-2b", "pdfa-converted-2b.pdf"),
                ("PDF/A-3b", "pdfa-converted-3b.pdf")
            };

            foreach (var (standard, file) in standards)
            {
                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.Write($"    {standard,-15} {file,-35}");
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine(" Converted");
            }
            System.Console.ResetColor();

            ConsoleHelper.WriteSuccess("\nAll PDF/A compliance demos completed successfully!");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"PDF/A Compliance demo failed: {ex.Message}");
        }
    }
}
