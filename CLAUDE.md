# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

Three demo applications showcasing the **Exis.PdfEditor** NuGet library (a commercial .NET PDF library). The Console, WPF, and WinForms apps each re-implement the same set of PDF features in their own UI idiom. There is no shared/common project — the only shared dependency is the `Exis.PdfEditor` package itself. There are no automated tests; verification is done by running the apps.

## Build & run

```bash
# Build the whole solution (slnx is the XML solution format; needs a recent .NET 8 SDK)
dotnet build ExisPdfDemoApps.slnx

# Run an app
dotnet run --project src/Exis.PdfEditor.Demo.Console
dotnet run --project src/Exis.PdfEditor.Demo.Wpf        # Windows only
dotnet run --project src/Exis.PdfEditor.Demo.WinForms   # Windows only
```

- **Console** targets `net8.0` (cross-platform). **WPF** and **WinForms** target `net8.0-windows10.0.19041` (Windows-only; the Win10 SDK level is required by `Exis.PdfOcr.Windows`, which uses `Windows.Data.Pdf` WinRT APIs).
- All three reference `Exis.PdfEditor` via `PackageReference` (currently **3.8.1**) — pinned identically in each `.csproj`. When bumping the library version, update all three.
- **OCR packages** (`Exis.PdfOcr` 1.0.5 core + `Exis.PdfOcr.Tesseract` 1.0.5 engine, both cross-platform) are referenced by all three apps. `Exis.PdfOcr.Windows` (the `net8.0-windows10.0.19041`-only page rasterizer) is referenced by **WPF and WinForms only** — it's deliberately omitted from Console to keep it cross-platform, so Console OCR has no built-in rasterizer (would need a custom `IPdfRasterizer`). OCR API: `await PdfOcr.MakeSearchableAsync(in, out, new OcrOptions { Languages = ["eng"] })`; depends on `ExisLicense.Initialize()` like the rest of the library.
- Console output PDFs are written to an `output/` directory under the current working directory; each demo generates its own sample PDFs first, so no external input files are needed.

## Licensing — required before any PDF operation

Every app calls `ExisLicense.Initialize()` once at startup (`using Exis.PdfEditor.Licensing;`) before touching the library. Without it, PDF operations fail. The entry points are:
- Console: top of `Program.Main`
- WPF: `App.OnStartup`
- WinForms: `Program.Main` (after `ApplicationConfiguration.Initialize()`)

The no-arg call runs in evaluation mode (watermarked output). `ExisLicense.Initialize("KEY")` activates a license.

## API surface — trust the code, not the docs

⚠️ **The `docs/` folder and `README.md` describe an older/aspirational fluent API (e.g. `await PdfEditor.OpenAsync(...).FindReplace(...)`, `new PdfBuilder()`) that does NOT match the actual 3.7.0 package used here.** The real API used throughout `src/` is built around **static helper classes** taking input/output paths:

- `PdfFindReplace.ExecuteAsync(input, output, search, replace, PdfFindReplaceOptions)` — returns a result with `.TotalReplacements`. Supports `FindReplacePair[]` for batch.
- `PdfBuilder.Create()` → `.AddPage(page => { page.Size(PdfPageSize.A4); page.AddText(...); })` → `.BuildToFile(path)`
- `PdfMerger.MergeToFileAsync(paths, outputPath)`
- `PdfInspector.InspectAsync(path)` — returns `.PageCount`, `.Author`, `.Title`, etc.
- Options/enums: `PdfFindReplaceOptions` (`UseRegex`, `PageRange`, `TextFitting`, `ReplacementTextColor`, `ReplacementHighlightColor`), `TextFittingMode`, `PdfColor.Red`/`.Yellow`/etc.

When adding code, copy the actual call signatures from existing files in `src/.../Demos/` or the ViewModels/Forms — do not rely on the API reference docs.

## Per-app architecture

### Console (`src/Exis.PdfEditor.Demo.Console`)
- `Program.cs` is a menu loop: a `switch` on the typed number dispatches to one `static async Task RunAsync()` per feature, each in its own class under `Demos/`.
- Each demo is self-contained: it builds its own sample PDF, wraps operations in `Stopwatch`, and reports via `ConsoleHelper` (colored `WriteHeader`/`WriteInfo`/`WriteSuccess`/`WriteError`). Failures are caught per-demo so one failure doesn't crash the menu.
- **To add a demo:** create `Demos/{Feature}Demo.cs` with `RunAsync()`, then add a `case` in `Program.cs`'s switch *and* a `PrintMenuItem` line in `PrintMenu()` (the case number and menu number must match).

### WinForms (`src/Exis.PdfEditor.Demo.WinForms`)
- `MainForm` hosts each feature as a child `Form` from `Forms/` embedded as a **non-top-level docked control** inside a tab (`EmbedSubForms()` sets `TopLevel = false; FormBorderStyle = None; Dock = Fill` then `.Show()`). Tabs are switched programmatically from menu items / dashboard buttons (`WireUpEvents()`).
- `Controls/DragDropPanel` is a custom control raising a `FilesDropped` event consumed by `MainForm`.
- Each form is split into `{Feature}Form.cs` (logic, an `InitializeComponent()` + `WireUpEvents()` constructor) and `{Feature}Form.Designer.cs` (generated layout).
- Long-running PDF work runs in `Task.Run`; UI updates marshal back via the `InvokeRequired`/`Invoke` pattern (see `SetStatus`/`ShowProgress`).
- **To add a feature:** create the Form pair under `Forms/`, then add an `_embedded...` field and an `EmbedSubForms()` block in `MainForm`, plus a tab/menu wiring entry.

### WPF (`src/Exis.PdfEditor.Demo.Wpf`)
- MVVM. `MainWindow.xaml` is a `TabControl` where each `TabItem` hosts a `views:{Feature}View`. The `MainWindow` `DataContext` is `MainViewModel` (only status/progress state).
- Each `View`'s code-behind constructor sets its own `DataContext = new {Feature}ViewModel()` — ViewModels are not injected; the View owns its VM.
- ViewModels implement `INotifyPropertyChanged` and expose `ICommand`s via `Helpers/RelayCommand`. Commands use `CanExecute` predicates (e.g. disabled while `IsProcessing`). Long work runs in `Task.Run`.
- `Resources/Styles.xaml` holds shared brushes/styles (`PrimaryBrush`, `SurfaceBrush`, `BoolToVisibility` converter, etc.) referenced as `StaticResource`.
- **To add a feature:** create `Views/{Feature}View.xaml`(+`.xaml.cs` wiring its VM) and `ViewModels/{Feature}ViewModel.cs`, then add a `<TabItem>` referencing the view in `MainWindow.xaml`.

## Feature parity

The same features are intentionally spread (unevenly) across the three apps — not every feature exists in every app. Before claiming a feature is "missing," check whether the app was meant to have it. Console has the widest coverage (incl. Report Generator, PDF/A, Digital Signatures, Optimization, Text Extraction, Full Pipeline); WinForms and WPF cover a UI-friendly subset. When asked to "add feature X everywhere," replicate it per each app's pattern above.

## Version-string note

Hardcoded display version strings are scattered and inconsistent (`README.md`, the Console banner, `MainWindow.xaml`) and lag the actual package version. They're cosmetic — the authoritative library version is the `PackageReference` in the `.csproj` files.
