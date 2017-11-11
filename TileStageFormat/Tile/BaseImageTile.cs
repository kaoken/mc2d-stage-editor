using MC2DUtil.ChunkFormat;

using MC2DUtil;

namespace TileStageFormat.Tile
{
    /// <summary>
    /// 基本と成画像
    /// </summary>
    public abstract class BaseImageTile : BasicBody
    {
        /// <summary>ファイル名ASCII文字で128文字</summary>
        protected byte[] szName;
        /// <summary>幅</summary>
        public int width;
        /// <summary>高さ</summary>
        public int height;
        /// <summary>タイルの幅</summary>
        public int tileWidht;
        /// <summary>タイルの高さ</summary>
        public int tileHeight;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BaseImageTile()
        {
            szName = new byte[128];
        }
        /// <summary>
        /// 文字列をASCIIに変換しセットする
        /// </summary>
        /// <param name="str">文字列</param>
        public void SetString(string str)
        {
            SetStringUtil(str, ref szName);
        }
        /// <summary>
        /// ファイル名を取得する
        /// </summary>
        /// <returns>文字列を取得</returns>
        public string GetString()
        {
            return GetStringUtil(szName);
        }
    }
}
