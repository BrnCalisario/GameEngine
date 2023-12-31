﻿using Engine.Extensions;
using Engine.Resource;
using Engine.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine.Kitchen;

public class Oven : Bench
{
   
    public Oven(Rectangle box, Direction dir = Direction.Bottom) : base(new Rectangle(box.Location, new(96, 48)), 1, null, dir)
    {     
        BenchSprite = benchSpriteLoader.GetAnimation(BenchTypes.Oven).Next();
    }

    public CookingTool PlacedItem { get; set; }

    public void SetItem(CookingTool pan)
    {
        this.PlacedItem = pan;
        var temp = PlacedItem.Box.AlignCenter(this.Box);

        PlacedItem.IsCooking = true;      

        var relativePoint = GetRelativeItemPoint(PlacedItem);
        PlacedItem.Box = new Rectangle(relativePoint, temp.Size);
    }

    public override void Interact(Player p)
    {
        var holding = p.holdingItem;

        if(holding as CookingTool is not null)
        {
            PlacedItem = holding as CookingTool;
            PlacedItem.Interact(p);            

            var temp = PlacedItem.Box.AlignCenter(this.Box);
            var relativePoint = GetRelativeItemPoint(PlacedItem);
            PlacedItem.IsCooking = true;

            PlacedItem.Box = new Rectangle(relativePoint, temp.Size);
            return;
        }
        
        if(holding as Food is not null)
        {
            PlacedItem?.Interact(p);
            return;
        }

        if(PlacedItem is not null)
        {         
            PlacedItem.IsCooking = false;
            PlacedItem.Interact(p);

            //PlacedItem.BenchParent = null;
            //PlacedItem.AssignBench(null);
            //this.SetItem(null);

            this.PlacedItem = null;                 
        }
    }
}

