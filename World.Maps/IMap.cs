using Life.Cell;

namespace World.Maps
{
    public interface IMap
    {
        int Height { get;  }
        int Width { get; }
        Cell[,] Cells { get; }
    }
}