using System;
using World.Maps;

namespace World.Settlers.Plants
{
    public class Plant : ISettler
    {
        private Cell _Cell;
        private int Color = 0x0000FF00;

        public Cell Cell { get =>_Cell; set=>SetCell(value); }

        private void SetCell(Cell cell)
        {
            _Cell = cell;
            _Cell.SetColor(this.Color);
            _Cell.CellType = CellType.Alive | CellType.Plant;
        }

        public IMap Map { get ; set ; }


    }
}