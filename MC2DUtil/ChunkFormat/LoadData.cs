namespace MC2DUtil.ChunkFormat
{
    public class LoadData
    {
        /// <summary>特になし</summary>
        protected static int CHANK_FLG_NOTHING = 0;
        /// <summary>チャンクの入れ子</summary>
        protected static int CHANK_FLG_NEST = 1;
        /// <summary>同じ層に入れ子</summary>
        protected static int CHANK_FLG_CHANK = 2;

        /// <summary>ファイル</summary>
        protected UtilFile m_file;
        /// <summary>次のファイルのオフセット値</summary>
        protected long m_nextOffset;
        /// <summary>オフセット値</summary>
        protected long m_offset;
        /// <summary>データカウント数</summary>
        protected int m_dataCnt;
        /// <summary>チャンク構造体</summary>
        protected Chunk m_chank;
        /// <summary>検索が終了したか</summary>
        protected bool m_isFindEnd;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="file"></param>
        /// <param name="chank"></param>
        /// <param name="offset"></param>
        public LoadData(UtilFile file, Chunk chank, long offset)
        {
            m_file = file;
            m_chank = new Chunk();
            m_chank.id = chank.id;
            m_chank.num = chank.num;
            m_chank.size = chank.size;
            m_offset = offset;
            m_nextOffset = offset;
            m_dataCnt = 0;
            m_isFindEnd = false;
        }

        /// <summary>
        /// まだ検索が終了したかどうか
        /// </summary>
        /// <returns>trueの場合終了している</returns>
        public bool GetFindEnd()
        {
            return m_isFindEnd;
        }

        /// <summary>
        /// 次のデータを探し出しLoadChankを渡す。
        /// </summary>
        /// <param name="bb"></param>
        /// <returns></returns>
        public LoadChank FindData(BasicBody bb)
        {
            Chunk chank = new Chunk();
            int flagss;

            if (m_dataCnt < m_chank.num)
            {
                ++m_dataCnt;

                m_file.Seek(m_nextOffset);
                bb.Read(m_file);
                flagss = m_file.ReadByte();
                m_nextOffset += bb.GetStructSize() + 1;

                if (flagss == CHANK_FLG_NEST)
                {
                    // 入れ子がある
                    chank.Read(m_file);
                    m_nextOffset += chank.size + chank.GetStructSize();
                    return new LoadChank(m_file, chank, m_file.Tell());
                }
                else
                {
                    return null;
                }
            }
            else
            {
                m_isFindEnd = true;
            }
            return null;
        }

        /// <summary>
        /// このクラス内のチャンクデータを取得する
        /// </summary>
        /// <param name="hOut">チャンクデータが渡される</param>
        public void GetChank(Chunk hOut)
        {
            hOut.id = m_chank.id;
            hOut.num = m_chank.num;
            hOut.size = m_chank.size;
        }

    }
}
