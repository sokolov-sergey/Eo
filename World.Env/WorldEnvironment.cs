using System;
using System.Collections.Generic;
using System.Linq;
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

        public int SettlersCount { get => God.SettlersCount; }

        public WorldEnvironment()
        {
            Map = new Map(height: 100, width: 100);
            God = new TheGod(Map);

            CurrentVideoSystem = new WorldVideoSystem(Map);

            PopulateInitial();
            //PopulateInitial();
        }

        private (int x, int y) PopulateInitial()
        {
            var (x, y) = RndXY;
            if ((Map[x, y].CellType & CellType.Alive) == CellType.Alive)
                return (x, y);

            var s = CreateASettler(null,x,y);


            return (x, y);
        }

        private ISettler CreateASettler(object p, int x, int y)
        {
            var s = new Plant();
            s.Map = Map;
            s.Cell = Map[x, y];
            return God.CreateLife(s);
        }

        private Cell GetRandomCell()
        {
            var (x, y) = RndXY;
            return Map.Cells[x, y];
        }

        public Cell GetCellInfo(int x, int y)
        {
            var (cx, cy) = GetViewPort().PixelToCell(x, y);

            try
            {
                return Map[cx, cy];
            }
            catch
            {

            }

            return null;
        }

        public void RandomSettle()
        {
            /// return;

            ThreadPool.QueueUserWorkItem(
                s =>
                {
                    for (int i = 0; i < 1; i++)
                    {
                        PopulateInitial();
                    }


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

        public IStatistics GatherStatistic()
        {
            IStatistics stat = new Stats();

            for (int i = 0; i < Map.Width; i++)
                for (int j = 0; j < Map.Height; j++)
                {
                    var c = Map[i, j];
                    stat.Agg(key: "Map>" + c.CellType.ToString(), val: 1);

                    if ((c.CellType & CellType.Alive) == CellType.Alive)
                    {
                        stat.Agg($"Life>{c.Settler?.Genome?.PopulationId.ToString("D2")}", 1);
                    }
                }

            return stat;
        }
    }

    internal class Stats : IStatistics
    {
        private Dictionary<string, int> Aggr = new Dictionary<string, int>();

        
        public IReadOnlyDictionary<string, int> Aggregations => Aggr as IReadOnlyDictionary<string, int>;

        public void Agg(string key, int val)
        {
            Aggr[key] = Aggr.ContainsKey(key) ? Aggr[key] + val : val;
        }
    }
}
