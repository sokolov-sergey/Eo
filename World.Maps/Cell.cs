using System;

namespace World.Maps
{
    public struct Cell : IDisposable
    {
        public static Cell Empty = new Cell(CellType.Empty, -1, -1);

        public CellType CellType;
        public int X, Y;


        public Cell(CellType type, int x, int y)
        {
            CellType = type;
            X = x;
            Y = y;         
        }

        public void Dispose()
        {
        }
    }
}