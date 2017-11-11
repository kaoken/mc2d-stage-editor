using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileStageFormat.Events
{
    public class ReadedHexagonImageEvent : EventArgs
    {
    }
    /// <summary>
    /// ヘキサゴン・タイル画像を呼び終えた後、呼び出すデリゲーター
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ReadedHexagonImageHandler(object sender, ReadedHexagonImageEvent e);

}
