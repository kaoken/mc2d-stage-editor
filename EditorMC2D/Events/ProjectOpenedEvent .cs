using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileStageFormat;

namespace EditorMC2D.Events
{
    /// <summary>
    /// プロジェクトをを開いた
    /// </summary>
    public class ProjectOpenedEventArgs : EventArgs
    {

    }
    /// <summary>
    /// プロジェクトをを開いた
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ProjectOpenedHandler(object sender, ProjectOpenedEventArgs e);

}
