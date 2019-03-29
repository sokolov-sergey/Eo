using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoSystem;

namespace Ww.GrapfComponints
{
    public partial class ViewMonitor : UserControl
    {
        Queue<Frame> Frames = new Queue<Frame>();
        System.Threading.Timer Timer;
        private int _fps = 24;
        private int frameCount;
        private object drawLocker = new object();
        private int flag = 0;
        private int maxQueueSize = 150;

        public Frame LastFrame { get; private set; } = Frame.Empty;

        public int FPS
        {
            get => _fps;
            set
            {

                Timer.Change(1000, 1000 / FPS);
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

            // if (Interlocked.CompareExchange(ref flag, 1, 0) == 0)
            {
                try
                {
                    Frame f = Frame.Empty;

                    if (Frames.Count > 0)
                        f = Frames.Dequeue();

                    if (!f.IsEmpty)
                    {
                        LastFrame = f;
                    }
                }
                catch { }



                e.Graphics.DrawImage((Image)LastFrame.Image.Clone(), 0, 0);
                DrawDebug(e.Graphics);

                // Interlocked.Decrement(ref flag);
            }

            if (frameCount++ > 100)
            {
                GC.Collect();
                frameCount = 0;

            }
        }

        private void DrawDebug(Graphics graphics)
        {
            graphics.DrawString($"MaxFPS:{FPS} fC:{frameCount} FS:{Frames.Count}", Font, Brushes.Blue, 0, 0);
        }

        internal int PushFrame(Frame frame)
        {
            if (Frames.Count > maxQueueSize)
                Frames.Clear();

            Frames.Enqueue(frame);
            return 0;
        }



    }
}
