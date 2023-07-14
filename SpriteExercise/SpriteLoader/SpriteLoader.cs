using SpriteExercise.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteExercise.SpriteController;

public abstract class SpriteLoader
{
    protected Dictionary<AnimationType, SpriteStream> Animations { get; set; } = new Dictionary<AnimationType, SpriteStream>();
    public SpriteStream GetAnimation(AnimationType animationType)
        => Animations[animationType];
    public abstract void LoadSprites();
    public SpriteLoader()
    {
        LoadSprites();
    }
}