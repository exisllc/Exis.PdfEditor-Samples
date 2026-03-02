# Exis.PdfEditor Demo Suite

[![NuGet](https://img.shields.io/nuget/v/Exis.PdfEditor?style=flat-square&color=2563EB)](https://www.nuget.org/packages/Exis.PdfEditor)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![License: MIT](https://img.shields.io/badge/License-MIT-green?style=flat-square)](LICENSE)

A comprehensive demo solution showcasing the full capabilities of [**Exis.PdfEditor**](https://www.nuget.org/packages/Exis.PdfEditor) — a high-performance .NET library for creating, editing, and processing PDF documents.

---

## What's Inside

| Project | Type | Description |
|---------|------|-------------|
| [**Console Demo**](src/Exis.PdfEditor.Demo.Console/) | CLI | 11 interactive demos covering every feature |
| [**WPF Demo**](src/Exis.PdfEditor.Demo.Wpf/) | Desktop | Modern MVVM app with tabbed interface |
| [**WinForms Demo**](src/Exis.PdfEditor.Demo.WinForms/) | Desktop | Drag-and-drop UI with batch processing |

## Features Demonstrated

| Feature | Description |
|---------|-------------|
| **Find & Replace** | Single, batch, regex replacements with TextFitting and page ranges |
| **Merge & Split** | Combine PDFs, split by page count, extract specific pages |
| **PDF Builder** | Create documents from scratch with text, shapes, and images |
| **Report Generator** | Auto-layout multi-page business reports with tables and branding |
| **Form Filling** | Read, fill, and flatten PDF form fields |
| **Redaction** | Text, regex, and area-based redaction with replacement options |
| **Optimization** | Reduce file size with default or aggressive compression profiles |
| **Text Extraction** | Simple and structured text extraction with layout preservation |
| **PDF/A Compliance** | Validate and convert documents to PDF/A-1b, 2b, or 3b |
| **Digital Signatures** | Sign with certificates, verify single and batch signatures |

## Quick Start

```bash
# Clone
git clone https://github.com/AkisExis/ExisPdfDemoApps.git
cd ExisPdfDemoApps

# Build all projects
dotnet build ExisPdfDemoApps.slnx

# Run the Console demo
dotnet run --project src/Exis.PdfEditor.Demo.Console
```

## Code Examples

### Find & Replace with Template Filling

```csharp
using var editor = await PdfEditor.OpenAsync("template.pdf");

editor.FindReplace(new Dictionary<string, string>
{
    ["{{NAME}}"]    = "Acme Corporation",
    ["{{DATE}}"]    = DateTime.Now.ToString("yyyy-MM-dd"),
    ["{{AMOUNT}}"]  = "$1,250.00"
});

await editor.SaveAsync("filled.pdf");
```

### Build a PDF from Scratch

```csharp
using var builder = new PdfBuilder();

builder.AddPage()
    .SetFont("Helvetica", 24)
    .DrawText("Hello, World!", 50, 750)
    .SetFillColor(37, 99, 235)
    .DrawRectangle(50, 700, 200, 2);

await builder.SaveAsync("created.pdf");
```

### Merge Multiple PDFs

```csharp
await PdfEditor.MergeAsync(
    new[] { "report.pdf", "appendix.pdf", "cover.pdf" },
    "combined.pdf"
);
```

### Optimize File Size

```csharp
using var editor = await PdfEditor.OpenAsync("large-file.pdf");
await editor.OptimizeAsync(OptimizationProfile.Aggressive);
await editor.SaveAsync("optimized.pdf");
// Typical reduction: 40-70%
```

### Sign a Document

```csharp
using var editor = await PdfEditor.OpenAsync("contract.pdf");
await editor.SignAsync(new SignatureOptions
{
    CertificatePath = "cert.pfx",
    CertificatePassword = "password",
    Reason = "Approval"
});
await editor.SaveAsync("signed.pdf");
```

## Requirements

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Windows 10/11 for WPF and WinForms projects
- Console project runs on Windows, Linux, and macOS

## Project Structure

```
ExisPdfDemoApps/
├── ExisPdfDemoApps.slnx
├── src/
│   ├── Exis.PdfEditor.Demo.Console/    # 11 interactive CLI demos
│   ├── Exis.PdfEditor.Demo.Wpf/        # Modern MVVM desktop app
│   └── Exis.PdfEditor.Demo.WinForms/   # Classic desktop with drag-drop
├── docs/
│   ├── GETTING_STARTED.md
│   ├── API_QUICK_REFERENCE.md
│   └── FEATURE_COMPARISON.md
└── samples/                             # Place your test PDFs here
```

## Documentation

- [Getting Started](docs/GETTING_STARTED.md) — Setup, build, and run instructions
- [API Quick Reference](docs/API_QUICK_REFERENCE.md) — Key classes and methods at a glance
- [Feature Comparison](docs/FEATURE_COMPARISON.md) — What each demo project covers

## License

This demo solution is licensed under the [MIT License](LICENSE).

**Exis.PdfEditor** is a commercial library. Evaluation mode is fully functional with a small watermark on output. Visit the [NuGet page](https://www.nuget.org/packages/Exis.PdfEditor) for licensing options.

---

Built with [Exis.PdfEditor](https://www.nuget.org/packages/Exis.PdfEditor) v1.5.2
