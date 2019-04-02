

using System;

namespace World.Maps
{
    public class Map : IMap
    {
        public Map(int height, int width)
        {
            Height = height;
            Width = width;
            Cells = new Cell[Width, Height];

            InitDefaultCells();
        }

        private void InitDefaultCells()
        {
            var ariaSize = 10;
            var r = new Random();


            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    Cells[x, y] = new Cell(CellType.Empty, x, y);


            for (int i = 0; i < 4; i++)
            {
                var (cx, cy) = (r.Next(10, Width - 10), r.Next(10, Height - 10));
                for (int x = cx - ariaSize; x < cx + ariaSize; x++)
                    for (int y = cy - ariaSize; y < cy + ariaSize; y++)
                    {
                        var (dx, dy) = (Math.Abs(x - cx), Math.Abs(y - cy));
                        var l = (dx > dy ? dx : dy);
                        l = l > 0 ? l : 1;

                        if (Cells[x, y].Modificators[0] < 100)
                            Cells[x, y].Modificators[0] += (int)(100 / Math.Pow(l, 2));

                        Cells[x, y].Modificators[0] = Cells[x, y].Modificators[0] > 100 ? 100 : Cells[x, y].Modificators[0];
                    }
            }


        }

        public int Height { get; private set; }
        public int Width { get; private set; }

        public Cell[,] Cells { get; private set; }

        public Cell this[int x, int y] { get => Cells[x, y]; set => Cells[x, y] = value; }
    }
}