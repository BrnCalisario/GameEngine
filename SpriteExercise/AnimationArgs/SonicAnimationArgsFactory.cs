using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteExercise.SpriteController;

public class SonicAnimationArgsFactory : AnimationArgsFactory
{
    public SonicAnimationArgsFactory(SpriteLoader spriteLoader)
        : base(spriteLoader) { }

    protected override AnimationArgs GetIdleArgs()
    {
        return new AnimationArgs()
        {
            Animation = spriteLoader.GetAnimation(AnimationType.Idle),
            Interval = 350
        };
    }

    protected override AnimationArgs GetJumpingArgs()
    {
        return new AnimationArgs()
        {
            Animation = spriteLoader.GetAnimation(AnimationType.Jumping),
            Interval = 150
        };
    }

    protected override AnimationArgs GetRunningArgs()
    {
        return new AnimationArgs()
        {
            Animation = spriteLoader.GetAnimation(AnimationType.Running),
            Interval = 30
        };

    }
}