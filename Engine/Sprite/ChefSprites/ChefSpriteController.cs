using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Sprite;
public class ChefSpriteLoader : SpriteLoader<ChefAnimationType>
{
    public ChefSpriteLoader() : base(3)
    {
    }

    Size SpriteSize = new Size(21, 32);

    protected override void Load()
    {
        var scaled = SpriteSize.Scale(this.Scale);

        var startPoint = new Point(0, 0);

        var idleFront = new SpriteStream();
        idleFront.Add(new Sprite(startPoint, scaled));
        Animations.Add(ChefAnimationType.IdleFront, idleFront);

        int startRunning = 22 * this.Scale;

        var runningFront = new SpriteStream();
        while (runningFront.Sprites.Count() < 10)
        {
            for (int i = startRunning; i < startRunning * 2 + 1; i += startRunning)
            {
                Sprite spr = new Sprite(new Point(i, 1), scaled);
                runningFront.Add(spr);
            }
        }

        Animations.Add(ChefAnimationType.WalkFront, runningFront);

        startPoint = new Point(startPoint.X, scaled.Height + 3);

        var idleSide = new SpriteStream();
        idleSide.Add(new Sprite(startPoint, scaled));

        Animations.Add(ChefAnimationType.IdleSide, idleSide);

        startPoint = new Point(startPoint.X, startPoint.Y + scaled.Height + 3);

        var idleUp = new SpriteStream();
        idleUp.Add(new Sprite(startPoint, scaled));

        Animations.Add(ChefAnimationType.IdleUp, idleUp);
    }
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

public static class ExtensionSize
{
    public static Size Scale(this Size size, int scale)
        => new(size.Width * scale, size.Height * scale);

    public static Size ScaleX(this Size size, int scale)
        => new(size.Width * scale, size.Height);

    public static Size ScaleY(this Size size, int scale)
        => new(size.Width, size.Height * scale);

}
