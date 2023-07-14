using SpriteExercise.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteExercise.SpriteController;

public class AnimationArgs : EventArgs
{
    public int Interval { get; set; }
    public SpriteStream Animation { get; set; }
}
