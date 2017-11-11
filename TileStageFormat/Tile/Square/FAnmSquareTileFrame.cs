using MC2DUtil.ChunkFormat;

using MC2DUtil;

namespace TileStageFormat.Tile.Square
{
    /// <summary>
    /// アニメーション・スクウェア・タイル
    /// </summary>
    public class FAnmSquareTileFrame : BasicBody
    {
        /// <summary>FImageSquareTileを表すID</summary>
        public static ulong ID { get { return CreateID8('A', 'S', 'T', 'F'); } }
        public const int SIZE = 6;

        /// <summary>タイルが0度右回転</summary>
        public const byte ROT_0 = 0x00;
        /// <summary>タイルが90度右回転</summary>
        public const byte ROT_90 = 0x01;
        /// <summary>タイルが180度右回転</summary>
        public const byte ROT_180 = 0x02;
        /// <summary>タイルが270度右回転</summary>
        public const byte ROT_270 = 0x04;
        /// <summary>衝突チップが左右反転</summary>
        public const byte FLIPHORIZONTAL = 0x08;

        /// <summary>1/30を１とした待機時間</summary>
        public short wait;
        /// <summary>指定イメージのマップチップ番号</summary>
        public short tileNo;
        /// <summary>変換フラグ</summary>
        public byte transformFlg;
        /// <summary>予約</summary>
        public byte tmp;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FAnmSquareTileFrame()
        {
            wait = 0;
            tileNo = 0;
            transformFlg = 0;
            tmp = 0;
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
            wait = f.ReadShort();
            tileNo = f.ReadShort();
            transformFlg = f.ReadByte();
            tmp = f.ReadByte();
        }
        /// <summary>
        /// ファイルに書き込む
        /// </summary>
        /// <param name="f"></param>
        public override void Write(UtilFile f)
        {
            f.WriteShort(wait);
            f.WriteShort(tileNo);
            f.WriteByte(transformFlg);
            f.WriteByte(tmp);
        }
    }
}
