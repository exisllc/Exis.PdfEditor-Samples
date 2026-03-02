namespace Exis.PdfEditor.Demo.Console.Demos;

using System.Diagnostics;
using Exis.PdfEditor;

internal static class FindReplaceDemo
{
    public static async Task RunAsync()
    {
        ConsoleHelper.WriteHeader("Find & Replace Demo");
        var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
        Directory.CreateDirectory(outputDir);

        try
        {
            // 1. Create a sample PDF with placeholder text
            ConsoleHelper.WriteInfo("Creating sample PDF with placeholder text...");
            var samplePath = Path.Combine(outputDir, "find-replace-sample.pdf");

            var builder = PdfBuilder.Create();
            builder.AddPage(page =>
            {
                page.Size(PdfPageSize.A4);
                page.AddText("Invoice #{{INVOICE_NUM}}", 50, 750, 12);
                page.AddText("Customer: {{CUSTOMER_NAME}}", 50, 730, 12);
                page.AddText("Date: {{DATE}}", 50, 710, 12);
                page.AddText("Amount: ${{AMOUNT}}", 50, 690, 12);
                page.AddText("Thank you, {{CUSTOMER_NAME}}, for your business!", 50, 650, 12);
                page.AddText("Contact support@example.com for questions.", 50, 630, 12);
            });
            builder.AddPage(page =>
            {
                page.Size(PdfPageSize.A4);
                page.AddText("Page 2 - Additional details for {{CUSTOMER_NAME}}", 50, 750, 12);
                page.AddText("Reference: REF-{{INVOICE_NUM}}-2024", 50, 730, 12);
            });
            builder.BuildToFile(samplePath);
            ConsoleHelper.WriteSuccess($"Sample PDF created: {samplePath}");

            // 2. Simple single find & replace
            ConsoleHelper.WriteInfo("\n--- Single Find & Replace ---");
            var sw = Stopwatch.StartNew();
            var singleOutputPath = Path.Combine(outputDir, "find-replace-single.pdf");
            var result = await PdfFindReplace.ExecuteAsync(samplePath, singleOutputPath,
                "{{INVOICE_NUM}}", "INV-2024-0042");
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Replaced {result.TotalReplacements} occurrences in {sw.ElapsedMilliseconds}ms");

            // 3. Batch find & replace with FindReplacePair array
            ConsoleHelper.WriteInfo("\n--- Batch Find & Replace ---");
            sw.Restart();
            var batchOutputPath = Path.Combine(outputDir, "find-replace-batch.pdf");
            var pairs = new[]
            {
                new FindReplacePair { Search = "{{INVOICE_NUM}}", Replace = "INV-2024-0042" },
                new FindReplacePair { Search = "{{CUSTOMER_NAME}}", Replace = "Acme Corporation" },
                new FindReplacePair { Search = "{{DATE}}", Replace = DateTime.Now.ToString("yyyy-MM-dd") },
                new FindReplacePair { Search = "{{AMOUNT}}", Replace = "1,250.00" }
            };
            var batchResult = await PdfFindReplace.ExecuteAsync(samplePath, batchOutputPath, pairs);
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Replaced {batchResult.TotalReplacements} total occurrences in {sw.ElapsedMilliseconds}ms");

            // 4. Regex-based replacement
            ConsoleHelper.WriteInfo("\n--- Regex Find & Replace ---");
            sw.Restart();
            var regexOutputPath = Path.Combine(outputDir, "find-replace-regex.pdf");
            var regexResult = await PdfFindReplace.ExecuteAsync(samplePath, regexOutputPath,
                @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b", "[REDACTED-EMAIL]",
                new PdfFindReplaceOptions { UseRegex = true });
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Regex replaced {regexResult.TotalReplacements} occurrences in {sw.ElapsedMilliseconds}ms");

            // 5. Page-range scoped replacement
            ConsoleHelper.WriteInfo("\n--- Page-Range Scoped Replace ---");
            sw.Restart();
            var pageRangeOutputPath = Path.Combine(outputDir, "find-replace-page-range.pdf");
            var pageRangeResult = await PdfFindReplace.ExecuteAsync(samplePath, pageRangeOutputPath,
                "{{CUSTOMER_NAME}}", "Page-1-Only Corp",
                new PdfFindReplaceOptions { PageRange = new[] { 1 } });
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Page-scoped replaced {pageRangeResult.TotalReplacements} occurrences in {sw.ElapsedMilliseconds}ms");

            // 6. TextFitting mode
            ConsoleHelper.WriteInfo("\n--- TextFitting Replace ---");
            sw.Restart();
            var textFittingOutputPath = Path.Combine(outputDir, "find-replace-textfitting.pdf");
            var textFittingResult = await PdfFindReplace.ExecuteAsync(samplePath, textFittingOutputPath,
                "{{CUSTOMER_NAME}}", "Very Long International Corporation Holdings Ltd.",
                new PdfFindReplaceOptions { TextFitting = TextFittingMode.Adaptive });
            sw.Stop();
            ConsoleHelper.WriteSuccess($"TextFitting replaced {textFittingResult.TotalReplacements} occurrences in {sw.ElapsedMilliseconds}ms");

            ConsoleHelper.WriteSuccess("\nAll find & replace demos completed successfully!");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Find & Replace demo failed: {ex.Message}");
        }
    }
}
