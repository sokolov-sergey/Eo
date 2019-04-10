using System;
using System.Collections.Generic;
using System.Text;
using World.Maps;
using World.Settlers;
using World.Settlers.Plants;

namespace World
{
    public delegate object Spark(object organism);

    public class MessageOfTheGod
    {
    }

    public class Freeze : MessageOfTheGod
    {
    }
    
    public class Spawn: MessageOfTheGod
    {
        public ISettler Settler;
        public Spawn(ISettler settler)
        {
            Settler = settler;
        }
    }


    public interface IGod
    {
        void PopulateCell(int x,int y, ISettler p);
        ISettler CreateLife(ISettler plant);

        int SettlersCount { get; }
        int SoulActorsCount { get; }
    }
}
