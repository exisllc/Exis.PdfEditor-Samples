# Exis.PdfEditor Console Demo

Interactive command-line demo showcasing all Exis.PdfEditor features through 11 self-contained demonstrations.

## Running

```bash
dotnet run --project src/Exis.PdfEditor.Demo.Console
```

## Demo Menu

| # | Demo | What It Shows |
|---|------|---------------|
| 1 | **Find & Replace** | Single, batch, regex replacements; TextFitting; page ranges |
| 2 | **Merge & Split** | Merge 3 PDFs, split by pages, extract pages, SplitToFiles |
| 3 | **PDF Builder** | Create letterhead, multi-page docs, structured documents |
| 4 | **Report Generator** | Crown jewel — auto-layout business report with tables and branding |
| 5 | **Form Filling** | Read fields, fill values, flatten forms |
| 6 | **Redaction** | Text, regex, replace-with, area-based redaction |
| 7 | **Optimization** | Default and aggressive profiles with before/after sizes |
| 8 | **Text Extraction** | Simple and structured extraction with formatted output |
| 9 | **PDF/A Compliance** | Validate, convert, and re-validate PDF/A conformance |
| 10 | **Digital Signatures** | Self-signed cert creation, signing, and verification |
| 11 | **Full Pipeline** | Enterprise workflow combining all features end-to-end |

## Key Design Decisions

- **Self-contained**: Every demo generates its own sample PDFs via `PdfBuilder` — no external files needed
- **Timed**: Every operation is wrapped with `Stopwatch` to show performance
- **Error-handled**: Try-catch blocks with user-friendly colored messages
- **Cross-platform**: Targets `net8.0` — runs on Windows, Linux, and macOS

## Output

Generated PDFs are saved to an `output/` directory in the current working directory. Each demo creates files with descriptive names (e.g., `find-replace-batch.pdf`, `report-generator.pdf`).
