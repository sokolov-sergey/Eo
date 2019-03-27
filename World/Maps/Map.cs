

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
            for (int x = 0; x < Width - 1; x++)
                for (int y = 0; y < Height - 1; y++)
                {
                    Cells[x, y] = new Cell(CellType.Empty, x, y);
                }
        }

        public int Height { get; private set; }
        public int Width { get; private set; }

        public Cell[,] Cells { get; private set; }
    }
}