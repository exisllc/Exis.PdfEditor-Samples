namespace Exis.PdfEditor.Demo.Console.Demos;

using System.Diagnostics;
using Exis.PdfEditor;

internal static class ReportGeneratorDemo
{
    // Exis brand colors (0.0–1.0 scale, converted from 0-255)
    private const double ExisBlueR = 0.145, ExisBlueG = 0.388, ExisBlueB = 0.921;       // #2563EB
    private const double DarkBlueR = 0.118, DarkBlueG = 0.227, DarkBlueB = 0.541;       // #1E3A8A
    private const double LightBlueR = 0.859, LightBlueG = 0.918, LightBlueB = 0.996;    // #DBEAFE
    private const double AccentR = 0.961, AccentG = 0.620, AccentB = 0.043;              // #F59E0B
    private const double DarkGrayR = 0.216, DarkGrayG = 0.255, DarkGrayB = 0.318;       // #374151
    private const double MedGrayR = 0.420, MedGrayG = 0.447, MedGrayB = 0.502;          // #6B7280
    private const double LightGrayR = 0.953, LightGrayG = 0.957, LightGrayB = 0.965;    // #F3F4F6
    private const double WhiteR = 1.0, WhiteG = 1.0, WhiteB = 1.0;
    private const double GreenR = 0.086, GreenG = 0.639, GreenB = 0.290;                // #16A34A

