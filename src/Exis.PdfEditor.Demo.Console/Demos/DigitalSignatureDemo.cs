namespace Exis.PdfEditor.Demo.Console.Demos;

using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Exis.PdfEditor;

internal static class DigitalSignatureDemo
{
    public static async Task RunAsync()
    {
        ConsoleHelper.WriteHeader("Digital Signatures Demo");
        var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
        Directory.CreateDirectory(outputDir);

        try
        {
            // 1. Create a self-signed certificate for demo purposes
            ConsoleHelper.WriteInfo("Creating self-signed certificate for demo...");
            var sw = Stopwatch.StartNew();
            var certPath = Path.Combine(outputDir, "demo-signing-cert.pfx");
            var certPassword = "ExisDemo2024!";

            using (var rsa = RSA.Create(2048))
            {
                var request = new CertificateRequest(
                    "CN=Exis Demo Signer, O=Exis Technologies, C=US",
                    rsa,
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1);

                request.CertificateExtensions.Add(
                    new X509KeyUsageExtension(
                        X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.NonRepudiation,
                        critical: true));

                request.CertificateExtensions.Add(
                    new X509EnhancedKeyUsageExtension(
                        new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") },
                        critical: false));

                using var cert = request.CreateSelfSigned(
                    DateTimeOffset.UtcNow.AddDays(-1),
                    DateTimeOffset.UtcNow.AddYears(2));

                var pfxBytes = cert.Export(X509ContentType.Pfx, certPassword);
                await File.WriteAllBytesAsync(certPath, pfxBytes);
            }
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Self-signed certificate created in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Certificate: {certPath}");
            ConsoleHelper.WriteInfo("  Subject:  CN=Exis Demo Signer, O=Exis Technologies, C=US");
            ConsoleHelper.WriteInfo("  Validity: 2 years");
            ConsoleHelper.WriteInfo("  Key:      RSA 2048-bit");

            // 2. Create a sample PDF to sign
            ConsoleHelper.WriteInfo("\nCreating sample PDF for signing...");
            var samplePath = Path.Combine(outputDir, "signature-sample.pdf");

            var builder = PdfBuilder.Create();
            builder.AddPage(page =>
            {
                page.Size(PdfPageSize.A4);

                page.AddRectangle(0, 790, 595, 52, fill: true,
                    fillRed: 0.145, fillGreen: 0.388, fillBlue: 0.921);
                page.AddText("Software License Agreement", 140, 810, 20,
                    o => o.Bold().Color(1.0, 1.0, 1.0));

                page.AddText("AGREEMENT", 50, 750, 12, o => o.Bold());

                var legalText = new[]
                {
                    "This Software License Agreement (\"Agreement\") is entered into as of the date",
                    $"of digital signature ({DateTime.Now:MMMM dd, yyyy}), by and between:",
                    "",
                    "LICENSOR:  Exis Technologies, Inc.",
                    "           123 Innovation Blvd, Suite 400, San Francisco, CA 94105",
                    "",
                    "LICENSEE:  Acme Corporation",
                    "           456 Business Ave, New York, NY 10001",
                    "",
                    "1. GRANT OF LICENSE",
                    "Subject to the terms of this Agreement, Licensor hereby grants Licensee a",
                    "non-exclusive, non-transferable license to use Exis.PdfEditor (the \"Software\")",
                    "for internal business purposes.",
                    "",
                    "2. TERM",
                    "This Agreement shall be effective for a period of one (1) year from the date",
                    "of execution, with automatic renewal for successive one-year terms.",
                    "",
                    "3. FEES",
                    "Licensee shall pay an annual license fee of $24,000 (Twenty-Four Thousand",
                    "US Dollars) payable within 30 days of invoice.",
                    "",
                    "4. INTELLECTUAL PROPERTY",
                    "All intellectual property rights in the Software remain with Licensor. This",
                    "Agreement does not transfer any ownership rights to Licensee.",
                    "",
                    "5. GOVERNING LAW",
                    "This Agreement shall be governed by the laws of the State of California.",
                    "",
                    "",
                    "IN WITNESS WHEREOF, the parties have executed this Agreement as of the date",
                    "first written above."
                };

                double y = 725;
                foreach (var line in legalText)
                {
                    page.AddText(line, 50, y, 11);
                    y -= 16;
                }

                // Signature blocks
                y -= 20;
                page.AddText("_______________________________", 50, y, 10);
                page.AddText("_______________________________", 320, y, 10);
                y -= 15;
                page.AddText("Authorized Signatory — Licensor", 50, y, 10);
                page.AddText("Authorized Signatory — Licensee", 320, y, 10);
                y -= 15;
                page.AddText("Exis Technologies, Inc.", 50, y, 10);
                page.AddText("Acme Corporation", 320, y, 10);
            });
            builder.BuildToFile(samplePath);
            ConsoleHelper.WriteSuccess($"Sample PDF created: {samplePath}");

            // 3. Sign the PDF
            ConsoleHelper.WriteInfo("\n--- Signing PDF ---");
            sw.Restart();
            var signedPath = Path.Combine(outputDir, "signed-document.pdf");

            var signingCert = new X509Certificate2(certPath, certPassword);
            await PdfSigner.SignAsync(samplePath, signedPath, new PdfSignOptions
            {
                Certificate = signingCert,
                Reason = "Contract Execution — License Agreement",
                Location = "San Francisco, CA",
                SignerName = "Sarah Chen, VP of Sales"
            });
            sw.Stop();

            ConsoleHelper.WriteSuccess($"PDF signed in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Output: {signedPath}");
            ConsoleHelper.WriteInfo("  Reason:    Contract Execution — License Agreement");
            ConsoleHelper.WriteInfo("  Location:  San Francisco, CA");
            ConsoleHelper.WriteInfo("  Signer:    Sarah Chen, VP of Sales");

            // 4. Verify the signature
            ConsoleHelper.WriteInfo("\n--- Verifying Signature ---");
            sw.Restart();
            var signatureInfo = await PdfSigner.VerifyAsync(signedPath);
            sw.Stop();

            ConsoleHelper.WriteSuccess($"Signature verification completed in {sw.ElapsedMilliseconds}ms");

            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine($"\n    Signature Details:");
            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.WriteLine($"    ─────────────────────────────────────");
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine($"    Signer:       {signatureInfo.SignerName}");
            System.Console.WriteLine($"    Reason:       {signatureInfo.Reason}");
            System.Console.WriteLine($"    Location:     {signatureInfo.Location}");
            System.Console.WriteLine($"    Date:         {signatureInfo.SignDate:yyyy-MM-dd HH:mm:ss}");
            System.Console.WriteLine($"    Certificate:  {signatureInfo.CertificateSubject}");

            System.Console.Write($"    Integrity:    ");
            if (signatureInfo.IsValid)
            {
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine("VALID — Signature is intact and verified");
            }
            else
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine($"INVALID — {signatureInfo.ValidationMessage}");
            }

            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.Write($"    Certificate:  ");
            if (!string.IsNullOrEmpty(signatureInfo.CertificateIssuer))
            {
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine($"Issuer: {signatureInfo.CertificateIssuer}");
            }
            else
            {
                System.Console.ForegroundColor = ConsoleColor.Yellow;
                System.Console.WriteLine("Self-signed (expected for demo)");
            }
            System.Console.ResetColor();

            // 5. Verify all signatures
            ConsoleHelper.WriteInfo("\n--- Verify All Signatures ---");
            sw.Restart();

            // Create a second signed PDF for batch testing
            var signedPath2 = Path.Combine(outputDir, "signed-document-2.pdf");
            await PdfSigner.SignAsync(samplePath, signedPath2, new PdfSignOptions
            {
                Certificate = signingCert,
                Reason = "Review and Approval",
                Location = "New York, NY",
                SignerName = "Robert Williams, Director of Engineering"
            });

            // Verify all signatures on each document
            var allSigs1 = await PdfSigner.VerifyAllAsync(signedPath);
            var allSigs2 = await PdfSigner.VerifyAllAsync(signedPath2);
            sw.Stop();

            ConsoleHelper.WriteSuccess($"Batch verification completed in {sw.ElapsedMilliseconds}ms");

            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.Write($"    {Path.GetFileName(signedPath),-35}");
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine($"{allSigs1.Count} signature(s) found");

            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.Write($"    {Path.GetFileName(signedPath2),-35}");
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine($"{allSigs2.Count} signature(s) found");
            System.Console.ResetColor();

            signingCert.Dispose();

            ConsoleHelper.WriteSuccess("\nAll digital signature demos completed successfully!");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Digital Signatures demo failed: {ex.Message}");
        }
    }
}
