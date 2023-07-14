using System;
using System.Collections.Generic;
using System.Drawing;

namespace SpriteExercise.Sprites;

public class Sprite
{
    public Point Position { get; set; }
    public Size Size { get; set; }

    public int X => Position.X;
    public int Y => Position.Y;
    public int Width => Size.Width;
    public int Height => Size.Height;

}

