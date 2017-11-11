using EditorMC2D.Common;
using System;

namespace EditorMC2D.Events
{
    /// <summary>
    /// MC2Dアプリのプロセスが終了した
    /// </summary>
    public class ProcessMC2DEndEventArgs : EventArgs
    {
    }
    /// <summary>
    /// MC2Dアプリのプロセスが終了した時に呼び出す
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ProcessMC2DEndHandler(object sender, ProcessMC2DEndEventArgs e);
}
