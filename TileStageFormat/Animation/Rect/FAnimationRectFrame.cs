using MC2DUtil.ChunkFormat;

using MC2DUtil;

namespace TileStageFormat.Animation.Rect
{
    /// <summary>
    /// 背景、全景で一枚絵を切り取って使用する長方形データ
    /// </summary>
    public class FAnimationRectFrame : BasicBody
    {
        //------------------------------------------
        // 定数
        public static ulong ID { get { return CreateID8('A', 'R', 'C', 'F'); } }
        public const int SIZE = 12;
	    // flag
	    public const short 	HORIZONTAL	= 0x0001;	// 左右反転
	    public const short 	VERTICAL	= 0x0002;	// 上下反転
	    //------------------------------------------
	    // 
	    public short	wait;			// 1/30を１とした待機時間
	    public short	imageFileNo;	// イメージファイル番号(0～32767:-1で指定なし)
        public short    imageRectNo;	// イメージRECT番号(0～32767:-1で指定なし)
        public short    flags;			// 左右、上下反転フラグを主に使用
        public short    moveX;			// スクーリン座標値でX移動
        public short    moveY;			// スクーリン座標値でY移動

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FAnimationRectFrame()
        {
            wait = imageFileNo = imageRectNo = flags = 0;
            moveX = moveY = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wait"></param>
        /// <param name="img"></param>
        /// <param name="rcNo"></param>
        /// <param name="f"></param>
	    public void Set(int wait, int img, int rcNo, int f, int x, int y ){
		    wait = (short)wait;
		    imageFileNo = (short)img;
            imageRectNo = (short)rcNo;
		    flags = (short)f;
            moveX = (short)x;
            moveY = (short)y;
        }
        //===========================================================
        // 以下BasicBodyより派生
        //===========================================================		
        public override ulong GetID() { return ID; }
        public override int GetStructSize()
        {
		    return SIZE;
	    }


	    public override void Read(UtilFile f)
        {
		    wait = f.ReadShort();
		    imageFileNo = f.ReadShort();
            imageRectNo = f.ReadShort();
		    flags = f.ReadShort();
            moveX = f.ReadShort();
            moveY = f.ReadShort();
        }

	    public override void Write(UtilFile f)
        {
		    f.WriteShort(wait);
		    f.WriteShort(imageFileNo);
            f.WriteShort(imageRectNo);
		    f.WriteShort(flags);
            f.WriteShort(moveX);
            f.WriteShort(moveY);
        }
    }
}
