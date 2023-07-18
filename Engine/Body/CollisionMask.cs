﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine;

public class CollisionMask
{
    public CollisionMask(Body parent, Rectangle mask)
    {
        Parent = parent;
        Mask = new Rectangle(Parent.X + mask.X, Parent.Y + mask.Y, mask.Width, mask.Height);
        RelativePosition = mask.Location;
    }

    public Point RelativePosition;

    public Body Parent { get; set; }
    public Rectangle Mask { get; set; }
    public void UpdatePoint(Point position)
    {
        var newPos = new Point(RelativePosition.X + position.X, RelativePosition.Y +  position.Y);
        Mask = new Rectangle(newPos, this.Mask.Size);
    }
}

