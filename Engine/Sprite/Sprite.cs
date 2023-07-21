
using System.Drawing;

namespace Engine.Sprites;

public struct Sprite
{
    public Sprite(Point location, Size size)
    {
        this.Location = location;
        this.Size = size;
    }

    public Point Location { get; set; }
    public Size Size { get; set; }

    public int X => Location.X;
    public int Y => Location.Y; 
    public int Width => Size.Width;
    public int Height => Size.Height;
}
