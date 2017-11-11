using System;
using WeifenLuo.WinFormsUI.Docking;

namespace EditorMC2D.Events
{
    /// <summary>
    /// ドックパネルでテーマが変更された時に呼び出す
    /// </summary>
    public class DockThemeChangeEventArgs : EventArgs
    {
        /// <summary>
        /// バージョン
        /// </summary>
        public VisualStudioToolStripExtender.VsVersion version;
        /// <summary>
        /// テーマ
        /// </summary>
        public ThemeBase theme;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        /// <param name="theme"></param>
        public DockThemeChangeEventArgs(VisualStudioToolStripExtender.VsVersion version, ThemeBase theme)
        {
            this.version = version;
            this.theme = theme;
        }
    }
    /// <summary>
    /// ドックパネルでテーマが変更された時に呼び出す
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DockThemeChangeHandler(object sender, DockThemeChangeEventArgs e);

}
