using System.Collections.Generic;

namespace TileStageFormat.Map.Square
{
    public class SquareTilesTransposeAnimation
    {
        public List<SquareTilesTranspose> aTC;
        // SquareTilesTranspose* stTC;		// 参照のみでdeleteはしないこと
        /// <summary>現在の秒数</summary>
        public float nowTime = 0;
        /// <summary>フレーム数</summary>
        public short frameNum = 0;
        /// <summary>予約</summary>
        public short tmp = 0;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SquareTilesTransposeAnimation()
        {
            aTC = new List<SquareTilesTranspose>();
        }
    }
}
