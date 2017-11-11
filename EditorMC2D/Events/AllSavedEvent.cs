using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorMC2D.Events
{
    /// <summary>
    /// 全てを保存した
    /// </summary>
    public class AllSavedEventArgs : EventArgs
    {

    }
    /// <summary>
    /// ドックパネルでテーマが変更された時に呼び出す
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void AllSavedHandler(object sender, AllSavedEventArgs e);
}
