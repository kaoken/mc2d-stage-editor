using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileStageFormat.Tile.Square;

namespace TileStageFormat.Events
{
    /// <summary>
    /// スクエア用のイメージが読み込まれた
    /// </summary>
    public class ReadedSquareImageEvent : EventArgs
    {
        ImageSquareTile imageSquareTile;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="obj"></param>
        public ReadedSquareImageEvent(ImageSquareTile obj)
        {
            imageSquareTile = obj;
        }
    }
    /// <summary>
    /// スクエア・タイル画像を呼び終えた後、呼び出すデリゲーター
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ReadedSquareImageHandler(object sender, ReadedSquareImageEvent e);

}
