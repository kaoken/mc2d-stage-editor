namespace TileStageFormat.Map.Square
{
    /// <summary>
    /// 置き換え
    /// </summary>
    public class SquareTilesTransposeFrame : FSquareTilesTransposeFrame
    {
        /// <summary>時間</summary>
        public float time;
        public D2ArrayObject<FSquareTileInfoMap> aaMapChip;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SquareTilesTransposeFrame()
        {
            aaMapChip = null;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="x">X軸に対してのタイル数</param>
        /// <param name="y">Y軸に対してのタイル数</param>
        public SquareTilesTransposeFrame(int x, int y)
        {
            aaMapChip = new D2ArrayObject<FSquareTileInfoMap>(x, y);
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="x">X軸に対してのタイル数</param>
        /// <param name="y">Y軸に対してのタイル数</param>
        /// <param name="target"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        public SquareTilesTransposeFrame(int x, int y, D2ArrayObject<FSquareTileInfoMap> target, int posX, int posY)
        {
            aaMapChip = new D2ArrayObject<FSquareTileInfoMap>(x, y);
            int i, j;
            for (i = 0; i < aaMapChip.LengthX; ++i)
            {
                for (j = 0; j < aaMapChip.LengthY; ++j)
                {
                    aaMapChip[i, j].L00._n = target[posY + i, posX + j].L00._n;
                    aaMapChip[i, j].L01._n = target[posY + i, posX + j].L01._n;
                }
            }
        }
        /// <summary>
        /// 複製
        /// </summary>
        /// <returns></returns>
        public SquareTilesTransposeFrame Clone()
        {
            int i, j;
            SquareTilesTransposeFrame tmp = new SquareTilesTransposeFrame();
            tmp.time = 0;
            tmp.wait = wait;
            tmp.aaMapChip = new D2ArrayObject<FSquareTileInfoMap>(aaMapChip.LengthX, aaMapChip.LengthY);
            for (i = 0; i < aaMapChip.LengthY; ++i)
            {
                for (j = 0; j < aaMapChip.LengthX; ++j)
                {
                    tmp.aaMapChip[i, j].L00._n = aaMapChip[i, j].L00._n;
                    tmp.aaMapChip[i, j].L01._n = aaMapChip[i, j].L01._n;
                }
            }
            return tmp;
        }
    }
}
