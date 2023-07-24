using System.Collections.Generic;
using System.Drawing;

namespace Engine.Extensions;

using static SizeExtension;

public static class RectangleExtension
{
    public static bool AnyPlaceMeeting<T>(this Rectangle rec, List<T> collList, out T collider)
        where T : CollidableBody
    {
        foreach (var coll in collList)
        {
            if (coll.CollisionMask.IsColling(rec))
            {
                collider = coll;
                return true;
            }
        }

        collider = null;
        return false;
    }
    
    public static Rectangle AlignCenter(this Rectangle rec, Rectangle parent)
    {
        var centerPoint = new Point(parent.X + parent.Width / 2, parent.Y + parent.Height / 2);
        var relativePoint = new Point(centerPoint.X - rec.Width / 2, centerPoint.Y - rec.Height / 2);

        rec = new Rectangle(relativePoint, rec.Size);

        return rec;
    }

    public static Rectangle AlignTopLeft(this Rectangle rec, Rectangle parent)
    {
        var topLeft = new Point(parent.Left, parent.Top);
        return new Rectangle(topLeft, rec.Size);
    }

    public static Rectangle AlignBottomLeft(this Rectangle rec, Rectangle parent)
    {
        var botLeft = new Point(parent.Left, parent.Bottom - rec.Height);
        return new Rectangle(botLeft, rec.Size);
    }

    public static Rectangle AlignTopRight(this Rectangle rec, Rectangle parent)
    {
        var topRight = new Point(parent.Right - rec.Width, parent.Top);
        return new Rectangle(topRight, rec.Size);
    }

    public static Rectangle AlignBottomRight(this Rectangle rec, Rectangle parent)
    {
        var botRight = new Point(parent.Right - rec.Width, parent.Bottom - rec.Height);
        return new Rectangle(botRight, rec.Size);
    }

    public static Rectangle AlignMiddleCenter(this Rectangle rec, Rectangle parent)
    {
        var centerTop = new Point(parent.X + parent.Width / 2, parent.Top);
        var relativePoint = new Point(centerTop.X - rec.Width / 2, centerTop.Y);

        return new Rectangle(relativePoint, rec.Size);
    }
    
    public static Rectangle AlignBesideLeft(this Rectangle rec, Rectangle target)
    {
        var pos = new Point(target.Left, target.Top);
        var relativePos = new Point(pos.X - rec.Width, pos.Y);

        return new Rectangle(relativePos, rec.Size);
    }

    public static Rectangle AlignBesideRight(this Rectangle rec, Rectangle target, int gapX = 0, int gapY = 0)
    {
        var pos = new Point(target.Right, target.Top);
        var relativePos = new Point(pos.X + gapX, pos.Y + gapY);

        return new Rectangle(relativePos, rec.Size);
    }

    public static Rectangle AlignBesideBottom(this Rectangle rec, Rectangle target, int gapX = 0, int gapY = 0) 
    {
        var pos = new Point(target.Left, target.Bottom);

        return new Rectangle(pos, rec.Size);
    }
}


