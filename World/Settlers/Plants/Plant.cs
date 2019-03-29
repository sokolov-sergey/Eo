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

            private (int level, int cmd) Sequenser(int gen) => (gen >> 8, ((gen>>8)<<8) ^ gen);


            protected override void LifeTick(object state)
            {
                foreach (var g in Body.Genome)
                {
                    var (lv, c) = Sequenser(g);

                    if (c == Gens.Photosynthesis)
                    {
                        Body.Feed(lv);
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
        }

        private Cell _Cell;
        private int Color = 0x7800FF00;


        public Plant(IGenome genome)
        {
            Genome = genome;
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