# Feature Comparison Across Demo Projects

This document shows which Exis.PdfEditor features are demonstrated in each project.

## Feature Matrix

| Feature | Console | WPF | WinForms |
|---------|:-------:|:---:|:--------:|
| **Find & Replace** | | | |
| Simple text replacement | X | X | |
| Batch (dictionary) replacement | X | X | |
| Regex replacement | X | X | |
| Page-range scoped replacement | X | X | |
| TextFitting (ShrinkToFit) | X | X | |
| **Merge & Split** | | | |
| Merge multiple PDFs | X | X | X |
| Split into page chunks | X | X | |
| Extract specific pages | X | X | |
| Split to individual files | X | | |
| **PDF Builder** | | | |
| Create from scratch | X | X | |
| Letterhead template | X | X | |
| Multi-page documents | X | X | |
| Styled text and shapes | X | X | |
| **Report Generator** | | | |
| Title page | X | | |
| Table of contents | X | | |
| Data tables with styling | X | | |
| Multi-column layout | X | | |
| Headers and footers | X | | |
| **Form Operations** | | | |
| Read form fields | X | | X |
| Fill form fields | X | | X |
| Flatten forms | X | | X |
| **Redaction** | | | |
| Text-based redaction | X | | X |
| Regex-based redaction | X | | X |
| Area-based redaction | X | X | |
| Replace-with text | X | X | |
| **Optimization** | | | |
| Default profile | X | | X |
| Aggressive profile | X | | X |
| Before/after comparison | X | | X |
| **Text Extraction** | | | |
| Simple extraction | X | X | X |
| Structured extraction | X | X | |
| **PDF/A Compliance** | | | |
| Validate compliance | X | | X |
| Convert to PDF/A | X | | X |
| **Digital Signatures** | | | |
| Create self-signed cert | X | | |
| Sign documents | X | | |
| Verify signatures | X | | |
| Batch verification | X | | |
| **Document Inspection** | | | |
| Page count & file size | | X | X |
| Metadata (author, title) | | X | X |
| Encryption detection | | X | X |
| **UI Features** | | | |
| Drag-and-drop | | | X |
| Async with progress | | X | X |
| MVVM architecture | | X | |
| Tabbed interface | | X | X |
| Batch processing | | | X |
| Document dashboard | | | X |

## Recommended Starting Point

| You want to... | Start with |
|----------------|------------|
| See all features quickly | **Console** — run the menu and try each demo |
| Integrate into a desktop app | **WPF** — clean MVVM patterns you can copy |
| Build a batch processing tool | **WinForms** — drag-and-drop + batch processor |
| Generate reports programmatically | **Console** — Report Generator demo (#4) |
| See the full enterprise workflow | **Console** — Full Pipeline demo (#11) |

## Lines of Code by Project

| Project | Approximate LOC | Files |
|---------|-----------------|-------|
| Console | ~2,000 | 13 |
| WPF | ~1,800 | 18 |
| WinForms | ~1,500 | 10 |
| **Total** | **~5,300** | **41** |
