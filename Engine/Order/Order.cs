using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine;

internal class Order
{
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