# Exis.PdfEditor WPF Demo

A modern WPF desktop application demonstrating Exis.PdfEditor integration with the MVVM pattern.

## Running

```bash
dotnet run --project src/Exis.PdfEditor.Demo.Wpf
```

> Requires Windows 10/11 with .NET 8.0 SDK.

## Tabs

| Tab | Features |
|-----|----------|
| **Find & Replace** | Open a PDF, search for text, replace with options (regex, ShrinkToFit) |
| **Merge & Split** | Manage file list, merge or split PDFs with page controls |
| **PDF Builder** | Choose a template, enter content, generate a styled PDF |
| **Inspector** | Load a PDF to view metadata, page count, and extracted text |
| **Redaction** | Apply text or area-based redaction with visual controls |

## Architecture

- **MVVM**: Each tab has a dedicated ViewModel and View
- **RelayCommand**: Lightweight ICommand implementation with async support
- **Async/Await**: All PDF operations run on background threads for UI responsiveness
- **Resource Dictionary**: Centralized styling with Exis brand colors

## UI Design

- Exis blue (#2563EB) accent color throughout
- Modern flat styling with rounded controls
- Status bar with operation feedback
- File dialogs for PDF input/output
