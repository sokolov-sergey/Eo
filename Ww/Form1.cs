using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            monitor1.FPS = ViewPort.MaxFPS;
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        
    }
}
