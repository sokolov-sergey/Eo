﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using VideoSystem;
using World;
using World.Maps;
using World.Settlers;
using World.Settlers.Plants;

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

        private string GetGenName(int id)
        {
            try
            {
                var t = typeof(Gens);
                var flds = t.GetFields(BindingFlags.Static | BindingFlags.Public);

                return flds.Where(f => ((int)f.GetRawConstantValue() == id)).Single().Name;
            }
            catch
            {
                return "no info";
            }
        }

        public Cell SelectedCell = null;

        public void DumpCellInfo()
        {
            var cell = SelectedCell;

            if (cell == null)
                return;

            listBox1.Items?.Clear();

            listBox1.Items.Add($"Type:{cell.CellType}");
            listBox1.Items.Add($"Mod. 0:{cell.Modificators[0]}");
            listBox1.Items.Add($"X,Y:{cell.X},{cell.Y}");

            bodyPanel.BackColor = Color.FromArgb(cell.Color);
            borderPanel.BackColor = Color.FromArgb(cell.Colors[1]);
            if (cell.Settler != null)
            {
                var s = cell.Settler;
                listBox1.Items.Add($"Enrg:{s.Energy}");
                listBox1.Items.Add($"Cycles left:{s.LifeCyclesCount}");

                foreach (var g in s.Genome)
                {
                    var (lv, cmd) = g.SequenceGen();
                    listBox1.Items.Add($"gen:{cmd}-{GetGenName(cmd)}, lv:{lv}");
                }
            }
        }

        private void GatherStatistic()
        {
            var statistic = Environment.GatherStatistic();
        }

        private void monitor1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!timer1.Enabled)
                    timer1.Start();

                SelectedCell = Environment.GetCellInfo(e.X, e.Y);
                DumpCellInfo();
            }

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
            fps = fps <= 1 ? 1 : fps;
            SetFps(fps);
        }

        private void SetFps(int fps)
        {
            ViewPort.MaxFPS = fps;
            //monitor1.FPS = fps;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //label1.Text = $"life count: {Environment.SettlersCount}";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ViewPort.Stop();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            DumpCellInfo();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Stop();

            try
            {
                var stat = StaticsticsStore.Last();

                listBox1.Items?.Clear();
                object[] obj = stat.Aggregations?
                    .Select(v => $"{v.Key}: {v.Value}")?
                    .OrderBy(s => s).ToArray();

                listBox1.Items.Add($"SnapShot counts:{StaticsticsStore.Count}");
                listBox1.Items.AddRange(obj);
            }
            catch
            {
            }
        }

        public List<IStatistics> StaticsticsStore = new List<IStatistics>();

        private void statisticTimer_Tick(object sender, EventArgs e)
        {
            var stat = Environment.GatherStatistic();
            StaticsticsStore.Add(stat);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Func<string, string> normalize = s =>
            {
                return s.Replace(">", "-").Replace(" ", "-").Replace(",", "-");
            };

            var itm = 1;
            XElement xElem = new XElement("items",
                StaticsticsStore.Select(x =>
                    new XElement("item",
                        new XAttribute("Frame", itm++),
                        x.Aggregations?.Select(el =>
                            new XAttribute(normalize(el.Key), el.Value)
                        )
                    )
                )
            );

            xElem.Save($@"c:\temp\eo-stat-{DateTime.Now.ToString("HHmm")}-{StaticsticsStore.Count}.xml");
        }
    }
    public class ServiceStatMapper
    {
        [XmlAttribute]
        public static string Key;
    }
}
