using System.Drawing;
using System.Linq;


namespace Engine.Sprites;

using Extensions;
using System;
using System.DirectoryServices.ActiveDirectory;

public class ChefSpriteLoader : SpriteLoader<ChefAnimationType>
{
    public ChefSpriteLoader() : base(scale: 3)
    {
    }

    Size SpriteSize = new(30, 40);

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
            Quantity = 3,
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
            Quantity = 3,
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
            Quantity = 3,
            Gap = 1
        };
        this.AddAnimation(walkUpArgs);




        //AQUI


        LoadAnimationArgs cutFrontArgs = new()
        {
            AnimationType = ChefAnimationType.CuttingFront,
            StartingPoint = new Point(0, 0),
            Quantity = 4,
            Gap = 1,
            AnimationTime = TimeSpan.FromSeconds(0.5)
        };
        this.AddAnimation(cutFrontArgs);

        LoadAnimationArgs cutSideArgs = new()
        {
            AnimationType = ChefAnimationType.CuttingSide,
            StartingPoint = new Point(0, scaled.Height),
            Quantity = 4,
            Gap = 1,
            AnimationTime = TimeSpan.FromSeconds(0.5)
        };
        this.AddAnimation(cutSideArgs);

        LoadAnimationArgs cutUpArgs = new()
        {
            AnimationType = ChefAnimationType.CuttingUp,
            StartingPoint = new Point(0, scaled.Height * 2 + 1),
            Quantity = 4,
            Gap = 1,
            AnimationTime = TimeSpan.FromSeconds(0.5)
        };
        this.AddAnimation(cutUpArgs);

        //---------------------

    }

    private void AddAnimation(LoadAnimationArgs args)
    {
        var animationStream = new SpriteStream()
        {
            Interval = args.AnimationTime ?? TimeSpan.FromSeconds(0.25)
        };
        for (int i = args.StartingPoint.X; i < args.Size.Width * args.Quantity + args.Gap; i += args.Size.Width)
        {
            var spr = new Sprite(new Point(i, args.StartingPoint.Y), args.Size);
            animationStream.Add(spr);

            if (animationStream.Sprites.Count() == args.Quantity)
                break;
        }

        if (animationStream.Sprites.Count() > 1)
            animationStream.Add(animationStream.Sprites.Skip(1).Take(1).First());

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
    public TimeSpan? AnimationTime { get; set; } = null;
}

public class ChefSpriteController
    : SpriteController<ChefSpriteLoader, ChefAnimationType>
{
    public ChefSpriteController()
    {
        this.SpriteLoader = new ChefSpriteLoader();
        StartAnimation(ChefAnimationType.IdleFront);
    }

    public static ChefAnimationType GetChefAnimationByDir(Direction dir)
    {
        var type = dir switch
        {
            Direction.Top => ChefAnimationType.CuttingUp,
            Direction.Left or Direction.Right => ChefAnimationType.CuttingSide,
            Direction.Bottom => ChefAnimationType.CuttingFront,
            _ => ChefAnimationType.CuttingFront
        };

        return type;
    }
}

public enum ChefAnimationType
{
    IdleFront,
    IdleSide,
    IdleUp,
    WalkFront,
    WalkSide,
    WalkUp,
    CuttingFront,
    CuttingSide,
    CuttingUp
}

