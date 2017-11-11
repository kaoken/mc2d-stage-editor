using MC2DUtil.ChunkFormat;
using MC2DUtil;

namespace TileStageFormat.Tile
{
    /// <summary>
    /// 対象画像から切り取った一つのスプライト
    /// </summary>
    public class FImageRectInfo : BasicBody
    {
        public static ulong ID { get { return CreateID8('I','R','I'); } }
        public const int SIZE = 12;


        /// <summary>イメージ内の座標 X</summary>
        public short x;
        /// <summary>イメージ内の座標 Y</summary>
        public short y;
        /// <summary>イメージの幅</summary>
        public short width;
        /// <summary>イメージの高さ</summary>
        public short height;
        /// <summary>中心の座標 X</summary>
        public short anchorX;
        /// <summary>中心の座標 Y</summary>
        public short anchorY;
        

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FImageRectInfo()
		{
            x = y = 0;
            width = height = 0;
            anchorX = anchorY = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void Set(int x, int y, int w, int h)
        {
            x = (short)x;
            y = (short)y;
            width = (short)w;
            height = (short)h;
        }
        //public Microsoft.Xna.Framework.Rectangle GetXRect()
        //{
        //    Microsoft.Xna.Framework.Rectangle rc = new Microsoft.Xna.Framework.Rectangle(x, y, width, height);
        //    return rc;
        //}

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
            x = f.ReadShort();
            y = f.ReadShort();
            width = f.ReadShort();
            height = f.ReadShort();
        }
        /// <summary>
        /// ファイルに書き込む
        /// </summary>
        /// <param name="f"></param>
        public override void Write(UtilFile f)
        {
            f.WriteShort(x);
            f.WriteShort(y);
            f.WriteShort(width);
            f.WriteShort(height);
            f.WriteShort(anchorX);
            f.WriteShort(anchorY);
        }
    }
}
