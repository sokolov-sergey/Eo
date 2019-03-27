using System;
using System.Drawing;

namespace VideoSystem
{
    public interface IViewDriver
    {
        void DrawMap(System.Drawing.Graphics graphics);
        void DrawEnvironment(System.Drawing.Graphics g);
        void ZoomIn(int x);
        void DrawDebug(Graphics g);
        void DrawCells(Graphics g);
    }
}