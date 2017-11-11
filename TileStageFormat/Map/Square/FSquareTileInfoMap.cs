using MC2DUtil.ChunkFormat;

using MC2DUtil;
using System.Runtime.InteropServices;

namespace TileStageFormat.Map.Square
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Layer
    {
        [FieldOffsetAttribute(0)]
        public long _n;
        // 通常チップ使用時
        /// <summary>
        /// イメージファイル番号(0～32767:-1で指定なし)
        /// </summary>
        [FieldOffsetAttribute(0)]
        public short imageFileNo;
        /// <summary>
        /// マップチップ番号(0～32767)
        /// </summary>
        [FieldOffsetAttribute(2)]
        public short tileNo;
        // アニメーションチップ使用時
        /// <summary>
        /// アニメーション番号(0～32767:-1で指定なし)
        /// </summary>
        [FieldOffsetAttribute(2)]
        public short anmTileNo;
        /// <summary>
        /// グループ番号(0～32767)
        /// </summary>
        [FieldOffsetAttribute(4)]
        public short anmGroupNo;
        /// <summary>
        /// フラグ
        /// </summary>
        [FieldOffsetAttribute(6)]
        public ushort flags;
    };

    /// <summary>
    /// MAP上で使う一つのスクエア・タイルひとつの情報
    /// </summary>
    public class FSquareTileInfoMap : BasicBody
    {
        public static ulong ID { get { return CreateID8('S', 'T', 'I', 'M'); } }
        public const int SIZE = 12;
        // チップフラグで使用する
        /// <summary>タイルが0度右回転</summary>
        public const ushort ROT_0       = 0x0000;
        /// <summary>タイルが90度右回転</summary>
        public const ushort ROT_90      = 0x0001;
        /// <summary>タイルが180度右回転</summary>
        public const ushort ROT_180     = 0x0002;
        /// <summary>タイルが270度右回転</summary>
        public const ushort ROT_270     = 0x0004;
        /// <summary>チップが左右反転</summary>
        public const ushort FLIPHORIZONTAL = 0x0008;

        // レイヤーフラグで使用する
        /// <summary>レイヤーは衝突判定を無効化</summary>
        public const ushort COLLI_INV   = 0x1000;
        /// <summary>レイヤーはアニメーションチップ</summary>
        public const ushort ANMCHIP     = 0x2000;

        public Layer L00;
        public Layer L01;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FSquareTileInfoMap()
        {
            this.Init();
        }
        /// <summary>
        /// 複製
        /// </summary>
        /// <returns></returns>
        public FSquareTileInfoMap Clone()
        {
            FSquareTileInfoMap tmp = new FSquareTileInfoMap();
            tmp.L00._n = L00._n;
            tmp.L01._n = L01._n;
            return tmp;
        }
        /// <summary>
        /// 初期化する
        /// </summary>
        public void Init()
        {
            L00._n = -1;
            L01._n = -1;
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
            L00.imageFileNo = f.ReadShort();
            L00.tileNo = f.ReadShort();
            L00.anmGroupNo = f.ReadShort();
            L00.flags = f.ReadUShort();
            L01.imageFileNo = f.ReadShort();
            L01.tileNo = f.ReadShort();
            L01.anmGroupNo = f.ReadShort();
            L01.flags = f.ReadUShort();
        }
        /// <summary>
        /// ファイルに書き込む
        /// </summary>
        /// <param name="f"></param>
        public override void Write(UtilFile f)
        {
            f.WriteShort(L00.imageFileNo);
            f.WriteShort(L00.tileNo);
            f.WriteShort(L00.anmGroupNo);
            f.WriteUShort(L00.flags);
            f.WriteShort(L01.imageFileNo);
            f.WriteShort(L01.tileNo);
            f.WriteShort(L01.anmGroupNo);
            f.WriteUShort(L01.flags);
        }
    }
}
