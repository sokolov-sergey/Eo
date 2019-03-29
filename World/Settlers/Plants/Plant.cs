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
                foreach (var g in Body.Genome)
                {
                    var (lv, c) = Sequenser(g);

                    if (c == Gens.Photosynthesis)
                    {
                        Body.Feed(lv);
                    }

                    if (c == Gens.Breed && Body.Energy > 100)
                    {
                        var s = Body.Spawn();
                        if (s is null)
                            return;

                        Body.Energy /= 4;
                        Context.System.EventStream.Publish(new Spawn(s));
                    }
                }

                Context.System.Scheduler
                    .ScheduleTellOnceCancelable(
                        100, Self, INTERNAL_LIFE_TICK, Self);
            }
        }

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

        protected ISettler Spawn()
        {
            Cell c = SelectCell();

            if (c is null)
            {
                this.Energy /= 3;
                return null;
            }

            int[] genome = (int[])Array.CreateInstance(typeof(int), Genome.Length);
            int i = 0;
            foreach (var g in Genome)
            {
                genome[i++] = g;
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

            Cell c = PickACell(x-1,y-1);
            if (c != null && c.CellType == CellType.Empty)
                return c;

            c = PickACell(x, y - 1);
            if (c != null && c.CellType == CellType.Empty)
                return c;

            c = PickACell(x+1, y - 1);
            if (c != null && c.CellType == CellType.Empty)
                return c;

            c = PickACell(x-1, y);
            if (c != null && c.CellType == CellType.Empty)
                return c;

            c = PickACell(x+1, y);
            if (c != null && c.CellType == CellType.Empty)
                return c;

            c = PickACell(x - 1, y + 1);
            if (c != null && c.CellType == CellType.Empty)
                return c;

            c = PickACell(x, y + 1);
            if (c != null && c.CellType == CellType.Empty)
                return c;

            c = PickACell(x + 1, y+ 1);
            if (c != null && c.CellType == CellType.Empty)
                return c;


            return null;
        }

        private Cell PickACell(int x, int y)
        {
            try
            {
                return Map[x, y];
            }
            catch { }

            return null;
        }

        public Cell Cell { get => _Cell; set => SetCell(value); }

        private void SetCell(Cell cell)
        {
            _Cell = cell;
            _Cell.SetColor(this.Color);
            _Cell.CellType = CellType.Alive | CellType.Plant;
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