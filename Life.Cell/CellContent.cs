namespace Life.Cell
{
    public abstract class CellContent
    {
        public static CellContent Empty = new EmptyContent();

        protected Cell Cell;

        public virtual void TakeCell(ref Cell cell)
        {
            Cell = cell;
        }

        public abstract void GiveupCell();
        
    }

    public class EmptyContent : CellContent
    {

        public override void GiveupCell()
        {
            Cell = Cell.Empty;
        }
    }
}