﻿using System;
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
        private Brush MapBg;
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
                Color.LightSeaGreen, Color.SeaGreen);

        }

        public void DrawMap(Graphics g)
        {
            g.FillRectangle(MapBg, MapBorder);
        }

        public void DrawEnvironment(Graphics g)
        {
            return;
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
            g.DrawString($"Sx{Scale}", debugFont, Brushes.DarkOrange, 100, 0);
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
            Pen pen;
            Brush brush;

            int margin = (Scale / MinCellScale) / 3;
            if (cell.CellType == CellType.Empty)
            {
                if (cell.Modificators[0] < 1)
                    return;
                else
                    brush = new SolidBrush(Color.FromArgb(cell.Modificators[0] * 255 / 100, Color.Gold));
            }
            else
                brush = CelltoBrush(cell);

            pen = CellToPen(cell);
            var rect = new Rectangle((cell.X * Scale), (cell.Y * Scale), Scale - margin, Scale - margin);

            g.FillRectangle(brush, rect);
           // g.DrawRectangle(pen, rect);

        }

        private Brush CelltoBrush(Cell cell)
        {
            var b = Brushes.Transparent;

            if (cell.CellType != CellType.Empty)
            {
                if (cell.CellType != CellType.Dead || (cell.Modificators[0] < 1 && cell.CellType == CellType.Dead))
                    b = new SolidBrush(Color.FromArgb(cell.Color));
                else                    
                    b = new SolidBrush(Color.FromArgb(cell.Modificators[0] * 200 / 100, Color.White));
            }

            return b;
        }

        private Pen CellToPen(Cell cell)
        {
            var pen = Pens.Transparent;

            if ((cell.CellType & CellType.Alive) == CellType.Alive)
                pen = new Pen(Color.FromArgb(cell.Colors[1]), 1);

            if (cell.CellType == CellType.Wall)
                pen = Pens.Black;

            return pen;
        }

        public (int x, int y) PixelToCell(int x, int y)
        {
            return (x / Scale, y / Scale);
        }
    }
}