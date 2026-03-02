namespace Exis.PdfEditor.Demo.Console.Demos;

using System.Diagnostics;
using Exis.PdfEditor;

internal static class MergeSplitDemo
{
    public static async Task RunAsync()
    {
        ConsoleHelper.WriteHeader("Merge & Split Demo");
        var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
        Directory.CreateDirectory(outputDir);

        try
        {
            // 1. Create sample PDFs for merging
            ConsoleHelper.WriteInfo("Creating sample PDFs for merge/split operations...");

            var samplePaths = new string[3];
            for (int i = 0; i < 3; i++)
            {
                samplePaths[i] = Path.Combine(outputDir, $"merge-sample-{i + 1}.pdf");
                var builder = PdfBuilder.Create();
                builder.AddPage(page =>
                {
                    page.Size(PdfPageSize.A4);
                    page.AddText($"Document {i + 1}", 200, 700, 24, o => o.Bold());
                    for (int line = 0; line < 5; line++)
                    {
                        page.AddText($"This is content line {line + 1} of document {i + 1}.", 50, 650 - (line * 25), 12);
                    }
                });

                if (i == 1)
                {
                    builder.AddPage(page =>
                    {
                        page.Size(PdfPageSize.A4);
                        page.AddText("Document 2 - Page 2", 200, 700, 14);
                        page.AddText("This document has multiple pages to demonstrate splitting.", 50, 660, 11);
                    });

                    builder.AddPage(page =>
                    {
                        page.Size(PdfPageSize.A4);
                        page.AddText("Document 2 - Page 3", 200, 700, 14);
                        page.AddText("Third page with additional content for split testing.", 50, 660, 11);
                    });
                }

                builder.BuildToFile(samplePaths[i]);
            }
            ConsoleHelper.WriteSuccess("Created 3 sample PDFs (document 2 has 3 pages)");

            // 2. Merge all PDFs into one
            ConsoleHelper.WriteInfo("\n--- Merge PDFs ---");
            var sw = Stopwatch.StartNew();
            var mergedPath = Path.Combine(outputDir, "merged-result.pdf");
            await PdfMerger.MergeToFileAsync(samplePaths, mergedPath);
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Merged 3 documents into one in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Output: {mergedPath}");

            // 3. Merge using byte array output
            ConsoleHelper.WriteInfo("\n--- Merge to Byte Array ---");
            sw.Restart();
            byte[] mergedBytes = await PdfMerger.MergeAsync(samplePaths);
            var mergedPath2 = Path.Combine(outputDir, "merged-static.pdf");
            await File.WriteAllBytesAsync(mergedPath2, mergedBytes);
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Merge to byte array completed in {sw.ElapsedMilliseconds}ms ({mergedBytes.Length:N0} bytes)");
            ConsoleHelper.WriteSuccess($"Output: {mergedPath2}");

            // 4. Split merged PDF into individual pages
            ConsoleHelper.WriteInfo("\n--- Split Into Individual Pages ---");
            sw.Restart();
            var splitDir = Path.Combine(outputDir, "split-pages");
            Directory.CreateDirectory(splitDir);
            await PdfSplitter.SplitToFilesAsync(mergedPath, Path.Combine(splitDir, "page-{0}.pdf"));
            sw.Stop();

            var splitFiles = Directory.GetFiles(splitDir, "page-*.pdf");
            ConsoleHelper.WriteSuccess($"Split into {splitFiles.Length} individual files in {sw.ElapsedMilliseconds}ms");
            foreach (var file in splitFiles)
            {
                ConsoleHelper.WriteInfo($"  Created: {Path.GetFileName(file)}");
            }

            // 5. Split to byte arrays
            ConsoleHelper.WriteInfo("\n--- Split to Byte Arrays ---");
            sw.Restart();
            List<byte[]> pageByteArrays = await PdfSplitter.SplitAsync(mergedPath);
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Split into {pageByteArrays.Count} page byte arrays in {sw.ElapsedMilliseconds}ms");
            for (int i = 0; i < pageByteArrays.Count; i++)
            {
                ConsoleHelper.WriteInfo($"  Page {i + 1}: {pageByteArrays[i].Length:N0} bytes");
            }

            // 6. Extract specific pages
            ConsoleHelper.WriteInfo("\n--- Extract Specific Pages ---");
            sw.Restart();
            var extractedPath = Path.Combine(outputDir, "extracted-pages-1-3-5.pdf");
            byte[] extractedBytes = await PdfSplitter.ExtractPagesAsync(mergedPath, new[] { 1, 3, 5 });
            await File.WriteAllBytesAsync(extractedPath, extractedBytes);
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Extracted pages 1, 3, 5 in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Output: {extractedPath}");

            ConsoleHelper.WriteSuccess("\nAll merge & split demos completed successfully!");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Merge & Split demo failed: {ex.Message}");
        }
    }
}
