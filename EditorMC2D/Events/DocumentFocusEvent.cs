using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileStageFormat;
using WeifenLuo.WinFormsUI.Docking;

namespace EditorMC2D.Events
{
    /// <summary>
    /// プロジェクトをを開いた
    /// </summary>
    public class DocumentFocusEventArgs : EventArgs
    {
        public DockContent DockContent;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="form">ドキュメントフォーム</param>
        public DocumentFocusEventArgs(DockContent dockContent)
        {
            DockContent = dockContent;
        }
    }
    /// <summary>
    /// プロジェクトをを開いた
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DocumentFocusHandler(object sender, DocumentFocusEventArgs e);

}
