using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileStageFormat.Tile.Square
{
    /// <summary>
    /// チップアニメーション情報
    /// </summary>
    public class SquareTileAnimationInfo
    {
        public const int STOP = 0;
        public const int PLAY = 1;
        public const int INV_PLAY = 2;
        public const int LOOP = -1;

        public float nowTime;
        public int frameNum;
        public int state;
        public int loopNum;

        public SquareTileAnimationInfo()
        {
            nowTime = 0;
            frameNum = 0;
            state = PLAY;
            loopNum = LOOP;
        }
    }
}
