using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Sprite;
public class ChefSpriteLoader : SpriteLoader<ChefAnimationType>
{
    public ChefSpriteLoader() { }

    Size SpriteSize = new Size(59, 93);

    protected override void Load()
    {
        var startPoint = new Point(3, 3);

        var idleFront = new SpriteStream();
        idleFront.Add(new Sprite(startPoint, SpriteSize));

        Animations.Add(ChefAnimationType.IdleFront, idleFront);

        startPoint = new Point(startPoint.X, SpriteSize.Height + 6);

        var idleSide = new SpriteStream();
        idleSide.Add(new Sprite(startPoint, SpriteSize));

        Animations.Add(ChefAnimationType.IdleSide, idleSide);

        startPoint = new Point(startPoint.X, startPoint.Y + SpriteSize.Height + 3);

        var idleUp = new SpriteStream();
        idleUp.Add(new Sprite(startPoint, SpriteSize));

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