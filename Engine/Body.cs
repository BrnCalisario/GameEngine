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
    bool IsColling(CollidableBody body);
    List<CollidableBody> IsCollidingList(List<CollidableBody> list);
}

public abstract class Body : IBody
{
    public Body(Rectangle box, Pen pen = null)
    {
        if (pen is not null)
            this.Pen = pen;

        this.Box = box;
    }

    public Rectangle Box { get; set; }
    public int X => Box.X;
    public int Y => Box.Y;
    public int Width => Box.Width;
    public int Height => Box.Height;

    public virtual Pen Pen { get; set; } = new Pen(Color.Black, 1);

    public abstract void Draw(Graphics g);
    public abstract void Update();
}

public abstract class CollidableBody : Body, ICollidableBody
{
    protected CollidableBody(Rectangle box, Pen pen = null) : base(box, pen)
    {
    }

    public bool IsColling(Rectangle box)
        => box.IntersectsWith(this.Box);

    public bool IsColling(CollidableBody body)
        => IsColling(body.Box);


    public List<CollidableBody> IsCollidingList(List<CollidableBody> list)
    {
        var query = list.Where(c => c.IsColling(this)).ToList();
        return query;    
    }

}

// EXEMPLO COLISAO

//public class Bot : CollidableBody
//{
//    public Bot(Rectangle box, Pen pen = null) : base(box, pen) {  }

//    public override void Draw(Graphics g)
//    {
//        g.DrawRectangle(this.Pen, this.Box);
//    }

//    bool goingRight = true;

//    public override void Update()
//    {
//        int increment = 30;
//        if (!goingRight)
//            increment *= -1;

//        if(this.Box.X > 400)
//            goingRight = false;

//        if(this.Box.X < 20)
//            goingRight = true;

//        var pos = new Point(Box.X + increment, Box.Y + increment);
//        this.Box = new Rectangle(pos, this.Box.Size);
//    }
//}


// EXEMPLO PLAYER

//public class Player : Body
//{
//    public Player(Rectangle box, Pen pen = null) : base(box, pen)
//    {

//    }

//    bool growing = true;

//    public override void Draw(Graphics g)
//    {
//        g.DrawRectangle(this.Pen, this.Box);
//    }

//    public override void Update()
//    {
//        if (this.Width > 500)
//            growing = false;

//        if (this.Width < 30)
//            growing = true;

//        Size size;
//        int increment = 15;

//        if (!growing)
//            increment *= -1;

//        size = new Size(Box.Width + increment, Box.Height + increment);
//        this.Box = new Rectangle(Box.Location, size);
//    }
//}

