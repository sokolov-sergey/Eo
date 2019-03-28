using System;
using System.Threading;
using System.Threading.Tasks;
using VideoSystem;
using VideoSystem.Implementation;
using World.Maps;
using World.Settlers;
using World.Settlers.Plants;

namespace World
{
    public class WorldEnvironment
    {
        private WorldVideoSystem CurrentVideoSystem;
        private IMap Map;
        private IGod God { get; }

        private Random Randomizer = new Random();

        public (int x, int y) RndXY { get => (Randomizer.Next(0, Map.Width), Randomizer.Next(0, Map.Height)); }

        public WorldEnvironment()
        {
            Map = new Map(height: 20, width: 20);
            God = new TheGod(Map);

            CurrentVideoSystem = new WorldVideoSystem(Map);

            PopulateInitial();
        }


        private (int x,int y) PopulateInitial()
        {
            var (x, y) = RndXY;
            if ((Map[x, y].CellType & CellType.Alive) == CellType.Alive)
                return (x,y) ;

            var s = CreateASettler(null);

            God.PopulateCell(x, y, s);

            /*     var ac = new AliveCell();
                 ac.TakeCell(ref Map.Cells[30, 30]);*/

            return (x, y);
        }

        private ISettler CreateASettler(object p)
        {
            return God.CreateLife(new Plant());
        }

        private Cell GetRandomCell()
        {
            var (x, y) = RndXY;
            return Map.Cells[x, y];
        }

        public void RandomSettle()
        {
           /// return;

            ThreadPool.QueueUserWorkItem(
                s =>
                {
                    var (x,y) = PopulateInitial();
                   
                    // Thread.Sleep((int)s);                  
                    //  Map.Cells[x, y] = Cell.Empty; 
                  
                    /*var c = GetRandomCell();
                    if (c.CellType == CellType.Wall)
                        return;

                    c.CellType = CellType.Wall;
                    Map.Cells[c.X, c.Y] = c;
                    Thread.Sleep((int)s);
                    Map.Cells[c.X, c.Y] = default(Cell);*/
                },
                Randomizer.Next(10000));
        }

        public IViewPort GetViewPort()
        {
            return CurrentVideoSystem.GetViewPort();
        }
    }
}
