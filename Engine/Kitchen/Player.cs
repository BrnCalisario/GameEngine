using System;
using System.Drawing;
using System.Windows.Forms;
using Engine.Sprites;
using System.Linq;

namespace Engine;

using static ChefSpriteController;


using Extensions;

using Resource;
using System.Drawing.Drawing2D;
using System.Data;

public class Player : CollidableBody
{
    public Player(Rectangle box, Pen pen = null) : base(box, pen)
    {
        this.Box = new Rectangle(box.X, box.Y, 60, 90);
        this.SetColllisionMask(new Rectangle(this.Box.Width / 2 - 12, this.Height / 2 + 16, 26, (int)(this.Height / 3.5)));
    }

    private readonly int speed = 10;

    public bool Cutting = false; //AQUI

    public bool Invert { get; private set; } = false;

    public bool canWalk = true;

    private bool walking = true;

    public Item holdingItem = null;

    public bool IsHolding => holdingItem is not null;

    private DateTime LastInteraction = DateTime.Now;

    private readonly TimeSpan InteractionDelay = TimeSpan.FromSeconds(0.25);

    private CollisionMask InteractionHitBox { get; set; }

    private bool CanInteract()
    {
        var diff = DateTime.Now - LastInteraction;
        return diff >= InteractionDelay;
    }

    public Image ChefSprite = Resources.CookerImage;
    public Image ChefCuttingSprite = Resources.CuttingImage; //AQUI

    public SpriteController<ChefSpriteLoader, ChefAnimationType> SpriteController
        = new ChefSpriteController();


    public Direction CurrentDirection = Direction.Bottom;

    public override void Draw(Graphics g)
    {
        var c = SpriteController.CurrentAnimation.Next();

        GraphicsContainer container = null;

        if (Invert)
        {
            container = this.InvertHorizontal(g);
            var newPos = new Point(BasicEngine.Current.Width - Box.X - Box.Width, Box.Y);
            this.Box = new Rectangle(newPos, this.Box.Size);
        }

        if (Cutting) //AQUI
        {
            this.SpriteController.StartAnimation(GetChefAnimationByDir(this.CurrentDirection));
            c = SpriteController.CurrentAnimation.Next();
        }

        if (Cutting)
        {
            g.DrawImage(
                ChefCuttingSprite, //AQUI
                this.Box,
                c.X,
                c.Y,
                c.Width,
                c.Height,
                GraphicsUnit.Pixel
            );
        }
        else
        {
            g.DrawImage(
                ChefSprite, //AQUI
                this.Box,
                c.X,
                c.Y,
                c.Width,
                c.Height,
                GraphicsUnit.Pixel
            );
        }



        if (container is not null)
        {
            g.EndContainer(container);
            var newPos = new Point(BasicEngine.Current.Width - Box.X - Box.Width, Box.Y);
            this.Box = new Rectangle(newPos, this.Box.Size);
        }

        //g.DrawRectangle(Pen, this.Box);
        g.DrawString($"{CurrentDirection}", SystemFonts.DefaultFont, Pen.Brush, new Point(1, 30));
        g.DrawString($"X:{X}, Y:{Y}", SystemFonts.DefaultFont, Pen.Brush, new Point(1, 90));


        if (InteractionHitBox is not null)
            g.DrawRectangle(new Pen(Color.Red), InteractionHitBox.Box);

        if (CollisionMask is not null)
            g.DrawRectangle(Pen, this.CollisionMask.Box);
    }

    private void FindInteraction()
    {
        SetInteractionMask();

        var items = InteractionHitBox.IsCollidingMaskList(BasicEngine.Current.Interactables);

        if (items.Count < 1)
            return;

        items = items.OrderBy(i =>
        {
            if (i is Plate && this.holdingItem is CookingTool)
                return 0;
            if (i is FoodBench bench && bench.PlacedItem is not null)
                return 1;
            if (i is Item item && item != holdingItem)
                return 2;
            if (i is Bench)
                return 3;
            return 4;
        }).ToList();

        var selected = items.FirstOrDefault();

        selected.Interact(this);
        this.LastInteraction = DateTime.Now;
    }