    public static async Task RunAsync()
    {
        ConsoleHelper.WriteHeader("Report Generator Demo — Crown Jewel");
        ConsoleHelper.WriteInfo("Generating a professional multi-page business report...");
        var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
        Directory.CreateDirectory(outputDir);

        try
        {
            var sw = Stopwatch.StartNew();
            var reportPath = Path.Combine(outputDir, "report-business-quarterly.pdf");

            var builder = PdfBuilder.Create();
            builder.WithMetadata(m => m.Title("Q4 2024 Quarterly Business Report").Author("Exis Technologies"));

            int totalPages = 8;
            int currentPage = 0;

            // -------------------------------------------------------
            // PAGE 1: TITLE PAGE
            // -------------------------------------------------------
            ConsoleHelper.WriteInfo("  Building title page...");
            currentPage++;
            builder.AddPage(page =>
            {
                page.Size(PdfPageSize.A4);

                // Full-page blue background
                page.AddRectangle(0, 0, 595, 842, fill: true,
                    fillRed: ExisBlueR, fillGreen: ExisBlueG, fillBlue: ExisBlueB);

                // Top accent bar
                page.AddRectangle(0, 800, 595, 42, fill: true,
                    fillRed: AccentR, fillGreen: AccentG, fillBlue: AccentB);

                // Logo area
                page.AddText("EXIS TECHNOLOGIES", 50, 815, 14, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                page.AddText("EST. 2018", 230, 815, 9, o => o.Color(WhiteR, WhiteG, WhiteB));

                // Decorative lines
                page.AddLine(50, 570, 545, 570, strokeWidth: 1, red: WhiteR, green: WhiteG, blue: WhiteB);
                page.AddLine(50, 400, 545, 400, strokeWidth: 1, red: WhiteR, green: WhiteG, blue: WhiteB);

                // Report title
                page.AddText("QUARTERLY", 50, 520, 42, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                page.AddText("BUSINESS", 50, 475, 42, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                page.AddText("REPORT", 50, 430, 42, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));

                // Subtitle
                page.AddText("Q4 2024 — Financial Overview & Strategic Analysis", 50, 375, 16,
                    o => o.Color(LightBlueR, LightBlueG, LightBlueB));

                // Report metadata
                page.AddText("Prepared by: Office of the CFO", 50, 330, 11,
                    o => o.Color(LightBlueR, LightBlueG, LightBlueB));
                page.AddText($"Date: {DateTime.Now:MMMM dd, yyyy}", 50, 310, 11,
                    o => o.Color(LightBlueR, LightBlueG, LightBlueB));
                page.AddText("Classification: Confidential — Internal Use Only", 50, 290, 11,
                    o => o.Color(LightBlueR, LightBlueG, LightBlueB));

                // Bottom band
                page.AddRectangle(0, 0, 595, 60, fill: true,
                    fillRed: DarkBlueR, fillGreen: DarkBlueG, fillBlue: DarkBlueB);
                page.AddText("Generated with Exis.PdfEditor v1.5.2  |  www.existech.com", 170, 25, 8,
                    o => o.Color(WhiteR, WhiteG, WhiteB));
            });

            // -------------------------------------------------------
            // PAGE 2: TABLE OF CONTENTS
            // -------------------------------------------------------
            ConsoleHelper.WriteInfo("  Building table of contents...");
            currentPage++;
            builder.AddPage(page =>
            {
                page.Size(PdfPageSize.A4);
                AddHeader(page, "Table of Contents");
                AddFooter(page, 2, totalPages);

                page.AddText("Contents", 50, 740, 24, o => o.Bold().Color(DarkGrayR, DarkGrayG, DarkGrayB));

                // Decorative accent under title
                page.AddRectangle(50, 730, 80, 3, fill: true,
                    fillRed: AccentR, fillGreen: AccentG, fillBlue: AccentB);

                var tocEntries = new[]
                {
                    ("1.", "Executive Summary", "3"),
                    ("2.", "Financial Highlights", "4"),
                    ("3.", "Revenue Analysis", "5"),
                    ("4.", "Operational Metrics", "6"),
                    ("5.", "Strategic Outlook", "7"),
                    ("6.", "Appendix: Data Tables", "8")
                };

                double tocY = 690;
                foreach (var (number, title, pg) in tocEntries)
                {
                    page.AddText(number, 60, tocY, 13, o => o.Bold().Color(ExisBlueR, ExisBlueG, ExisBlueB));
                    page.AddText(title, 85, tocY, 13, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));

                    // Dotted leader line
                    string dots = new string('.', 60 - title.Length);
                    page.AddText(dots, 85 + (title.Length * 7), tocY, 10,
                        o => o.Color(MedGrayR, MedGrayG, MedGrayB));

                    page.AddText(pg, 520, tocY, 13, o => o.Bold().Color(ExisBlueR, ExisBlueG, ExisBlueB));
                    tocY -= 35;
                }
            });

            // -------------------------------------------------------
            // PAGE 3: EXECUTIVE SUMMARY
            // -------------------------------------------------------
            ConsoleHelper.WriteInfo("  Building executive summary...");
            currentPage++;
            builder.AddPage(page =>
            {
                page.Size(PdfPageSize.A4);
                AddHeader(page, "Executive Summary");
                AddFooter(page, 3, totalPages);

                DrawSectionTitle(page, "1. Executive Summary", 740);

                // KPI boxes
                var kpis = new[]
                {
                    ("Revenue", "$24.8M", "+18.3%"),
                    ("Net Income", "$6.2M", "+22.1%"),
                    ("Customers", "1,847", "+340"),
                    ("Retention", "96.8%", "+1.2%")
                };

                double kpiX = 50;
                foreach (var (label, value, change) in kpis)
                {
                    // KPI card background
                    page.AddRectangle(kpiX, 670, 118, 55, fill: true,
                        fillRed: LightGrayR, fillGreen: LightGrayG, fillBlue: LightGrayB);

                    // Top accent bar on card
                    page.AddRectangle(kpiX, 722, 118, 4, fill: true,
                        fillRed: ExisBlueR, fillGreen: ExisBlueG, fillBlue: ExisBlueB);

                    // KPI value
                    page.AddText(value, kpiX + 8, 695, 18, o => o.Bold().Color(DarkGrayR, DarkGrayG, DarkGrayB));

                    // KPI label
                    page.AddText(label, kpiX + 8, 678, 9, o => o.Color(MedGrayR, MedGrayG, MedGrayB));

                    // Change indicator
                    page.AddText(change, kpiX + 75, 678, 9, o => o.Bold().Color(GreenR, GreenG, GreenB));

                    kpiX += 130;
                }

                // Executive summary paragraphs
                var execSummary = new[]
                {
                    "Q4 2024 represents a landmark quarter for Exis Technologies, achieving record-breaking",
                    "revenue of $24.8 million — an 18.3% increase year-over-year. This growth was driven by",
                    "strong enterprise adoption and the successful launch of Exis.PdfEditor v1.5, which",
                    "introduced advanced form-filling and digital signature capabilities.",
                    "",
                    "Customer acquisition accelerated in Q4, with 340 new enterprise accounts onboarded,",
                    "bringing our total active customer base to 1,847. Retention remained strong at 96.8%,",
                    "reflecting high satisfaction with product quality and support responsiveness.",
                    "",
                    "Net income grew 22.1% to $6.2 million, driven by operational efficiencies and the",
                    "favorable revenue mix shift toward higher-margin enterprise contracts. Gross margins",
                    "expanded 280 basis points to 78.4%, reflecting improved infrastructure utilization.",
                    "",
                    "Looking ahead, we are well-positioned for continued growth in 2025 with a robust",
                    "pipeline of enterprise opportunities and planned expansion into the APAC market."
                };

                double summaryY = 640;
                foreach (var line in execSummary)
                {
                    page.AddText(line, 50, summaryY, 11, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                    summaryY -= 17;
                }
            });

            // -------------------------------------------------------
            // PAGE 4: FINANCIAL HIGHLIGHTS
            // -------------------------------------------------------
            ConsoleHelper.WriteInfo("  Building financial highlights...");
            currentPage++;
            builder.AddPage(page =>
            {
                page.Size(PdfPageSize.A4);
                AddHeader(page, "Financial Highlights");
                AddFooter(page, 4, totalPages);

                DrawSectionTitle(page, "2. Financial Highlights", 740);

                // Revenue table
                page.AddText("Quarterly Revenue Breakdown ($M)", 50, 700, 12,
                    o => o.Bold().Color(DarkGrayR, DarkGrayG, DarkGrayB));

                // Table headers
                double tableTop = 680;
                page.AddRectangle(50, tableTop - 5, 495, 22, fill: true,
                    fillRed: ExisBlueR, fillGreen: ExisBlueG, fillBlue: ExisBlueB);
                page.AddText("Category", 60, tableTop, 10, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                page.AddText("Q1 2024", 200, tableTop, 10, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                page.AddText("Q2 2024", 280, tableTop, 10, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                page.AddText("Q3 2024", 360, tableTop, 10, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                page.AddText("Q4 2024", 440, tableTop, 10, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));

                // Table rows
                var tableData = new[]
                {
                    ("Subscriptions", "4.2", "4.6", "5.1", "5.8", false),
                    ("Enterprise Licenses", "8.1", "8.9", "9.5", "11.2", false),
                    ("Professional Services", "2.3", "2.5", "2.8", "3.1", false),
                    ("Support & Maintenance", "3.1", "3.3", "3.5", "3.8", false),
                    ("Training & Certification", "0.5", "0.6", "0.7", "0.9", false),
                    ("TOTAL", "18.2", "19.9", "21.6", "24.8", true)
                };

                double rowY = tableTop - 25;
                bool alternate = false;
                foreach (var (category, q1, q2, q3, q4, isTotal) in tableData)
                {
                    if (isTotal)
                    {
                        page.AddRectangle(50, rowY - 5, 495, 20, fill: true,
                            fillRed: DarkBlueR, fillGreen: DarkBlueG, fillBlue: DarkBlueB);
                        page.AddText(category, 60, rowY, 10, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                        page.AddText(q1, 210, rowY, 10, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                        page.AddText(q2, 290, rowY, 10, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                        page.AddText(q3, 370, rowY, 10, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                        page.AddText(q4, 450, rowY, 10, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                    }
                    else
                    {
                        if (alternate)
                        {
                            page.AddRectangle(50, rowY - 5, 495, 20, fill: true,
                                fillRed: LightGrayR, fillGreen: LightGrayG, fillBlue: LightGrayB);
                        }
                        page.AddText(category, 60, rowY, 10, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                        page.AddText(q1, 210, rowY, 10, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                        page.AddText(q2, 290, rowY, 10, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                        page.AddText(q3, 370, rowY, 10, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                        page.AddText(q4, 450, rowY, 10, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                    }

                    rowY -= 22;
                    alternate = !alternate;
                }

                // Chart placeholder area
                double chartY = rowY - 30;
                page.AddText("Revenue Trend (Quarterly)", 50, chartY, 12,
                    o => o.Bold().Color(DarkGrayR, DarkGrayG, DarkGrayB));

                chartY -= 15;
                page.AddRectangle(50, chartY - 170, 495, 170, fill: true,
                    fillRed: LightGrayR, fillGreen: LightGrayG, fillBlue: LightGrayB);

                // Simulated bar chart
                var bars = new[] { (18.2, "Q1"), (19.9, "Q2"), (21.6, "Q3"), (24.8, "Q4") };
                double barX = 100;
                double barMaxHeight = 130;
                double barBottom = chartY - 155;
                foreach (var (value, label) in bars)
                {
                    double barHeight = (value / 25.0) * barMaxHeight;
                    page.AddRectangle(barX, barBottom, 70, barHeight, fill: true,
                        fillRed: ExisBlueR, fillGreen: ExisBlueG, fillBlue: ExisBlueB);

                    // Bar value label
                    page.AddText($"${value}M", barX + 10, barBottom + barHeight - 18, 11,
                        o => o.Bold().Color(WhiteR, WhiteG, WhiteB));

                    // Bar category label
                    page.AddText(label, barX + 25, barBottom - 14, 9,
                        o => o.Color(MedGrayR, MedGrayG, MedGrayB));

                    barX += 110;
                }
            });

            // -------------------------------------------------------
            // PAGE 5: REVENUE ANALYSIS
            // -------------------------------------------------------
            ConsoleHelper.WriteInfo("  Building revenue analysis...");
            currentPage++;
            builder.AddPage(page =>
            {
                page.Size(PdfPageSize.A4);
                AddHeader(page, "Revenue Analysis");
                AddFooter(page, 5, totalPages);

                DrawSectionTitle(page, "3. Revenue Analysis", 740);

                double col1X = 50;
                double col2X = 310;
                double colWidth = 240;

                // Left column: Revenue by Region
                page.AddText("Revenue by Region", col1X, 700, 11,
                    o => o.Bold().Color(DarkGrayR, DarkGrayG, DarkGrayB));
                page.AddRectangle(col1X, 692, 60, 2, fill: true,
                    fillRed: ExisBlueR, fillGreen: ExisBlueG, fillBlue: ExisBlueB);

                var regions = new[]
                {
                    ("North America", "$14.9M", 60),
                    ("Europe", "$6.2M", 25),
                    ("Asia Pacific", "$2.5M", 10),
                    ("Rest of World", "$1.2M", 5)
                };

                double regionY = 670;
                foreach (var (region, amount, pct) in regions)
                {
                    page.AddRectangle(col1X, regionY - 4, colWidth, 14, fill: true,
                        fillRed: LightGrayR, fillGreen: LightGrayG, fillBlue: LightGrayB);

                    double fillWidth = (pct / 100.0) * colWidth;
                    page.AddRectangle(col1X, regionY - 4, fillWidth, 14, fill: true,
                        fillRed: ExisBlueR, fillGreen: ExisBlueG, fillBlue: ExisBlueB);

                    page.AddText($"{region}  {amount}  ({pct}%)", col1X, regionY - 15, 9,
                        o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                    regionY -= 38;
                }

                // Right column: Revenue by Product
                page.AddText("Revenue by Product Line", col2X, 700, 11,
                    o => o.Bold().Color(DarkGrayR, DarkGrayG, DarkGrayB));
                page.AddRectangle(col2X, 692, 60, 2, fill: true,
                    fillRed: AccentR, fillGreen: AccentG, fillBlue: AccentB);

                var products = new[]
                {
                    ("Exis.PdfEditor", "$10.4M", 42),
                    ("Exis.PdfBuilder", "$6.9M", 28),
                    ("Exis.PdfForms", "$4.3M", 17),
                    ("Exis.PdfSecurity", "$3.2M", 13)
                };

                double productY = 670;
                foreach (var (product, amount, pct) in products)
                {
                    page.AddRectangle(col2X, productY - 4, colWidth, 14, fill: true,
                        fillRed: LightGrayR, fillGreen: LightGrayG, fillBlue: LightGrayB);

                    double fillWidth = (pct / 100.0) * colWidth;
                    page.AddRectangle(col2X, productY - 4, fillWidth, 14, fill: true,
                        fillRed: AccentR, fillGreen: AccentG, fillBlue: AccentB);

                    page.AddText($"{product}  {amount}  ({pct}%)", col2X, productY - 15, 9,
                        o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                    productY -= 38;
                }

                // Year-over-Year Comparison
                double yoyY = 480;
                page.AddText("Year-over-Year Revenue Growth", 50, yoyY, 11,
                    o => o.Bold().Color(DarkGrayR, DarkGrayG, DarkGrayB));
                page.AddRectangle(50, yoyY - 8, 60, 2, fill: true,
                    fillRed: ExisBlueR, fillGreen: ExisBlueG, fillBlue: ExisBlueB);

                var yoyData = new[]
                {
                    ("2021", "$42.1M", "+24%"),
                    ("2022", "$58.3M", "+38%"),
                    ("2023", "$72.8M", "+25%"),
                    ("2024", "$84.5M", "+16%"),
                    ("2025 (Proj)", "$102.0M", "+21%")
                };

                double yoyTableTop = yoyY - 25;
                page.AddRectangle(50, yoyTableTop - 5, 495, 22, fill: true,
                    fillRed: ExisBlueR, fillGreen: ExisBlueG, fillBlue: ExisBlueB);
                page.AddText("Fiscal Year", 60, yoyTableTop, 10, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                page.AddText("Annual Revenue", 220, yoyTableTop, 10, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                page.AddText("YoY Growth", 400, yoyTableTop, 10, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));

                double yoyRowY = yoyTableTop - 25;
                bool yoyAlt = false;
                foreach (var (year, revenue, growth) in yoyData)
                {
                    if (yoyAlt)
                    {
                        page.AddRectangle(50, yoyRowY - 5, 495, 20, fill: true,
                            fillRed: LightGrayR, fillGreen: LightGrayG, fillBlue: LightGrayB);
                    }

                    page.AddText(year, 60, yoyRowY, 10, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                    page.AddText(revenue, 220, yoyRowY, 10, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                    page.AddText(growth, 410, yoyRowY, 10, o => o.Bold().Color(GreenR, GreenG, GreenB));

                    yoyRowY -= 22;
                    yoyAlt = !yoyAlt;
                }
            });

            // -------------------------------------------------------
            // PAGE 6: OPERATIONAL METRICS
            // -------------------------------------------------------
            ConsoleHelper.WriteInfo("  Building operational metrics...");
            currentPage++;
            builder.AddPage(page =>
            {
                page.Size(PdfPageSize.A4);
                AddHeader(page, "Operational Metrics");
                AddFooter(page, 6, totalPages);

                DrawSectionTitle(page, "4. Operational Metrics", 740);

                // Metrics grid (2x3)
                var metrics = new[]
                {
                    ("Monthly Active Users", "48,320", "+12.4%", "MAU growth driven by enterprise expansion"),
                    ("API Calls (Monthly)", "2.1B", "+34.8%", "Record throughput with v1.5 optimizations"),
                    ("Avg. Response Time", "42ms", "-18.2%", "Infrastructure upgrades improve latency"),
                    ("Uptime SLA", "99.97%", "+0.02%", "Exceeded 99.95% target for 8th quarter"),
                    ("Support Tickets", "847", "-22.5%", "Improved docs reduce inbound volume"),
                    ("NPS Score", "78", "+6 pts", "Highest score since company founding")
                };

                double metricX = 50;
                double metricY = 700;
                int metricCount = 0;
                foreach (var (label, value, change, desc) in metrics)
                {
                    // Card background
                    page.AddRectangle(metricX, metricY - 10, 240, 85, fill: true,
                        fillRed: LightGrayR, fillGreen: LightGrayG, fillBlue: LightGrayB);

                    // Left accent bar
                    page.AddRectangle(metricX, metricY - 10, 4, 85, fill: true,
                        fillRed: ExisBlueR, fillGreen: ExisBlueG, fillBlue: ExisBlueB);

                    // Metric value
                    page.AddText(value, metricX + 15, metricY + 45, 22,
                        o => o.Bold().Color(DarkGrayR, DarkGrayG, DarkGrayB));

                    // Metric label
                    page.AddText(label, metricX + 15, metricY + 28, 9,
                        o => o.Color(MedGrayR, MedGrayG, MedGrayB));

                    // Change indicator
                    page.AddText(change, metricX + 160, metricY + 50, 10,
                        o => o.Bold().Color(GreenR, GreenG, GreenB));

                    // Description
                    page.AddText(desc, metricX + 15, metricY, 8,
                        o => o.Color(MedGrayR, MedGrayG, MedGrayB));

                    metricCount++;
                    if (metricCount % 2 == 0)
                    {
                        metricX = 50;
                        metricY -= 110;
                    }
                    else
                    {
                        metricX = 305;
                    }
                }

                // Engineering velocity section
                double engY = metricY - 30;
                page.AddText("Engineering Velocity", 50, engY, 11,
                    o => o.Bold().Color(DarkGrayR, DarkGrayG, DarkGrayB));
                page.AddRectangle(50, engY - 8, 60, 2, fill: true,
                    fillRed: ExisBlueR, fillGreen: ExisBlueG, fillBlue: ExisBlueB);

                var engMetrics = new[]
                {
                    ("Commits this Quarter", "2,847"),
                    ("Pull Requests Merged", "423"),
                    ("Features Shipped", "34"),
                    ("Bugs Resolved", "156"),
                    ("Test Coverage", "94.2%"),
                    ("Deployment Frequency", "12x / week")
                };

                double engItemY = engY - 25;
                foreach (var (metric, val) in engMetrics)
                {
                    page.AddText(metric, 60, engItemY, 10, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                    page.AddText(val, 250, engItemY, 10, o => o.Bold().Color(ExisBlueR, ExisBlueG, ExisBlueB));
                    engItemY -= 18;
                }
            });

            // -------------------------------------------------------
            // PAGE 7: STRATEGIC OUTLOOK
            // -------------------------------------------------------
            ConsoleHelper.WriteInfo("  Building strategic outlook...");
            currentPage++;
            builder.AddPage(page =>
            {
                page.Size(PdfPageSize.A4);
                AddHeader(page, "Strategic Outlook");
                AddFooter(page, 7, totalPages);

                DrawSectionTitle(page, "5. Strategic Outlook — 2025", 740);

                var pillars = new[]
                {
                    ("Product Innovation", new[]
                    {
                        "Launch Exis.PdfEditor v2.0 with AI-powered content extraction",
                        "Introduce real-time collaboration features for PDF editing",
                        "Expand PDF/A-3 compliance with embedded file support",
                        "Deliver native ARM64 builds for Apple Silicon and Windows on ARM"
                    }),
                    ("Market Expansion", new[]
                    {
                        "Establish direct sales presence in Tokyo and Singapore",
                        "Launch localized documentation in Japanese, Korean, and German",
                        "Partner with 5 major system integrators in APAC region",
                        "Target $15M in international revenue (vs. $9.7M in 2024)"
                    }),
                    ("Customer Success", new[]
                    {
                        "Achieve 97.5% annual retention rate",
                        "Launch Exis Academy with certification program",
                        "Reduce average first-response time to < 2 hours",
                        "Expand dedicated account management to top 100 accounts"
                    })
                };

                double pillarY = 700;
                int pillarNum = 1;
                foreach (var (pillarTitle, items) in pillars)
                {
                    page.AddRectangle(50, pillarY - 2, 495, 24, fill: true,
                        fillRed: ExisBlueR, fillGreen: ExisBlueG, fillBlue: ExisBlueB);
                    page.AddText($"Pillar {pillarNum}: {pillarTitle}", 60, pillarY + 3, 11,
                        o => o.Bold().Color(WhiteR, WhiteG, WhiteB));

                    double itemY = pillarY - 22;
                    foreach (var item in items)
                    {
                        page.AddText(item, 75, itemY, 10, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                        itemY -= 17;
                    }

                    pillarY = itemY - 15;
                    pillarNum++;
                }

                // Risk factors box
                page.AddRectangle(50, pillarY - 10, 495, 80, fill: true,
                    fillRed: 1.0, fillGreen: 0.953, fillBlue: 0.804);
                page.AddRectangle(50, pillarY - 10, 495, 80, fill: false,
                    strokeRed: AccentR, strokeGreen: AccentG, strokeBlue: AccentB);

                page.AddText("Key Risk Factors", 65, pillarY + 50, 10,
                    o => o.Bold().Color(AccentR, AccentG, AccentB));
                page.AddText("- Competitive pressure from open-source alternatives", 65, pillarY + 32, 9,
                    o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                page.AddText("- Currency fluctuation impact on international revenue", 65, pillarY + 18, 9,
                    o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                page.AddText("- Enterprise sales cycle elongation in uncertain macro environment", 65, pillarY + 4, 9,
                    o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
            });

            // -------------------------------------------------------
            // PAGE 8: APPENDIX DATA TABLES
            // -------------------------------------------------------
            ConsoleHelper.WriteInfo("  Building appendix data tables...");
            currentPage++;
            builder.AddPage(page =>
            {
                page.Size(PdfPageSize.A4);
                AddHeader(page, "Appendix");
                AddFooter(page, 8, totalPages);

                DrawSectionTitle(page, "6. Appendix: Detailed Data Tables", 740);

                // Monthly Revenue Breakdown
                page.AddText("Table A: Q4 2024 Monthly Revenue Detail ($M)", 50, 710, 10,
                    o => o.Bold().Color(DarkGrayR, DarkGrayG, DarkGrayB));

                double appTableTop = 690;
                page.AddRectangle(50, appTableTop - 5, 495, 20, fill: true,
                    fillRed: ExisBlueR, fillGreen: ExisBlueG, fillBlue: ExisBlueB);
                page.AddText("Product Line", 60, appTableTop, 9, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                page.AddText("Oct", 190, appTableTop, 9, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                page.AddText("Nov", 250, appTableTop, 9, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                page.AddText("Dec", 310, appTableTop, 9, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                page.AddText("Q4 Total", 380, appTableTop, 9, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                page.AddText("vs Q3", 460, appTableTop, 9, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));

                var monthlyData = new[]
                {
                    ("PdfEditor", "3.2", "3.4", "3.8", "10.4", "+9.5%", false),
                    ("PdfBuilder", "2.1", "2.2", "2.6", "6.9", "+11.3%", false),
                    ("PdfForms", "1.3", "1.4", "1.6", "4.3", "+13.2%", false),
                    ("PdfSecurity", "0.9", "1.0", "1.3", "3.2", "+18.5%", false),
                    ("TOTAL", "7.5", "8.0", "9.3", "24.8", "+14.8%", true)
                };

                double appRowY = appTableTop - 25;
                bool appAlt = false;
                foreach (var (product, oct, nov, dec, total, vsQ3, isTotal) in monthlyData)
                {
                    if (isTotal)
                    {
                        page.AddRectangle(50, appRowY - 5, 495, 18, fill: true,
                            fillRed: DarkBlueR, fillGreen: DarkBlueG, fillBlue: DarkBlueB);
                        page.AddText(product, 60, appRowY, 9, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                        page.AddText(oct, 195, appRowY, 9, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                        page.AddText(nov, 255, appRowY, 9, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                        page.AddText(dec, 315, appRowY, 9, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                        page.AddText(total, 385, appRowY, 9, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                        page.AddText(vsQ3, 462, appRowY, 9, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                    }
                    else
                    {
                        if (appAlt)
                        {
                            page.AddRectangle(50, appRowY - 5, 495, 18, fill: true,
                                fillRed: LightGrayR, fillGreen: LightGrayG, fillBlue: LightGrayB);
                        }
                        page.AddText(product, 60, appRowY, 9, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                        page.AddText(oct, 195, appRowY, 9, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                        page.AddText(nov, 255, appRowY, 9, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                        page.AddText(dec, 315, appRowY, 9, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                        page.AddText(total, 385, appRowY, 9, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                        page.AddText(vsQ3, 462, appRowY, 9, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                    }
                    appRowY -= 20;
                    appAlt = !appAlt;
                }

                // Customer Segmentation Table
                appRowY -= 25;
                page.AddText("Table B: Customer Segmentation Analysis", 50, appRowY, 10,
                    o => o.Bold().Color(DarkGrayR, DarkGrayG, DarkGrayB));

                appRowY -= 20;
                page.AddRectangle(50, appRowY - 5, 495, 20, fill: true,
                    fillRed: ExisBlueR, fillGreen: ExisBlueG, fillBlue: ExisBlueB);
                page.AddText("Segment", 60, appRowY, 9, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                page.AddText("Customers", 180, appRowY, 9, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                page.AddText("Revenue", 270, appRowY, 9, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                page.AddText("ARPU", 360, appRowY, 9, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
                page.AddText("Growth", 440, appRowY, 9, o => o.Bold().Color(WhiteR, WhiteG, WhiteB));

                var segData = new[]
                {
                    ("Enterprise (>$50K)", "142", "$12.8M", "$90,141", "+24%"),
                    ("Mid-Market ($10-50K)", "385", "$7.2M", "$18,701", "+18%"),
                    ("SMB ($1-10K)", "920", "$3.6M", "$3,913", "+12%"),
                    ("Starter (<$1K)", "400", "$1.2M", "$3,000", "+8%")
                };

                appRowY -= 22;
                appAlt = false;
                foreach (var (segment, customers, revenue, arpu, growth) in segData)
                {
                    if (appAlt)
                    {
                        page.AddRectangle(50, appRowY - 5, 495, 18, fill: true,
                            fillRed: LightGrayR, fillGreen: LightGrayG, fillBlue: LightGrayB);
                    }

                    page.AddText(segment, 60, appRowY, 9, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                    page.AddText(customers, 195, appRowY, 9, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                    page.AddText(revenue, 280, appRowY, 9, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                    page.AddText(arpu, 368, appRowY, 9, o => o.Color(DarkGrayR, DarkGrayG, DarkGrayB));
                    page.AddText(growth, 448, appRowY, 9, o => o.Bold().Color(GreenR, GreenG, GreenB));

                    appRowY -= 20;
                    appAlt = !appAlt;
                }

                // Disclaimer footer
                page.AddText("Note: All financial figures are unaudited and subject to revision. Forward-looking statements involve risks and uncertainties.",
                    50, 70, 7, o => o.Italic().Color(MedGrayR, MedGrayG, MedGrayB));
                page.AddText("This report is confidential and intended for internal use only. Distribution outside the organization requires written approval.",
                    50, 58, 7, o => o.Italic().Color(MedGrayR, MedGrayG, MedGrayB));
            });

            // -------------------------------------------------------
            // SAVE THE REPORT
            // -------------------------------------------------------
            builder.BuildToFile(reportPath);
            sw.Stop();

            ConsoleHelper.WriteSuccess($"Report generated in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Output: {reportPath}");
            ConsoleHelper.WriteSuccess($"Total pages: {totalPages}");
            ConsoleHelper.WriteInfo("Pages: Title | TOC | Executive Summary | Financial Highlights | Revenue Analysis | Operational Metrics | Strategic Outlook | Appendix");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Report Generator demo failed: {ex.Message}");
        }
    }

    private static void AddHeader(PdfPageBuilder page, string sectionName)
    {
        page.AddRectangle(0, 815, 595, 27, fill: true,
            fillRed: ExisBlueR, fillGreen: ExisBlueG, fillBlue: ExisBlueB);
        page.AddText("EXIS TECHNOLOGIES  |  Q4 2024 Quarterly Business Report", 50, 823, 9,
            o => o.Bold().Color(WhiteR, WhiteG, WhiteB));
        page.AddText(sectionName, 450, 823, 8, o => o.Color(WhiteR, WhiteG, WhiteB));
    }

    private static void AddFooter(PdfPageBuilder page, int pageNum, int totalPages)
    {
        page.AddLine(50, 42, 545, 42, strokeWidth: 0.5,
            red: MedGrayR, green: MedGrayG, blue: MedGrayB);
        page.AddText($"Page {pageNum} of {totalPages}", 270, 28, 8,
            o => o.Color(MedGrayR, MedGrayG, MedGrayB));
        page.AddText("Confidential — Internal Use Only", 50, 28, 8,
            o => o.Color(MedGrayR, MedGrayG, MedGrayB));
        page.AddText($"{DateTime.Now:yyyy-MM-dd}", 480, 28, 8,
            o => o.Color(MedGrayR, MedGrayG, MedGrayB));
    }

    private static void DrawSectionTitle(PdfPageBuilder page, string title, double y)
    {
        page.AddText(title, 50, y, 20, o => o.Bold().Color(DarkGrayR, DarkGrayG, DarkGrayB));
        page.AddRectangle(50, y - 8, 80, 3, fill: true,
            fillRed: AccentR, fillGreen: AccentG, fillBlue: AccentB);
    }
}
