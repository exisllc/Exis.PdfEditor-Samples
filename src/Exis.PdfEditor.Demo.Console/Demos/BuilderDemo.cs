namespace Exis.PdfEditor.Demo.Console.Demos;

using System.Diagnostics;
using Exis.PdfEditor;

internal static class BuilderDemo
{
    // Exis brand colors (0.0–1.0 scale)
    private const double ExisBlueR = 0.145, ExisBlueG = 0.388, ExisBlueB = 0.921;  // #2563EB (37,99,235)
    private const double WhiteR = 1.0, WhiteG = 1.0, WhiteB = 1.0;
    private const double BlackR = 0.0, BlackG = 0.0, BlackB = 0.0;
    private const double LightGrayR = 0.941, LightGrayG = 0.941, LightGrayB = 0.941;
    private const double MedGrayR = 0.392, MedGrayG = 0.392, MedGrayB = 0.392;
    private const double DarkGrayR = 0.196, DarkGrayG = 0.196, DarkGrayB = 0.196;

    public static async Task RunAsync()
    {
        ConsoleHelper.WriteHeader("PDF Builder Demo");
        var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
        Directory.CreateDirectory(outputDir);

        try
        {
            // 1. Professional Letterhead
            ConsoleHelper.WriteInfo("Building professional letterhead...");
            var sw = Stopwatch.StartNew();
            var letterheadPath = Path.Combine(outputDir, "builder-letterhead.pdf");

            var builder = PdfBuilder.Create();
            builder.WithMetadata(m => m.Title("Professional Letterhead").Author("Exis Technologies"));

            builder.AddPage(page =>
            {
                page.Size(PdfPageSize.A4);

                // Company header bar
                page.AddRectangle(0, 780, 595, 62, fill: true,
                    fillRed: ExisBlueR, fillGreen: ExisBlueG, fillBlue: ExisBlueB);

                // Company name in header
                page.AddText("EXIS TECHNOLOGIES", 50, 800, 22, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                page.AddText("Innovation Through Excellence", 50, 785, 10, o => o.Color(WhiteR, WhiteG, WhiteB));

                // Logo placeholder
                page.AddRectangle(460, 790, 40, 40, fill: false,
                    strokeRed: WhiteR, strokeGreen: WhiteG, strokeBlue: WhiteB);
                page.AddText("[LOGO]", 468, 805, 8, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));

                // Contact info bar
                page.AddRectangle(0, 760, 595, 20, fill: true,
                    fillRed: LightGrayR, fillGreen: LightGrayG, fillBlue: LightGrayB);
                page.AddText("123 Innovation Blvd, Suite 400, San Francisco, CA 94105  |  (555) 123-4567  |  info@existech.com",
                    50, 766, 8, o => o.Color(MedGrayR, MedGrayG, MedGrayB));

                // Date and recipient
                page.AddText(DateTime.Now.ToString("MMMM dd, yyyy"), 50, 720, 11);
                page.AddText("John Smith", 50, 690, 11);
                page.AddText("Director of Engineering", 50, 676, 11);
                page.AddText("Acme Corporation", 50, 662, 11);
                page.AddText("456 Business Ave", 50, 648, 11);
                page.AddText("New York, NY 10001", 50, 634, 11);

                // Salutation and body
                page.AddText("Dear Mr. Smith,", 50, 600, 11);

                var bodyLines = new[]
                {
                    "Thank you for your interest in Exis Technologies' enterprise solutions. We are pleased to",
                    "present our comprehensive PDF processing platform, designed to meet the demanding",
                    "requirements of modern document management workflows.",
                    "",
                    "Our solution offers industry-leading performance in PDF generation, manipulation, and",
                    "analysis. With support for PDF/A compliance, digital signatures, and advanced text",
                    "extraction capabilities, Exis.PdfEditor provides everything your team needs.",
                    "",
                    "We look forward to discussing how we can support your organization's goals.",
                    "",
                    "Best regards,"
                };

                double y = 570;
                foreach (var line in bodyLines)
                {
                    page.AddText(line, 50, y, 11);
                    y -= 18;
                }

                // Signature area
                page.AddText("Sarah Chen", 50, y - 20, 11, o => o.Bold());
                page.AddText("VP of Sales, Exis Technologies", 50, y - 35, 10);

                // Footer
                page.AddRectangle(0, 0, 595, 30, fill: true,
                    fillRed: ExisBlueR, fillGreen: ExisBlueG, fillBlue: ExisBlueB);
                page.AddText("www.existech.com  |  Confidential", 220, 12, 8,
                    o => o.Color(WhiteR, WhiteG, WhiteB));
            });

            builder.BuildToFile(letterheadPath);
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Letterhead created in {sw.ElapsedMilliseconds}ms: {letterheadPath}");

            // 2. Multi-page document with headers/footers
            ConsoleHelper.WriteInfo("\nBuilding multi-page document with headers and footers...");
            sw.Restart();
            var multiPagePath = Path.Combine(outputDir, "builder-multipage.pdf");

            var chapters = new[]
            {
                ("Introduction", new[] {
                    "This document demonstrates the multi-page capabilities of PdfBuilder.",
                    "Each page automatically receives consistent headers and footers.",
                    "The PdfBuilder API makes it simple to create professional documents",
                    "with minimal code, while maintaining full control over layout.",
                    "",
                    "Key features demonstrated in this document:",
                    "  - Consistent header and footer styling across pages",
                    "  - Automatic page numbering",
                    "  - Chapter-based document structure",
                    "  - Professional typography and spacing"
                }),
                ("Architecture Overview", new[] {
                    "Exis.PdfEditor is built on a high-performance rendering engine that",
                    "processes documents with exceptional speed and accuracy.",
                    "",
                    "The architecture consists of three main components:",
                    "",
                    "  1. Document Parser - Reads and interprets PDF structure",
                    "  2. Content Engine - Manages text, images, and vector graphics",
                    "  3. Output Writer  - Produces optimized PDF output",
                    "",
                    "Each component is designed for thread safety, enabling parallel",
                    "processing of multiple documents in server environments."
                }),
                ("Performance Benchmarks", new[] {
                    "Our internal benchmarks demonstrate industry-leading performance:",
                    "",
                    "  Document Generation:     1,000 pages/second",
                    "  Text Extraction:         5,000 pages/second",
                    "  Merge Operations:        500 documents/second",
                    "  Find & Replace:          10,000 replacements/second",
                    "",
                    "All benchmarks performed on standard hardware (8-core, 16GB RAM).",
                    "Actual performance may vary based on document complexity and",
                    "system configuration."
                }),
                ("Getting Started", new[] {
                    "Install Exis.PdfEditor via NuGet:",
                    "",
                    "  dotnet add package Exis.PdfEditor",
                    "",
                    "Initialize the license in your application startup:",
                    "",
                    "  ExisLicense.Initialize();",
                    "",
                    "Create your first PDF:",
                    "",
                    "  var builder = PdfBuilder.Create();",
                    "  builder.AddPage(page => {",
                    "      page.AddText(\"Hello, PDF!\", 50, 750, 12);",
                    "  });",
                    "  builder.BuildToFile(\"output.pdf\");"
                })
            };

            var multiBuilder = PdfBuilder.Create();
            multiBuilder.WithMetadata(m => m.Title("Exis.PdfEditor Technical Guide").Author("Exis Technologies"));

            int totalPages = chapters.Length;
            for (int i = 0; i < chapters.Length; i++)
            {
                var (title, content) = chapters[i];
                int pageIndex = i; // capture for closure

                multiBuilder.AddPage(page =>
                {
                    page.Size(PdfPageSize.A4);

                    // Header
                    page.AddRectangle(0, 815, 595, 27, fill: true,
                        fillRed: ExisBlueR, fillGreen: ExisBlueG, fillBlue: ExisBlueB);
                    page.AddText("Exis.PdfEditor Technical Guide", 50, 822, 10,
                        o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                    page.AddText($"Chapter {pageIndex + 1}", 490, 822, 9,
                        o => o.Color(WhiteR, WhiteG, WhiteB));

                    // Divider line below header
                    page.AddLine(50, 800, 545, 800, strokeWidth: 1,
                        red: ExisBlueR, green: ExisBlueG, blue: ExisBlueB);

                    // Chapter title
                    page.AddText($"Chapter {pageIndex + 1}: {title}", 50, 770, 20,
                        o => o.Bold().Color(ExisBlueR, ExisBlueG, ExisBlueB));

                    // Chapter content
                    double y = 735;
                    foreach (var line in content)
                    {
                        page.AddText(line, 50, y, 11, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                        y -= 18;
                    }

                    // Footer
                    page.AddLine(50, 40, 545, 40, strokeWidth: 0.5,
                        red: MedGrayR, green: MedGrayG, blue: MedGrayB);
                    page.AddText($"Page {pageIndex + 1} of {totalPages}", 270, 25, 8,
                        o => o.Color(MedGrayR, MedGrayG, MedGrayB));
                    page.AddText("© 2024 Exis Technologies. All rights reserved.", 50, 25, 8,
                        o => o.Color(MedGrayR, MedGrayG, MedGrayB));
                });
            }

            multiBuilder.BuildToFile(multiPagePath);
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Multi-page document created in {sw.ElapsedMilliseconds}ms: {multiPagePath}");

            // 3. PdfDocumentBuilder with metadata and structured content
            ConsoleHelper.WriteInfo("\nBuilding structured document with metadata...");
            sw.Restart();
            var structuredPath = Path.Combine(outputDir, "builder-structured.pdf");

            var doc = PdfDocumentBuilder.Create();
            doc.WithMetadata(m => m.Title("Exis.PdfEditor Product Catalog").Author("Exis Technologies"));
            doc.PageSize(PdfPageSize.A4);
            doc.Margins(72, 50, 72, 50);
            doc.Header(h => h.AddText("Exis Technologies — Product Catalog", PdfHorizontalAlignment.Left).AddPageNumber());
            doc.Footer(f => f.AddText("Confidential", PdfHorizontalAlignment.Center).AddLine());

            // Cover section
            doc.AddParagraph("PRODUCT CATALOG", 36, o => o.Bold());
            doc.AddSpacing(10);
            doc.AddParagraph("Exis Technologies — 2024 Edition", 16);
            doc.AddSpacing(5);
            doc.AddParagraph("Complete guide to our PDF processing solutions", 12);
            doc.AddPageBreak();

            // Product pages
            var productItems = new[]
            {
                ("Exis.PdfEditor", "Core PDF manipulation library with find & replace, merge, split, and more."),
                ("Exis.PdfBuilder", "Create PDFs from scratch with a fluent API for text, images, and shapes."),
                ("Exis.PdfForms", "Fill, flatten, and create interactive PDF forms programmatically."),
                ("Exis.PdfSecurity", "Digital signatures, encryption, redaction, and PDF/A compliance.")
            };

            foreach (var (name, description) in productItems)
            {
                doc.AddParagraph(name, 24, o => o.Bold());
                doc.AddSpacing(5);
                doc.AddParagraph(description, 11);
                doc.AddSpacing(10);
                doc.AddHorizontalRule(1, ExisBlueR, ExisBlueG, ExisBlueB);
                doc.AddSpacing(5);

                doc.AddParagraph("Key Features", 14, o => o.Bold());
                doc.AddSpacing(5);

                var features = new[]
                {
                    "High-performance processing engine",
                    "Cross-platform .NET 8+ support",
                    "Comprehensive API with async/await",
                    "Extensive documentation and samples",
                    "Enterprise-grade reliability",
                    "Active development and support"
                };

                doc.AddTable(t =>
                {
                    t.Columns(new[] { 5.0 });
                    t.AlternatingRowBackground(0.95, 0.95, 0.97);
                    foreach (var feature in features)
                    {
                        t.AddRow(r => r.AddCell($"  {feature}"));
                    }
                });

                doc.AddPageBreak();
            }

            doc.BuildToFile(structuredPath);
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Structured document created in {sw.ElapsedMilliseconds}ms: {structuredPath}");

            ConsoleHelper.WriteSuccess("\nAll builder demos completed successfully!");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Builder demo failed: {ex.Message}");
        }
    }
}