    private void SetInteractionMask()
    {
        if (this.IsHolding)
        {
            var size = this.holdingItem.Box.Size.Scale(1.25f);
            var scaledRect = new Rectangle(new Point(), size).AlignCenter(holdingItem.Box);
            InteractionHitBox = new(null, scaledRect);
            return;
        }

        var distX = CurrentDirection == Direction.Right ? Width / 2 : 0;
        var distY = CurrentDirection == Direction.Bottom ? Height / 2 : 0;

        if (CurrentDirection.IsHorizontal())
        {
            distY = Height / 2;
        }

        float sizeX = CurrentDirection.IsHorizontal() ? 2 : 1;
        var sizeY = CurrentDirection.IsVertical() ? 2 : 2;

        if (CurrentDirection.IsVertical())
        {
            sizeX = 2.1f;
            distY += CurrentDirection == Direction.Top ? -16 : 16;
        }

        var rect = new Rectangle(X + distX, Y + distY, (int)(Width / sizeX), (int)(Height / sizeY));

        if (CurrentDirection.IsVertical())
        {
            rect.X = CollisionMask.X;
        }

        InteractionHitBox = new(null, rect);
    }

    public override void Update()
    {
        var keyMap = BasicEngine.Current.KeyMap;

        var lastDir = CurrentDirection;

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

        if (keyMap[Keys.E] && CanInteract())
        {
            FindInteraction();
        }

        this.Move();
        this.ChangeSprite();
    }

    private void ChangeSprite()
    {
        var keyMap = BasicEngine.Current.KeyMap;

        if (keyMap[Keys.D] || keyMap[Keys.A])
            this.Invert = !keyMap[Keys.D] || keyMap[Keys.A];

        ChefAnimationType animation = CurrentDirection switch
        {
            Direction.Top => walking ? ChefAnimationType.WalkUp : ChefAnimationType.IdleUp,
            Direction.Right or Direction.Left => walking ? ChefAnimationType.WalkSide : ChefAnimationType.IdleSide,
            Direction.Bottom => walking ? ChefAnimationType.WalkFront : ChefAnimationType.IdleFront,
            _ => ChefAnimationType.IdleFront
        };

        SpriteController.StartAnimation(animation);
    }

    private void Move()
    {
        if (Cutting) return;

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

        walking = keyMap[Keys.A] || keyMap[Keys.D] || keyMap[Keys.W] || keyMap[Keys.S];
    }

    private Point WallCollide(float velX, float velY)
    {
        var maskRect = this.CollisionMask.Box;
        var newVelX = 0;

        var previewRectX = maskRect;
        previewRectX.X += (int)velX;

        if (previewRectX.AnyPlaceMeeting(BasicEngine.Current.Walls, out CollidableBody w))
        {
            maskRect.X += MathF.Sign(velX);
            while (!maskRect.IntersectsWith(w.CollisionMask.Box))
            {
                maskRect.X += MathF.Sign(velX);
                newVelX += MathF.Sign(velX);
            }
            velX = 0;
        }

        var previewRectY = this.CollisionMask.Box;
        previewRectY.Y += (int)velY;
        var newVelY = 0;

        if (previewRectY.AnyPlaceMeeting(BasicEngine.Current.Walls, out CollidableBody c))
        {
            maskRect.Y += MathF.Sign(velY);
            while (!maskRect.IntersectsWith(c.CollisionMask.Box))
            {
                maskRect.Y += MathF.Sign(velY);
                newVelY += MathF.Sign(velY);
            }
            velY = 0;
        }

        return new Point(this.Box.X + (int)velX + newVelX, this.Box.Y + (int)velY + newVelY);
    }

    public void AssignItem(Item i)
    {
        if (this.holdingItem is not null) return;

        this.holdingItem = i;
        this.holdingItem.AssignPlayer(this);
    }
}



public enum Direction
{
    Top,
    Bottom,
    Left,
    Right
}

public static class DirectionExtension
{
    public static bool IsHorizontal(this Direction direction)
        => direction == Direction.Left || direction == Direction.Right;

    public static bool IsVertical(this Direction direction)
        => direction == Direction.Top || direction == Direction.Bottom;
}