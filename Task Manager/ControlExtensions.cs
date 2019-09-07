using System;
using System.Windows.Forms;

namespace Task_Manager
{
    public static class ControlExtensions
    {
        public static void Invoke(this Control control, Action action)
        {
            if (control.InvokeRequired)
                control.Invoke(new MethodInvoker(action), null);
            else
                action.Invoke();
        }
    }
}
