using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Engine;

public interface IBody
{
    void Draw(Graphics g);
    void Update();
}

public interface ICollidableBody : IBody
{
    bool IsColling(Rectangle box);
    bool IsColling(Body body)
        => IsColling(body.Box);
    bool IsCollidingMask(CollidableBody body);
    
    List<T> IsCollidingList<T>(List<T> list) where T : Body
    {
        var query = list.Where(c => IsColling(c)).ToList();
        return query;
    }

    List<T> IsCollidingMaskList<T>(List<T> list) where T : CollidableBody;
    void SetColllisionMask(Rectangle mask);
}

public abstract class Body : IBody
{
    public Body(Rectangle box, Pen pen = null)
    {
        if (pen is not null)
            this.Pen = pen;

        Box = box;
    }

    public Rectangle Box { get; set; }

    public int X => Box.X;
    public int Y => Box.Y;
    public int Width => Box.Width;
    public int Height => Box.Height;

    public int Top => Box.Top;
    public int Bottom => Box.Bottom;        
    public int Left => Box.Left;
    public int Right => Box.Right;

    public bool Filled { get; set; } = false;

    public virtual Pen Pen { get; set; } = new Pen(Color.Black, 1);

    protected void InvertDraw(Graphics g)
    {
        g.TranslateTransform(BasicEngine.Current.Width / 2, 0);
        g.ScaleTransform(-1, 1);
        g.TranslateTransform(-BasicEngine.Current.Width / 2, 0);

        var newPos = new Point(BasicEngine.Current.Width - Box.X - Box.Width, Box.Y);

        this.Box = new Rectangle(newPos, this.Box.Size);
    }

    public virtual void Draw(Graphics g)
    {
        if(Filled)       
            g.FillRectangle(Pen.Brush, Box);
        else
            g.DrawRectangle(Pen, Box);
    }

    public abstract void Update();
}

public abstract class CollidableBody : Body, ICollidableBody
{
    protected CollidableBody(Rectangle box, Pen pen = null) : base(box, pen)
    {
    }

    public CollisionMask CollisionMask { get; set; }

    public bool IsColling(Rectangle box)
        => box.IntersectsWith(this.Box);
    
    public bool IsCollidingMask(CollidableBody coll)
    {
        return IsColling(coll.CollisionMask.Box);
    }

    public void SetColllisionMask(Rectangle mask)
    {
        this.CollisionMask = new CollisionMask(this, mask);
    }

    public List<T> IsCollidingMaskList<T>(List<T> list)
        where T : CollidableBody
    {
        var query = list.Where(c => this.IsColling(c.CollisionMask.Box)).ToList();
        return query;
    }


    public EventHandler<CollideEventArgs> OnCollide;
}

public class CollideEventArgs : EventArgs
{
    public CollideEventArgs(CollidableBody collider, Type bodyType)
    {
        Collider = collider;
        BodyType = bodyType;
    }

    public CollidableBody Collider { get; set; }
    public Type BodyType { get; set;  }
}