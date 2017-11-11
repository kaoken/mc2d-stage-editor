using MC2DUtil.ChunkFormat;

using MC2DUtil;

namespace TileStageFormat.Tile.Square
{
    /// <summary>
    /// スクエア・タイル情報
    /// </summary>
    public class FSquareTileInfo : BasicBody
    {
        /// <summary>FImageSquareTileを表すID</summary>
        public static ulong ID { get { return CreateID8('S', 'T', 'I'); } }
        public const int SIZE = 8;
        // 衝突チップフラグで使用する
        /// <summary>衝突チップが0度回転</summary>
        public const byte COLLI_ROT_0           = 0x00;
        /// <summary>衝突チップが90度右回転</summary>
        public const byte COLLI_ROT_L90         = 0x01;
        /// <summary>衝突チップが180度右回転</summary>
        public const byte COLLI_ROT_L180        = 0x02;
        /// <summary>衝突チップが270度右回転</summary>
        public const byte COLLI_ROT_L270        = 0x04;
        /// <summary>衝突チップが90度左回転</summary>
        public const byte COLLI_ROT_R90         = 0x04;
        /// <summary>衝突チップが180度左回転</summary>
        public const byte COLLI_ROT_R180        = 0x02;
        /// <summary>衝突チップが270度左回転</summary>
        public const byte COLLI_ROT_R270        = 0x01;
        /// <summary>衝突チップが左右反転</summary>
        public const byte COLLI_FLIPHORIZONTAL  = 0x08;
	    // チップフラグ 
	    public const ushort FLG_LADDER       = 0x0001;	// ハシゴである
	    public const ushort FLG_HERODAMAGE   = 0x0002;	// 主人公がダメージを受ける
	    // 衝突ターゲットフラグなどで使用
	    public const ushort TARGET_COLLI_01  = 0x0001;
	    public const ushort TARGET_COLLI_02  = 0x0002;
	    public const ushort TARGET_COLLI_03	 = 0x0004;
	    public const ushort TARGET_COLLI_04  = 0x0008;
	    public const ushort TARGET_COLLI_05	 = 0x0010;
	    public const ushort TARGET_COLLI_06  = 0x0020;
        public const ushort TARGET_COLLI_07  = 0x0040;
        public const ushort TARGET_COLLI_08  = 0x0080;
        public const ushort TARGET_COLLI_09  = 0x0100;
        public const ushort TARGET_COLLI_10  = 0x0200;
        public const ushort TARGET_COLLI_11  = 0x0400;
        public const ushort TARGET_COLLI_12  = 0x0800;
        public const ushort TARGET_COLLI_13  = 0x1000;
        public const ushort TARGET_COLLI_14  = 0x2000;
        public const ushort TARGET_COLLI_15  = 0x4000;
        public const ushort TARGET_COLLI_16  = 0x8000;
        public const ushort TARGET_COLLI_ALL = 0xFFFF;
        //------------------------------------------
        // 
        /// <summary>衝突チップの番号(0～127:0以下で指定なし)</summary>
        public byte collisionChipNo;
        /// <summary>衝突チップフラグ</summary>
        public byte collisionChipFlgs;
        /// <summary>衝突ターゲットフラグ</summary>
        public ushort collisionTargetFlgs;
        /// <summary>チップフラグ</summary>
        public uint tileFlg;

        /// <summary>
        /// コンストラクタ
        /// </summary>
	    public FSquareTileInfo()
	    {
		    this.Init();
	    }
	    /// <summary>
	    /// 初期化
	    /// </summary>
	    public void Init()
	    {
		    collisionChipNo = 0;
		    collisionChipFlgs = 0;
            collisionTargetFlgs = TARGET_COLLI_ALL;
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
        public override int GetStructSize() {return SIZE;}

        /// <summary>
        /// ファイルから読み込む
        /// </summary>
        /// <param name="f"></param>
	    public override void Read(UtilFile f) 
        {
		    collisionChipNo = f.ReadByte();
		    collisionChipFlgs = f.ReadByte();
		    collisionTargetFlgs = f.ReadUShort();
		    tileFlg = f.ReadUInt();
	    }

        /// <summary>
        /// ファイルに書き込む
        /// </summary>
        /// <param name="f"></param>
        public override void Write(UtilFile f)     
        {
		    f.WriteByte(collisionChipNo);
		    f.WriteByte(collisionChipFlgs);
		    f.WriteUShort(collisionTargetFlgs);
		    f.WriteUInt(tileFlg);
	    }
    }
}
