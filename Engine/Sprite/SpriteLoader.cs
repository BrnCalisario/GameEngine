using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Sprite;

public abstract class SpriteLoader<T> where T : struct
{
    public SpriteLoader()
    {
        if (!typeof(T).IsEnum)
            throw new ArgumentException("Sprite Loader must be from enum");

        this.Load();
    }

    protected abstract void Load();

    protected Dictionary<T, SpriteStream> Animations { get; set; } = new Dictionary<T, SpriteStream>();

    public SpriteStream GetAnimation(T animationType)
        => Animations[animationType];
}

//public class ChefSpriteLoader<ChefAnimationType>
//{
//    protected override void Load()
//    {

//    }
//}


public enum ChefAnimationType
{
    LookingDown,
    LookingUp,
    LookingSides,
}