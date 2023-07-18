using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Engine.Sprite;

namespace Engine;

public class Player : CollidableBody
{
    private readonly int Speed = 10;

    public Image ChefSprite = Image.FromFile("../../../../assets/player.png");

    public SpriteController<ChefSpriteLoader, ChefAnimationType> SpriteController 
        = new ChefSpriteController();

    public Player(Rectangle box, Pen pen = null) : base(box, pen)
    {
        this.Box = new Rectangle(box.X, box.Y, 59, 93);
    }

    public override void Draw(Graphics g)
    {
        var c = SpriteController.CurrentAnimation.Next();

        g.DrawImage(
            ChefSprite,
            this.Box,
            c.X,
            c.Y,
            c.Width,
            c.Height,
            GraphicsUnit.Pixel            
            );

        if(CollisionMask is not null)
            g.DrawRectangle(Pen, this.CollisionMask.Mask);
    }

    public override void Update()
    {
        this.Move();
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
}