using MC2DUtil;
using MC2DUtil.ChunkFormat;


namespace KaokenFileFormat.Format
{
    public class FF_BACKGROUND : BasicBody
    {
        //------------------------------------------
	    // 定数
	    public const long ID = 18242;
	    public const int VERSION = 1000;
	    public const int SIZE = 24;
	    //------------------------------------------
	    // 
	    protected byte[]	szName;			// 名前
	    public short		tileNumX;			// X軸のマップチップ数
	    public short		tileNumY;			// Y軸のマップチップ数
	    public int			tmp;				// 予約

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FF_BACKGROUND()
        {
            szName = new byte[16];
            tileNumX = tileNumY = 0;
            tmp = 0;
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
		    f.ReadByte(szName, 0, 16);
		    tileNumX = f.ReadShort();
		    tileNumY = f.ReadShort();
		    tmp = f.ReadInt();		
	    }

	    public override void Write(UtilFile f)
        {
		    f.WriteByte(szName);
		    f.WriteShort(tileNumX);
		    f.WriteShort(tileNumY);
		    f.WriteInt(tmp);
	    }
    }
}
