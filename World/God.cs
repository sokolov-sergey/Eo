using Akka.Actor;
using System;
using World.Maps;
using World.Settlers;

namespace World
{
    public class TheGod : IGod
    {
        readonly ActorSystem Spark = ActorSystem.Create("THE-GOD");
        private readonly IMap Map;

        public int SettlersCount { get; private set; } = 0;

        public TheGod(IMap map)
        {
            this.Map = map;


            //Spark.Scheduler.ScheduleOnce(TimeSpan.FromSeconds(20), () => { Spark.EventStream.Publish(new Freeze()); });
        }

        public void PopulateCell(int x, int y, ISettler s)
        {
            s.Map = Map;
            s.Cell = Map[x, y].Populate(s);
        }

        public ISettler CreateLife(ISettler settler)
        {
            var s = settler.SparkSoul(Spark);
            Spark.EventStream.Subscribe(s.Soul, typeof(Freeze));
            SettlersCount++;
            return s;
        }
    }
}