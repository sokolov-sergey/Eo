using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life.Cell
{
    public class AliveCell : CellContent
    {
        public AliveCell()
        {            
        }

        public override void TakeCell(ref Cell cell)
        {
            base.TakeCell(ref cell);
            cell.CellType = CellType.Alive;
        }

        public override void GiveupCell()
        {
            
        }
    }
}
