# Exis.PdfEditor WinForms Demo

A classic Windows Forms desktop application with drag-and-drop support, batch processing, and a document dashboard.

## Running

```bash
dotnet run --project src/Exis.PdfEditor.Demo.WinForms
```

> Requires Windows 10/11 with .NET 8.0 SDK.

## Features

| Area | Description |
|------|-------------|
| **Quick Actions** | Drag-and-drop panel for instant PDF processing |
| **Batch Processor** | Queue multiple PDFs for bulk merge, optimize, convert, or extract |
| **Form Filler** | Load PDFs with form fields, edit values in a grid, save or flatten |
| **Dashboard** | Browse PDF collections, view metadata, preview extracted text |

## Architecture

- **Async/await** with progress reporting for responsive batch operations
- **Custom DragDropPanel** control with visual feedback (dashed border idle, solid on hover)
- **DataGridView** for form field editing
- **TreeView** for folder-based document browsing
- **StatusStrip** with real-time progress bars

## UI Design

- Exis blue (#2563EB) accent color
- Segoe UI font family
- Professional padding and anchoring
- Menu bar with keyboard shortcuts
