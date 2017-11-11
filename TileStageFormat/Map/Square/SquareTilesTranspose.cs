using System.Collections.Generic;


namespace TileStageFormat.Map.Square
{
    /// <summary>
    /// スクエア・タイルの置き換え
    /// </summary>
    public class SquareTilesTranspose : FSquareTilesTransposeHeader
    {
        public const float TIME = 1 / 30F;
        public float period;
        public List<SquareTilesTransposeFrame> aTCF;          //SquareTilesTransposeFrame* pTCF;	// 
        public SquareTilesTranspose()
        {
            aTCF = new List<SquareTilesTransposeFrame>();
        }
        /// <summary>
        /// フレーム数
        /// </summary>
        public int FrameCount { get { return aTCF.Count; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool CheckDrawRange(int x, int y)
        {
            if (x < tilePosX || x >= (tilePosX + tileNumX))
                return false;
            if (y < tilePosY || y >= (tilePosY + tileNumY))
                return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="wait"></param>
        public void CreateInsertNew(int idx, int wait)
        {
            SquareTilesTransposeFrame tmp = new SquareTilesTransposeFrame(tileNumX, tileNumY);
            tmp.wait = (short)wait;
            this.Insert(idx, tmp);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="copyFrameNo">コピー対象のフレーム番号</param>
        public void CreateInsertCopy(int idx, int copyFrameNo)
        {
            this.Insert(idx, aTCF[copyFrameNo].Clone());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="frameNo">フレーム番号</param>
        /// <returns></returns>
        public FSquareTileInfoMap GetMapchipInMapReference(int x, int y, int frameNo)
        {
            return aTCF[frameNo].aaMapChip[y - tilePosY, x - tilePosX];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aaMapChip"></param>
        public void CopyAdd(D2ArrayObject<FSquareTileInfoMap> aaMapChip)
        {
            SquareTilesTransposeFrame tmp = new SquareTilesTransposeFrame(tileNumX, tileNumY, aaMapChip, tilePosX, tilePosY);
            tmp.wait = 1;
            aTCF.Add(tmp);
            frameNum = (short)aTCF.Count;
            this.TimeReset();
        }
        /// <summary>
        /// 新しく追加
        /// </summary>
        public void NewAdd()
        {
            SquareTilesTransposeFrame tmp = new SquareTilesTransposeFrame(tileNumX, tileNumY);
            tmp.wait = 1;
            aTCF.Add(tmp);
            frameNum = (short)aTCF.Count;
            this.TimeReset();
        }
        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="r">追加対象</param>
        public void Add(SquareTilesTransposeFrame r)
        {
            aTCF.Add(r);
            frameNum = (short)aTCF.Count;
            this.TimeReset();
        }
        /// <summary>
        /// 指定位置を削除
        /// </summary>
        /// <param name="index">インデックス</param>
        public void Del(int index)
        {
            aTCF.RemoveAt(index);
            frameNum = (short)aTCF.Count;
            this.TimeReset();
        }
        /// <summary>
        /// 指定位置に挿入
        /// </summary>
        /// <param name="index"></param>
        /// <param name="r"></param>
        public void Insert(int index, SquareTilesTransposeFrame r)
        {
            aTCF.Insert(index, r);
            frameNum = (short)aTCF.Count;
            this.TimeReset();
        }
        /// <summary>
        /// 時間のリセット
        /// </summary>
        public void TimeReset()
        {
            period = 0;
            for (int i = 0; i < aTCF.Count; ++i)
            {
                period += aTCF[i].wait * TIME;
                aTCF[i].time = period;
            }
        }

    }
}
