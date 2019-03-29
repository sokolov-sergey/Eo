using System;
using System.Collections.Generic;
using System.Text;
using World.Maps;
using World.Settlers;
using World.Settlers.Plants;

namespace World
{
    public class MessageOfTheGod
    {
    }

    public class Freeze : MessageOfTheGod
    {
    }


    public interface IGod
    {
        void PopulateCell(int x,int y, ISettler p);
        ISettler CreateLife(ISettler plant);

        int SettlersCount { get; }
    }
}
