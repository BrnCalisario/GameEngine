using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine;

public class CollisionMask : CollidableBody
{
    public CollisionMask(Body parent, Rectangle mask) : base(mask, null)
    {
        if(parent is not null)
        {
            Parent = parent;
            Box = new Rectangle(Parent.X + mask.X, Parent.Y + mask.Y, mask.Width, mask.Height);

        }
        RelativePosition = mask.Location;
    }

    public Point RelativePosition;
    public Body Parent { get; set; }

    public void UpdatePoint(Point position)
    {
        var newPos = new Point(RelativePosition.X + position.X, RelativePosition.Y +  position.Y);
        this.Box = new Rectangle(newPos, this.Box.Size);
    }

    public void Rotate90deg()
    {
        var newSize = new Size(Box.Height, Box.Width);
        this.Box = new(Box.Location, newSize);
    }

    public override void Update()
    {   
    }
}

