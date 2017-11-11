using MC2DUtil.ChunkFormat;

using MC2DUtil;

namespace TileStageFormat.Map.Rect
{
    /// <summary>
    /// 
    /// </summary>
    public class FImageRectMap : BasicBody
    {
        //------------------------------------------
        // 定数
        public static ulong ID { get { return CreateID8('I', 'R', 'M'); } }
        public const int SIZE = 64;						// サイズ
        //------------------------------------------
        /// <summary>
        /// イメージRECTマップ名
        /// </summary>
        protected byte[] szName;
        /// <summary>
        /// 現在使用しない。予約
        /// </summary>
        public int dumy;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FImageRectMap()
        {
            szName = new byte[SIZE];
            dumy = 0;
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
        /// 名を取得する
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
            f.ReadByte(szName, 0, SIZE);
            dumy = f.ReadInt();
        }

        public override void Write(UtilFile f)
        {
            f.WriteByte(szName);
            f.WriteInt(dumy);
        }
    }
}
