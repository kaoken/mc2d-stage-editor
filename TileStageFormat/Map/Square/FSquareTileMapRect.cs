using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MC2DUtil.ChunkFormat;

using MC2DUtil;

namespace TileStageFormat.Map.Square
{
    /// <summary>
    /// イベントなどに使用する長方形
    /// </summary>
    public class FSquareTileMapRect : BasicBody
    {
        //------------------------------------------
        // 定数
        public static ulong ID { get { return CreateID8('S', 'M', 'E', 'R'); } }
	    public const int SIZE = 136;
	    //------------------------------------------
	    // 
        protected byte[]    szName;			// マップ名ASCII文字で16文字
        public short        x;				// スクリーン座標 X
	    public short		y;				// スクリーン座標 Y
	    public short		width;			// 幅
	    public short		height;		// 高さ
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FSquareTileMapRect()
        {
            szName = new byte[128];
            x = y = width = height = 0;
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
		    x = f.ReadShort();
		    y = f.ReadShort();
		    width = f.ReadShort();
		    height = f.ReadShort();
	    }

        public override void Write(UtilFile f)
        {
		    f.WriteByte(szName);
		    f.WriteShort(x);
		    f.WriteShort(y);
		    f.WriteShort(width);
		    f.WriteShort(height);
	    }

    }
}
