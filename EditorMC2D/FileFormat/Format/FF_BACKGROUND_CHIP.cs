using TileStageFormat.Map.Square;
using MC2DUtil;
using MC2DUtil.ChunkFormat;

namespace KaokenFileFormat.Format
{
    /// <summary>
    /// 背景、全景で使用するチップ
    /// </summary>
    public class FF_BACKGROUND_CHIP : BasicBody
    {
        //------------------------------------------
        // 定数
        public const long ID = 4409154;
        public const int SIZE = 8;
        // チップフラグで使用する
        public const short ROT_0			= 0x0000;	// チップが0度右回転
        public const short ROT_90			= 0x0001;	// チップが90度右回転
        public const short ROT_180			= 0x0002;	// チップが180度右回転
        public const short ROT_270			= 0x0004;	// チップが270度右回転
        public const short FLIPHORIZONTAL	= 0x0008;	// チップが左右反転
        public const short COLLI_INV		= 0x1000;	// レイヤーは衝突判定を無効化
        public const short ANMCHIP			= 0x2000;	// レイヤーはアニメーションチップ
        //------------------------------------------
        //
        public Layer L;
        public short		tmp;	// 予約
	    //===========================================================
	    // 以下BasicBodyより派生
	    //===========================================================	
        public override ulong GetID() { return ID; }
	    public override int GetStructSize()
        {
		    // TODO 自動生成されたメソッド・スタブ
		    return SIZE;
	    }


	    public override void Read(UtilFile f)
        {
		    // TODO 自動生成されたメソッド・スタブ
		    L.anmTileNo = f.ReadShort();
		    L.anmGroupNo = f.ReadShort();
		    L.flags = f.ReadUShort();
		    tmp = f.ReadShort();
	    }

	    public override void Write(UtilFile f)
        {
		    // TODO 自動生成されたメソッド・スタブ
            f.WriteShort(L.anmTileNo);
            f.WriteShort(L.anmGroupNo);
		    f.WriteUShort(L.flags);
		    f.WriteShort(tmp);
	    }
    }
}
