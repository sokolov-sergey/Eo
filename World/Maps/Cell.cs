using System;
using World.Settlers;

namespace World.Maps
{
    public class Cell : IDisposable
    {
        public static Cell Empty = new Cell(CellType.Empty, -1, -1);

        public CellType CellType;
        public int X, Y;
        private ISettler Settler;

        public void SetColor(int color)
        {
            Color = color;
        }

        public int Organics;
        public int Minedals;

        public int Color { get; private set; }

        public Cell(CellType type, int x, int y)
        {
            CellType = type;
            X = x;
            Y = y;
            Settler = null;
        }

        public Cell Populate(ISettler s)
        {
            Settler = s;
            return this;
        }

        public void Dispose()
        {
        }
    }
}