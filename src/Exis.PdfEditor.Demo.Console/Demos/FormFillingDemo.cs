namespace Exis.PdfEditor.Demo.Console.Demos;

using System.Diagnostics;
using Exis.PdfEditor;

internal static class FormFillingDemo
{
    public static async Task RunAsync()
    {
        ConsoleHelper.WriteHeader("Form Filling Demo");
        var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
        Directory.CreateDirectory(outputDir);

        try
        {
            // 1. Create a sample PDF that simulates a form document
            ConsoleHelper.WriteInfo("Creating sample PDF form document...");
            var formPath = Path.Combine(outputDir, "form-sample.pdf");
            var sw = Stopwatch.StartNew();

            var builder = PdfBuilder.Create();
            builder.AddPage(page =>
            {
                page.Size(PdfPageSize.A4);

                // Form header
                page.AddRectangle(0, 780, 595, 62, fill: true,
                    fillRed: 0.145, fillGreen: 0.388, fillBlue: 0.921);
                page.AddText("Employee Onboarding Form", 130, 805, 20,
                    o => o.Bold().Color(1.0, 1.0, 1.0));
                page.AddText("Human Resources Department — Confidential", 170, 788, 10,
                    o => o.Color(1.0, 1.0, 1.0));

                // Form field labels
                double y = 740;
                page.AddText("Personal Information", 50, y, 11, o => o.Bold());
                page.AddRectangle(50, y - 8, 150, 2, fill: true,
                    fillRed: 0.145, fillGreen: 0.388, fillBlue: 0.921);

                y -= 30;
                page.AddText("First Name: ________________________", 50, y, 11);
                page.AddText("Last Name: ________________________", 300, y, 11);

                y -= 25;
                page.AddText("Email: _____________________________", 50, y, 11);
                page.AddText("Phone: ____________________________", 300, y, 11);

                y -= 25;
                page.AddText("Street Address: ___________________________________________________", 50, y, 11);

                y -= 25;
                page.AddText("City: ________________", 50, y, 11);
                page.AddText("State: _____", 220, y, 11);
                page.AddText("Zip: ________", 320, y, 11);
                page.AddText("Country: ________", 420, y, 11);

                y -= 40;
                page.AddText("Employment Details", 50, y, 11, o => o.Bold());
                page.AddRectangle(50, y - 8, 150, 2, fill: true,
                    fillRed: 0.145, fillGreen: 0.388, fillBlue: 0.921);

                y -= 30;
                page.AddText("Position: __________________________", 50, y, 11);
                page.AddText("Department: ________________________", 300, y, 11);

                y -= 25;
                page.AddText("Start Date: _______________", 50, y, 11);
                page.AddText("Annual Salary: _______________", 220, y, 11);
                page.AddText("Type: _______________", 420, y, 11);

                y -= 40;
                page.AddText("Agreements", 50, y, 11, o => o.Bold());
                page.AddRectangle(50, y - 8, 150, 2, fill: true,
                    fillRed: 0.145, fillGreen: 0.388, fillBlue: 0.921);

                y -= 25;
                page.AddText("[ ] I agree to the Non-Disclosure Agreement", 50, y, 11);
                y -= 20;
                page.AddText("[ ] I agree to the Code of Conduct", 50, y, 11);
                y -= 20;
                page.AddText("[ ] I acknowledge company policies and employee handbook", 50, y, 11);

                y -= 35;
                page.AddText("Employee Signature: _____________________________", 50, y, 11);
                page.AddText("Date: _______________", 350, y, 11);
            });
            builder.BuildToFile(formPath);
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Sample form created in {sw.ElapsedMilliseconds}ms: {formPath}");

            // 2. Read form fields (demonstrates GetFieldsAsync)
            ConsoleHelper.WriteInfo("\n--- Reading Form Fields ---");
            sw.Restart();
            var fields = await PdfFormFiller.GetFieldsAsync(formPath);
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Found {fields.Count} form fields in {sw.ElapsedMilliseconds}ms:");

            foreach (var field in fields)
            {
                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                System.Console.Write("    ");
                System.Console.ForegroundColor = ConsoleColor.Cyan;
                System.Console.Write($"[{field.Type,-12}]");
                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.Write($" {field.Name,-25}");
                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                System.Console.WriteLine($" Value: \"{field.Value ?? "(empty)"}\"");
                System.Console.ResetColor();
            }

            // 3. Fill form fields
            ConsoleHelper.WriteInfo("\n--- Filling Form Fields ---");
            sw.Restart();
            var filledPath = Path.Combine(outputDir, "form-filled.pdf");
            var fieldValues = new Dictionary<string, string>
            {
                ["first_name"] = "Sarah",
                ["last_name"] = "Chen",
                ["email"] = "sarah.chen@existech.com",
                ["phone"] = "(555) 987-6543",
                ["address_line1"] = "123 Innovation Blvd, Suite 400",
                ["city"] = "San Francisco",
                ["state"] = "CA",
                ["zip"] = "94105",
                ["country"] = "USA",
                ["position"] = "Senior Software Engineer",
                ["department"] = "Platform Engineering",
                ["start_date"] = DateTime.Now.AddDays(14).ToString("MM/dd/yyyy"),
                ["salary"] = "$185,000",
                ["employment_type"] = "Full-Time",
                ["agree_nda"] = "true",
                ["agree_conduct"] = "true",
                ["agree_policies"] = "true",
                ["signature_date"] = DateTime.Now.ToString("MM/dd/yyyy")
            };

            var fillResult = await PdfFormFiller.FillAsync(formPath, filledPath, fieldValues);
            sw.Stop();
            ConsoleHelper.WriteSuccess($"Form filled with {fieldValues.Count} field values in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Output: {filledPath}");

            // 4. Flatten form (make fields non-editable)
            ConsoleHelper.WriteInfo("\n--- Flattening Form ---");
            sw.Restart();
            var flattenedPath = Path.Combine(outputDir, "form-flattened.pdf");
            await PdfFormFiller.FlattenAsync(filledPath, flattenedPath);
            sw.Stop();

            var filledSize = new FileInfo(filledPath).Length;
            var flatSize = new FileInfo(flattenedPath).Length;
            ConsoleHelper.WriteSuccess($"Form flattened in {sw.ElapsedMilliseconds}ms");
            ConsoleHelper.WriteSuccess($"Filled form: {filledSize:N0} bytes → Flattened: {flatSize:N0} bytes");
            ConsoleHelper.WriteSuccess($"Output: {flattenedPath}");

            // 5. Verify flattened form has no editable fields
            ConsoleHelper.WriteInfo("\n--- Verifying Flattened Form ---");
            var flatFields = await PdfFormFiller.GetFieldsAsync(flattenedPath);
            if (flatFields.Count == 0)
                ConsoleHelper.WriteSuccess("Flattened form has 0 editable fields — form data is now permanent");
            else
                ConsoleHelper.WriteInfo($"Flattened form still has {flatFields.Count} fields (partial flatten)");

            ConsoleHelper.WriteSuccess("\nAll form filling demos completed successfully!");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Form Filling demo failed: {ex.Message}");
        }
    }
}
