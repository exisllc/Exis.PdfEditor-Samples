namespace Exis.PdfEditor.Demo.WinForms.Controls;

public class DragDropPanel : Panel
{
    public event EventHandler<string[]>? FilesDropped;
    private bool _isDragOver;

    public DragDropPanel()
    {
        AllowDrop = true;
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        BackColor = Color.FromArgb(248, 250, 252);
        Padding = new Padding(20);
    }

    protected override void OnDragEnter(DragEventArgs e)
    {
        if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
        {
            e.Effect = DragDropEffects.Copy;
            _isDragOver = true;
            Invalidate();
        }
        base.OnDragEnter(e);
    }

    protected override void OnDragLeave(EventArgs e)
    {
        _isDragOver = false;
        Invalidate();
        base.OnDragLeave(e);
    }

    protected override void OnDragDrop(DragEventArgs e)
    {
        _isDragOver = false;
        Invalidate();
        if (e.Data?.GetData(DataFormats.FileDrop) is string[] files)
        {
            var pdfFiles = files.Where(f => f.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)).ToArray();
            if (pdfFiles.Length > 0)
                FilesDropped?.Invoke(this, pdfFiles);
        }
        base.OnDragDrop(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        using var pen = new Pen(_isDragOver ? Color.FromArgb(37, 99, 235) : Color.FromArgb(203, 213, 225), 2f);
        pen.DashStyle = _isDragOver ? System.Drawing.Drawing2D.DashStyle.Solid : System.Drawing.Drawing2D.DashStyle.Dash;

        var rect = new Rectangle(4, 4, Width - 8, Height - 8);
        g.DrawRectangle(pen, rect);

        var text = _isDragOver ? "Release to process PDF files" : "Drop PDF files here for quick processing";
        using var font = new Font("Segoe UI", 11f, FontStyle.Regular);
        using var brush = new SolidBrush(_isDragOver ? Color.FromArgb(37, 99, 235) : Color.FromArgb(100, 116, 139));
        var size = g.MeasureString(text, font);
        g.DrawString(text, font, brush, (Width - size.Width) / 2, (Height - size.Height) / 2);
    }
}
