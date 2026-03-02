namespace Exis.PdfEditor.Demo.Console.Demos;

using System.Diagnostics;
using Exis.PdfEditor;

internal static class RedactionDemo
{
    public static async Task RunAsync()
    {
        ConsoleHelper.WriteHeader("Redaction Demo");
        var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
        Directory.CreateDirectory(outputDir);

        try
        {
            // 1. Create a sample PDF with sensitive data
            ConsoleHelper.WriteInfo("Creating sample PDF with sensitive data...");
            var samplePath = Path.Combine(outputDir, "redaction-sample.pdf");

            var builder = PdfBuilder.Create();
            builder.AddPage(page =>
            {
                page.Size(PdfPageSize.A4);

                page.AddRectangle(0, 790, 595, 52, fill: true,
                    fillRed: 0.145, fillGreen: 0.388, fillBlue: 0.921);
                page.AddText("Customer Record — Confidential", 140, 810, 18,
                    o => o.Bold().Color(1.0, 1.0, 1.0));

                page.AddText("Personal Information", 50, 760, 12, o => o.Bold());

                double y = 735;
                var lines = new[]
                {
                    "Full Name: John Michael Smith",
                    "Social Security Number: 123-45-6789",
                    "Date of Birth: March 15, 1985",
                    "Email: john.smith@personalmail.com",
                    "Phone: (555) 123-4567",
                    "Address: 742 Evergreen Terrace, Springfield, IL 62704",
                    "",
                    "Financial Information",
                    "Bank Account: 9876543210",
                    "Routing Number: 021000021",
                    "Credit Card: 4532-1234-5678-9012",
                    "Annual Income: $125,000",
                    "",
                    "Medical Information",
                    "Primary Physician: Dr. Sarah Johnson",
                    "Medical Record #: MRN-2024-88421",
                    "Diagnosis: Patient presented with mild hypertension.",
                    "Medication: Lisinopril 10mg daily, Aspirin 81mg daily.",
                    "",
                    "Employment History",
                    "Current Employer: Acme Corporation (since 2018)",
                    "Previous: Beta Industries (2015-2018)",
                    "Manager: Robert Williams, r.williams@acme.com"
                };

                foreach (var line in lines)
                {
                    bool isHeader = line == "Financial Information" || line == "Medical Information" || line == "Employment History";
                    if (isHeader)
                        page.AddText(line, 50, y, 12, o => o.Bold());
                    else
                        page.AddText(line, 50, y, 11);
                    y -= 20;
                }
            });
            builder.BuildToFile(samplePath);
            ConsoleHelper.WriteSuccess($"Sample PDF with sensitive data created: {samplePath}");

            // 2. Text-based redaction (names)
            ConsoleHelper.WriteInfo("\n--- Text-Based Redaction ---");
            var sw = Stopwatch.StartNew();
            var textRedactedPath = Path.Combine(outputDir, "redacted-text.pdf");
            var textRedactions = new List<PdfRedaction>
            {
                new PdfRedaction { Text = "John Michael Smith", CaseSensitive = true },
                new PdfRedaction { Text = "742 Evergreen Terrace, Springfield, IL 62704", CaseSensitive = true },
                new PdfRedaction { Text = "Dr. Sarah Johnson", CaseSensitive = true },
                new PdfRedaction { Text = "Robert Williams", CaseSensitive = true }
            };
            var textResult = await PdfRedactor.RedactAsync(samplePath, textRedactedPath, textRedactions);
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Text-based redaction: {textResult.RedactionsApplied} items redacted in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Output: {textRedactedPath}");

            // 3. Regex-based redaction (SSN, email, phone, credit card patterns)
            ConsoleHelper.WriteInfo("\n--- Regex-Based Redaction ---");
            sw.Restart();
            var regexRedactedPath = Path.Combine(outputDir, "redacted-regex.pdf");
            var regexRedactions = new List<PdfRedaction>
            {
                // SSN pattern
                new PdfRedaction { Text = @"\d{3}-\d{2}-\d{4}", IsRegex = true, ReplaceWith = "[SSN REDACTED]" },
                // Email pattern
                new PdfRedaction { Text = @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b", IsRegex = true, ReplaceWith = "[EMAIL REDACTED]" },
                // Phone pattern
                new PdfRedaction { Text = @"\(\d{3}\)\s?\d{3}-\d{4}", IsRegex = true, ReplaceWith = "[PHONE REDACTED]" },
                // Credit card pattern
                new PdfRedaction { Text = @"\d{4}-\d{4}-\d{4}-\d{4}", IsRegex = true, ReplaceWith = "[CC# REDACTED]" }
            };
            var regexResult = await PdfRedactor.RedactAsync(samplePath, regexRedactedPath, regexRedactions);
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Regex-based redaction: {regexResult.RedactionsApplied} items redacted in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteInfo($"  Pages modified: {regexResult.PagesModified}");
            ConsoleHelper.WriteSuccess($"Output: {regexRedactedPath}");

            // 4. Replace-with text redaction (financial data)
            ConsoleHelper.WriteInfo("\n--- Replace-With-Text Redaction ---");
            sw.Restart();
            var replaceRedactedPath = Path.Combine(outputDir, "redacted-replaced.pdf");
            var replaceRedactions = new List<PdfRedaction>
            {
                new PdfRedaction { Text = "$125,000", CaseSensitive = true, ReplaceWith = "[INCOME CLASSIFIED]" },
                new PdfRedaction { Text = "9876543210", CaseSensitive = true, ReplaceWith = "[ACCOUNT PROTECTED]" },
                new PdfRedaction { Text = "021000021", CaseSensitive = true, ReplaceWith = "[ROUTING PROTECTED]" }
            };
            var replaceResult = await PdfRedactor.RedactAsync(samplePath, replaceRedactedPath, replaceRedactions);
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Replace-with-text redaction: {replaceResult.RedactionsApplied} items in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Output: {replaceRedactedPath}");

            // 5. Area-based redaction (coordinate-based)
            ConsoleHelper.WriteInfo("\n--- Area-Based Redaction ---");
            sw.Restart();
            var areaRedactedPath = Path.Combine(outputDir, "redacted-area.pdf");
            var areaRedactions = new List<PdfRedaction>
            {
                // Redact the Medical Information section by area
                new PdfRedaction { Area = new PdfRect(40, 340, 520, 100), PageNumber = 1 },
                // Redact the Financial section by area
                new PdfRedaction { Area = new PdfRect(40, 450, 520, 90), PageNumber = 1 }
            };
            var areaResult = await PdfRedactor.RedactAsync(samplePath, areaRedactedPath, areaRedactions);
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Area-based redaction: {areaResult.RedactionsApplied} regions redacted in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Output: {areaRedactedPath}");

            ConsoleHelper.WriteSuccess("\nAll redaction demos completed successfully!");
            ConsoleHelper.WriteInfo("Note: Redactions are permanent — the underlying text is irrecoverably removed from the PDF.");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Redaction demo failed: {ex.Message}");
        }
    }
}
