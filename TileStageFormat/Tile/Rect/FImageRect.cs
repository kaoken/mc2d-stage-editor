using MC2DUtil.ChunkFormat;

using MC2DUtil;

namespace TileStageFormat.Tile
{
    /// <summary>
    /// RECT・イメージ
    /// </summary>
    public class FImageRect : BasicBody
    {
        public static ulong ID { get { return CreateID8('I', 'R'); } }
        public const int SIZE = 136;
        /// <summary>ファイル名ASCII文字で128文字</summary>
        protected byte[] szName;
        /// <summary>幅</summary>
        public int width;
        /// <summary>高さ</summary>
        public int height;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FImageRect()
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


        /// <summary>
        /// この構造体を表す一意なID
        /// </summary>
        /// <returns></returns>
        public override ulong GetID() { return ID; }
        /// <summary>
        /// 構造体（クラス）のサイズを返す。ファイルの書き込み読み込み時に使用する
        /// </summary>
        /// <returns></returns>
        public override int GetStructSize() { return SIZE; }
        /// <summary>
        /// ファイルから読み込む
        /// </summary>
        /// <param name="f"></param>
        public override void Read(UtilFile f)
        {
            f.ReadByte(szName, 0, szName.Length);
            width = f.ReadInt();
            height = f.ReadInt();
        }
        /// <summary>
        /// ファイルに書き込む
        /// </summary>
        /// <param name="f"></param>
        public override void Write(UtilFile f)
        {
            f.WriteByte(szName);
            f.WriteInt(width);
            f.WriteInt(height);
        }
    }
}
