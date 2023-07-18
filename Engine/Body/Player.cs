using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Engine.Sprite;
using System.Linq;

namespace Engine;

public class Player : CollidableBody
{
    private readonly int Speed = 10;
    private bool invert = false;

    public Image ChefSprite = Image.FromFile("../../../../assets/player.png");

    public SpriteController<ChefSpriteLoader, ChefAnimationType> SpriteController 
        = new ChefSpriteController();

    public Player(Rectangle box, Pen pen = null) : base(box, pen)
    {
        this.Box = new Rectangle(box.X, box.Y, 63, 96);
    }

    public Direction CurrentDirection = Direction.Bottom;

    private bool walking = true;

    public override void Draw(Graphics g)
    {
        var c = SpriteController.CurrentSprite;

        if (invert)
            this.InvertDraw(g);

        g.DrawImage(
            ChefSprite,
            this.Box,
            c.X,
            c.Y,
            c.Width,
            c.Height,
            GraphicsUnit.Pixel            
            );

        if(invert)
            this.InvertDraw(g);


        g.DrawRectangle(Pen, this.Box);
        g.DrawString($"{CurrentDirection.ToString()}", SystemFonts.DefaultFont, Pen.Brush, new Point(500, 60));

        if (CollisionMask is not null)
            g.DrawRectangle(Pen, this.CollisionMask.Mask);
    }

    public override void Update()
    {
        var keyMap = BasicEngine.Current.KeyMap;
        var f = keyMap.FirstOrDefault(c => c.Value).Key;
        CurrentDirection =
            f switch
            {
                Keys.W => Direction.Top,
                Keys.D => Direction.Right,
                Keys.A => Direction.Left,
                Keys.S => Direction.Bottom,
                _ => CurrentDirection,
            };

        this.Move();
        this.ChangeSprite();
    }

    private void ChangeSprite()
    {
        var keyMap = BasicEngine.Current.KeyMap;

        if (keyMap[Keys.D] || keyMap[Keys.A])
            this.invert = !keyMap[Keys.D] || keyMap[Keys.A];

        ChefAnimationType result = CurrentDirection switch
        {
            Direction.Top => walking ? ChefAnimationType.WalkUp : ChefAnimationType.IdleUp,
            Direction.Right or Direction.Left => walking ? ChefAnimationType.WalkSide : ChefAnimationType.IdleSide,
            Direction.Bottom => walking ? ChefAnimationType.WalkFront : ChefAnimationType.IdleFront,
            _ => ChefAnimationType.IdleFront
        };

        SpriteController.StartAnimation(result);
    }

    private void Move()
    {
        var keyMap = BasicEngine.Current.KeyMap;

        float velX = keyMap[Keys.A] ? Speed * -1 : keyMap[Keys.D] ? Speed : 0;
        float velY = keyMap[Keys.W] ? Speed * -1 : keyMap[Keys.S] ? Speed : 0;

        if(velX != 0 && velY != 0)
        { 
            velX *= 0.707f;
            velY *= 0.707f;
        }

        var newPos = this.IncrementPoint(velX, velY);

        this.Box = new Rectangle(newPos, this.Box.Size);
        CollisionMask?.UpdatePoint(newPos);

        walking = (keyMap[Keys.A] || keyMap[Keys.D] || keyMap[Keys.W] || keyMap[Keys.S]);
    }

    private Point IncrementPoint(float velx, float vely)
    {
        int gap = 15;

        float incX = velx;
        float incY = vely;

        if ((X + Width >= BasicEngine.Current.Width - gap && velx > 0) || (X <= 0 + gap && velx < 0))
            incX = 0;

        if ((Y + Height >= BasicEngine.Current.Height - gap && vely > 0) || (Y <= 0 + gap && vely < 0))
            incY = 0;


        return new Point((int)(X + incX), (int)(Y + incY));   
    }

    private bool AnyPlaceMeeting(Point p, Size size, List<CollidableBody> collList)
    {
        var r = new Rectangle(p, size);

        foreach(var coll in collList) 
        {
            if (coll.IsColling(r))
                return true;
        }
        return false;  
    }
}

public enum Direction
{
    Top,
    Bottom,
    Left,
    Right
}