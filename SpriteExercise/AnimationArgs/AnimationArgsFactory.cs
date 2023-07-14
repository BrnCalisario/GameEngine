using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteExercise.SpriteController;

public abstract class AnimationArgsFactory
{
    public AnimationArgsFactory(SpriteLoader spriteLoader)
    {
        this.spriteLoader = spriteLoader;
    }

    protected SpriteLoader spriteLoader { get; private set; }

    protected abstract AnimationArgs GetRunningArgs();
    protected abstract AnimationArgs GetIdleArgs();
    protected abstract AnimationArgs GetJumpingArgs();

    public virtual AnimationArgs GetAnimationArgs(AnimationType type)
        => type switch
        {
            AnimationType.Running => GetRunningArgs(),
            AnimationType.Idle => GetIdleArgs(),
            AnimationType.Jumping => GetJumpingArgs(),
            _ => throw new Exception()
        };
}
