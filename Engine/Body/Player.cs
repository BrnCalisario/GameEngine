using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Engine.Sprite;
using System.Linq;

namespace Engine;

public class Player : CollidableBody
{
    private readonly int speed = 10;
    
    private bool invert = false;
    
    private bool walking = true;

    public Image ChefSprite = Image.FromFile("../../../../assets/player.png");

    public SpriteController<ChefSpriteLoader, ChefAnimationType> SpriteController
        = new ChefSpriteController();

    public Player(Rectangle box, Pen pen = null) : base(box, pen)
    {
        this.Box = new Rectangle(box.X, box.Y, 63, 96);

        this.SetColllisionMask(new Rectangle(13, this.Height / 2, 40, this.Height / 2));
    }

    public Direction CurrentDirection = Direction.Bottom;


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

        if (invert)
            this.InvertDraw(g);


        g.DrawRectangle(Pen, this.Box);
        g.DrawString($"{CurrentDirection}", SystemFonts.DefaultFont, Pen.Brush, new Point(1, 30));

        if (CollisionMask is not null)
            g.DrawRectangle(Pen, this.CollisionMask.Box);
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

        float velX = keyMap[Keys.A] ? speed * -1 : keyMap[Keys.D] ? speed : 0;
        float velY = keyMap[Keys.W] ? speed * -1 : keyMap[Keys.S] ? speed : 0;

        if (velX != 0 && velY != 0)
        {
            velX *= 0.707f;
            velY *= 0.707f;
        }

        var newPos = WallCollide(velX, velY);

        this.Box = new Rectangle(newPos, this.Box.Size);
        CollisionMask?.UpdatePoint(newPos);

        walking = (keyMap[Keys.A] || keyMap[Keys.D] || keyMap[Keys.W] || keyMap[Keys.S]);
    }

    private Point WallCollide(float velX, float velY)
    {
        var maskRect = this.CollisionMask.Box;

        var newVelX = 0;

        var previewRectX = maskRect;
        previewRectX.X += (int)velX;

        if (previewRectX.AnyPlaceMeeting(BasicEngine.Current.Walls))
        {
            maskRect.X += MathF.Sign(velX);
            while (!maskRect.AnyPlaceMeeting(BasicEngine.Current.Walls))
            {
                maskRect.X += MathF.Sign(velX);
                newVelX += MathF.Sign(velX);
            }
            velX = 0;
        }

        var previewRectY = this.CollisionMask.Box;
        previewRectY.Y += (int)velY;

        var newVelY = 0;

        if (previewRectY.AnyPlaceMeeting(BasicEngine.Current.Walls))
        {
            maskRect.Y += MathF.Sign(velY);
            while (!maskRect.AnyPlaceMeeting(BasicEngine.Current.Walls))
            {
                maskRect.Y += MathF.Sign(velY);
                newVelY += MathF.Sign(velY);
            }
            velY = 0;
        }

        return new Point(this.Box.X + (int)velX + newVelX, this.Box.Y + (int)velY + newVelY);
    }
}

public enum Direction
{
    Top,
    Bottom,
    Left,
    Right
}

public static class RectangleExtensions
{
    public static bool AnyPlaceMeeting(this Rectangle rec, List<CollidableBody> collList)
    {
        foreach (var coll in collList)
        {
            if (coll.IsColling(rec))
                return true;
        }
        return false;
    }
}