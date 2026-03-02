# Getting Started with Exis.PdfEditor Demos

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Windows 10/11 (required for WPF and WinForms projects)
- Visual Studio 2022 17.8+ or JetBrains Rider 2024.1+ (optional)

## Quick Start

### 1. Clone the Repository

```bash
git clone https://github.com/AkisExis/ExisPdfDemoApps.git
cd ExisPdfDemoApps
```

### 2. Restore & Build

```bash
dotnet restore ExisPdfDemoApps.slnx
dotnet build ExisPdfDemoApps.slnx
```

### 3. Run the Console Demo

```bash
dotnet run --project src/Exis.PdfEditor.Demo.Console
```

You'll see an interactive menu with 11 demos covering every major feature.

### 4. Run the WPF Demo

```bash
dotnet run --project src/Exis.PdfEditor.Demo.Wpf
```

A modern tabbed UI opens with interactive controls for Find & Replace, Merge & Split, PDF Builder, Inspector, and Redaction.

### 5. Run the WinForms Demo

```bash
dotnet run --project src/Exis.PdfEditor.Demo.WinForms
```

A professional tabbed interface with drag-and-drop support, batch processing, form filling, and a document dashboard.

## Project Structure

| Project | Type | Target | Description |
|---------|------|--------|-------------|
| `Exis.PdfEditor.Demo.Console` | Console | `net8.0` | Interactive CLI demos — 11 feature showcases |
| `Exis.PdfEditor.Demo.Wpf` | WPF | `net8.0-windows` | Modern MVVM desktop app with tabbed UI |
| `Exis.PdfEditor.Demo.WinForms` | WinForms | `net8.0-windows` | Classic desktop app with drag-and-drop |

## Licensing

Exis.PdfEditor operates in evaluation mode by default. All demos call `ExisLicense.Initialize()` at startup. To use your own license key:

```csharp
ExisLicense.Initialize("YOUR-LICENSE-KEY");
```

Evaluation mode is fully functional — output PDFs will include a small watermark.

## Output Files

The Console demos create an `output/` directory in the working directory with all generated PDFs. This directory is excluded from Git via `.gitignore`.

## Troubleshooting

### Build fails with "platform not supported"

The WPF and WinForms projects target `net8.0-windows` and require Windows. To build only the Console project on Linux/macOS:

```bash
dotnet build src/Exis.PdfEditor.Demo.Console
```

### NuGet restore fails

Ensure you have internet access and the NuGet.org feed is configured:

```bash
dotnet nuget list source
```

If NuGet.org is missing:

```bash
dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org
```

### "ExisLicense not found" error

Make sure the `Exis.PdfEditor` package restored successfully. Check the project's `obj/` folder for package references.

## Next Steps

- Read the [API Quick Reference](API_QUICK_REFERENCE.md) for a summary of key classes and methods
- Check the [Feature Comparison](FEATURE_COMPARISON.md) to see what each demo covers
- Explore the source code — every demo is self-contained and well-commented
