using System.Drawing;

namespace Engine.Extensions;

public static class SizeExtension
{
    public static Size Scale(this Size size, int scale)
        => new(size.Width * scale, size.Height * scale);

    public static Size ScaleX(this Size size, int scale)
        => new(size.Width * scale, size.Height);

    public static Size ScaleY(this Size size, int scale)
        => new(size.Width, size.Height * scale);

}