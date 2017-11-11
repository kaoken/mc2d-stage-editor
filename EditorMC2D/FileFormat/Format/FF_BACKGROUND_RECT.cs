using MC2DUtil.ChunkFormat;
using MC2DUtil;

namespace KaokenFileFormat.Format
{
    /// <summary>
    /// 背景、全景で一枚絵から切り取るヘッダ部分
    /// </summary>
    public class FF_BACKGROUND_RECT : BasicBody
    {
	    //------------------------------------------
	    // 定数
	    public const long ID = 5392194;
	    public const int VERSION = 1000;
	    public const int SIZE = 20;
	    // flag
	    public const short CHIPBACK	= 0x0001;	// チップより後ろ
	    public const short CHIPFRONT= 0x0002;	// チップより手前
        public const short ANM_RECT = 0x0010;   // アニメーションRECT使用
	    //------------------------------------------
	    // 
	    public int		x;					// スクリーン座標 X
	    public int		y;					// スクリーン座標 Y
	    public short	nGroupNo;			// グループ番号
	    public short	flag;				// フラグ
	    public short	nDrawPriorityNo;	// 描画優先番号 [0~32768]
        public short    nAnmRectNo;			// イメージ番号も使用する。アニメーションRECT番号
        public short    nRectNo;			// 
        public short    tmp;			    // 
	    
        /// <summary>
        /// コンストラクタ
        /// </summary>
	    public FF_BACKGROUND_RECT()
	    {
            this.Set(0,0,0,0,0,0,0);
	    }
	    public void Set(int gn, int xx, int yy, int flg, int dn, int no, int rcNo){
		    nGroupNo	= (short)gn;
		    x			= xx;
		    y			= yy;
		    flag		= (short)flg;
		    nDrawPriorityNo = (short)dn;
            nAnmRectNo = (short)no;
            nRectNo = (short)rcNo;
        }
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
		    x = f.ReadInt();
		    y = f.ReadInt();
		    nGroupNo = f.ReadShort();
		    flag = f.ReadShort();
		    nDrawPriorityNo = f.ReadShort();
            nAnmRectNo = f.ReadShort();
            nRectNo = f.ReadShort();
            tmp = f.ReadShort();
        }

	    public override void Write(UtilFile f)
        {
		    // TODO 自動生成されたメソッド・スタブ
		    f.WriteInt(x);
		    f.WriteInt(y);
		    f.WriteShort(nGroupNo);
		    f.WriteShort(flag);
		    f.WriteShort(nDrawPriorityNo);
            f.WriteShort(nAnmRectNo);
            f.WriteShort(nRectNo);
            f.WriteShort(tmp);
        }

    }
}
