using System;
using System.Threading;
using System.Threading.Tasks;
using VideoSystem;
using VideoSystem.Implementation;
using World.Maps;

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
            Map = new Map(height: 100, width: 100);
            God = new TheGod(Map);

            CurrentVideoSystem = new WorldVideoSystem(Map);

            InitialSettle();
        }


        private void InitialSettle()
        {
            God.SettleCell(GetRandomCell(), null);

            /*     var ac = new AliveCell();
                 ac.TakeCell(ref Map.Cells[30, 30]);*/
        }

        private Cell GetRandomCell()
        {
            var (x, y) = RndXY;
            return Map.Cells[x, y];
        }

        public void RandomSettle()
        {
            /* Task.Run(() =>
             {
                 var ac = new AliveCell();
                 var x = Randomizer.Next(0, 99);
                 var y = Randomizer.Next(0, 99);
                 ac.TakeCell(ref Map.Cells[x, y]);
                 Thread.Sleep(1000);
                 return new int[2] { x, y };

             }).ContinueWith(t =>
             {
                 var res = t.Result;
                 Map.Cells[res[0], res[1]] = default(Cell);
             });*/
        }

        public IViewPort GetViewPort()
        {
            return CurrentVideoSystem.GetViewPort();
        }
    }
}
