using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileStageFormat.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateSquareTilesMapEvent : EventArgs
    {
    }
    /// <summary>
    /// スクエアータイルマップ作成後、呼び出すデリゲーター
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void CreateSquareTilesMapHandler(object sender, CreateSquareTilesMapEvent e);

}
