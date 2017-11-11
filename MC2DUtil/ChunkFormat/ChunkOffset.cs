namespace MC2DUtil.ChunkFormat
{
    /// <summary>
    /// チャンクオフセット
    /// </summary>
    public class ChunkOffset
    {
        /// <summary>チャンクデータ</summary>
        public Chunk chank;
        /// <summary>オフセット</summary>
        public long offset;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ChunkOffset()
        {
            chank = new Chunk();
        }
    }
}
