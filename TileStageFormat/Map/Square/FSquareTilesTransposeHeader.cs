using MC2DUtil.ChunkFormat;

using MC2DUtil;

namespace TileStageFormat.Map.Square
{
    /// <summary>
    /// 置き換えるスクエア・タイルの集まり
    /// </summary>
    public class FSquareTilesTransposeHeader : BasicBody
    {
        public static ulong ID { get { return CreateID8('S', 'T', 'T', 'H'); } }
        public const int SIZE = 140;
        // flags
        /// <summary>
        /// レイヤー0のマップチップを無効化（上書きしない）
        /// </summary>
        public const ushort INVALID_L00 = 0x0001;
        /// <summary>
        /// レイヤー1のマップチップを無効化（上書きしない）
        /// </summary>
        public const ushort INVALID_L01 = 0x0002;


        /// <summary>
        /// 置き換え名16文字
        /// </summary>
        protected byte[] szName;
        /// <summary>
        /// X軸のタイル数
        /// </summary>
        public short tileNumX;
        /// <summary>
        /// Y軸のタイル数
        /// </summary>
        public short tileNumY;
        /// <summary>
        ///  X軸上のタイル単位での置き換え初期位置
        /// </summary>
        public short tilePosX;
        /// <summary>
        ///  Y軸上のタイル単位での置き換え初期位置
        /// </summary>
        public short tilePosY;
        /// <summary>
        /// フレーム数
        /// </summary>
        public short frameNum;
        /// <summary>
        /// フラグ
        /// </summary>
        public ushort flags;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FSquareTilesTransposeHeader()
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
            f.ReadByte(szName, 0, 16);
            tileNumX = f.ReadShort();
            tileNumY = f.ReadShort();
            tilePosX = f.ReadShort();
            tilePosY = f.ReadShort();
            frameNum = f.ReadShort();
            flags = f.ReadUShort();
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
            f.WriteShort(tilePosX);
            f.WriteShort(tilePosY);
            f.WriteShort(frameNum);
            f.WriteUShort(flags);
        }
    }
}
