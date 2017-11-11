using System.IO;

namespace MC2DUtil.ChunkFormat
{

    public class LoadState
    {
        /// <summary>ファイル名</summary>
        protected string m_fileName;
        /// <summary>ファイル</summary>
        protected UtilFile m_file;
        /// <summary>ヘッダー</summary>
        protected Header m_header;
        /// <summary>現在の深さ</summary>
        protected int m_currentDepth;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LoadState()
        {
            m_file = null;
            m_header = new Header();
        }


        /**
		 * 作成するファイル名を設定する。
		 * @param fileName
		 */
        public void InitSetting(string fileName)
        {
            m_fileName = fileName;
        }
        /// <summary>
        /// 読み込みを開始する。ppOutへLoadChankのポインタを渡す。使い終わったら必ずRelease関数を呼び出す。
        /// </summary>
        /// <returns></returns>
        public LoadChank Start()
        {
            Chunk chank = new Chunk();
            this.End();

            m_file = new UtilFile(m_fileName, FileMode.Open);

            // ヘッダの読み込み
            m_header.Read(m_file);
            // 初回チャンク設定読み込み
            chank.Read(m_file);
            return new LoadChank(m_file, chank, m_file.Tell());
        }
        /// <summary>
        /// 書き出しを終了する。
        /// </summary>
        public void End()
        {
            if (m_file != null)
            {
                m_file.Close();
                m_file = null;
            }
        }

        /// <summary>
        /// ヘッダデータ取得
        /// </summary>
        /// <param name="hOut"></param>
        public void GetHeader(Header hOut)
        {
            hOut.version = m_header.version;
            hOut.fileType = m_header.fileType;
            hOut.size = m_header.size;
        }
    }

}
