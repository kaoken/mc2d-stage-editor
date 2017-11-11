using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorMC2D.Events
{

    /// <summary>
    /// プロジェクトを閉じた
    /// </summary>
    public class ClosedProjectEventArgs : EventArgs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ClosedProjectEventArgs()
        {

        }
    }

    /// <summary>
    /// ソリューションを閉じた
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ClosedSolutionHandler(object sender, ClosedProjectEventArgs e);

}
