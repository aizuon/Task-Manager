using MetroFramework.Forms;
using System.ComponentModel;

namespace Task_Manager
{
    public partial class MainForm : MetroForm
    {
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

                metroGrid1.Invoke(() => metroGrid1.Rows.Clear());
                foreach (var p in list)
                    metroGrid1.Invoke(() => metroGrid1.Rows.Add(p.Name, p.CPU, p.Memory));

                metroGrid1.Invoke(() => metroGrid1.Sort(metroGrid1.Columns["CPU"], ListSortDirection.Descending));
            }
        }
    }
}
