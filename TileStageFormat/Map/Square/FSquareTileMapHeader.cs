using MC2DUtil.ChunkFormat;

using MC2DUtil;

namespace TileStageFormat.Map.Square
{
    /// <summary>
    /// スクエア・タイル・マップ・ヘッダー
    /// </summary>
    public class FSquareTileMapHeader : BasicBody
    {
        public static ulong ID { get { return CreateID8('S', 'T', 'M', 'H'); } }
        public const int SIZE = 132;
        /// <summary>FAnmSquareTileHeaderを表すID</summary>
        protected byte[] szName;
        /// <summary>X軸のタイル数</summary>
        public short tileNumX;
        /// <summary>Y軸のタイル数</summary>
        public short tileNumY;



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FSquareTileMapHeader()
        {
            szName = new byte[128];

            tileNumX = 0;
            tileNumY = 0;
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
            f.ReadByte(szName, 0, 16);
            tileNumX = f.ReadShort();
            tileNumY = f.ReadShort();
        }
        /// <summary>
        /// ファイルに書き込む
        /// </summary>
        /// <param name="f"></param>
        public override void Write(UtilFile f)
        {
            f.WriteByte(szName);
            f.WriteShort(tileNumX);
            f.WriteShort(tileNumY);
        }
    }
}
