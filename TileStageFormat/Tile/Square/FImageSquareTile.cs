using MC2DUtil.ChunkFormat;

using MC2DUtil;

namespace TileStageFormat.Tile.Square
{
    /// <summary>
    /// スクウェア画像
    /// </summary>
    public class FImageSquareTile : BaseImageTile
    {
        /// <summary>FImageSquareTileを表すID</summary>
        public static ulong ID { get { return CreateID8('I', 'S', 'T'); } }
        public const int SIZE = 140;

        /// <summary>
        /// この構造体を表す一意なID
        /// </summary>
        /// <returns></returns>
        public override ulong GetID() { return ID; }

        /// <summary>
        /// 構造体（クラス）のサイズを返す。ファイルの書き込み読み込み時に使用する
        /// </summary>
        /// <returns></returns>
        public override int GetStructSize(){return SIZE;}

        /// <summary>
        /// ファイルから読み込む
        /// </summary>
        /// <param name="f"></param>
        public override void Read(UtilFile f)
        {
            f.ReadByte(szName, 0, szName.Length);
            length = f.ReadInt();
            width = f.ReadInt();
            height = f.ReadInt();
        }

        /// <summary>
        /// ファイルに書き込む
        /// </summary>
        /// <param name="f"></param>
        public override void Write(UtilFile f)
        {
            f.WriteByte(szName);
            //f.WriteInt(lenght);
            f.WriteInt(width);
            f.WriteInt(height);
        }
    }
}
