using MetroFramework.Controls;
using MetroFramework.Forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Task_Manager
{
    public partial class MainForm : MetroForm
    {
        public static string LastProcess;

        [DllImport("user32.dll")]
        private static extern long LockWindowUpdate(long handle);

        public MainForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            var backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                var list = ProcessManager.GetProcesses();

                this.Invoke(() => LockWindowUpdate(Handle.ToInt64()));
                metroGrid1.Invoke(() => metroGrid1.Rows.Clear());
                foreach (var p in list)
                    metroGrid1.Invoke(() => metroGrid1.Rows.Add(p.Name, p.CPU, p.Memory));

                metroGrid1.Invoke(() => metroGrid1.Sort(metroGrid1.Columns["CPU"], ListSortDirection.Descending));
                this.Invoke(() => LockWindowUpdate(0));
            }
        }

        private void MetroGrid1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var m = new MetroContextMenu(metroGrid1.Container);
                var terminate = new ToolStripMenuItem();
                terminate.Text = "Terminate";
                terminate.Click += TerminateToolStripMenuItem_Click;
                m.Items.Add(terminate);

                var test = metroGrid1.HitTest(e.X, e.Y);

                if (test.RowIndex >= 0)
                    LastProcess = metroGrid1.Rows[test.RowIndex].Cells["ProcessName"].Value.ToString();

                m.Show(metroGrid1, new Point(e.X, e.Y));

            }
        }

        private void TerminateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessManager.Terminate(LastProcess);
        }

        private void RefreshRateButton_Click(object sender, EventArgs e)
        {
            var m = new MetroContextMenu(refreshRateButton.Container);
            var fast = new ToolStripMenuItem();
            fast.Text = "Fast (0.5s)";
            fast.Click += FastToolStripMenuItem_Click;

            var normal = new ToolStripMenuItem();
            normal.Text = "Normal (1s)";
            normal.Click += NormalToolStripMenuItem_Click;

            var slow = new ToolStripMenuItem();
            slow.Text = "Slow (2s)";
            slow.Click += SlowToolStripMenuItem_Click;

            if (ProcessManager.Interval == 500)
                fast.Checked = true;
            else if (ProcessManager.Interval == 1000)
                normal.Checked = true;
            else
                slow.Checked = true;

            m.Items.Add(fast);
            m.Items.Add(normal);
            m.Items.Add(slow);

            m.Show(metroGrid1, new Point(refreshRateButton.Width, refreshRateButton.Height));
        }

        private void FastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessManager.Interval = 500;
        }

        private void NormalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessManager.Interval = 1000;
        }

        private void SlowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessManager.Interval = 2000;
        }
    }
}
