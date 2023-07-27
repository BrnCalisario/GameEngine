using Engine.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Engine;

public class Order
{ 
    public string Name { get; set; }
    public string Description { get; set; }
    public OrderType Type { get; set; }
}

public class OrderTab : Body
{
    public OrderTab() 
        : base(new(), Pens.Black)
    {
        this.orders = new List<Order>();

        var gameBox = BasicEngine.Current.Box;

        var tileSetBox = BasicEngine.Current.tileSet.Box;

        var size = new Size(tileSetBox.Width, 160);

        var pos = new Point(tileSetBox.X, tileSetBox.Y - size.Height - 30);

        var rect = new Rectangle(pos, size);
        
        this.Box = rect;

        LoadPossibilities();
    }

    private void LoadPossibilities()
    {
        Possibilities = new List<Order>();

        Order o1 = new()
        { 
            Name = "Sopa de Tomate",
            Type = OrderType.TomatoSoup,    
        };
        Possibilities.Add(o1);

        Order o2 = new()
        {
            Name = "Sopa de Cebola",
            Type = OrderType.OnionSoup,
        };
        Possibilities.Add(o2);

        Order o3 = new()
        {
            Name = "Sopa Mista",
            Type = OrderType.MixedSoup,
        };
        Possibilities.Add(o3);

        Order o4 = new()
        {
            Name = "Bife Assado",
            Type = OrderType.Steak,
        };
        Possibilities.Add(o4);

        Order o5 = new()
        {
            Name = "Peixinho",
            Type = OrderType.Fish,
        };
        Possibilities.Add(o5);
    }

    private static OrderTab current = null;
    public static OrderTab Current
    {
        get
        {
            current ??= new OrderTab();
            return current;
        }
    }

    public static void New()
        => current = new OrderTab();


    private List<Order> orders;
    public IEnumerable<Order> Orders => orders;

    public List<Order> Possibilities;

    public TimeSpan OrderCoolDown { get; set; } = TimeSpan.FromSeconds(15);
    public DateTime LastOrderTime { get; set; } = DateTime.Now;

    public void CompleteOrder(Order order)
    {
        this.orders.Remove(order);
    }

    public override void Draw(Graphics g)
    {
        var pos = this.Box.Location;

        g.DrawString("Order Tab", SystemFonts.MenuFont, Pen.Brush, pos.X, pos.Y);

        g.DrawRectangle(Pen, this.Box);

        for(int i = 0; i < orders.Count; i++)
        {
            var orderPos = new Point(pos.X + 15 + (130 * i + 15 * i), pos.Y + 15);
            DrawOrder(g, orderPos, this.orders[i]);
        }
    }

    public override void Update()
    {
        if (orders.Count > 8) return;

        var diff = DateTime.Now - LastOrderTime;

        if (diff >= OrderCoolDown)
        {
            GetRandomOrder();
            this.LastOrderTime = DateTime.Now;
        }
    }

    private void GetRandomOrder()
    {

        var randV = Random.Shared.Next(this.Possibilities.Count - 1);
        
        var randOrder = this.Possibilities[randV];

        this.orders.Add(randOrder);
    }

    private void DrawOrder(Graphics g, Point pos, Order order)
    {
        var orderRect = new Rectangle(pos, new(130, 130));
        g.DrawRectangle(Pens.Blue, orderRect);
        g.DrawString($"{order.Name}", SystemFonts.MenuFont, Pen.Brush, pos.X, pos.Y + 130 / 2 - 8);
    }


}

public enum OrderType : byte
{
    TomatoSoup =1 ,
    OnionSoup = 2,
    MixedSoup = 4,
    Steak = 8,
    Fish = 16,
    InvalidOrder = 32,
}

public static class OrderExtensions
{
    public static bool IsProtein(this OrderType order)
        => order == OrderType.Steak || order == OrderType.Fish;
}