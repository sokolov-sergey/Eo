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

            private (int level, int cmd) Sequencer(int gen) => gen.SequenceGen();

            private void Kill()
            {
                Context.System.EventStream.Unsubscribe(Self);
                this.Self.Tell(PoisonPill.Instance);
                Body.Die();
            }

            protected override void LifeTick(object state)
            {
                if (Body._Cell.Settler != Body
                    || Body.Energy <= -15
                    )
                    Kill();


                foreach (var g in Body.Genome)
                {
                    var (lv, c) = Sequencer(g);

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
                        Kill();
                        return;
                    }

                }

                Context.System.Scheduler
                    .ScheduleTellOnceCancelable(
                        200, Self, INTERNAL_LIFE_TICK, Self);
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
            var mpl = 0;
            for (int i = 0; i < 9; i++)
                if (Neighborhood[i] != null 
                    && (Neighborhood[i].CellType | CellType.Alive) == CellType.Alive)
                    mpl++;



            Color = Color - (5 << 8);
            _Cell.SetColor(Color);
            Energy += lv / (mpl+1) - 1;
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
                || lv <= 10
                || ((c.CellType != CellType.Empty) && (c.CellType != CellType.Dead))
                || (c.CellType == CellType.Dead && lv < 80)
                || (((c.CellType | CellType.Alive) == CellType.Alive)
                        && (c.Settler != null && c.Settler.Genome.DistanceBetween(this.Genome) > 20 && c.Settler.Energy - 10 <= this.Energy + lv))

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
                var (lev, cmd) = g.SequenceGen();

                // mutation of gen's level component
                genome[i++] = (Randomizer.Next(lev - 1, lev + 2), cmd).CodeGen();
            }

            var body = new Plant(new PlantGenome(genome));
            body.Map = Map;
            body.Cell = c;
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

            {

                var (dx, dy) = points[Randomizer.Next(0, 8)];
                c = PickACell(dx, dy);
                if (c != null)
                    return c;

                (dx, dy) = points[Randomizer.Next(0, 8)];
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

        private readonly Cell[] Neighborhood = new Cell[5 * 5 - 1];

        private void SetCell(Cell cell)
        {
            var n = 0;
            for (int i = -2; i < 3; i++)
                for (int j = -2; j < 3; j++)
                {
                    if (i != 0 && j != 0)
                        Neighborhood[n++] = PickACell(i, j);
                }

            _Cell = cell;
            _Cell.SetColor(this.Color);
            _Cell.CellType = CellType.Alive | CellType.Plant;
            _Cell.SetColor(254 << 24 | (this.Genome.PopulationId * 255 / 100) << 16, 1);
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