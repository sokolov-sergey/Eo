using Akka.Actor;
using Akka.Util;
using System;
using World.Maps;


namespace World.Settlers.Plants
{

    public class Plant : ISettler
    {
        public class PlantSoul : SoulActor<Plant>
        {
            public PlantSoul(Plant settler) : base(settler)
            {
            }

            private (int level, int cmd) Sequenser(int gen) => (gen >> 8, ((gen >> 8) << 8) ^ gen);

            protected override void LifeTick(object state)
            {
                if (Body._Cell.Settler != Body)
                    Body.Die();

                foreach (var g in Body.Genome)
                {
                    var (lv, c) = Sequenser(g);

                    if (c == Gens.Photosynthesis)
                    {
                        Body.Feed(lv);
                    }

                    if (c == Gens.Breed && Body.Energy > 100)
                    {
                        var s = Body.Spawn(lv);
                        if (s != null)
                        {
                            Body.Energy /= 4;
                            Context.System.EventStream.Publish(new Spawn(s));
                        }
                    }

                    if (Body.FailedSpawns > 10)
                    {
                        this.Self.Tell(PoisonPill.Instance);

                        Body.Die();
                    }

                }

                Context.System.Scheduler
                    .ScheduleTellOnceCancelable(
                        100, Self, INTERNAL_LIFE_TICK, Self);
            }
        }

        private void Die()
        {
            _Cell.CellType = CellType.Dead;
            _Cell.SetColor(0X78FFFFFF);
            _Cell.Populate(null);
        }

        private Random Randomizer = new Random(DateTime.Now.Millisecond);


        private void Feed(int lv)
        {
            Color = Color - (5 << 8);
            _Cell.SetColor(Color);
            Energy += 10;
        }

        private Cell _Cell;
        private int Color = 0x7800FF00;


        public Plant() : this(PlantGenome.Defaul)
        {
        }

        public Plant(IGenome genome)
        {
            Genome = genome;
        }

        int FailedSpawns = 0;

        protected ISettler Spawn(int lv)
        {
            Cell c = SelectCell();

            if (c is null
                || ((c.CellType != CellType.Empty) && (c.CellType != CellType.Dead))
                || (c.CellType == CellType.Dead && lv < 50)
                || (((c.CellType | CellType.Alive) == CellType.Alive) && (c.Settler != null && c.Settler.Energy - 10 <= lv))
                )
            {
                this.Energy /= 3;
                FailedSpawns++;

                return null;
            }

            //if ((c.CellType | CellType.Alive) == CellType.Alive && c.Settler.)

            int[] genome = (int[])Array.CreateInstance(typeof(int), Genome.Length);
            int i = 0;
            foreach (var g in Genome)
            {
                var (lev, cmd) = (g >> 8, 0);
                cmd = (g << 8) ^ g;

                // mutation of gen's level component
                genome[i++] = cmd + (Randomizer.Next(lev - 1, lev + 4) << 8);
            }

            var body = new Plant(new PlantGenome(genome));
            body.Cell = c;
            body.Map = Map;
            body.Energy = this.Energy / 6;


            return body;
        }

        private Cell SelectCell()
        {
            var (x, y) = (_Cell.X, _Cell.Y);

            (int, int)[] points = new (int, int)[] {
                (x - 1, y - 1),(x, y - 1),(x+1, y - 1),
                (x-1, y),(x+1, y),(x - 1, y + 1),
                (x, y + 1),(x + 1, y+ 1)
            };

            Cell c;

            for (int i = 0; i < 3; i++)
            {

                var (dx, dy) = points[Randomizer.Next(0, 8)];
                c = PickACell(dx, dy);
                if (c != null)
                    return c;
            }

            return null;
        }

        private Cell PickACell(int x, int y)
        {
            if ((x < Map.Width && x > -1) && (y > -1 && y < Map.Height))
                return Map[x, y];

            return null;
        }

        public Cell Cell { get => _Cell; set => SetCell(value); }
        private (int x, int y) Heighbors;

        private void SetCell(Cell cell)
        {
            _Cell = cell;
            _Cell.SetColor(this.Color);
            _Cell.CellType = CellType.Alive | CellType.Plant;
            _Cell.Populate(this);
        }

        public ISettler SparkSoul(ActorSystem spark)
        {
            Soul = spark.ActorOf(Props.Create<PlantSoul>(this));
            return this;
        }

        public IMap Map { get; set; }

        public IActorRef Soul { get; private set; }
        public float Energy { get; set; }

        public float Organics { get; set; }

        public float Minerals { get; set; }

        public IGenome Genome { get; set; }

        public int Age { get; } = 0;
    }
}