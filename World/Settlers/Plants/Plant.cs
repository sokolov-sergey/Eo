using Akka.Actor;
using Akka.Util;
using System;
using World.Maps;

namespace World.Settlers.Plants
{
    

    public class Plant : ISettler
    {
        public class PlantSoul : ReceiveActor
        {
            private readonly Plant settler;
            System.Threading.Timer Timer;

            public PlantSoul(ISettler settler)
            {
                this.settler = (Plant)settler;
                Timer = new System.Threading.Timer(LifeTick, null, 1000, 2200);
            }

            private void LifeTick(object state)
            {                
                var g = settler.Color >> 8 & 0xFF - 5;
                settler.Color = settler.Color - (5 << 8);
                settler._Cell.SetColor(settler.Color);
            }
        }
        
        private Cell _Cell;
        private int Color = 0x7800FF00;   
        
        public Plant()
        {
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
    }
}