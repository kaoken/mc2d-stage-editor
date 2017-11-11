namespace MC2DUtil.ChunkFormat
{
    /// <summary>
    /// チャンク読み込み
    /// </summary>
    public class LoadChank
    {
        /// <summary>ファイル</summary>
        protected UtilFile m_file;
        /// <summary>チャンク</summary>
        protected Chunk m_chank;
        /// <summary>次のファイルのオフセット値</summary>
        protected long m_offset;
        /// <summary>次のファイルのオフセット値</summary>
        protected long m_nextOffset;
        /// <summary>検索した回数</summary>
        protected int m_chankCnt;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="file"></param>
        /// <param name="chank"></param>
        /// <param name="offset"></param>
        public LoadChank(UtilFile file, Chunk chank, long offset)
        {
            m_file = file;
            m_chank = new Chunk();
            m_chank.id = chank.id;
            m_chank.num = chank.num;
            m_chank.size = chank.size;
            m_offset = offset;
            m_nextOffset = offset;
            m_chankCnt = 0;
        }
        /// <summary>
        /// このクラス内のブロックチャンクデータを取得する
        /// </summary>
        /// <param name="chank">チャンクデータが渡される</param>
        public void GetBlockChank(Chunk chank)
        {
            chank.id = m_chank.id;
            chank.num = m_chank.num;
            chank.size = m_chank.size;
        }
        /// <summary>
        /// 次のチャンクデータを探し出す。
        /// </summary>
        /// <param name="ckOut">値を入れるチャンク</param>
        /// <returns></returns>
        public LoadData FindChank(Chunk ckOut)
        {
            LoadData tmp;

            if (m_chankCnt < m_chank.num)
            {
                m_file.Seek(m_nextOffset);
                ckOut.Read(m_file);
                m_nextOffset += ckOut.size + ckOut.GetStructSize();
                tmp = new LoadData(m_file, ckOut, m_file.Tell());

                ++m_chankCnt;
                return tmp;
            }
            return null;
        }
    }
}
