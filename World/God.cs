using Akka.Actor;
using Akka.Routing;
using System;
using System.Collections.Generic;
using World.Maps;
using World.Settlers;

namespace World
{

    public class TheGod : IGod
    {
        public class CleanUpMap
        {
            public readonly IMap Map;

            public CleanUpMap(IMap map)
            {
                this.Map = map;
            }
        }

        public class Angel : ReceiveActor
        {
            private readonly IGod God;

            public Angel(IGod god)
            {
                this.God = god;
                Receive<Spawn>(m => SpawnASettler(m));
                Receive<ReceiveTimeout>(m => Context.System.Stop(Self));
                SetReceiveTimeout(TimeSpan.FromMinutes(2));
            }

            private void SpawnASettler(Spawn m)
            {
                var s = m.Settler.SparkSoul(Context);
                Context.System.EventStream.Subscribe(s.Soul, typeof(Freeze));
            }
        }

        public class Cleaner : ReceiveActor
        {
            public Cleaner()
            {
                Receive<CleanUpMap>(m => CleanupMap(m.Map), m => m.Map != null);
            }

            private void CleanupMap(IMap map)
            {
                var cnt = 0;
                for (int i = 0; i < map.Height; i++)
                    for (int j = 0; j < map.Width; j++)
                    {
                        var c = map[j, i];
                        var stlr = c.Settler;
                        if (stlr != null && stlr.Soul == null)
                        {
                            c.Populate(null);
                            c.CellType = CellType.Dead;
                            cnt++;
                        }
                    }

                System.Diagnostics.Debug.WriteLine($"Found {cnt} soulless bodies");
                //Self.Tell(PoisonPill.Instance);
                Context.System.Stop(Self);

            }
        }

        public class AbsoluteKnowledge : ReceiveActor
        {
            private TheGod God;
            private Dictionary<int, IActorRef> Angels = new Dictionary<int, IActorRef>();

            public AbsoluteKnowledge(TheGod god)
            {
                God = god;
                Receive<Spawn>(m => SpawnASettler(m));
                Receive<string>(m => CleanupMap(), m => "CLEANUP".Equals(m));

                Context.System.Scheduler.ScheduleTellRepeatedly(
                    TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1),
                    Self, "CLEANUP", Self);
            }

            private void CleanupMap()
            {
                Context
                    .ActorOf<Cleaner>($"God-is-looking-after-you-{DateTime.Now.Ticks}")
                    .Tell(new CleanUpMap(God.Map));
            }
            
            private void SpawnASettler(Spawn m)
            {
                int id = m.Settler.Genome.PopulationId;
                IActorRef angel = null;
                if (!Angels.TryGetValue(m.Settler.Genome.PopulationId, out angel))
                {
                    angel = Context.ActorOf(Props.Create<Angel>(God).WithRouter(new RoundRobinPool(100)), $"Angel-{m.Settler.Genome.PopulationId}");
                    Angels[m.Settler.Genome.PopulationId] = angel;
                }

                angel.Tell(m);
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
            //Spark.EventStream.Subscribe(Knowledge, typeof(Spawn));

            //Spark.Scheduler.ScheduleOnce(TimeSpan.FromSeconds(20), () => { Spark.EventStream.Publish(new Freeze()); });
        }

        public void PopulateCell(int x, int y, ISettler s)
        {
            s.Map = Map;
            s.Cell = Map[x, y].Populate(s);
        }

        public ISettler CreateLife(ISettler settler)
        {
            settler.Map = Map;
            Knowledge.Tell(new Spawn(settler));
            return null;
        }
    }
}