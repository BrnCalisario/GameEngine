using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine.Sprite;

public class SpriteStream
{
    public SpriteStream() { }


    private TimeSpan Interval = TimeSpan.FromSeconds(0.25);
    private int pointer { get; set; } = -1;
    private List<Sprite> sprites { get; set; } = new List<Sprite>();

    public DateTime StartStream { get; set; } = default;

    public IEnumerable<Sprite> Sprites => sprites;


    public void Add(Sprite sprite)
        => sprites.Add(sprite);

    public void Remove(Sprite sprite)
        => sprites.Remove(sprite);

    public Sprite Next()
    {
        if(StartStream == default)
            StartStream = DateTime.Now;
        
        var diff = DateTime.Now - StartStream;

        int actualSprite = (int)(diff / Interval);

        if(actualSprite >= sprites.Count)
        {
            StartStream = DateTime.Now;
            actualSprite = sprites.Count - 1;
            OnEndStream?.Invoke(this, EventArgs.Empty);
        }

        return sprites[actualSprite];
    }

    public void Reset()
        => pointer = 0;

    public event EventHandler OnEndStream;

}
