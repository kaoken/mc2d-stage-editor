using EditorMC2D.Common;
using System;

namespace EditorMC2D.Events
{
    /// <summary>
    /// MC2Dアプリのプロセスが開始した
    /// </summary>
    public class ProcessMC2DStartEventArgs : EventArgs
    {
    }
    /// <summary>
    /// MC2Dアプリのプロセスが開始した時に呼び出す
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ProcessMC2DStartHandler(object sender, ProcessMC2DStartEventArgs e);
}
