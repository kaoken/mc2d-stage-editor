using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileStageFormat;

namespace EditorMC2D.Events
{
    /// <summary>
    /// MC2D ステージファイルを開いた
    /// </summary>
    public class D2StageFileOpenedEventArgs : EventArgs
    {
        /// <summary>
        /// テーマ
        /// </summary>
        public D2StageFile stageFile;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="stg">MC2D ステージファイル</param>
        public D2StageFileOpenedEventArgs(D2StageFile stg)
        {
            this.stageFile = stg;
        }
    }
    /// <summary>
    /// MC2D ステージファイルを開いた
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void D2StageFileOpenedHandler(object sender, D2StageFileOpenedEventArgs e);

}
