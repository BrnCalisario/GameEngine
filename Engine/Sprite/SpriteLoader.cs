using System;
using System.Collections.Generic;
using System.Drawing;

namespace Engine.Sprites;

public interface ISpriteLoader<T>
    where T : Enum
{
    SpriteStream GetAnimation(T animationType);
    bool Contains(T animationType);
}


public abstract class SpriteLoader<T> : ISpriteLoader<T>
    where T : Enum
{
    public SpriteLoader(int scale = 1)
    {
        this.Scale = scale;
        this.Load();
    }

    protected abstract void Load();

    public Dictionary<T, SpriteStream> Animations { get; private set; } = new Dictionary<T, SpriteStream>();
    public int Scale { get; set; }

    public SpriteStream GetAnimation(T animationType)
        => Animations[animationType];

    public bool Contains(T animationType)
        => Animations.ContainsKey(animationType);
}

public interface ISpriteController<T, S>
        where T : SpriteLoader<S>
        where S : Enum
{
    void StartAnimation(S type);
    void SetOnStreamEnd(S type, EventHandler action);
}

public abstract class SpriteController<T, S> : 
    ISpriteController<T, S>
    where T : SpriteLoader<S>
    where S : Enum
    
{
    public SpriteController()
    {
    }

    protected T SpriteLoader { get; set; }
    public SpriteStream CurrentAnimation { get; set; }
    public Sprite CurrentSprite => CurrentAnimation.Next();

    public virtual void SetOnStreamEnd(S type, EventHandler action)
    {
        if (SpriteLoader.Animations.TryGetValue(type, out SpriteStream value))
            value.OnEndStream += action;
    }

    public virtual void StartAnimation(S type)
    {
        if(SpriteLoader.Contains(type))
        {
            var animation = SpriteLoader.GetAnimation(type);

            if (animation == CurrentAnimation) return;

            this.CurrentAnimation = animation;
        }
    }
}