using System.Drawing;
using System.Linq;


namespace Engine.Sprite;

using Extensions;

public class ChefSpriteLoader : SpriteLoader<ChefAnimationType>
{
    public ChefSpriteLoader() : base(scale: 3)
    {
    }

    Size SpriteSize = new(21, 32);

    protected override void Load()
    {
        var scaled = SpriteSize.Scale(this.Scale);
        int startRunning = scaled.Width;

        LoadAnimationArgs idleFrontArgs = new()
        {
            AnimationType = ChefAnimationType.IdleFront,
            StartingPoint = new Point(0, 0),
            Size = scaled
        };
        this.AddAnimation(idleFrontArgs);

        LoadAnimationArgs walkFrontArgs = new()
        {
            AnimationType = ChefAnimationType.WalkFront,
            StartingPoint = new Point(startRunning, 1),
            Quantity = 2,
            Gap = 1,
            Size = scaled
        };
        this.AddAnimation(walkFrontArgs);

        LoadAnimationArgs idleSideArgs = new()
        {
            AnimationType = ChefAnimationType.IdleSide,
            StartingPoint = new Point(0, scaled.Height + 1),
            Size = scaled
        };
        this.AddAnimation(idleSideArgs);

        LoadAnimationArgs walkSideArgs = new()
        {
            AnimationType = ChefAnimationType.WalkSide,
            StartingPoint = new Point(startRunning, scaled.Height + 1),
            Quantity = 2,
            Gap = 1,
            Size = scaled
        };
        this.AddAnimation(walkSideArgs);

        LoadAnimationArgs idleUpArgs = new()
        {
            AnimationType = ChefAnimationType.IdleUp,
            StartingPoint = new Point(0, scaled.Height * 2 + 1),
            Size = scaled
        };
        this.AddAnimation(idleUpArgs);

        LoadAnimationArgs walkUpArgs = new()
        {
            AnimationType = ChefAnimationType.WalkUp,
            StartingPoint = new Point(startRunning, scaled.Height * 2 + 1),
            Size = scaled,
            Quantity = 2,
            Gap = 1
        };
        this.AddAnimation(walkUpArgs);
    }

    private void AddAnimation(LoadAnimationArgs args)
    {
        var animationStream = new SpriteStream();
        for(int i = args.StartingPoint.X; i < args.Size.Width * args.Quantity + args.Gap; i += args.StartingPoint.X)
        {
            var spr = new Sprite(new Point(i, args.StartingPoint.Y), args.Size);
            animationStream.Add(spr);

            if (animationStream.Sprites.Count() == args.Quantity)
                break;
        }

        this.Animations.Add(args.AnimationType, animationStream);
    }
}

public class LoadAnimationArgs
{
    public ChefAnimationType AnimationType;
    public Point StartingPoint { get; set; }
    public int Quantity { get; set; } = 1;
    public Size Size { get; set; }
    public int Gap { get; set; } = 0;
}



public class ChefSpriteController
    : SpriteController<ChefSpriteLoader, ChefAnimationType>
{
    public ChefSpriteController()
    {
        this.SpriteLoader = new ChefSpriteLoader();
        StartAnimation(ChefAnimationType.IdleFront);
    }




}

public enum ChefAnimationType
{
    IdleFront,
    IdleSide,
    IdleUp,
    WalkFront,
    WalkSide,
    WalkUp
}

