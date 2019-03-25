using System;

namespace Life.Cell
{
    public struct Cell : IDisposable
    {
        public static Cell Empty = new Cell(CellType.Empty, -1, -1, CellContent.Empty);

        public CellType CellType;
        public int X, Y;


        public Cell(CellType type, int x, int y, CellContent content)
        {
            CellType = type;
            X = x;
            Y = y;

            Content = content;
            Content.TakeCell(ref this);
        }

        public CellContent Content;


        public void Dispose()
        {
        }
    }
}