using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileStageFormat.Events
{
    public class ReadedIsometricImageEvent : EventArgs
    {
    }
    /// <summary>
    /// アイソメトリック・タイル画像を呼び終えた後、呼び出すデリゲーター
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ReadedIsometricImageHandler(object sender, ReadedIsometricImageEvent e);

}
