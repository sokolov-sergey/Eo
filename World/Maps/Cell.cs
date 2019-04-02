using System;
using World.Settlers;

namespace World.Maps
{
    public class Cell : IDisposable
    {
        public static Cell Empty = new Cell(CellType.Empty, -1, -1);

        public CellType CellType;
        public int X, Y;
        public ISettler Settler { get; private set; }

        public int[] Modificators { get; set; } = new int[255];

        public void SetColor(int color,int idx=0)
        {
            Colors[idx] = color;
        }

        public int[] Colors { get; private set; } = new int[255];

        public int Organics;
        public int Minedals;


        public int Color { get=>Colors[0]; private set=>Colors[0]=value; }

        public Cell(CellType type, int x, int y)
        {
            CellType = type;
            X = x;
            Y = y;
            Settler = null;
        }

        object lockObject = new object();
        public Cell Populate(ISettler s)
        {
            lock (lockObject)
            {
                Settler = s;
                return this;
            }
        }

        public void Dispose()
        {
        }
    }
}