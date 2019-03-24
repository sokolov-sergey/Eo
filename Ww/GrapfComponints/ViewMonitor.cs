using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoSystem;

namespace Ww.GrapfComponints
{
    public partial class ViewMonitor : UserControl
    {
        Queue<Frame> Frames = new Queue<Frame>();
        System.Threading.Timer Timer;
        private int _fps = 30;
        private int frameCount;

        public Frame LastFrame { get; private set; } = Frame.Empty;

        public int FPS
        {
            get => _fps;
            set {

                Timer.Change(1000, 1000/FPS);
                _fps = value;
            }
        }

        public ViewMonitor()
        {
            InitializeComponent();
            Paint += Monitor_Paint;
            Timer = new System.Threading.Timer(TickHandler, null, 1000, 1000 / FPS);
        }

        private void TickHandler(object state)
        {
            this.Invalidate();
        }

        private void Monitor_Paint(object sender, PaintEventArgs e)
        {
            if (DesignMode)
                return;          

            try
            {
                Frame f = Frames.Dequeue();

                if (f != null)
                {
                    LastFrame.Image.Dispose();
                    LastFrame = f;
                }
            }
            catch { }
                
            e.Graphics.DrawImage(LastFrame.Image, 0, 0);
            DrawDebug(e.Graphics);

            if (frameCount++>100)
            {
                GC.Collect();               
                frameCount = 0;

            }
        }

        private void DrawDebug(Graphics graphics)
        {
            graphics.DrawString($"MaxFPS:{FPS} fC:{frameCount}",Font, Brushes.Blue,0,0);
        }

        internal void PushFrame(Frame frame)
        {
            Frames.Enqueue(frame);
        }



    }
}
