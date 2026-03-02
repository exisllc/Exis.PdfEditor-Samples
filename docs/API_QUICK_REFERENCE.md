# Exis.PdfEditor — API Quick Reference

> This is a condensed reference for the classes and methods demonstrated in this solution. For full API documentation, see the [official docs](https://www.nuget.org/packages/Exis.PdfEditor).

## Licensing

```csharp
using Exis.PdfEditor;

// Initialize with evaluation mode (watermarked output)
ExisLicense.Initialize();

// Initialize with a license key
ExisLicense.Initialize("YOUR-LICENSE-KEY");
```

---

## PdfEditor — Open & Edit Existing PDFs

```csharp
// Open an existing PDF
using var editor = await PdfEditor.OpenAsync("input.pdf");

// Open with password
using var editor = await PdfEditor.OpenAsync("encrypted.pdf", "password");
```

### Find & Replace

```csharp
// Simple replacement — returns count of replacements made
int count = editor.FindReplace("old text", "new text");

// Batch replacement
int count = editor.FindReplace(new Dictionary<string, string>
{
    ["{{PLACEHOLDER}}"] = "Actual Value",
    ["{{DATE}}"] = DateTime.Now.ToShortDateString()
});

// Regex replacement
int count = editor.FindReplaceRegex(@"\b\d{3}-\d{2}-\d{4}\b", "[REDACTED]");

// With options
int count = editor.FindReplace("text", "replacement", new FindReplaceOptions
{
    PageRange = new PageRange(1, 3),
    TextFitting = TextFitting.ShrinkToFit
});
```

### Merge & Split

```csharp
// Merge multiple PDFs
await PdfEditor.MergeAsync(new[] { "a.pdf", "b.pdf", "c.pdf" }, "merged.pdf");

// Split into chunks of N pages
var files = await editor.SplitAsync("output-dir", pagesPerFile: 5);

// Extract specific pages
await editor.ExtractPagesAsync(new[] { 1, 3, 5 }, "extracted.pdf");

// Split into individual page files
var files = await editor.SplitToFilesAsync("output-dir");
```

### Redaction

```csharp
// Redact by text
editor.Redact("confidential text");

// Redact by regex pattern
editor.RedactRegex(@"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}\b");

// Redact with replacement text
editor.Redact("secret", new RedactionOptions { ReplacementText = "[REDACTED]" });

// Area-based redaction (x, y, width, height in points)
editor.RedactArea(100, 700, 200, 20, pageNumber: 1);
```

### Text Extraction

```csharp
// Simple text extraction
string text = editor.ExtractText();

// Structured extraction (preserves layout)
var structured = editor.ExtractStructuredText();

// Extract from specific pages
string text = editor.ExtractText(new PageRange(1, 5));
```

### Form Operations

```csharp
// Read form fields
var fields = editor.GetFormFields();
foreach (var field in fields)
    Console.WriteLine($"{field.Name}: {field.Value} ({field.FieldType})");

// Fill form fields
editor.SetFormField("FirstName", "John");
editor.SetFormField("LastName", "Doe");

// Flatten form (make fields non-editable)
editor.FlattenForm();
```

### Optimization

```csharp
// Default optimization
await editor.OptimizeAsync();

// Aggressive optimization
await editor.OptimizeAsync(OptimizationProfile.Aggressive);
```

### PDF/A Compliance

```csharp
// Validate
var result = editor.ValidatePdfA();
Console.WriteLine($"Compliant: {result.IsCompliant}, Standard: {result.Standard}");

// Convert to PDF/A
await editor.ConvertToPdfAAsync(PdfAStandard.PdfA2b);
```

### Digital Signatures

```csharp
// Sign a PDF
await editor.SignAsync(new SignatureOptions
{
    CertificatePath = "cert.pfx",
    CertificatePassword = "password",
    Reason = "Approval",
    Location = "New York"
});

// Verify signatures
var result = editor.VerifySignature();
Console.WriteLine($"Valid: {result.IsValid}, Signer: {result.SignerName}");

// Verify all signatures
var results = editor.VerifyAll();
```

### Save

```csharp
// Save to new file
await editor.SaveAsync("output.pdf");

// Get metadata
int pages = editor.PageCount;
string? author = editor.Author;
string? title = editor.Title;
```

---

## PdfBuilder — Create PDFs from Scratch

```csharp
using var builder = new PdfBuilder();

// Add a page and draw content
builder.AddPage(PageSize.A4)
    .SetFont("Helvetica", 24)
    .DrawText("Hello, World!", 50, 750)
    .SetFont("Helvetica", 12)
    .DrawText("Built with Exis.PdfEditor", 50, 720)
    .SetFillColor(37, 99, 235)
    .DrawRectangle(50, 600, 200, 30)
    .SetFillColor(255, 255, 255)
    .DrawText("Colored box", 60, 607);

// Add another page
builder.AddPage()
    .SetFont("Helvetica", 12)
    .DrawText("Page 2 content", 50, 750);

// Save
await builder.SaveAsync("created.pdf");
```

### PdfBuilder Methods

| Method | Description |
|--------|-------------|
| `AddPage(PageSize?)` | Add a new page (default: A4) |
| `SetFont(name, size)` | Set current font |
| `DrawText(text, x, y)` | Draw text at coordinates |
| `DrawLine(x1, y1, x2, y2)` | Draw a line |
| `DrawRectangle(x, y, w, h)` | Draw a filled rectangle |
| `SetFillColor(r, g, b)` | Set fill color (RGB 0-255) |
| `SetStrokeColor(r, g, b)` | Set stroke color |
| `DrawImage(path, x, y, w, h)` | Draw an image |
| `SaveAsync(path)` | Save the document |

---

## PdfDocumentBuilder — Structured Documents

```csharp
using var docBuilder = new PdfDocumentBuilder();

docBuilder.Title = "Annual Report";
docBuilder.Author = "Exis Corp";
docBuilder.Subject = "Financial Summary";

docBuilder.AddSection("Introduction")
    .AddParagraph("This report covers...")
    .AddSection("Revenue")
    .AddParagraph("Total revenue was...");

await docBuilder.SaveAsync("report.pdf");
```

---

## Enums & Options

### TextFitting

| Value | Description |
|-------|-------------|
| `None` | No fitting (default) |
| `ShrinkToFit` | Reduce font size to fit original space |
| `Truncate` | Truncate text that exceeds space |

### OptimizationProfile

| Value | Description |
|-------|-------------|
| `Default` | Balanced optimization |
| `Aggressive` | Maximum compression, may reduce image quality |
| `Lossless` | Optimize without quality loss |

### PdfAStandard

| Value | Description |
|-------|-------------|
| `PdfA1b` | PDF/A-1b (basic conformance) |
| `PdfA2b` | PDF/A-2b (recommended) |
| `PdfA3b` | PDF/A-3b (with attachments) |
