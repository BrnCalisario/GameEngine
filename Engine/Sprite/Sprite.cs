using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Sprite;

public struct Sprite
{
    public Point Location { get; set; }
    public Size Size { get; set; }

    public int X => Location.X;
    public int Y => Location.Y; 
    public int Width => Size.Width;
    public int Height => Size.Height;
}
