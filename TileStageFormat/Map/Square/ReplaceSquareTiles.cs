

namespace TileStageFormat.Map.Square
{
    /// <summary>
    /// 置き換えチップ情報
    /// </summary>
    public class ReplaceSquareTiles
    {
        public SquareTilesTranspose rTC = null;
        public int nCopyFrameNo = -1;
        /// <summary>
        /// 
        /// </summary>
        public ReplaceSquareTiles()
        {
            this.Init(null);
        }
        public void CreateInsertNew(int idx, int wait)
        {
            rTC.CreateInsertNew(idx, wait);
        }
        public void CreateInsertCopy(int idx, int nCopyFrameNo)
        {
            rTC.CreateInsertCopy(idx, nCopyFrameNo);
        }
        public void Init(SquareTilesTranspose r)
        {
            rTC = r;
            nCopyFrameNo = -1;
        }
        public SquareTilesTransposeFrame this[int idx]
        {
            get
            {
                return rTC.aTCF[idx];
            }
        }
    }
}
