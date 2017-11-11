using MC2DUtil.ChunkFormat;

using MC2DUtil;


namespace TileStageFormat.Map.Rect
{
    public class FImageRectMapSprite : BasicBody
    {
        /// <summary>ID</summary>
        public static ulong ID { get { return CreateID8('I', 'R', 'M', 'S'); } }
        /// <summary>データサイズ</summary>
        public const int SIZE = 20;
        #region flagsで使用する
        /// <summary>左右反転</summary>
        public const short HORIZONTAL   = 0x0001;
        /// <summary>上下反転</summary>
        public const short VERTICAL     = 0x0002;
        /// <summary>アニメーションRECTである</summary>
        public const short ANM_RECT     = 0x1000;
        #endregion
        //------------------------------------------
        // 
        /// <summary>フラグ</summary>
        public short flags;
        /// <summary>イメージファイル番号(0～32767:-1で指定なし)</summary>
        public short imageFileNo;
        /// <summary>イメージRECT番号(0～32767:-1で指定なし) ただし、flagsがANM_RECTがあった場合、アニメーションRECTを参照する</summary>
        public short imageRectNo;
        /// <summary>グループ番号(0～32767)</summary>
        public short groupNo;
        /// <summary>描画するレイヤー位置（０が一番奥）</summary>
        public short drawLayer;
        /// <summary>予約</summary>
        public short dumy;
        /// <summary>表示位置X</summary>
        public int x;
        /// <summary>表示位置Y</summary>
        public int y;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FImageRectMapSprite()
        {
            flags = imageFileNo = imageRectNo = groupNo = 0;
            x = y = dumy = 0;
        }
        /// <summary>
        /// 表示位置をセットする
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public void Set(int a, int b)
        {
            x = a; y = b;
        }
        /// <summary>
        /// 表示位置をセットする
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public void Set(float a, float b)
        {
            x = (int)a; y = (int)b;
        }
        ///// <summary>
        ///// 3次元ベクトルを取得する
        ///// </summary>
        ///// <returns></returns>
        //public Vector3 GetVector3()
        //{
        //    return new Vector3((float)x, (float)y, 0);
        //}
        ///// <summary>
        ///// 2次元ベクトルを取得する
        ///// </summary>
        ///// <returns></returns>
        //public Vector2 GetVector2()
        //{
        //    return new Vector2((float)x, (float)y);
        //}
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
            flags = f.ReadShort();
            imageFileNo = f.ReadShort();
            imageRectNo = f.ReadShort();
            groupNo     = f.ReadShort();
            drawLayer   = f.ReadShort();
            dumy        = f.ReadShort();
            x       = f.ReadInt();
            y       = f.ReadInt();
        }

        public override void Write(UtilFile f)
        {
            f.WriteShort(flags);
            f.WriteShort(imageFileNo);
            f.WriteShort(imageRectNo);
            f.WriteShort(groupNo);
            f.WriteShort(drawLayer);
            f.WriteShort(dumy);
            f.WriteInt(x);
            f.WriteInt(y);
        }
    }
}
