﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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

    protected GraphicsContainer InvertHorizontal(Graphics g, GraphicsContainer container = null)
    {
        container ??= g.BeginContainer();

        g.TranslateTransform(BasicEngine.Current.Width / 2, 0);
        g.ScaleTransform(-1, 1);
        g.TranslateTransform(-BasicEngine.Current.Width / 2, 0);

        //var newPos = new Point(BasicEngine.Current.Width - Box.X - Box.Width, Box.Y);
        //this.Box = new Rectangle(newPos, this.Box.Size);

        return container;
    }

    protected GraphicsContainer InvertVertical(Graphics g, GraphicsContainer container = null)
    {
        container ??= g.BeginContainer();

        int hei = BasicEngine.Current.Height;

        g.TranslateTransform(0, hei / 2);
        g.ScaleTransform(1, -1);
        g.TranslateTransform(0, -hei / 2);
        
        //var newPos = new Point(Box.X, hei - Box.Y - Box.Height);
        //this.Box = new Rectangle(newPos, this.Box.Size);      
        
        return container;
    }

    public virtual void Draw(Graphics g)
    {
        if(Filled)       
            g.FillRectangle(Pen.Brush, Box);
        else
            g.DrawRectangle(Pen, Box);
    }

    public virtual void Update() { }
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

    public override void Draw(Graphics g)
    {
        base.Draw(g);

        //this.CollisionMask?.Draw(g);

    }

    protected Rectangle CorrectHorizontal(Rectangle box)
    {        
        var newPos = new Point(BasicEngine.Current.Width - box.X - box.Width, box.Y);
        return new Rectangle(newPos, box.Size);
    }

    protected Rectangle CorrectVertical(Rectangle box)
    {
        var newPos = new Point(box.X, BasicEngine.Current.Height - box.Y - box.Height);
        return new Rectangle(newPos, box.Size);
    }    
}