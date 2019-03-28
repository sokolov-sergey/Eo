using Akka.Actor;
using World.Maps;
using World.Settlers;

namespace World
{    
    public class TheGod : IGod
    {
        ActorSystem Spark = ActorSystem.Create("THE-GOD");
        private readonly IMap Map;

        public TheGod(IMap map)
        {
            this.Map = map;            
        }

        public void PopulateCell(int x, int y, ISettler s)
        {
            s.Map = Map;            
            s.Cell = Map[x, y].Populate(s);
        }

        public ISettler CreateLife(ISettler settler) => settler.SparkSoul(Spark);
    }
}