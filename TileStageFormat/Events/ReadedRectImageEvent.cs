using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileStageFormat.Tile.Rect;

namespace TileStageFormat.Events
{
    /// <summary>
    /// RECT用イメージが読み込まれた
    /// </summary>
    public class ReadedRectImageEvent : EventArgs
    {
        ImageRect imageRect;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="obj"></param>
        public ReadedRectImageEvent(ImageRect obj)
        {
            imageRect = obj;
        }
    }
    /// <summary>
    /// RECT画像を呼び終えた後、呼び出すデリゲーター
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ReadedRectImageHandler(object sender, ReadedRectImageEvent e);

}
