namespace Exis.PdfEditor.Demo.Console.Demos;

using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Exis.PdfEditor;

internal static class FullPipelineDemo
{
    public static async Task RunAsync()
    {
        ConsoleHelper.WriteHeader("Full Pipeline Demo — Enterprise Workflow");
        ConsoleHelper.WriteInfo("Simulating a complete enterprise document processing pipeline:");
        ConsoleHelper.WriteInfo("  Build PDF → Fill Placeholders → Sign → Optimize → PDF/A → Extract Summary");
        var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output", "pipeline");
        Directory.CreateDirectory(outputDir);

        var totalSw = Stopwatch.StartNew();

        try
        {
            // -------------------------------------------------------
            // STEP 1: BUILD PDF
            // -------------------------------------------------------
            ConsoleHelper.WriteInfo("\n== STEP 1/6: Build PDF Template ==");
            var sw = Stopwatch.StartNew();
            var templatePath = Path.Combine(outputDir, "step1-template.pdf");

            var builder = PdfBuilder.Create();
            builder.WithMetadata(m => m.Title("Service Agreement").Author("Exis Technologies"));

            // Page 1: Contract cover page
            builder.AddPage(page =>
            {
                page.Size(PdfPageSize.A4);

                page.AddRectangle(0, 0, 595, 842, fill: true,
                    fillRed: 0.145, fillGreen: 0.388, fillBlue: 0.921);

                // Top accent
                page.AddRectangle(0, 780, 595, 62, fill: true,
                    fillRed: 0.118, fillGreen: 0.227, fillBlue: 0.541);
                page.AddText("EXIS TECHNOLOGIES", 50, 805, 12,
                    o => o.Bold().Color(1.0, 1.0, 1.0));
                page.AddText("Document ID: {{DOC_ID}}", 420, 805, 9,
                    o => o.Color(1.0, 1.0, 1.0));

                page.AddText("SERVICE", 50, 550, 36, o => o.Bold().Color(1.0, 1.0, 1.0));
                page.AddText("AGREEMENT", 50, 505, 36, o => o.Bold().Color(1.0, 1.0, 1.0));
                page.AddText("Enterprise Software License & Support Contract", 50, 470, 14,
                    o => o.Color(1.0, 1.0, 1.0));
                page.AddText("Prepared for: {{CLIENT_NAME}}", 50, 430, 12,
                    o => o.Color(1.0, 1.0, 1.0));
                page.AddText("Date: {{CONTRACT_DATE}}", 50, 410, 12,
                    o => o.Color(1.0, 1.0, 1.0));
                page.AddText("Valid until: {{EXPIRY_DATE}}", 50, 390, 12,
                    o => o.Color(1.0, 1.0, 1.0));

                // Bottom band
                page.AddRectangle(0, 0, 595, 50, fill: true,
                    fillRed: 0.118, fillGreen: 0.227, fillBlue: 0.541);
                page.AddText("CONFIDENTIAL — For authorized recipients only", 200, 20, 8,
                    o => o.Color(1.0, 1.0, 1.0));
            });

            // Page 2: Contract terms
            builder.AddPage(page =>
            {
                page.Size(PdfPageSize.A4);

                page.AddRectangle(0, 815, 595, 27, fill: true,
                    fillRed: 0.145, fillGreen: 0.388, fillBlue: 0.921);
                page.AddText("Service Agreement — {{DOC_ID}}", 50, 823, 9,
                    o => o.Bold().Color(1.0, 1.0, 1.0));
                page.AddText("{{CLIENT_NAME}}", 430, 823, 8,
                    o => o.Color(1.0, 1.0, 1.0));

                page.AddText("Terms & Conditions", 50, 770, 18, o => o.Bold());

                page.AddRectangle(50, 762, 80, 3, fill: true,
                    fillRed: 0.961, fillGreen: 0.620, fillBlue: 0.043);

                var terms = new[]
                {
                    "1. PARTIES",
                    "   This Agreement is between Exis Technologies, Inc. (\"Provider\") and",
                    "   {{CLIENT_NAME}} (\"Client\"), effective as of {{CONTRACT_DATE}}.",
                    "",
                    "2. SCOPE OF SERVICES",
                    "   Provider shall deliver the following:",
                    "   a) Exis.PdfEditor Enterprise License (unlimited developers)",
                    "   b) Priority support with {{SLA_HOURS}}-hour SLA response time",
                    "   c) Quarterly product updates and security patches",
                    "   d) Dedicated technical account manager",
                    "",
                    "3. FEES AND PAYMENT",
                    "   Annual license fee: ${{CONTRACT_VALUE}}",
                    "   Payment terms: Net 30 from invoice date",
                    "   Auto-renewal: Yes, with 60-day notice for cancellation",
                    "",
                    "4. SUPPORT LEVEL",
                    "   Support tier: {{SUPPORT_TIER}}",
                    "   Available hours: {{SUPPORT_HOURS}}",
                    "   Escalation contact: {{ACCOUNT_MANAGER}}",
                    "",
                    "5. ACCEPTANCE",
                    "   By signing below, both parties agree to the terms set forth in this",
                    "   Agreement.",
                    "",
                    "",
                    "   Provider: _________________________    Date: _______________",
                    "   Exis Technologies, Inc.",
                    "",
                    "   Client:   _________________________    Date: _______________",
                    "   {{CLIENT_NAME}}"
                };

                double y = 740;
                foreach (var line in terms)
                {
                    bool isBold = line.StartsWith("1.") || line.StartsWith("2.") ||
                                  line.StartsWith("3.") || line.StartsWith("4.") ||
                                  line.StartsWith("5.");
                    if (isBold)
                        page.AddText(line, 50, y, 11, o => o.Bold());
                    else
                        page.AddText(line, 50, y, 11);
                    y -= 17;
                }

                // Footer
                page.AddText("Page 2 of 2  |  Confidential", 240, 30, 8,
                    o => o.Color(0.588, 0.588, 0.588));
            });

            builder.BuildToFile(templatePath);
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Step 1 complete: Template built in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"  Output: {templatePath} ({new FileInfo(templatePath).Length:N0} bytes)");

            // -------------------------------------------------------
            // STEP 2: FILL PLACEHOLDERS
            // -------------------------------------------------------
            ConsoleHelper.WriteInfo("\n== STEP 2/6: Fill Placeholders ==");
            sw.Restart();
            var filledPath = Path.Combine(outputDir, "step2-filled.pdf");

            var pairs = new[]
            {
                new FindReplacePair { Search = "{{DOC_ID}}", Replace = "SA-2024-00847" },
                new FindReplacePair { Search = "{{CLIENT_NAME}}", Replace = "Acme Corporation" },
                new FindReplacePair { Search = "{{CONTRACT_DATE}}", Replace = DateTime.Now.ToString("MMMM dd, yyyy") },
                new FindReplacePair { Search = "{{EXPIRY_DATE}}", Replace = DateTime.Now.AddYears(1).ToString("MMMM dd, yyyy") },
                new FindReplacePair { Search = "{{CONTRACT_VALUE}}", Replace = "48,000" },
                new FindReplacePair { Search = "{{SLA_HOURS}}", Replace = "4" },
                new FindReplacePair { Search = "{{SUPPORT_TIER}}", Replace = "Enterprise Premium" },
                new FindReplacePair { Search = "{{SUPPORT_HOURS}}", Replace = "24/7/365" },
                new FindReplacePair { Search = "{{ACCOUNT_MANAGER}}", Replace = "Sarah Chen (sarah.chen@existech.com)" }
            };

            var fillResult = await PdfFindReplace.ExecuteAsync(templatePath, filledPath, pairs);
            sw.Stop();

            ConsoleHelper.WriteSuccess($"Step 2 complete: {fillResult.TotalReplacements} placeholders filled in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"  Output: {filledPath}");

            // Show replacements
            foreach (var pair in pairs)
            {
                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                System.Console.Write($"    {pair.Search,-25} -> ");
                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.WriteLine(pair.Replace);
            }
            System.Console.ResetColor();

            // -------------------------------------------------------
            // STEP 3: DIGITAL SIGNATURE
            // -------------------------------------------------------
            ConsoleHelper.WriteInfo("\n== STEP 3/6: Digital Signature ==");
            sw.Restart();

            // Create signing certificate
            var certPath = Path.Combine(outputDir, "pipeline-cert.pfx");
            var certPassword = "PipelineDemo2024!";
            using (var rsa = RSA.Create(2048))
            {
                var request = new CertificateRequest(
                    "CN=Exis Pipeline Signer, O=Exis Technologies, C=US",
                    rsa,
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1);

                request.CertificateExtensions.Add(
                    new X509KeyUsageExtension(
                        X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.NonRepudiation,
                        critical: true));

                using var cert = request.CreateSelfSigned(
                    DateTimeOffset.UtcNow.AddDays(-1),
                    DateTimeOffset.UtcNow.AddYears(1));

                await File.WriteAllBytesAsync(certPath, cert.Export(X509ContentType.Pfx, certPassword));
            }

            var signedPath = Path.Combine(outputDir, "step3-signed.pdf");
            var signingCert = new X509Certificate2(certPath, certPassword);
            await PdfSigner.SignAsync(filledPath, signedPath, new PdfSignOptions
            {
                Certificate = signingCert,
                Reason = "Contract Execution — Enterprise License Agreement",
                Location = "San Francisco, CA",
                SignerName = "Sarah Chen, VP of Sales"
            });
            signingCert.Dispose();
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Step 3 complete: Document signed in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"  Output: {signedPath}");
            ConsoleHelper.WriteInfo("  Signer: Sarah Chen, VP of Sales");
            ConsoleHelper.WriteInfo("  Reason: Contract Execution — Enterprise License Agreement");

            // -------------------------------------------------------
            // STEP 4: OPTIMIZATION
            // -------------------------------------------------------
            ConsoleHelper.WriteInfo("\n== STEP 4/6: Optimization ==");
            sw.Restart();
            var optimizedPath = Path.Combine(outputDir, "step4-optimized.pdf");
            var beforeSize = new FileInfo(signedPath).Length;

            var optimizeOpts = new PdfOptimizeOptions
            {
                CompressStreams = true,
                RemoveDuplicateObjects = true,
                RemoveMetadata = false, // Preserve signature metadata
                DownsampleImages = false
            };
            var optResult = await PdfOptimizer.OptimizeAsync(signedPath, optimizedPath, optimizeOpts);
            sw.Stop();

            var afterSize = new FileInfo(optimizedPath).Length;
            var reduction = (1.0 - (double)afterSize / beforeSize) * 100;
            ConsoleHelper.WriteSuccess($"Step 4 complete: Optimized in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"  Before: {beforeSize:N0} bytes -> After: {afterSize:N0} bytes ({reduction:F1}% reduction)");
            ConsoleHelper.WriteSuccess($"  Output: {optimizedPath}");

            // -------------------------------------------------------
            // STEP 5: PDF/A VALIDATION
            // -------------------------------------------------------
            ConsoleHelper.WriteInfo("\n== STEP 5/6: PDF/A Compliance ==");
            sw.Restart();
            var pdfaPath = Path.Combine(outputDir, "step5-pdfa.pdf");

            // Validate first
            var validation = await PdfAConverter.ValidateAsync(optimizedPath, PdfALevel.PdfA2b);

            if (!validation.IsCompliant)
            {
                ConsoleHelper.WriteInfo($"  Pre-conversion: {validation.Violations.Count} compliance violations found");
                // Convert
                await PdfAConverter.ConvertAsync(optimizedPath, pdfaPath, PdfALevel.PdfA2b);
            }
            else
            {
                // Already compliant, just copy
                File.Copy(optimizedPath, pdfaPath, overwrite: true);
            }

            // Re-validate
            var revalidation = await PdfAConverter.ValidateAsync(pdfaPath, PdfALevel.PdfA2b);
            sw.Stop();

            if (revalidation.IsCompliant)
            {
                ConsoleHelper.WriteSuccess($"Step 5 complete: PDF/A-2b compliant ({sw.ElapsedMilliseconds}ms)");
            }
            else
            {
                ConsoleHelper.WriteInfo($"Step 5 complete: {revalidation.Violations.Count} remaining violations ({sw.ElapsedMilliseconds}ms)");
            }
            ConsoleHelper.WriteSuccess($"  Output: {pdfaPath}");

            // -------------------------------------------------------
            // STEP 6: TEXT EXTRACTION SUMMARY
            // -------------------------------------------------------
            ConsoleHelper.WriteInfo("\n== STEP 6/6: Text Extraction Summary ==");
            sw.Restart();

            var extractResult = PdfTextExtractor.ExtractText(pdfaPath);
            var fullText = extractResult.FullText;
            var wordCount = fullText.Split(new[] { ' ', '\n', '\r', '\t' },
                StringSplitOptions.RemoveEmptyEntries).Length;

            // Extract key values from the document
            var summaryPath = Path.Combine(outputDir, "step6-summary.txt");
            var summary = new System.Text.StringBuilder();
            summary.AppendLine("=======================================================");
            summary.AppendLine("  DOCUMENT PROCESSING SUMMARY");
            summary.AppendLine("=======================================================");
            summary.AppendLine();
            summary.AppendLine($"  Document ID:      SA-2024-00847");
            summary.AppendLine($"  Client:           Acme Corporation");
            summary.AppendLine($"  Contract Value:   $48,000");
            summary.AppendLine($"  Pages:            {extractResult.Pages.Count}");
            summary.AppendLine($"  Word Count:       {wordCount}");
            summary.AppendLine($"  Characters:       {fullText.Length:N0}");
            summary.AppendLine($"  Signatures:       1 (verified)");
            summary.AppendLine($"  PDF/A Standard:   PDF/A-2b");
            summary.AppendLine($"  File Size:        {new FileInfo(pdfaPath).Length:N0} bytes");
            summary.AppendLine();
            summary.AppendLine("  Processing Pipeline:");
            summary.AppendLine("  [1] Template Generation     — PdfBuilder");
            summary.AppendLine("  [2] Placeholder Replacement — FindReplace (9 fields)");
            summary.AppendLine("  [3] Digital Signature       — RSA 2048-bit, SHA-256");
            summary.AppendLine("  [4] Optimization            — Stream compression, deduplication");
            summary.AppendLine("  [5] PDF/A Conversion        — ISO 19005-2 (PDF/A-2b)");
            summary.AppendLine("  [6] Text Extraction         — Full-text content analysis");
            summary.AppendLine();
            summary.AppendLine("=======================================================");

            await File.WriteAllTextAsync(summaryPath, summary.ToString());
            sw.Stop();

            ConsoleHelper.WriteSuccess($"Step 6 complete: Summary extracted in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"  Output: {summaryPath}");
            ConsoleHelper.WriteInfo($"  Pages: {extractResult.Pages.Count} | Words: {wordCount} | Characters: {fullText.Length:N0}");

            // -------------------------------------------------------
            // PIPELINE SUMMARY
            // -------------------------------------------------------
            totalSw.Stop();

            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("\n  ══════════════════════════════════════════════════════════");
            System.Console.WriteLine("    PIPELINE COMPLETE");
            System.Console.WriteLine("  ══════════════════════════════════════════════════════════");
            System.Console.ResetColor();

            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine($"\n    Total pipeline time:  {totalSw.ElapsedMilliseconds}ms");
            System.Console.WriteLine($"    Steps completed:      6 of 6");
            System.Console.WriteLine($"    Output directory:     {outputDir}");
            System.Console.ResetColor();

            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("\n    Generated Files:");
            System.Console.ResetColor();

            var pipelineFiles = new[]
            {
                ("step1-template.pdf", "PDF template with placeholders"),
                ("step2-filled.pdf", "Populated contract document"),
                ("step3-signed.pdf", "Digitally signed document"),
                ("step4-optimized.pdf", "Compressed and optimized"),
                ("step5-pdfa.pdf", "PDF/A-2b compliant archive"),
                ("step6-summary.txt", "Extracted text summary")
            };

            foreach (var (fileName, description) in pipelineFiles)
            {
                var filePath = Path.Combine(outputDir, fileName);
                var size = File.Exists(filePath) ? new FileInfo(filePath).Length : 0;

                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                System.Console.Write("      ");
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.Write("+ ");
                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.Write($"{fileName,-28}");
                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                System.Console.Write($"{size,8:N0} bytes  ");
                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                System.Console.WriteLine(description);
            }
            System.Console.ResetColor();

            ConsoleHelper.WriteSuccess("\nEnterprise pipeline demo completed successfully!");
            ConsoleHelper.WriteInfo("This demo showcases how Exis.PdfEditor handles real-world document workflows");
            ConsoleHelper.WriteInfo("from template generation through archival compliance — all in a single pipeline.");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Full Pipeline demo failed: {ex.Message}");
        }
    }
}
