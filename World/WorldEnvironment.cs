using Life.Cell;
using System;
using System.Threading;
using System.Threading.Tasks;
using VideoSystem;
using World.Maps;

namespace World
{
    public class WorldEnvironment
    {
        private WorldVideoSystem CurrentVideoSystem;
        private IMap Map;
        private Random Randomizer = new Random();

        public WorldEnvironment()
        {
            Map = new Map(height: 100, width: 100);

            CurrentVideoSystem = new WorldVideoSystem(Map);

            InitialSettle();
        }


        private void InitialSettle()
        {
            var ac = new AliveCell();
            ac.TakeCell(ref Map.Cells[30, 30]);
        }

        public void RandomSettle()
        {
            Task.Run(() =>
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
            });
        }

        public IViewPort GetViewPort()
        {
            return CurrentVideoSystem.GetViewPort();
        }
    }
}
