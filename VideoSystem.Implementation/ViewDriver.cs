using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using World.Maps;

namespace VideoSystem.Implementation
{
    internal class ViewDriver : IViewDriver
    {
        private readonly IMap Map;
        private Size PhysicalSize;
        private Rectangle MapBorder;
        private LinearGradientBrush MapBg;
        private readonly int MinCellScale = 3;
        private int Scale = 3;
        private readonly Font debugFont = new Font(FontFamily.GenericMonospace, 8);
        private readonly Random Randomizer = new Random();

        public ViewDriver(IMap map)
        {
            this.Map = map;
            UpdateStaticParams();
        }

        private void UpdateStaticParams()
        {
            PhysicalSize = new Size(Map.Width * Scale, Map.Height * Scale);


            MapBorder = new Rectangle(new Point(1, 1), PhysicalSize);
            MapBg = new LinearGradientBrush(
                new Point(MapBorder.Width / 2, 0), new Point(MapBorder.Width / 2, MapBorder.Height),
                Color.LightSeaGreen, Color.DarkSlateBlue);
        }

        public void DrawMap(Graphics g)
        {
            g.FillRectangle(MapBg, MapBorder);
        }

        public void DrawEnvironment(Graphics g)
        {
            Rectangle[] rects = new Rectangle[Map.Width * Map.Height];
            //   var filledIdx = Randomizer.Next(0, rects.Length);

            var i = 0;
            for (int x = 1; x <= Map.Width; x++)
                for (int y = 1; y <= Map.Height; y++)
                {

                    var x2 = (x - 1) * Scale;
                    var y2 = (y - 1) * Scale;

                    rects[i++] = new Rectangle(x2, y2, Scale, Scale);
                }


            var b = new SolidBrush(Color.FromArgb(0x20FF0000));
            g.FillRectangles(b, rects);

            //            g.FillRectangle(Brushes.Lime, rects[filledIdx]);

        }

        public void ZoomIn(int x)
        {
            Scale += x == 0 ? MinCellScale : x;

            if (Scale > MinCellScale * 7)
                Scale = MinCellScale;

            UpdateStaticParams();
        }

        public void DrawDebug(Graphics g)
        {
            g.DrawString($"Sx{Scale}", debugFont, Brushes.DarkOrange, 0, MapBorder.Height - 11);
        }

        public void DrawCells(Graphics g)
        {
            for (int x = 0; x < Map.Width; x++)
                for (int y = 0; y < Map.Height; y++)
                {
                    var cell = Map.Cells[x, y];
                    DrawCell(cell, g);
                }
        }

        private void DrawCell(Cell cell, Graphics g)
        {
            if (cell.CellType == CellType.Empty)
                return;

            var pen = CellToPen(cell);
            var brush = CelltoBrush(cell);
            var rect = new Rectangle(cell.X * Scale, cell.Y * Scale, Scale, Scale);

            g.FillRectangle(brush, rect);
            g.DrawRectangle(pen, rect);

        }

        private Brush CelltoBrush(Cell cell)
        {
            var b = Brushes.Transparent;

            if (cell.CellType != CellType.Empty)
                b = new SolidBrush(Color.FromArgb(cell.Color));

            return b;
        }

        private Pen CellToPen(Cell cell)
        {
            var pen = Pens.Transparent;
            if (cell.CellType == CellType.Alive)
                pen = Pens.White;

            if (cell.CellType == CellType.Wall)
                pen = Pens.Black;


            return pen;
        }
    }
}