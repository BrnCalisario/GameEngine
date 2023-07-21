using System.Drawing;

namespace Engine.Extensions;

public static class SizeExtension
{
    public static Size Scale(this Size size, float scale)
        => new((int)(size.Width * scale), (int)(size.Height * scale));

    public static Size ScaleX(this Size size, float scale)
        => new((int)(size.Width * scale), size.Height);

    public static Size ScaleY(this Size size, float scale)
        => new(size.Width, (int)(size.Height * scale));

}