using System;
using System.Collections.Generic;
using System.Drawing;

namespace Engine.Sprite;


public interface ISpriteLoader<T>
    where T : Enum
{
    SpriteStream GetAnimation(T animationType);
    bool Contains(T animationType);
}


public abstract class SpriteLoader<T> : ISpriteLoader<T>
    where T : Enum
{
    public SpriteLoader()
    {
        this.Load();
    }

    protected abstract void Load();

    public Dictionary<T, SpriteStream> Animations { get; private set; } = new Dictionary<T, SpriteStream>();

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
        if (!typeof(S).IsEnum)
            throw new ArgumentException("Sprite Loader must be from enum");
    }

    protected T SpriteLoader { get; set; }
    public SpriteStream CurrentAnimation { get; set; }

    public virtual void SetOnStreamEnd(S type, EventHandler action)
    {
        if (SpriteLoader.Animations.TryGetValue(type, out SpriteStream value))
            value.OnEndStream += action;
    }

    public virtual void StartAnimation(S type)
    {
        if(SpriteLoader.Contains(type))
            this.CurrentAnimation = SpriteLoader.GetAnimation(type);
    }
}