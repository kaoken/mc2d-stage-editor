using System;


namespace EditorMC2D.Events
{
    /// <summary>
    /// 新しいプロジェクトが作られた
    /// </summary>
    public class NewProjectCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// MC2D.exeがあるディレクトリパス
        /// </summary>
        string dirFilePathMC2D = "";
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path">MC2D.exeがあるディレクトリパス</param>
        public NewProjectCreatedEventArgs(string path)
        {
            this.dirFilePathMC2D = path;
        }
    }
    /// <summary>
    /// 新しいプロジェクトが作られた
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void NewProjecCreatedHandler(object sender, NewProjectCreatedEventArgs e);

}
