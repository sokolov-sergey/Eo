using Akka.Actor;
using System;
using World.Maps;
using World.Settlers;

namespace World
{

    public class TheGod : IGod
    {
        public class AbsoluteKnowledge : ReceiveActor
        {
            private TheGod God;

            public AbsoluteKnowledge(TheGod god)
            {
                God = god;
                Receive<Spawn>(m => SpawnASettler(m));
                Receive<string>(m => CleanupMap(), m => "CLEANUP".Equals(m));

                Context.System.Scheduler.ScheduleTellRepeatedly(
                    TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1),
                    Self, "CLEANUP",Self);
            }

            private void CleanupMap()
            {
                for (int i = 0; i < God.Map.Height; i++)
                    for (int j = 0; j < God.Map.Width; j++)
                    {
                        var c = God.Map[j, i];
                        var stlr = c.Settler;
                        if (stlr != null && stlr.Soul == null)
                        {
                            c.Populate(null);
                            c.CellType = CellType.Dead;
                        }
                    }
            }

            private void SpawnASettler(Spawn m)
            {
                God.CreateLife(m.Settler);
            }
        }

        readonly ActorSystem Spark = ActorSystem.Create("THE-GOD");
        private readonly IMap Map;
        private readonly IActorRef Knowledge;

        public int SettlersCount { get; private set; } = 0;

        public int SoulActorsCount => 0;

        public TheGod(IMap map)
        {
            this.Map = map;

            Knowledge = Spark.ActorOf(Props.Create<AbsoluteKnowledge>(this));
            Spark.EventStream.Subscribe(Knowledge, typeof(Spawn));
            

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