using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteExercise.Sprites;

public class SpriteStream
{
    public SpriteStream() { }
    public SpriteStream(List<Sprite> spriteList)
    {
        sprites = spriteList;
    }

    private int pointer { get; set; } = -1;

    private List<Sprite> sprites = new();
    public IEnumerable<Sprite> Sprites => sprites;

    public void Add(Sprite sprite)
        => sprites.Add(sprite);

    public void Remove(Sprite sprite)
        => sprites.Remove(sprite);

    public Sprite Next()
    {
        pointer++;
        if (pointer >= sprites.Count)
        {
            var lastPoint = sprites.Count() - 1;
            OnEndStream?.Invoke(this, EventArgs.Empty);
            Reset();
            return sprites[lastPoint];
        }

        return sprites[pointer];
    }

    public void Reset()
        => pointer = 0;

    public event EventHandler OnEndStream;
}