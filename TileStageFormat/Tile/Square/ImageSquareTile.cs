using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileStageFormat.Tile.Square
{
    /// <summary>
    /// ツールで使用するイメージファイルクラス
    /// </summary>
    public class ImageSquareTile : FImageSquareTile
    {
        public List<FSquareTileInfo> tileInfos = null;
        public int TileWidthSize;
        public int TileHeightSize;
        public ImageSquareTile()
        {
            tileInfos = new List<FSquareTileInfo>();
        }
    };
}
