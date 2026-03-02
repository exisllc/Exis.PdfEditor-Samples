namespace Exis.PdfEditor.Demo.Console.Demos;

using System.Diagnostics;
using Exis.PdfEditor;

internal static class TextExtractionDemo
{
    public static async Task RunAsync()
    {
        ConsoleHelper.WriteHeader("Text Extraction Demo");
        var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
        Directory.CreateDirectory(outputDir);

        try
        {
            // 1. Create a sample PDF with various text content
            ConsoleHelper.WriteInfo("Creating sample PDF with diverse text content...");
            var samplePath = Path.Combine(outputDir, "extraction-sample.pdf");

            var builder = PdfBuilder.Create();

            // Page 1: Article-style content
            builder.AddPage(page =>
            {
                page.Size(PdfPageSize.A4);

                page.AddRectangle(0, 800, 595, 42, fill: true,
                    fillRed: 0.145, fillGreen: 0.388, fillBlue: 0.921);
                page.AddText("The Future of PDF Processing in .NET", 80, 815, 18,
                    o => o.Bold().Color(1.0, 1.0, 1.0));

                page.AddText("Published: January 15, 2024  |  Author: Technical Team  |  Category: Engineering",
                    80, 780, 10, o => o.Italic().Color(0.392, 0.392, 0.392));

                page.AddText("Abstract", 50, 750, 13, o => o.Bold());

                var abstractLines = new[]
                {
                    "Modern enterprise applications increasingly require sophisticated PDF processing",
                    "capabilities. This paper examines the current state of PDF manipulation in the .NET",
                    "ecosystem and presents Exis.PdfEditor as a comprehensive solution for document",
                    "generation, manipulation, and analysis workloads."
                };
                double y = 730;
                foreach (var line in abstractLines)
                {
                    page.AddText(line, 50, y, 11);
                    y -= 16;
                }

                y -= 15;
                page.AddText("1. Introduction", 50, y, 13, o => o.Bold());
                y -= 20;
                var introLines = new[]
                {
                    "The Portable Document Format (PDF) remains the de facto standard for document",
                    "interchange in business environments. With over 2.5 trillion PDF documents in",
                    "circulation worldwide, the ability to programmatically create, modify, and extract",
                    "information from PDFs is critical for modern software systems.",
                    "",
                    "Exis.PdfEditor provides a comprehensive, high-performance API for .NET developers,",
                    "supporting operations ranging from simple text replacement to complex multi-page",
                    "report generation with charts, tables, and professional formatting."
                };
                foreach (var line in introLines)
                {
                    page.AddText(line, 50, y, 11);
                    y -= 16;
                }

                y -= 15;
                page.AddText("2. Key Capabilities", 50, y, 13, o => o.Bold());
                y -= 20;
                var capabilities = new[]
                {
                    "  2.1  Find & Replace — Template-based and regex-powered text replacement",
                    "  2.2  Merge & Split  — Combine multiple PDFs or extract page ranges",
                    "  2.3  Form Filling   — Programmatic form population and flattening",
                    "  2.4  Redaction      — Secure, permanent removal of sensitive content",
                    "  2.5  Optimization   — Reduce file sizes while preserving quality",
                    "  2.6  PDF/A          — Archival compliance validation and conversion",
                    "  2.7  Signatures     — Digital signing with certificate support"
                };
                foreach (var line in capabilities)
                {
                    page.AddText(line, 50, y, 11);
                    y -= 16;
                }
            });

            // Page 2: Table content and conclusion
            builder.AddPage(page =>
            {
                page.Size(PdfPageSize.A4);

                page.AddText("3. Performance Benchmarks", 50, 780, 13, o => o.Bold());
                page.AddText("The following table summarizes benchmark results across key operations:", 50, 755, 11);

                // Table header
                double tableY = 725;
                page.AddRectangle(50, tableY, 495, 20, fill: true,
                    fillRed: 0.145, fillGreen: 0.388, fillBlue: 0.921);
                page.AddText("Operation", 60, tableY + 5, 10, o => o.Bold().Color(1.0, 1.0, 1.0));
                page.AddText("Documents/sec", 200, tableY + 5, 10, o => o.Bold().Color(1.0, 1.0, 1.0));
                page.AddText("Pages/sec", 330, tableY + 5, 10, o => o.Bold().Color(1.0, 1.0, 1.0));
                page.AddText("Memory (MB)", 450, tableY + 5, 10, o => o.Bold().Color(1.0, 1.0, 1.0));

                var benchmarks = new[]
                {
                    ("PDF Generation", "250", "1,000", "45"),
                    ("Text Extraction", "500", "5,000", "32"),
                    ("Find & Replace", "300", "3,000", "38"),
                    ("Merge (10-page)", "180", "1,800", "52"),
                    ("Optimization", "150", "1,500", "64"),
                    ("Form Filling", "400", "4,000", "28"),
                    ("Digital Signing", "200", "2,000", "40")
                };

                tableY -= 20;
                bool alt = false;
                foreach (var (op, docs, pages, mem) in benchmarks)
                {
                    if (alt)
                    {
                        page.AddRectangle(50, tableY, 495, 18, fill: true,
                            fillRed: 0.953, fillGreen: 0.957, fillBlue: 0.965);
                    }
                    page.AddText(op, 60, tableY + 4, 10);
                    page.AddText(docs, 220, tableY + 4, 10);
                    page.AddText(pages, 350, tableY + 4, 10);
                    page.AddText(mem, 470, tableY + 4, 10);
                    tableY -= 18;
                    alt = !alt;
                }

                // Conclusion
                tableY -= 30;
                page.AddText("4. Conclusion", 50, tableY, 13, o => o.Bold());
                tableY -= 20;
                var conclusionLines = new[]
                {
                    "Exis.PdfEditor delivers enterprise-grade PDF processing with a developer-friendly",
                    "API, exceptional performance, and comprehensive feature coverage. Whether building",
                    "document generation pipelines, processing form submissions, or ensuring regulatory",
                    "compliance through PDF/A conversion, the library provides the tools modern .NET",
                    "applications require.",
                    "",
                    "For more information, visit https://www.nuget.org/packages/Exis.PdfEditor"
                };
                foreach (var line in conclusionLines)
                {
                    page.AddText(line, 50, tableY, 11);
                    tableY -= 16;
                }
            });

            builder.BuildToFile(samplePath);
            ConsoleHelper.WriteSuccess($"Sample PDF created: {samplePath}");

            // 2. Simple text extraction
            ConsoleHelper.WriteInfo("\n--- Simple Text Extraction ---");
            var sw = Stopwatch.StartNew();
            var extractResult = PdfTextExtractor.ExtractText(samplePath);
            string allText = extractResult.FullText;
            sw.Stop();

            ConsoleHelper.WriteSuccess($"Extracted {allText.Length:N0} characters from {extractResult.Pages.Count} pages in {sw.ElapsedMilliseconds}ms");

            // Show preview
            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.WriteLine("\n    -- Text Preview (first 500 characters) --");
            System.Console.ForegroundColor = ConsoleColor.White;
            var preview = allText.Length > 500 ? allText[..500] + "..." : allText;
            foreach (var line in preview.Split('\n'))
            {
                System.Console.WriteLine($"    {line}");
            }
            System.Console.ResetColor();

            // Save to file
            var textOutputPath = Path.Combine(outputDir, "extracted-text-simple.txt");
            await File.WriteAllTextAsync(textOutputPath, allText);
            ConsoleHelper.WriteSuccess($"Full text saved to: {textOutputPath}");

            // 3. Per-page structured extraction
            ConsoleHelper.WriteInfo("\n--- Structured Per-Page Extraction ---");
            sw.Restart();
            var structuredOutput = new System.Text.StringBuilder();

            foreach (var page in extractResult.Pages)
            {
                var wordCount = page.Text.Split(new[] { ' ', '\n', '\r', '\t' },
                    StringSplitOptions.RemoveEmptyEntries).Length;
                var lineCount = page.Text.Split('\n').Length;

                structuredOutput.AppendLine($"=== Page {page.PageNumber} ===");
                structuredOutput.AppendLine($"Words: {wordCount} | Lines: {lineCount} | Characters: {page.Text.Length}");
                structuredOutput.AppendLine(new string('-', 60));
                structuredOutput.AppendLine(page.Text);
                structuredOutput.AppendLine();

                ConsoleHelper.WriteSuccess($"Page {page.PageNumber}: {wordCount} words, {lineCount} lines, {page.Text.Length:N0} characters");
            }

            sw.Stop();
            var structuredPath = Path.Combine(outputDir, "extracted-text-structured.txt");
            await File.WriteAllTextAsync(structuredPath, structuredOutput.ToString());
            ConsoleHelper.WriteSuccess($"Structured extraction completed in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Output: {structuredPath}");

            // 4. Structured extraction with position data
            ConsoleHelper.WriteInfo("\n--- Structured Extraction with Position Data ---");
            sw.Restart();
            var structured = PdfTextExtractor.ExtractStructured(samplePath);
            sw.Stop();

            ConsoleHelper.WriteSuccess($"Structured extraction completed in {sw.ElapsedMilliseconds}ms");
            foreach (var sPage in structured.Pages)
            {
                ConsoleHelper.WriteInfo($"  Page {sPage.PageNumber}: {sPage.TextBlocks.Count} text blocks");

                // Show first few text blocks with position info
                int shown = 0;
                foreach (var block in sPage.TextBlocks)
                {
                    if (shown >= 3) break;
                    System.Console.ForegroundColor = ConsoleColor.DarkGray;
                    System.Console.Write("      ");
                    System.Console.ForegroundColor = ConsoleColor.Cyan;
                    System.Console.Write($"[{block.X:F0},{block.Y:F0}]");
                    System.Console.ForegroundColor = ConsoleColor.White;
                    var blockPreview = block.Text.Length > 50 ? block.Text[..50] + "..." : block.Text;
                    System.Console.Write($" {blockPreview}");
                    System.Console.ForegroundColor = ConsoleColor.DarkGray;
                    System.Console.WriteLine($"  ({block.FontName} {block.FontSize}pt)");
                    System.Console.ResetColor();
                    shown++;
                }

                if (sPage.TextBlocks.Count > 3)
                {
                    System.Console.ForegroundColor = ConsoleColor.DarkGray;
                    System.Console.WriteLine($"      ... and {sPage.TextBlocks.Count - 3} more text blocks");
                    System.Console.ResetColor();
                }
            }

            ConsoleHelper.WriteSuccess("\nAll text extraction demos completed successfully!");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Text Extraction demo failed: {ex.Message}");
        }
    }
}
