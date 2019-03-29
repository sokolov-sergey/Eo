using System;
using System.Windows.Forms;
using VideoSystem;
using World;

namespace Ww
{
    public partial class Form1 : Form
    {

        private IViewPort ViewPort;
        private WorldEnvironment Environment;

        public Form1()
        {
            Environment = new WorldEnvironment();

            

            ViewPort = Environment.GetViewPort();

            InitializeComponent();
            ViewPort.ProvideFrames(a => monitor1.PushFrame(a));
            ViewPort.SetDeviceSize(monitor1.Width, monitor1.Height);
            monitor1.FPS = ViewPort.MaxFPS = 24;
            monitor1.Resize += Monitor1_Resize;
            monitor1.KeyDown += Monitor1_KeyDown;

        }

        private void Monitor1_KeyDown(object sender, KeyEventArgs e)
        {
            Environment.RandomSettle();
            label1_Click(null, null);
        }

        private void Monitor1_Resize(object sender, EventArgs e)
        {
            ViewPort.SetDeviceSize(monitor1.Width, monitor1.Height);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
      
        }

        private void monitor1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                Environment.RandomSettle();

            if (e.Button == MouseButtons.Middle)
                ViewPort.ZoomIn(x: 0);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var fps = ViewPort.MaxFPS + 5;
            fps = fps >= 60 ? 60 : fps;
            SetFps(fps);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var fps = ViewPort.MaxFPS - 5;
            fps = fps <= 5 ? 5 : fps;
            SetFps(fps);
        }

        private void SetFps(int fps)
        {
            ViewPort.MaxFPS = fps;
            //monitor1.FPS = fps;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            label1.Text =  $"life count: {Environment.SettlersCount}";
        }
    }
}
